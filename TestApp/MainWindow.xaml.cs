using com.apldbio.pcr.exception;
using com.apldbio.pcr.instrument.@event;
using com.apldbio.pcr.protocol;
using System;
using System.ComponentModel;
using System.Windows;
using System.Runtime.CompilerServices;
using ATCVWorksPlugin;

namespace TestApp
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public InstrumentControl atc;
        public TestAppInstrumentListener listener;
        private string _address;
        public string filePath = @"C:\Users\tlee.CALICOLABS\source\repos\calico\tlee133\ATCDriver\TestData\OneCycleTest.xml";

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            DataContext = this;
            atc = new InstrumentControl();
            listener = new TestAppInstrumentListener();
            _address = atc.address;
        }

        private void OnPropertyChange([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public string Address
        {
            get
            {
                return atc.address;
            }
            set
            {
                atc.address = value;
            }
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            OutputMessage("starting connection.");
            atc.Connect(atc.address,InstrumentControl.ConnectionMode.IPV4,listener);
            OutputMessage("connected");
        }

        private void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            atc.Disconnect();
            OutputMessage("Disconnected");
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            // Call ProtocolUtil  API to load protocol xml file
            RunProtocol dummy_protocol = ProtocolUtil.load(new java.io.File(filePath));

            try
            {
                atc.StartRun(dummy_protocol);
                OutputMessage("run started..");
            }
            catch (PCRException pcrExcpetion)
            {
                OutputMessage(pcrExcpetion.getDefaultErrorMessage());
            }
            catch (Exception ex)
            {
                OutputMessage(ex.Message);
            }
            atc.StartRun(dummy_protocol);

        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            atc.AbortRun();
            OutputMessage("run aborted..");
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            atc.PauseRun();
            OutputMessage("run paused.");
        }

        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            atc.ResumeRun();
            OutputMessage("run resumed.");
        }

        private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            atc.Disconnect();
        }

        private void BtnOpenLid_Click(object sender, RoutedEventArgs e)
        {
            atc.OpenLid();
            OutputMessage("lid opened.");
        }

        private void BtnCloseLid_Click(object sender, RoutedEventArgs e)
        {
            atc.CloseLid();
            OutputMessage("lid closed.");
        }

        private void BtnInstrumentProperties_Click(object sender, RoutedEventArgs e)
        {
            var properties = atc.GetInstrumentProperties();
            OutputMessage(string.Join("\n", new string[] {
            "HostName: " + properties.getHostName(),
            "ProductName:" + properties.getProductName(),
            "SerialNumber:" + properties.getSerialNumber(),
            "Version:" + properties.getVersion()}));
        }

        private void BtnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (atc != null && atc.IsConnected())
            {
                OutputMessage(atc.GetInstrumentState().toString());
            }
        }

        private void OutputMessage(string msg)
        {
            Output.Text += "\n" + msg;
        }

        private void BtnStartLongRun_Click(object sender, RoutedEventArgs e)
        {
            filePath = @"C:\Users\tlee.CALICOLABS\source\repos\calico\tlee133\ATCDriver\TestData\ThirtyCycleTest.xml";
            BtnStart_Click(sender, e);
        }    
    }
}
