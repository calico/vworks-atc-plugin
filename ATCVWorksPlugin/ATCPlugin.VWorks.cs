using com.apldbio.pcr.exception;
using com.apldbio.pcr.instrument;
using com.apldbio.pcr.protocol;
using IWorksDriver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static VworksAtcPlugin.VWorksXml;

namespace VworksAtcPlugin
{
    public partial class ATCPlugin : IWorksDriver.IWorksDriver, CControllerClient, IWorksDiags, IDisposable
    {
        #region IWorksDriver Members

        public void Abort(string ErrorContext)
        {
            try
            {
                _cancelToken.Cancel();
                _atc.AbortRun();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
            }
        }

        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// VWorks software calls the Command method to tell the plugin to execute the
        /// specified task. IMPORTANT Plugins must implement the Command method if the device has 
        /// any associated tasks. The plugin should not return until the task is completed.
        /// </summary>
        /// <param name="CommandXML"></param>
        /// <returns></returns>
        public ReturnCode Command(string CommandXML)
        {
            _LastCommand = CommandXML;

            try
            {
                var VworksCommand = CommandXML.XmlDeserializeFromString<Velocity11>();               
                var commandsToExecute = new List<Task>();

                switch (VworksCommand.Command.Name)
                {
                    case nameof(_COMMANDS.CLOSE):
                        commandsToExecute.Add(Task.Factory.StartNew(() => _atc.CloseLid()));
                        break;

                    case nameof(_COMMANDS.OPEN):
                        if(_atc.GetInstrumentState() == InstrumentState.IDLE || 
                            _atc.GetInstrumentState() == InstrumentState.STANDBY)
                        {
                            commandsToExecute.Add(Task.Factory.StartNew(() => _atc.OpenLid()));
                            break;
                        }
                        else
                        {
                            _LastError = "Error instrument busy.";
                            return ReturnCode.RETURN_FAIL;
                        }                        

                    case nameof(_COMMANDS.PAUSE):
                        commandsToExecute.Add(Task.Factory.StartNew(() => _atc.PauseRun()));
                        break;

                    case nameof(_COMMANDS.RESUME):
                        commandsToExecute.Add(Task.Factory.StartNew(() => _atc.ResumeRun()));
                        break;

                    case nameof(_COMMANDS.START):
                        if(_atc.GetInstrumentState() == InstrumentState.IDLE ||
                            _atc.GetInstrumentState() == InstrumentState.STANDBY)
                        {
                            foreach (var param in VworksCommand.Command.Parameters)
                            {
                                if (param.Name == PROTOCOL_FILE_PATH && File.Exists(param.Value))
                                {
                                    // Reset Token and ATC Listener 
                                    CreateTokenAndListener();

                                    // Get Protocol
                                    RunProtocol protocol = ProtocolUtil.load(new java.io.File(param.Value));

                                    // Execute Run
                                    commandsToExecute.Add(Task.Factory.StartNew(() =>
                                    {
                                        try
                                        {
                                            _atc.StartRun(protocol);
                                        }
                                        catch (PCRException pcrEx)
                                        {
                                            _LastError = PCRExceptionFormatter.GetDetailFailureMessage(pcrEx);
                                            log.Error("ATC run error");
                                            log.Error(_LastError);
                                            throw new Exception(_LastError);
                                        }
                                    }).ContinueWith((prevTask) => WaitForRunToComplete(_cancelToken.Token)));

                                    // Update VWorks log
                                    log.Info("Protocol run started.");
                                    break;
                                }
                                else
                                {
                                    _LastError = "Invalid protocol file or path.";
                                    return ReturnCode.RETURN_BAD_ARGS;
                                }
                            }
                        }
                        else
                        {
                            _LastError = "Instrument Busy";
                            return ReturnCode.RETURN_FAIL;
                        }
                        break;                
                }

                // Wait for command to finish.
                Task.WaitAll(commandsToExecute.ToArray());

                // Delay 3 seconds to make sure command completes.
                Thread.Sleep(3000);

                return ReturnCode.RETURN_SUCCESS;
            }
            catch (AggregateException ax)
            {
                log.Info(CommandXML);
                _LastError = ax.InnerException.Message;
                log.Error(_LastError);
                return ReturnCode.RETURN_FAIL;
            }
            catch(Exception ex)
            {
                _LastError = ex.Message;
                log.Error(_LastError);
                return ReturnCode.RETURN_FAIL;
            }            
        }        

        public string Compile(IWorksDriver.CompileType iCompileType, string MetaDataXML)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// VWorks software uses the ControllerQuery method in conjunction with the
        /// IWorksController Query method to provide the means for two plugins to
        /// communicate with each other.
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public string ControllerQuery(string Query)
        {
            log.Info($"Received Query: {Query}");
            throw new NotImplementedException();
        }

        public stdole.IPictureDisp Get32x32Bitmap(string CommandName)
        {
            return AxHostConverter.ImageToPictureDisp(Properties.Resources.atc);
        }

        /// <summary>
        /// VWorks software calls the GetDescription method to get the description for
        /// the specified task from the plugin.Depending on the value of the Verbose
        /// parameter, the plugin returns one of following:
        /// • A short description of the task to display in the protocol editor under the
        /// icon associated with the task
        /// • A full, dynamic description of the task to enter in the Main Log
        /// </summary>
        /// <param name="CommandXML"></param>
        /// <param name="Verbose"></param>
        /// <returns></returns>
        public string GetDescription(string CommandXML, bool Verbose)
        {
            try
            {
                var command = CommandXML.XmlDeserializeFromString<Velocity11>();
                switch (command.Command.Name)
                {
                    case nameof(_COMMANDS.CLOSE):
                        return "Close ATC Lid";                        
                    case nameof(_COMMANDS.OPEN):
                        return "Open ATC Lid";
                    case nameof(_COMMANDS.PAUSE):
                        return "Pause protocol run";
                    case nameof(_COMMANDS.RESUME):
                        return "Resume protocol run";
                    case nameof(_COMMANDS.START):
                        return "Start a protocol";
                    case nameof(_COMMANDS.STOP):
                        return "Stop a protocol";
                    default:
                        return "ATC desc";
                }
            }
            catch
            {
                return "Error";
            }            
        }

        public string GetErrorInfo()
        {
            return _LastError;
        }

        public stdole.IPictureDisp GetLayoutBitmap(string LayoutInfoXML)
        {
            throw new NotImplementedException();
        }

        public string GetMetaData(IWorksDriver.MetaDataType iDataType, string current_metadata)
        {            
            if(current_metadata != "")
            {
                CheckAndUpdateMetaData(current_metadata);
            }
            
            switch (iDataType)
            { 
                case MetaDataType.METADATA_ALL:
                    return _vworks.SerializeObject();

                case MetaDataType.METADATA_COMMAND:
                    var commands = new Velocity11
                    {
                        MetaData = new MetaDataElement
                        {
                            Commands = _vworks.MetaData.Commands
                        }
                    };                    
                    return commands.SerializeObject();

                case MetaDataType.METADATA_DEVICE:
                    var device = new Velocity11
                    {
                        MetaData = new MetaDataElement
                        {
                            Device = _vworks.MetaData.Device
                        }
                    };                    
                    return device.SerializeObject();

                case MetaDataType.METADATA_VERSION:
                    var versions = new Velocity11
                    {
                        MetaData = new MetaDataElement
                        {
                            Versions = _vworks.MetaData.Versions
                        }
                    };
                    return versions.SerializeObject();

                default:
                    return _vworks.SerializeObject();
            }            
        }

        public ReturnCode Ignore(string ErrorContext)
        {
            throw new NotImplementedException();
        }

        public ReturnCode Initialize(string CommandXML)
        {
            log.Info("Initializing...");
            try
            {
                if (null == _atc.GetAddress())
                {
                    _LastError = "The ATC needs a host name or IP address to connect.";
                    return ReturnCode.RETURN_BAD_ARGS;
                }

                var mode = (IPAddress.TryParse(_atc.GetAddress(), out IPAddress address) && (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)) ?
                    InstrumentControl.ConnectionMode.IPV4 : InstrumentControl.ConnectionMode.HOST_NAME;

                log.Info(mode.ToString() + " : " +  _atc.address);

                var connectTask = Task.Run(() =>
                {
                    try
                    {
                        _atc.Connect(_atc.address, mode, _DeviceListener);
                    }
                    catch(PCRException pcrEx)
                    {
                        throw new Exception(PCRExceptionFormatter.GetDetailFailureMessage(pcrEx));
                    }                    
                });

                // Wait for up to 5 seconds.
                if (connectTask.Wait(5000))
                {
                    if (_atc.IsConnected())
                    {
                        log.Info("Connected to ATC.");
                        log.Info($"ATC: {_atc.GetInstrumentState().ToString()}");
                        return ReturnCode.RETURN_SUCCESS;
                    }
                    else
                    {
                        _LastError = "ATC failed to connect.";
                        return ReturnCode.RETURN_FAIL;
                    }
                }                                        
                else
                {
                    _LastError = "ATC connection timed out.";
                    return ReturnCode.RETURN_FAIL;
                }                                
            }
            catch (Exception e)
            {
                _LastError = e.Message;
                log.Error(e.Message);
                return ReturnCode.RETURN_FAIL;
            }
        }

        public bool IsLocationAvailable(string LocationAvailableXML)
        {               
            return true;
        }

        public ReturnCode MakeLocationAvailable(string LocationAvailableXML)
        {
            log.Info("Make Location Available");
            if(_atc.GetInstrumentState() == InstrumentState.IDLE || _atc.GetInstrumentState() == InstrumentState.STANDBY)
            {
                Task open = Task.Factory.StartNew(() => _atc.OpenLid());
                Task.WaitAny(open);
                Thread.Sleep(1000);
                return ReturnCode.RETURN_SUCCESS;
            }
            else
            {
                _LastError = "Instrument is running, errored, or in diagnostics.";
                return ReturnCode.RETURN_FAIL;
            }            
        }

        public ReturnCode PlateDroppedOff(string PlateInfoXML)
        {            
            try
            {
                log.Info("Plate dropped off.");
                _IsPlatePresent = true;
                return ReturnCode.RETURN_SUCCESS;
            }
            catch (Exception e)
            {
                _LastError = e.Message;
                return ReturnCode.RETURN_FAIL;
            }
        }

        public ReturnCode PlatePickedUp(string PlateInfoXML)
        {
            try
            {
                log.Info("PlatePickedUp");
                _IsPlatePresent = false;
                return ReturnCode.RETURN_SUCCESS;
            }
            catch (Exception e)
            {               
                _LastError = e.Message;
                log.Error(_LastError);
                return ReturnCode.RETURN_FAIL;
            }
        }

        public void PlateTransferAborted(string PlateInfoXML)
        {
            log.Info($"PlateTransferAborted, IsPlatePresent:{_IsPlatePresent}");                       
        }

        public ReturnCode PrepareForRun(string LocationInfoXML)
        {
            try
            {
                return ReturnCode.RETURN_SUCCESS;
            }
            catch(Exception e)
            {
                _LastError = e.Message;
                return ReturnCode.RETURN_FAIL;
            }
        }

        public ReturnCode Retry(string ErrorContext)
        {            
            try
            {
                log.Info("Executing Retry.");
                log.Error(ErrorContext);
                return Command(_LastCommand);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return ReturnCode.RETURN_FAIL;
            }            
        }

        /// <summary>
        /// VWorks should not call this function. Implement as E_NOTIMPL
        /// </summary>
        /// <param name="iSecurity"></param>
        public void ShowDiagsDialog(IWorksDriver.SecurityLevel iSecurity)
        {
            switch (iSecurity)
            {
                case SecurityLevel.SECURITY_LEVEL_NO_ACCESS:
                    MessageBox.Show("Insufficient Privileges");
                    break;
                case SecurityLevel.SECURITY_LEVEL_ADMINISTRATOR:
                case SecurityLevel.SECURITY_LEVEL_GUEST:
                case SecurityLevel.SECURITY_LEVEL_OPERATOR:
                case SecurityLevel.SECURITY_LEVEL_TECHNICIAN:
                    _DialogWindow = new ATCDialog();
                    _DialogWindow.Closing += WindowClosing;
                    _DialogWindow.ShowDialog();                    
                    break;
            }
        }

        /// <summary>
        /// Checks if there are any changes to meta data object and if so updates
        /// MetaData object. 
        /// </summary>
        /// <param name="current_data"></param>
        public void CheckAndUpdateMetaData(string current_data)
        {            
            try
            {
                var data = current_data.XmlDeserializeFromString<Velocity11>();

                switch (data.Command.Name)
                {
                    case "DeviceData":
                        UpdateDeviceData(data);
                        break;
                    case "CommandData":
                    // TO DO: Update Command Data
                    default:
                        break;
                }
            }
            catch(Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        ///  Update Device Data from Vworks.
        /// </summary>
        /// <param name="data"></param>
        public void UpdateDeviceData(Velocity11 data)
        {
            for (int i = 0; i < data.Command.Parameters.Length; i++)
            {
                if (null != data.Command.Parameters[i].Value)
                {
                    if (data.Command.Parameters[i].Name == IP_ADDRESS || data.Command.Parameters[i].Name == HOST_NAME)
                    {
                        // update atc address
                        _atc.address = data.Command.Parameters[i].Value;
                        log.Info($"atc address: {_atc.address}");

                        // update device in memory
                        for (int j = 0; j < _vworks.MetaData.Device.Parameters.Length; j++)
                        {                            
                            if(data.Command.Parameters[i].Name == _vworks.MetaData.Device.Parameters[j].Name)
                            {
                                _vworks.MetaData.Device.Parameters[j].Value = data.Command.Parameters[i].Value;                                
                            }
                        }
                    }
                }
            }
            log.Info("Updated device meta data");
        }
        #endregion

        #region IControllerClient Members
        public void SetController(CWorksController Controller)
        {            
            this._IWorksController = (CWorksControllerClass)Controller;
        }

        #endregion

        #region IWorksDiags
        public void ShowDiagsDialog(SecurityLevel iSecurity, bool bModal)
        {
            if (bModal)
            {
                ShowDiagsDialog(iSecurity);               
            }
            else
            {
                switch (iSecurity)
                {
                    case SecurityLevel.SECURITY_LEVEL_NO_ACCESS:
                        MessageBox.Show("Insufficient Privileges");
                        break;
                    case SecurityLevel.SECURITY_LEVEL_ADMINISTRATOR:
                    case SecurityLevel.SECURITY_LEVEL_GUEST:
                    case SecurityLevel.SECURITY_LEVEL_OPERATOR:
                    case SecurityLevel.SECURITY_LEVEL_TECHNICIAN:
                        _DialogWindow = new ATCDialog();
                        _DialogWindow.Closing += WindowClosing;                        
                        _DialogWindow.Show();
                        break;
                }
            }                                    
        }

        void WindowClosing(object sender, EventArgs e)
        {
            this._IWorksController.OnCloseDiagsDialog(this);
        }         

        public ReturnCode CloseDiagsDialog()
        {
            try
            {
                _DialogWindow.Close();
                return ReturnCode.RETURN_SUCCESS;
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                return ReturnCode.RETURN_FAIL;
            }
        }

        /// <summary>
        /// VWorks should not call this function. Implement as E_NOTIMPL
        /// </summary>
        public ReturnCode IsDiagsDialogOpen()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IDispose Methods
        public void Dispose()
        {
            try
            {
                _cancelToken.Cancel();
                _atc.Disconnect();
            }
            catch(Exception e)
            {
                log.Error("DISPOSE: " + e.ToString());
            }            
        }        
        #endregion
    }
}
