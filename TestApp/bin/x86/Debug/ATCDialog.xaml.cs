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
        // Actual API call to control instrument is encapsulated into InstrumentControl
        private InstrumentControl ic;
        private DialogMainViewModel dialogModel;

       // Constructor. Call this when Protocol editor is displayed as dialog instead of main window
        public ATCDialog()
        {
            InitializeComponent();

            // Bind Main View Model
            dialogModel = new DialogMainViewModel();
            this.DataContext = dialogModel;

            // Create new instrument control
            ic = new InstrumentControl();

            // By default select stage 1
            SetDefaultStageSelection();
        }        

    }
}
