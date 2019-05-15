using com.apldbio.pcr.instrument;
using IWorksDriver;
using System;
using System.IO;
using System.Threading;


namespace VworksAtcPlugin
{
    public partial class ATCPlugin
    {
        /// <summary>
        /// Creates a new cancellation token source which can 
        /// be pased to the ATCListener class to stop the waiting function
        /// when a ATC protocol is complete.
        /// </summary>
        public void CreateTokenAndListener()
        {
            if (_cancelToken.Token.IsCancellationRequested)
            {
                // reset token and listener
                _cancelToken = new CancellationTokenSource();
                _DeviceListener = new ATCListener(_cancelToken);
            }
        }

        /// <summary>
        /// Waits for the protocol to finish by checking the instrument state.
        /// This can also be interrupted by a cancellation token that is triggered
        /// by the ATC listener.
        /// </summary>
        /// <param name="token"></param>
        public void WaitForRunToComplete(CancellationToken token)
        {
            try
            {
                while (_atc.GetInstrumentState() == InstrumentState.RUNNING)
                {
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested)
                    {
                        string msg = "Exiting PCR wait.";
                        log.Info(msg);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error.  
                log.Error(ex.ToString());

                // If disconnected retry connection
                if (!_atc.IsConnected())
                {
                    log.Info("Connection Lost. Attempting to reconnect");
                    ReconnectToAtc();
                    _DeviceListener = new ATCListener(_cancelToken);
                    WaitForRunToComplete(token);
                }
            }
        }

        /// <summary>
        /// Attempts to reconnect to the ATC with an exponential delay.
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="maxRetries"></param>
        public void ReconnectToAtc(int retryCount = 1, int maxRetries = 5)
        {
            while (retryCount <= maxRetries)
            {
                ReturnCode AtcInitialized = Initialize("initialize");
                if (AtcInitialized == ReturnCode.RETURN_SUCCESS)
                {
                    break;
                }
                else
                {
                    // Delay
                    int delay = 3750 * Convert.ToInt32(Math.Pow(2, (double)retryCount));
                    Thread.Sleep(delay);

                    // Retry
                    retryCount += 1;
                    ReconnectToAtc(retryCount, maxRetries);
                }
            }
        }

        static byte[] StreamToBytes(Stream input)
        {
            var capacity = input.CanSeek ? (int)input.Length : 0;
            using (var output = new MemoryStream(capacity))
            {
                int readLength;
                var buffer = new byte[4096];

                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                }
                while (readLength != 0);

                return output.ToArray();
            }
        }
    }
}
