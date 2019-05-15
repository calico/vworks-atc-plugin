using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using com.apldbio.pcr.exception;
using com.apldbio.pcr.protocol;


namespace ATCVWorksPlugin
{
    /// <summary>
    /// This example shows how to create a simple protocol editor using remora API to
    /// - create protocol (add/remove stage/step)
    /// - validate protocol (and exception handling)
    /// - load and save protocol from/to XML files
    /// - export protocol to SiLA standard XML
    /// </summary>
    public partial class ATCDialog : Window
    {
        private WpfProtocolModel protocolModel;

        // Actual API call to control instrument is encapsulated into InstrumentControl
        private InstrumentControl ic;

        // Encapusulate the UI bound properties into a model
        public WpfDeviceControlModel DeviceControlModel { get; set; }


        // Default window constructor
        public ATCDialog() : this(new ProtocolModel())
        {
        }

        // Constructor. Call this when Protocol editor is displayed as dialog instead of main window
        public ATCDialog(ProtocolModel protocol)
        {
            InitializeComponent();

            // Put all the binding properties into MainWindowModel and set DataContext to the model object
            // All the direct API calls are coded within MainWindowModel class
            protocolModel = new WpfProtocolModel(protocol);
            this.DataContext = protocolModel;

            // Create new instrument control
            ic = new InstrumentControl();

            // By default select stage 1
            SetDefaultStageSelection();
        }        

    }
}
