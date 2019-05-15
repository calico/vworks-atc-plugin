using System;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using com.apldbio.pcr.instrument;
using com.apldbio.pcr.protocol;
using com.apldbio.pcr.exception;

   
    namespace ATCVWorksPlugin
    {
    /// <summary>
    /// This example is an interactive GUI application to demonstrate below features:
    /// - Discover instruments on local network
    /// - Connect/Disconnect instrument
    /// - Real time monitor instrumnet by subscribing to instrument events
    /// - Open/Close Lid
    /// - Run control: Start, pause, resume and abort
    /// </summary>

    public partial class ATCDialog
    {
       
        // Event handlers
        private void OpenLidBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                ic.OpenLid();
            });
        }

        private void CloseLidBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                ic.CloseLid();
            });
        }

        private void StartRunBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                if (ic.IsConnected())
                {
                    ic.StartRun(ProtocolFactory.GetTestProtocol());
                }
                else
                {
                    MessageBox.Show("No instrument conneted. Please connect instrument and try again.");
                }
            }, "Unable to start run.");
        }

        private void PauseRunBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                ic.PauseRun();
            });
        }

        private void ResumeRunBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                ic.ResumeRun();
            });
        }

        private void AbortRunBtn_Click(object sender, RoutedEventArgs e)
        {
            InstrumentTaskFactory.StartTask(() =>
            {
                ic.AbortRun();
            });
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            String instrumentId = DeviceControlModel.InstrumentId;
            if (null != instrumentId && instrumentId.Length > 0)
            {
                Boolean proceed = true;

                // Inform user current connected instrument will be disconnected
                if (ic.IsConnected())
                {
                    MessageBoxResult result = MessageBox.Show("You will disconnect current instrument before connecting to another.",
                        "Continue?",
                        MessageBoxButton.OKCancel);
                    proceed = (result == MessageBoxResult.OK);
                }

                if (proceed)
                {
                    DeviceControlModel.LoadingVisibility = Visibility.Visible;
                    IPAddress address;
                    ConnectionMode mode = (IPAddress.TryParse(instrumentId, out address) && (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)) ?
                        ConnectionMode.IPV4 : ConnectionMode.HOST_NAME;

                    InstrumentTaskFactory.StartTask(() =>
                    {
                        ConnectInstrument(instrumentId, mode);
                    }, "Unable to connect instrument.", () =>
                    {
                        DeviceControlModel.LoadingVisibility = Visibility.Hidden;
                    });
                }
            }
            else
            {
                MessageBox.Show("Please enter valid host name or IP!");
            }
        }

        private void Window_Close(object sender, EventArgs e)
        {
            DisconnectInstrument();
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            DeviceControlModel.LoadingVisibility = Visibility.Visible;
            InstrumentTaskFactory.StartTask(() =>
            {
                DisconnectInstrument();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DeviceControlModel.LoadingVisibility = Visibility.Hidden;
                });
            });
        }

        private void ConnectInstrument(string address, ConnectionMode mode)
        {
            // Disconnect first if Instrumnet control still connects to an instrument
            if (ic.IsConnected())
            {
                DisconnectInstrument();
            }

            if (ic.Connect(address, mode, new WpfDeviceControlListener(DeviceControlModel)))
            {
                InstrumentProperties properties = ic.GetInstrumentProperties();
                RunProgress progress = ic.GetRunProgress();

                // Update UI
                Application.Current.Dispatcher.Invoke(() =>
                {

                    DeviceControlModel.UpdateInstrumentProperties(properties, ic.GetAddress());

                    // Update progress if a run is in progress while connected
                    if (null != progress)
                    {
                        if (progress.getRunTitle() != null || progress.getRunTitle() == "-")
                        {
                            DeviceControlModel.UpdateRunProgress(progress.getStage(), progress.getCycle(), progress.getStep());
                        }
                    }

                    DeviceControlModel.LoadingVisibility = Visibility.Hidden;
                });
            }
            else
            {
                MessageBox.Show("Unable to resolve instrument. Please try again or use IP address to connect.");
                DeviceControlModel.LoadingVisibility = Visibility.Hidden;
            }
        }

        private void DisconnectInstrument()
        {
            ic.Disconnect();
            // Update UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                DeviceControlModel.ResetInstrumentInfo();
                DeviceControlModel.ResetTemperature();
                DeviceControlModel.ResetRunProgress();
            });
        }

        private void DiscoverBtn_Click(object sender, RoutedEventArgs e)
        {
            DiscoverWindow discoverWindow = new DiscoverWindow(DeviceControlModel);
            discoverWindow.ShowDialog();
        }

        private void RunTimerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            DeviceControlModel.UpdateRunTimer();
        }
    }       
}
