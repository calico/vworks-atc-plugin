using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using com.apldbio.pcr.instrument;


namespace VworksAtcPlugin
{
    /// <summary>
    /// Interaction logic for DiscoverWindow.xaml
    /// </summary>
    public partial class DiscoverWindow : Window
    {
        private DeviceControlViewModel parentModel;
        private InstrumentFinder finder;
        public ObservableCollection<SimpleInstrument> InstrumentList { get; set; }

        public DiscoverWindow()
        {
            InitializeComponent();

            InstrumentList = new ObservableCollection<SimpleInstrument>();
            AllInstruments.ItemsSource = InstrumentList;

            LoadingProgress.Visibility = Visibility.Visible;
            finder = new InstrumentFinder();
            finder.findAll(new WpfInstrumentFinderListener(this), 5000);

            DataContext = this;
        }

        public void HideLoading()
        {
            Debug.WriteLine("Discover timeout.");
            Dispatcher.Invoke(() => {
                LoadingProgress.Visibility = Visibility.Collapsed;
            });
        }

        public DiscoverWindow(DeviceControlViewModel parentModel) : this()
        {
            this.parentModel = parentModel;
        }

        public void AddFoundInstrument(string host, string address)
        {
            Dispatcher.Invoke(() =>
            {
                Debug.WriteLine("Found instrument: {0} ({1})", host, address);
                InstrumentList.Add(new SimpleInstrument()
                {
                    HostName = host,
                    IpAddress = address
                });
            });
        }

        private void instrument_DblClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                string hostName = (item as SimpleInstrument).HostName;
                string ipAddress = (item as SimpleInstrument).IpAddress;
                Debug.WriteLine(hostName + "-" + ipAddress);

                if (hostName != null)
                {
                    parentModel.InstrumentId = hostName;
                }
                else if (ipAddress != null)
                {
                    parentModel.InstrumentId = hostName;
                }
                this.Close();
            }
        }


    }

    public class SimpleInstrument
    {
        public string HostName { get; set; }
        public string IpAddress { get; set; }
    }

    class WpfInstrumentFinderListener : InstrumentFinderListener
    {
        private DiscoverWindow ui;
        public WpfInstrumentFinderListener(DiscoverWindow parent)
        {
            ui = parent;
        }

        public void handleFindingCompleted()
        {
            ui.HideLoading();
        }

        public void handleInstrumentFound(string host, string address)
        {
            ui.AddFoundInstrument(host, address);
        }
    }
}
