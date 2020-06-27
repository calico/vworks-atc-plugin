using System;
using static VworksAtcPlugin.VWorksXml;
using System.Reflection;
using IWorksDriver;
using System.Threading;

namespace VworksAtcPlugin
{
    public partial class ATCPlugin 
    {
        // Setup Logger        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Device Defaults:        
        enum _COMMANDS { OPEN, CLOSE, START, STOP, PAUSE, RESUME, SETTEMP };        
        private const string DEFAULT_PROTOCOL = "Protocol File - 1";
        private const string PROTOCOL_FILE_PATH = "Protocol File Path";
        private const string LID_TEMPERATURE = "Lid Temperature";
        private const string LID_TEMP_CONTROL_ENABLE = "Enable Lid Temp Control";
        private const string BLOCK_TEMPERATURE = "Block Temperature";
        private const string BLOCK_TEMP_CONTROL_ENABLE = "Enable Block Temp Control";
        private const string WAIT_FOR_TEMP_CONTROL = "Wait For Temperature";
        private const string IP_ADDRESS = "IP Address";
        private const string HOST_NAME = "Host Name";
        private const bool PLATE_IS_NOT_PRESENT = false;
        private const bool PLATE_IS_PRESENT = true;

        // Private Variables
        private Velocity11 _vworks;
        private InstrumentControl _atc;
        private CWorksController _IWorksController = null;
        private ATCDialog _DialogWindow;
        private ATCListener _DeviceListener;
        private string _LastError = "";
        private string _LastCommand = "";
        private bool _IsPlatePresent;
        private CancellationTokenSource _cancelToken;



        /// <summary>
        /// Device Constructor. Define MetaData, Commands, Device, and Version here.
        /// </summary>
        public ATCPlugin()
        {
            #region Define ATC
            _vworks = new Velocity11
            {
                MetaData = new MetaDataElement
                {
                    Device = new DeviceElement(),
                    Commands = new CommandElement[Enum.GetNames(typeof(_COMMANDS)).Length]
                }
            };

            // Define Device Commands
            var _commands = Enum.GetNames(typeof(_COMMANDS));
            for (int i = 0; i < _commands.Length; i++)
            {
                _vworks.MetaData.Commands[i] = new CommandElement
                {
                    Name = _commands[i],
                    Description = _commands[i] + " ATC",
                    ProtocolName = DEFAULT_PROTOCOL,
                    VisibleAvailability = (_commands[i] == "PAUSE" || _commands[i] == "RESUME") ? 0 : 1,
                    Editor = (int)(CommandElement.EditorValues.Editor_Primary),
                    TaskRequiresLocation = (int)CommandElement.TaskLocation.NotRequired,
                    RequiresRefresh = 1
                };
            }

            // Command Parameters for start
            _vworks.MetaData.Commands[(int)_COMMANDS.START].Parameters = new Parameter[1]
            {
                new Parameter
                {
                    Name = PROTOCOL_FILE_PATH,
                    Description = "File Path of ATC Protocol",
                    Type = (int)Parameter.TypeAttribute.UserSpecifyFilePath,
                    Scriptable = 1

                }
            };

            //Command Parameters for lid temp
            _vworks.MetaData.Commands[(int)_COMMANDS.SETTEMP].Parameters = new Parameter[5]
            {
                new Parameter
                {
                    Name = LID_TEMPERATURE,
                    Description = "Lid Temperature (C)",
                    Type = (int)Parameter.TypeAttribute.SpecifyDecimalFraction
                },

                new Parameter
                {
                    Name = LID_TEMP_CONTROL_ENABLE,
                    Description = "Enable Lid Temp Control",
                    Type = (int)Parameter.TypeAttribute.CheckBox
                },

                new Parameter
                {
                    Name = BLOCK_TEMPERATURE,
                    Description = "Block Temperature (C)",
                    Type = (int)Parameter.TypeAttribute.SpecifyDecimalFraction
                },

                new Parameter
                {
                    Name = BLOCK_TEMP_CONTROL_ENABLE,
                    Description = "Enable Block Temp Control",
                    Type = (int)Parameter.TypeAttribute.CheckBox
                },  
                new Parameter
                {
                    Name = WAIT_FOR_TEMP_CONTROL,
                    Description = "Wait For Enabled Temperatures",
                    Type = (int)Parameter.TypeAttribute.CheckBox
                }
            };

            // Plugin Version
            _vworks.MetaData.Versions = new VWorksXml.Version[1]
            {
                new VWorksXml.Version
                {
                    Author = "Travis Lee",
                    Company = "Calico",
                    Date = DateTime.Now.ToString("MM / dd / yyyy"),
                    VersionAttribute = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                    Name = "ATC Plugin"
                }
            };

            // Device Description
            _vworks.MetaData.Device = new DeviceElement
            {
                Description = "Thermo ATC",
                HardwareManufacturer = "Thermofisher",
                RegistryName = @"ATC\Profiles",
                Name = "ATC",
                HasBarcodeReader = 0,
                PreferredTab = "Other",
                Locations = new Location[1]
                {
                    new Location
                    {
                        Group = 0,
                        Name = "Nest",
                        Offset = 0,
                        Type = (int)Location.TypeAttribute.LabwareAllowed
                    }
                },
                Parameters = new Parameter[2]
                {
                    new Parameter
                    {
                        Description = IP_ADDRESS,
                        Name = IP_ADDRESS,
                        Type = (int)Parameter.TypeAttribute.UserSpecifyIPAddress
                    },
                    new Parameter
                    {
                        Description = HOST_NAME,
                        Name = HOST_NAME,
                        Type = (int)Parameter.TypeAttribute.CharacterString
                    },

                }                
            };
            #endregion

            // Initialize Private Variables
            _atc = new InstrumentControl();

            // This token is used to stop a task.
            _cancelToken = new CancellationTokenSource();
            _DeviceListener = new ATCListener(_cancelToken);

            // Define private fields
            _IsPlatePresent = PLATE_IS_NOT_PRESENT;

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var resName = args.Name + ".dll";
                var thisAssembly = Assembly.GetExecutingAssembly();
                using (var input = thisAssembly.GetManifestResourceStream(resName))
                {
                    return input != null
                         ? Assembly.Load(StreamToBytes(input))
                         : null;
                }
            };
        }       
    }
}
