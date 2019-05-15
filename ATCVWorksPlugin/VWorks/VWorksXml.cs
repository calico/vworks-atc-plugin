using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace VworksAtcPlugin
{
    /// <summary>
    /// VWorks Plugin Base Class.
    /// </summary>
    public class VWorksXml    
    {
        /// <summary>
        /// Velocity11 Model for constructing XML objects for VWorks.
        /// </summary>
        [Serializable]
        public class Velocity11
        {
            private readonly List<string> ValidFileAttributes = new List<string>() {
                "BarCodeMisreadResult",
                "BarCodeReadResult",
                "Device name",
                "JSSerialize",
                "MetaData",
                "Measurement",
                "Query",
                "QueryResponse"
            };

            private string _file;

            public Velocity11()
            {
                Version = "1.0";
                File = "MetaData";
            }

            /// <summary>
            /// Required: NO
            /// Default: MetaData.
            /// </summary>
            [XmlAttribute("file")]
            public string File
            {
                get { return _file; }
                set
                {
                    if (ValidFileAttributes.Contains(value))
                    {
                        _file = value;
                    }
                    else
                    {
                        var options = string.Join(", ", ValidFileAttributes.ToArray());
                        throw new ArgumentException(
                            $"Enter a valid tab option: {options}.");
                    }
                }
            }

            [XmlAttribute("version")]
            public string Version { get; set; }

            /// <summary>
            /// Required: NO
            /// </summaryx>
            [XmlAttribute]
            public string md5sum { get; set; }

            [XmlElement(IsNullable = true, ElementName = "MetaData")]
            public MetaDataElement MetaData { get; set; }

            public bool ShouldSerializeMetaData()
            {
                return (null != this.MetaData);
            }

            [XmlElement(IsNullable = true, ElementName = "Command")]
            public CommandElement Command { get; set; }

            public bool ShouldSerializeCommand()
            {
                return (null != this.Command);
            }

            [XmlArray(IsNullable = true)]
            public CompilerError [] CompilerErrors { get; set; }

            public bool ShouldSerializeCompilerErrors()
            {
                return (null != this.CompilerErrors);
            }
        }

        /// <summary>
        /// This class contains the structure for the version element. 
        /// </summary>
        [Serializable]
        public class Version
        {
            public Version()
            {
                Author = "Calico";
                Company = "Calico";
                Date = "1999";
                Name = "Calico Driver";
                VersionAttribute = "1.0";
            }

            [XmlAttribute]
            public string Author { get; set; }

            [XmlAttribute]
            public string Company { get; set; }

            [XmlAttribute]
            public string Date { get; set; }

            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute("Version")]
            public string VersionAttribute { get; set; }
        }

        #region CommandElement
        /// <summary>
        /// This class contains the structure for the command element. 
        /// A task in VWorks is the same things a command in the driver.
        /// </summary>
        [Serializable]
        public class CommandElement
        {
            // Default Compiler Attributes
            public enum CompilerAttributes
            {
                Compiler_No_Action = 0,
                Compiler_Disallow_Sealed_Plates = 1,
                Compiler_Disallow_Unseealed_Plates = 2,
                Compiler_Seals_Plate = 4,
                Compiler_Unseals_Plate = 8,
                Compiler_Disallow_Lidded_Plates = 16,
                Compiler_Disallow_Unlidded_Plates = 32,
                Compiler_Lids_Plate = 64,
                Compiler_Unlids_Plate = 128,
            }

            // Default Editor Values
            public enum EditorValues
            {
                Editor_None = 0,
                Editor_Hidden = 1,
                Editor_Primary = 2,
                Editor_Secondary = 4,
                Editor_PrePost = 8,
                Editor_OmniPresent = 16,
            }

            public enum TaskLocation
            {
                NotRequired = 0,
                Required = 1
            }

            private readonly List<string> ValidTabValues = new List<string>() {
                "IO Device Handling",
                "Plate Handling",
                "Plate Storage",
                "Liquid Handling",
                "Reading",
                "Other"
            };


            private string _PreferredTab;

            // Create Nullable Attributes
            private int? _Editor { get; set; }
            private int? _Compiler { get; set; }
            private int? _NextTaskToExecute { get; set; }
            private int? _RequiresRefresh { get; set; }
            private int? _TaskRequiresLocation { get; set; }
            private int? _VisibleAvailability { get; set; }

            /// <summary>
            /// Required: NO
            /// The Compiler attribute contains a bitmask that represents the actions that
            /// the task performs on the labware.
            /// To determine which value to use, perform a bitwise inclusive OR operation on
            /// the actions to be enabled for the task.
            /// </summary>
            [XmlAttribute]
            public int Compiler
            {
                get
                {
                    return _Compiler.Value;
                }
                set
                {
                    if (Enum.IsDefined(typeof(CompilerAttributes), value))
                    {
                        _Compiler = value;
                    }
                }
            }

            public bool ShouldSerializeCompiler()
            {
                return _Compiler.HasValue;
            }

            /// <summary>
            /// Required: NO.
            /// The Description attribute contains the description of the task.
            /// </summary>
            [XmlAttribute]
            public string Description { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string DisplayName { get; set; }

            /// <summary>
            /// Required: NO
            /// The Editor attribute contains a bitmask that represents the part of a protocol
            /// in which the task is available. 
            /// To determine the value to use, perform a bitwise inclusive OR operation on the
            /// options to be enabled.
            /// </summary>
            [XmlAttribute("Editor")]
            public int Editor
            {
                get { return _Editor.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(EditorValues), value))
                    {
                        _Editor = value;
                    }
                }
            }

            public bool ShouldSerializeEditor()
            {
                return _Editor.HasValue;
            }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Name { get; set; }

            /// <summary>
            /// Required: NO
            /// 0 means the task is not the next one to be executed.       
            /// 1 means the task is the next one to be exxecuted.        
            /// </summary>
            [XmlAttribute]
            public int NextTaskToExecute
            {
                get { return _NextTaskToExecute.Value; }
                set
                {
                    _NextTaskToExecute = value;
                }
            }

            public bool ShouldSerializeNextTaskToExecute()
            {
                return _NextTaskToExecute.HasValue;
            }

            /// <summary>
            /// Required: NO.
            /// If specified in Command element it overrides the
            /// value in the Device Element's PreferredTab attribute.
            /// </summary>
            [XmlAttribute]
            public string PreferredTab
            {
                get { return _PreferredTab; }
                set
                {
                    if (ValidTabValues.Contains(value))
                    {
                        _PreferredTab = value;
                    }
                    else
                    {
                        var options = string.Join(", ", ValidTabValues.ToArray());
                        throw new ArgumentException(
                            $"Enter a valid tab option: {options}.");
                    }
                }
            }

            /// <summary>
            /// Required: YES
            /// The name of the protocol that contains the task.
            /// </summary>
            [XmlAttribute]
            public string ProtocolName { get; set; }

            /// <summary>
            /// Required: NO
            /// Possible values:
            /// 0 = VWorks software should not request command metadata from the plugin
            /// 1 = VWorks software should request command metadata from the plugin
            /// </summary>
            [XmlAttribute]
            public int RequiresRefresh
            {
                get { return _RequiresRefresh.GetValueOrDefault(); }
                set { _RequiresRefresh = value; }
            }

            public bool ShouldSerializeRequireRefresh()
            {
                return _RequiresRefresh.HasValue;
            }

            /// <summary>
            /// Required: NO
            /// 0 = The task does not require a location
            /// 1 = The task requires a location
            /// </summary>
            [XmlAttribute]
            public int TaskRequiresLocation
            {
                get { return _TaskRequiresLocation.GetValueOrDefault(); }
                set { _TaskRequiresLocation = value; }
            }

            public bool ShouldSerializeTaskRequiresLocation()
            {
                return _TaskRequiresLocation.HasValue;
            }

            /// <summary>
            /// Required: NO.
            /// 0 = The task is not displayed in the Available Tasks area
            /// 1 = The task is displayed in the Available Tasks area
            /// </summary>
            [XmlAttribute]
            public int VisibleAvailability
            {
                get { return _VisibleAvailability.GetValueOrDefault(); }
                set { _VisibleAvailability = value; }
            }

            public bool ShouldSerializeVisibleAvailability()
            {
                return _VisibleAvailability.HasValue;
            }

            [XmlArray(IsNullable = true)]
            public Parameter[] Parameters { get; set; }

            public bool ShouldSerializeParameters()
            {
                return (null != this.Parameters);
            }

            [XmlArray(IsNullable = true)]
            public Value[] Locations { get; set; }

            public bool ShouldSerializeLocations()
            {
                return (null != this.Locations);
            }

        }

        [Serializable]
        public class Value
        {
            [XmlAttribute("Value")]
            public string LocationValue { get; set; }
        }
        #endregion

        #region DeviceElement
        /// <summary>
        /// The Device element has four children: Parameters, Locations,
        /// StorageDimensions, and RobotMetaData.
        /// </summary>
        [Serializable]
        public class DeviceElement
        {
            private readonly List<string> ValidTabValues = new List<string>() {
                "IO Device Handling",
                "Plate Handling",
                "Plate Storage",
                "Liquid Handling",
                "Reading",
                "Other"
            };

            public enum Barcode { NoReader = 0, HasReader = 1 };

            private int? _HasBarcodeReader { get; set; }
            private int? _MiscAttributes { get; set; }
            private string _PreferredTab { get; set; }

            /// <summary>
            /// Device description, which is displayed in Device File area.
            /// </summary>
            [XmlAttribute]
            public string Description { get; set; }

            /// <summary>
            /// Name of the Harware Manufacturer.
            /// </summary>
            [XmlAttribute]
            public string HardwareManufacturer { get; set; }

            /// <summary>
            /// 0 = NoReader, 1 = HasReader.
            /// </summary>
            [XmlAttribute]
            public int HasBarcodeReader
            {
                get { return _HasBarcodeReader.GetValueOrDefault(); }
                set
                {
                    if (Enum.IsDefined(typeof(Barcode), value))
                    {
                        _HasBarcodeReader = value;
                    }
                }
            }

            public bool ShouldSerializeHasBarcodeReader()
            {
                return _HasBarcodeReader.HasValue;
            }

            [XmlAttribute]
            public int MiscAttributes
            {
                get { return _MiscAttributes.GetValueOrDefault(); }
                set
                {
                    _MiscAttributes = value;
                }
            }

            public bool ShouldSerializeMiscAttributes()
            {
                return _MiscAttributes.HasValue;
            }

            /// <summary>
            /// Instance of Device, must be unique in VWorks.
            /// </summary>
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute]
            public string PreferredTab
            {
                get { return _PreferredTab; }
                set
                {
                    if (ValidTabValues.Contains(value))
                    {
                        _PreferredTab = value;
                    }
                    else
                    {
                        var options = string.Join(", ", ValidTabValues.ToArray());
                        throw new ArgumentException(
                            $"Enter a valid tab option: {options}.");
                    }
                }
            }

            /// <summary>
            /// Required: NO.
            /// </summary>
            [XmlAttribute]
            public string RegistryName { get; set; }

            /// <summary>
            /// Required: NO.
            /// </summary>
            [XmlElement(IsNullable = true)]
            public StorageDimensionsElement StorageDimensions { get; set; }

            public bool ShouldSerializeStorageDimensions()
            {
                return (null != this.StorageDimensions);
            }

            [XmlArray(IsNullable = true)]
            public Parameter[] Parameters { get; set; }

            public bool ShouldSerializeParameters()
            {
                return (null != this.Parameters);
            }

            [XmlArray(IsNullable = true)]
            public Location[] Locations { get; set; }

            public bool ShouldSerializeLocations()
            {
                return (null != this.Locations);
            }
        }
        #endregion

        #region StorageDimensions
        /// <summary>
        /// Child of the DeviceElement. The values of this class change depending on 
        /// device type (storage or non-storage). Storage devices require all attributes
        /// to be defined. Non-storage devices only require DirectStorageAccess = 0
        /// </summary>
        [Serializable]
        public class StorageDimensionsElement
        {
            public enum RobotAccess
            {
                ExternalStage = 0,
                IntenalStage = 1
            }

            public StorageDimensionsElement()
            {
                _DirectStorageAccess = (int)RobotAccess.ExternalStage;
            }

            private int? _DirectStorageAccess { get; set; }

            [XmlAttribute]
            public string Name0 { get; set; }

            [XmlAttribute]
            public string Name1 { get; set; }

            [XmlAttribute]
            public int DirectStorageAccess
            {
                get { return _DirectStorageAccess.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(RobotAccess), value))
                    {
                        _DirectStorageAccess = value;
                    }
                }
            }

            public bool ShouldSerializeDirectStorageAccess()
            {
                return _DirectStorageAccess.HasValue;
            }

            [XmlArray(IsNullable = true)]
            public StorageDimension[] Dimensions { get; set; }

            public bool ShouldSerializeDimensions()
            {
                return (null != this.Dimensions);
            }
        }
        #endregion

        #region StorageDimensionElement
        [Serializable]
        public class StorageDimension
        {
            private int? _Size { get; set; }

            public StorageDimension()
            {
                _Size = 1;
            }
            /// <summary>
            /// Required: YES
            /// </summary>
            [XmlAttribute]
            public int Size
            {
                get { return _Size.Value; }
                set { _Size = value; }
            }

            public bool ShouldSerializeSize()
            {
                return _Size.HasValue;
            }
        }
        #endregion

        #region RobotMetaDataElement
        [Serializable]
        public class RobotMetaDataElement
        {
            public enum RobotLocations
            {
                CannotReachExternalLocations = 0,
                CanReachExternalLocations = 1
            }

            private int? _ReachesExternalLocations { get; set; }

            [XmlAttribute]
            public int ReachesExternalLocations
            {
                get { return _ReachesExternalLocations.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(RobotLocations), value))
                    {
                        _ReachesExternalLocations = value;
                    }
                }
            }

            public bool ShouldSerializeReachesExternalLocations()
            {
                return _ReachesExternalLocations.HasValue;
            }
        }
        #endregion

        #region LocationElement
        [Serializable]
        public class Location
        {
            public enum GroupAttribute
            {
                NotExclusive,
                Group1,
                Group2,
                Group3,
                Group4,
                Group5,
                Group6,
                Group7,
                Group8,
                Group9,
                Group10
            }

            public enum TypeAttribute
            {
                NoLabware = 0,
                LabwareAllowed = 1,
                LabwareAllowedToBeStacked = 2,
                LabwareMoveInAndOutAllowed = 4,
                LabwareAllowedToBeIncubated = 8,
                LabwareAllowedToBeLidded = 16,
                LabwareAllowedToBeMovedIntoSystem = 32,
                LabwareAllowedToBeMovedOutOfSystem = 64,
                RobotAllowedToMoveLabwareIntoWaste = 128,
                LabareAllowedToBeMounted = 256,
                StaticLabwareCanBeAssigned = 512,
                OnlyCentrifugeLoadRobotCanUsePosition = 1024
            }

            private int? _Group { get; set; }
            private int? _Type { get; set; }
            private double? _MaxStackHeight { get; set; }
            private double? _Offset { get; set; }

            /// <summary>
            /// Required: NO
            /// The Group attribute is a bitmask that defines a location grouping for this
            /// device. Grouping creates mutually exclusive locations on a device, that is, only one
            /// labware can be at a location in the group at a time.To enable this behavior,
            /// the Group attribute must be set to a value other than 0.
            /// </summary>
            [XmlAttribute]
            public int Group
            {
                get
                {
                    return _Group.Value;
                }
                set
                {
                    if (Enum.IsDefined(typeof(GroupAttribute), value))
                    {
                        _Group = value;
                    }
                }
            }

            public bool ShouldSerializeGroup()
            {
                return _Group.HasValue;
            }

            /// <summary>
            /// Required: NO
            /// The MaxStackHeight attribute is the maximum height to which a stack of
            /// labware is allowed to grow on a device.
            /// </summary>
            [XmlAttribute]
            public double MaxStackHeight
            {
                get { return _MaxStackHeight.Value; }
                set { _MaxStackHeight = value; }
            }

            public bool ShouldSerializeMaxStackHeight()
            {
                return _MaxStackHeight.HasValue;
            }

            /// <summary>
            /// Required: YES
            /// </summary>
            [XmlAttribute]
            public string Name { get; set; }

            /// <summary>
            /// Required No
            /// </summary>
            [XmlAttribute]
            public double Offset
            {
                get { return _Offset.Value; }
                set { _Offset = value; }
            }

            public bool ShouldSerializeOffset()
            {
                return _Offset.HasValue;
            }

            /// <summary>
            /// Required: NO.
            /// The Type attribute is a bitmask that represents the type of access for the
            /// location. To determine the value to use, do a bitwise inclusive OR operation on all the
            /// access types for the location.
            /// </summary>
            [XmlAttribute]
            public int Type
            {
                get { return _Type.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(TypeAttribute), value))
                    {
                        _Type = value;
                    }
                }
            }

            public bool ShouldSerializeType()
            {
                return _Type.HasValue;
            }
        }
        #endregion

        #region MetaDataElement
        /// <summary>
        /// This class represents the Meta Data Element.
        /// The MetaData element has three children: Device, Versions, and
        /// Commands.The MetaData element has no attributes.
        /// </summary>
        [Serializable]
        public class MetaDataElement
        {
            [XmlArray(IsNullable = true)]
            public Version[] Versions { get; set; }

            public bool ShouldSerializeVersions()
            {
                return (null != this.Versions);
            }

            [XmlArray(IsNullable = true, ElementName = "Commands")]
            [XmlArrayItem("Command")]
            public CommandElement[] Commands { get; set; }

            public bool ShouldSerializeCommands()
            {
                return (null != this.Commands);
            }

            [XmlElement(IsNullable = true)]
            public DeviceElement Device { get; set; }

            public bool ShouldSerializeDevice()
            {
                return (null != this.Device);
            }
        }
        #endregion

        #region ParameterElement
        /// <summary>
        /// The Parameter element contains all information related to a single task
        /// parameter, including the following: 
        ///     Information needed by VWorks software to properly display the task
        ///     parameter in the protocol area
        ///     Information needed by the plugin to know the value specified by the user
        ///     for the parameter when executing the associated task
        /// </summary>
        public class Parameter
        {
            public enum StyleAttribute { ReadWrite, ReadOnly, Hidden };
            public enum TypeAttribute
            {
                CheckBox = 0,
                CharacterString = 1,
                DropDownListBox = 2,
                DropDownComboBox = 3,
                DeviceLocation = 4,
                LabwareOrLocation = 5,
                LabwareAndLocation = 6,
                OpenWellSelectionDialogBox = 7,
                UserSpecifyInteger = 8,
                UserSpecifyFilePath = 9,
                LabwareDropdownListBox = 10,
                LiquidClassDropDownListBox = 11,
                SpecifyDecimalFraction = 12,
                SpecifyFilePath = 13,
                UserSpecifyPassword = 14,
                UserSpecifyIPAddress = 15,
                UserSelectDirectory = 16,
                UserEnterTime = 17,
                ReferToJavaScriptObject = 18,
                EnterADate = 19,
                EnterCharacterStrings = 20,
                OpenPipetteTechniqueEditor = 21,
                OpenHeadSelectorMode = 22,
                DescribeTipPositions = 23,
                OpenFieldComposer = 24,
                DisplayHitPickFileFormats = 25,
                DeprecatedAnalogInputNames = 26,
                DeprecatedDigitalInputNames = 27,
                DeprecatedDigitalOutputNames = 28,
                ConvertAndAccessAsJavaScriptArray = 29,
                UserSpecifyDuration = 30,
                DisplayMultilineTextBox = 31,
                OpenColorPalette = 32
            };
            enum ScriptableAttribute
            {
                HideScriptVariableDialogBox = 0,
                ShowScriptVariableDialogBox = 1
            };

            private int? _Style { get; set; }
            private int _type;
            private int? _Scriptable { get; set; }
            private bool? _Hide_if { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Category { get; set; }

            /// <summary>
            /// Required: YES
            /// </summary>
            [XmlAttribute]
            public string Description { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public bool Hide_if
            {
                get { return _Hide_if.Value; }
                set { _Hide_if = value; }
            }

            public bool ShouldSerializeHide_if()
            {
                return _Hide_if.HasValue;
            }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Name { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Script { get; set; }

            /// <summary>
            /// Required: NO
            /// The Scriptable attribute indicates whether a Script Variable dialog box
            /// opens when the user selects the parameter in the Task Parameters area and
            /// then presses the = (equals)key. 
            /// The Scriptable attribute is only used for parameters where the value of the
            /// Type attribute is 19 or 30.       
            /// </summary>
            [XmlAttribute]
            public int Scriptable
            {
                get { return _Scriptable.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(ScriptableAttribute), value))
                    {
                        _Scriptable = value;
                    }
                }
            }

            public bool ShouldSerializeScriptable()
            {
                return _Scriptable.HasValue;
            }

            /// <summary>
            /// Required: NO
            /// The Style attribute represents how the parameter is rendered in the Task
            /// Parameters area.
            /// </summary>
            [XmlAttribute]
            public int Style
            {
                get
                {
                    return _Style.Value;
                }
                set
                {
                    if (Enum.IsDefined(typeof(StyleAttribute), value))
                    {
                        _Style = value;
                    }
                }
            }

            public bool ShouldSerializeStyle()
            {
                return _Style.HasValue;
            }

            /// <summary>
            /// Required:YES
            /// The Type attribute represents the type of the field in the Task Parameters
            /// area. 
            /// </summary>
            [XmlAttribute]
            public int Type
            {
                get
                {
                    return _type;
                }
                set
                {
                    if (Enum.IsDefined(typeof(TypeAttribute), value))
                    {
                        _type = value;
                    }
                }
            }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Units { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string Value { get; set; }

            /// <summary>
            /// Required: NO
            /// </summary>
            [XmlAttribute]
            public string ValueToDisplay { get; set; }

            [XmlArray(IsNullable = true)]
            public Range[] Ranges { get; set; }

            public bool ShouldSerializeRanges()
            {
                return (null != this.Ranges);
            }

        }
        #endregion

        #region RangeAndRangesElements
        [Serializable]
        public class Range
        {
            public Range()
            {
                Value = "value";
                ValueToDisplay = "value";
            }

            [XmlAttribute]
            public string Value { get; set; }

            [XmlAttribute]
            public string ValueToDisplay { get; set; }
        }
        #endregion

        #region CompileErrors
        [Serializable]
        public class CompilerError
        {
            enum TypeOfError { Error, Warning };
            private int? _errorType;

            /// <summary>
            /// Required: No.
            /// </summary>
            [XmlAttribute]
            public string Value { get; set; }

            /// <summary>
            /// Required: NO.
            /// </summary>
            [XmlAttribute]
            public int ErrorType
            {
                get { return _errorType.Value; }
                set
                {
                    if (Enum.IsDefined(typeof(TypeOfError), value))
                    {
                        _errorType = value;
                    }
                }
            }

            public bool ShouldSerializeErrorType()
            {
                return _errorType.HasValue;
            }
        }
        #endregion
    }
}