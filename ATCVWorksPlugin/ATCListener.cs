using com.apldbio.pcr.instrument;
using com.apldbio.pcr.instrument.@event;
using System.Threading;

namespace VworksAtcPlugin
{
    public class ATCListener : InstrumentListener
    {        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CancellationTokenSource _source;

        public ATCListener(CancellationTokenSource source)
        {
            _source = source;
        }

        public void CancelTask()
        {
            _source.Cancel();            
        }        

        public void handleConnectionLostEvent(ConnectionLostEvent cle)
        {
            log.Error(cle.toString());                  
        }

        public void handleErrorEvent(ErrorEvent ee)
        {
            log.Error($"Id: {ee.getId()}, Severity: {ee.getSeverity()}, " +
                $"Data: {ee.getData()}, Text: {ee.getText()}");
         
            _source.Cancel();
        }

        public void handleRunEvent(RunEvent re)
        {
            log.Info(re.toString());
            if(re.getRunState() == RunEvent.RunEventType.ERROR || 
                re.getRunState() == RunEvent.RunEventType.ENDED || 
                re.getRunState() == RunEvent.RunEventType.ABORTED)
            {
                CancelTask();
            }
        }

        public void handleRunTimeEvent(RunTimeEvent rte)
        {
            // log.Info(rte.toString());
        }

        public void handleTemperatureEvent(TemperatureEvent te)
        {
            log.Info(te.toString());
        }
    }
}
