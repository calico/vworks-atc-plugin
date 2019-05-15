using com.apldbio.pcr.instrument;
using com.apldbio.pcr.instrument.@event;
using System.Windows;

namespace TestApp
{    
    public class TestAppInstrumentListener : InstrumentListener
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void handleConnectionLostEvent(ConnectionLostEvent cle)
        {
            logger.Error(cle.toString());
            MessageBox.Show(cle.toString());
        }

        public void handleErrorEvent(ErrorEvent ee)
        {
            logger.Debug(ee.toString());
            MessageBox.Show(ee.toString());
        }

        public void handleRunEvent(RunEvent re)
        {
            logger.Info(re.toString());
        }

        public void handleRunTimeEvent(RunTimeEvent rte)
        {
            logger.Info(rte.toString());
        }

        public void handleTemperatureEvent(TemperatureEvent te)
        {
            logger.Info(te.toString());
        }
    }
}
