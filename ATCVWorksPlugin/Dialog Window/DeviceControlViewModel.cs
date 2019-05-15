using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using LiveCharts.Configurations;
using com.apldbio.pcr.instrument;
using LiveCharts;

namespace VworksAtcPlugin
{
    public class DeviceControlViewModel : INotifyPropertyChanged
    {
        private const long activeTempPoint = 120;

        private string instrumentId;
        private string instrumentStatus;
        private string instrumentAddress;
        private string instrumentFirmware;
        private string instrumetnType;
        private string instrumentSerial;

        private string runStage;
        private string runCycle;
        private string runStep;
        private string runTimer;
        private Boolean viewByRemainingTime;
        private long runRemainingTime;
        private long runElapsedTime;

        private string coverTemperature;
        private string blockTemperature;

        private double xAxisMax;
        private double xAxisMin;
        private double yAxisMax;
        private double yAxisMin;

        private Visibility loadingVisibility;

        public Visibility LoadingVisibility
        {
            get { return loadingVisibility; }
            set { loadingVisibility = value; NotifyPropertyChanged("LoadingVisibility"); }
        }

        // Instrumenti info dynamic bindings
        public string InstrumentId
        {
            get { return instrumentId; }
            set { instrumentId = value; NotifyPropertyChanged("InstrumentId"); }
        }
        public string InstrumentStatus
        {
            get { return instrumentStatus; }
            set { instrumentStatus = value; NotifyPropertyChanged("InstrumentStatus"); }
        }
        public string InstrumentFirmware
        {
            get { return instrumentFirmware; }
            set { instrumentFirmware = value; NotifyPropertyChanged("InstrumentFirmware"); }
        }
        public string InstrumentAddress
        {
            get { return instrumentAddress; }
            set { instrumentAddress = value; NotifyPropertyChanged("InstrumentAddress"); }
        }
        public string InstrumentType
        {
            get { return instrumetnType; }
            set { instrumetnType = value; NotifyPropertyChanged("InstrumentType"); }
        }
        public string InstrumentSerial
        {
            get { return instrumentSerial; }
            set { instrumentSerial = value; NotifyPropertyChanged("InstrumentSerial"); }
        }

        // Temperature dynamic binding
        public string CoverTemperature
        {
            get { return coverTemperature; }
            set { coverTemperature = value; NotifyPropertyChanged("CoverTemperature"); }
        }
        public string BlockTemperature
        {
            get { return blockTemperature; }
            set { blockTemperature = value; NotifyPropertyChanged("BlockTemperature"); }
        }

        // Temperature plot dynamic bindings
        public Func<double, string> TemperatureFormatter { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public ChartValues<TemperatureModel> CoverPlotValues { get; set; }
        public ChartValues<TemperatureModel> BlockPlotValues { get; set; }
        public double XAxisStep { get; set; }
        public double XAxisMax
        {
            get { return xAxisMax; }
            set { xAxisMax = value; NotifyPropertyChanged("XAxisMax"); }
        }
        public double XAxisMin
        {
            get { return xAxisMin; }
            set { xAxisMin = value; NotifyPropertyChanged("XAxisMin"); }
        }
        public double YAxisMax
        {
            get { return yAxisMax; }
            set { yAxisMax = value; NotifyPropertyChanged("YAxisMax"); }
        }
        public double YAxisMin
        {
            get { return yAxisMin; }
            set { yAxisMin = value; NotifyPropertyChanged("YAxisMin"); }
        }

        // Run progress dynamic bindings
        public string RunStage
        {
            get { return runStage; }
            set { runStage = value; NotifyPropertyChanged("RunStage"); }
        }
        public string RunCycle
        {
            get { return runCycle; }
            set { runCycle = value; NotifyPropertyChanged("RunCycle"); }
        }
        public string RunStep
        {
            get { return runStep; }
            set { runStep = value; NotifyPropertyChanged("RunStep"); }
        }
        public string RunTimer
        {
            get { return runTimer; }
            set { runTimer = value; NotifyPropertyChanged("RunTimer"); }
        }
        public bool ViewByRemainingTime
        {
            get { return viewByRemainingTime; }
            set
            {
                viewByRemainingTime = value;
                NotifyPropertyChanged("ViewByRemainingTime");
                NotifyPropertyChanged("ViewByElapsedTime");
            }
        }
        public bool ViewByElapsedTime
        {
            get { return !viewByRemainingTime; }
            set
            {
                viewByRemainingTime = !value;
                NotifyPropertyChanged("ViewByElapsedTime");
                NotifyPropertyChanged("ViewByRemainingTime");
            }
        }

        // Implement the notify methods for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        public DeviceControlViewModel()
        {
            InitializeTemperaturePlot();
            LoadingVisibility = Visibility.Hidden;
            ResetInstrumentInfo();
            ResetTemperature();
            ResetRunProgress();
        }

        public void ResetInstrumentInfo()
        {
            InstrumentAddress = "-";
            InstrumentFirmware = "-";
            InstrumentSerial = "-";
            InstrumentType = "-";
            InstrumentStatus = "Unknown";
        }

        public void ResetTemperature()
        {
            CoverTemperature = "-";
            BlockTemperature = "-";

            CoverPlotValues.Clear();
            BlockPlotValues.Clear();
        }

        public void ResetRunProgress()
        {
            RunStage = RunCycle = RunStep = "-";
            runRemainingTime = runElapsedTime = -1;
            ViewByRemainingTime = true;
            RunTimer = "--:--:--";
        }

        // Call InstrumentProperties API to get the instrument infomartion
        public void UpdateInstrumentProperties(InstrumentProperties properties, string address)
        {
            if (null != properties)
            {
                InstrumentAddress = address;
                InstrumentFirmware = properties.getVersion();
                if (null != properties.getHostName() && properties.getHostName().Length > 0 && !properties.getHostName().Equals(InstrumentId))
                {
                    InstrumentId = properties.getHostName();
                }
                InstrumentSerial = properties.getSerialNumber();
                InstrumentType = properties.getProductName();

                InstrumentStatus = "Connected";
            }
        }

        public void UpdateTemperature(double coverTemperature, double[] blockTemperatures)
        {
            double coverTemp = coverTemperature;
            double blockTemp = 0;
            if (blockTemperatures.Length >= 1)
            {
                blockTemp = blockTemperatures.Sum() / blockTemperatures.Length;
            }

            CoverTemperature = coverTemp.ToString("N2") + " °C";
            BlockTemperature = blockTemp.ToString("N2") + " °C";

            SetXAxisLimits(DateTime.Now);

            CoverPlotValues.Add(new TemperatureModel
            {
                DateTime = DateTime.Now,
                Value = coverTemp
            });

            if (CoverPlotValues.Count > activeTempPoint + 10) CoverPlotValues.RemoveAt(0);

            BlockPlotValues.Add(new TemperatureModel
            {
                DateTime = DateTime.Now,
                Value = blockTemp
            });

            if (BlockPlotValues.Count > activeTempPoint + 10) BlockPlotValues.RemoveAt(0);
        }

        public void UpdateRunTime(long elapsedTime, long remainingTime)
        {
            runRemainingTime = remainingTime;
            runElapsedTime = elapsedTime;
            UpdateRunTimer();
        }

        public void UpdateRunTimer()
        {
            double timeValue = (ViewByRemainingTime) ? runRemainingTime : runElapsedTime;
            RunTimer = timeValue == -1 ? "--:--:--" :
                TimeSpan.FromSeconds((double)timeValue).ToString(@"hh\:mm\:ss");
        }

        public void UpdateRunProgress(int stage, int cycle, int step)
        {
            RunStage = stage.ToString();
            RunCycle = cycle.ToString();
            RunStep = step.ToString();
        }

        private void InitializeTemperaturePlot()
        {
            var mapper = Mappers.Xy<TemperatureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            Charting.For<TemperatureModel>(mapper);

            CoverPlotValues = new ChartValues<TemperatureModel>();
            BlockPlotValues = new ChartValues<TemperatureModel>();

            TemperatureFormatter = value => value.ToString("N2") + " °C";
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");

            XAxisStep = TimeSpan.FromSeconds(30).Ticks;
            SetXAxisLimits(DateTime.Now);

            YAxisMax = 120;
            YAxisMin = 10;
        }

        // To tailor a active window (X axis) for plot display
        private void SetXAxisLimits(DateTime now)
        {
            if (CoverPlotValues.Count == 0)
            {
                XAxisMax = now.Ticks + TimeSpan.FromSeconds(activeTempPoint).Ticks;
                XAxisMin = now.Ticks - TimeSpan.FromSeconds(0).Ticks;
            }
            else if (CoverPlotValues.Count > activeTempPoint)
            {
                XAxisMax = now.Ticks + TimeSpan.FromSeconds(0).Ticks;
                XAxisMin = now.Ticks - TimeSpan.FromSeconds(activeTempPoint).Ticks;
            }
        }
    }

    /// <summary>
    /// Temperature model for temperature plot
    /// </summary>
    public class TemperatureModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
}
