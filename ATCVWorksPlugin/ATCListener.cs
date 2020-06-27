using com.apldbio.pcr.instrument;
using com.apldbio.pcr.instrument.@event;
using System.Linq;
using System.Threading;

namespace VworksAtcPlugin
{
    public class ATCListener : InstrumentListener, System.IDisposable
    {        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private CancellationTokenSource _source;

        public double LastBlockTemperature { get; set; }
        public double LastLidTemperature { get; set; }

        public ATCListener(CancellationTokenSource source)
        {
            _source = source;

            LastBlockTemperature = 0;
            LastLidTemperature = 0;
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

            LastLidTemperature = te.getCoverTemperature();

            double[] BlockTemperatures = te.getBlockTemperatures();

            LastBlockTemperature = BlockTemperatures.Sum() / BlockTemperatures.Count();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    log.Debug("ATC Listener Disposed of.");
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ATCListener()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
