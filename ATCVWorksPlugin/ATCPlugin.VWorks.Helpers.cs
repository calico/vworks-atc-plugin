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

                //Do we need to re-attach the listener to the atc, ie re-connect?
                ReconnectToAtc();
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

        public void WaitForTemperature(CancellationToken token, bool WaitForLid, double LidTemp, bool WaitForBlock, double BlockTemp)
        {
            try
            {
                bool TempReached = false;

                if (!WaitForBlock && !WaitForLid)
                {
                    TempReached = true;
                }

                while (!TempReached)
                {
                    System.Diagnostics.Debug.WriteLine("Still waiting for temp: " + _atc.GetInstrumentState());

                    //Restart the Device Listener if needed.  Not sure what might be killing it, but this seems to happen occasionally.  
                    //if (_DeviceListener == null)
                    //{
                    //    _cancelToken = new CancellationTokenSource();
                    //    _DeviceListener = new ATCListener(_cancelToken);
                    //}

                    //TODO:  Is this logic right?  I'm not 100% confident...
                    if (WaitForBlock)
                    {
                        if (_DeviceListener.LastBlockTemperature > (BlockTemp - 4) && _DeviceListener.LastBlockTemperature < (BlockTemp + 4))
                        {
                            TempReached = true;
                        }
                        else
                        {
                            TempReached = false;
                        }
                    }

                    if (WaitForLid)
                    {
                        if (_DeviceListener.LastLidTemperature > (LidTemp - 4) && _DeviceListener.LastLidTemperature < (LidTemp + 4))
                        {
                            if (WaitForBlock)
                            {
                                //TempReached = TempReached;
                            }
                            else
                            {
                                TempReached = true;
                            }
                        }
                        else
                        {
                            TempReached = false;
                        }
                    }

              

                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested)
                    {
                        string msg = "Exiting wait for lid or block to reach temperature.";
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
                    WaitForTemperature(token, WaitForLid, LidTemp, WaitForBlock, BlockTemp);
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
