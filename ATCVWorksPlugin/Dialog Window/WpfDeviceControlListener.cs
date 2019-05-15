using System.Windows;
using com.apldbio.pcr.instrument;
using com.apldbio.pcr.instrument.@event;

namespace VworksAtcPlugin
{
    class WpfDeviceControlListener : InstrumentListener
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private DialogMainViewModel uiModel;

        public WpfDeviceControlListener(DialogMainViewModel parentModel)
        {
            uiModel = parentModel;
        }

        public void handleConnectionLostEvent(ConnectionLostEvent cle)
        {
            log.Error(cle.toString());
        }

        public void handleErrorEvent(ErrorEvent ee)
        {
            log.Error(ee.toString());
            MessageBox.Show("Instrument Error: " + ee.toString());
        }

        public void handleRunEvent(RunEvent re)
        {
            log.Error(re.toString());
            if (re.getRunState() == RunEvent.RunEventType.ERROR)
            {
                MessageBox.Show("Run Error encountered. Please abort run.");
            }
            else
            {
                uiModel.DeviceTab.UpdateRunProgress(re.getStage(), re.getCycle(), re.getStep());
            }
        }

        public void handleRunTimeEvent(RunTimeEvent rte)
        {
            log.Info(rte.toString());
            uiModel.DeviceTab.UpdateRunTime(rte.getElapsedTime(), rte.getRemainingTime());
        }

        public void handleTemperatureEvent(TemperatureEvent te)
        {
            log.Info(te.toString());
            uiModel.DeviceTab.UpdateTemperature(te.getCoverTemperature(), te.getBlockTemperatures());
        }
    }
}
