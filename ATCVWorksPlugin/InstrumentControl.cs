using System;
using com.apldbio.pcr.instrument;
using com.apldbio.pcr.protocol;

namespace VworksAtcPlugin
{    
    public class InstrumentControl
    {
        // Setup Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Instrument _atc;
        private InstrumentListener _listener;        
        private static int FINDER_TIMEOUT = 4000;
        public enum ConnectionMode { HOST_NAME, IPV4 };




        public InstrumentControl()
        {
        }

        public string address { get; set; }
        public string hostname { get; set; }


        public bool Connect(string instrumentId, ConnectionMode mode, InstrumentListener listener)
        {
            if (_atc != null)
            {
                if (_atc.isConnected())
                {
                    return true;
                }
            }

            // Get IP Address
            if (mode == ConnectionMode.HOST_NAME)
            {
                hostname = instrumentId;
                InstrumentFinder finder = new InstrumentFinder();
                address = finder.find(instrumentId, FINDER_TIMEOUT);
            }
            else
            {
                address = instrumentId;
            }
            
            if (null != address)
            {
                _atc = new AutoThermalCycler(address);
                _atc.connect();
                this._listener = listener;
                _atc.addListener(listener);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            return _atc != null && _atc.isConnected();
        }

        public void Disconnect()
        {
            if (null != this._atc)
            {
                SetBlockTemperature(false, 25);
                SetLidTemp(false, 25);

                if (this._listener != null)
                {
                    _atc.removeListener(this._listener);
                    this._listener = null;
                }
                _atc.disconnect();
                _atc = null;
                address = null;
            }
        }

        public void OpenLid()
        {
            if (_atc != null)
            {
                _atc.openLid();
            }
        }

        public void CloseLid()
        {
            if (_atc != null)
            {
                _atc.closeLid();
            }
        }

        public void StartRun(RunProtocol tcprotocol)
        {
            if (_atc != null)
            {
                _atc.startRun(tcprotocol);
            }
        }

        public void PauseRun()
        {
            if (_atc != null)
            {
                if(_atc.getInstrumentState() != InstrumentState.IDLE)
                {
                    _atc.pauseRun();
                }                
            }
        }

        public void ResumeRun()
        {
            if (_atc != null)
            {
                _atc.resumeRun();
            }
        }

        public void AbortRun()
        {
            if (_atc != null)
            {
                _atc.stopRun();
            }
        }


        public void SetLidTemp(bool Enabled, double Temperature)
        {
            if (_atc != null)
            {
                IdleCoverSetting idleCoverSetting = new IdleCoverSetting();

                idleCoverSetting.setEnabled(Enabled);
                idleCoverSetting.setTemperature(Temperature);

                _atc.setIdleCoverSetting(idleCoverSetting);
            }
        }

        public void SetBlockTemperature(bool Enabled, double Temperature)
        {
            if (_atc != null)
            {
                IdleBlockSetting idleBlockSetting = new IdleBlockSetting();

                idleBlockSetting.setEnabled(Enabled);
                idleBlockSetting.setTemperature(Temperature);

                _atc.setIdleBlockSetting(idleBlockSetting);
            }
        }

        public InstrumentProperties GetInstrumentProperties()
        {
            return (_atc != null && _atc.isConnected()) ? _atc.getInstrumentProperties() : null;
        }

        public string GetAddress()
        {
            return address;
        }

        public RunProgress GetRunProgress()
        {
            return (_atc != null && _atc.isConnected()) ? _atc.getRunProgress() : null;
        }

        public InstrumentState GetInstrumentState()
        {
            return (_atc != null && _atc.isConnected()) ? _atc.getInstrumentState() : null;
        }
    }
}
