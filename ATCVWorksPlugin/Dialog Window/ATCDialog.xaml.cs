using System.Windows;

namespace VworksAtcPlugin
{
    /// <summary>
    /// This is main dialog window for the ATC.
    /// </summary>
    public partial class ATCDialog : Window
    {
        // Actual API call to control instrument is encapsulated into InstrumentControl
        private InstrumentControl ic;
        private DialogMainViewModel _mainModel;

        // Constructor. Call this when Protocol editor is displayed as dialog instead of main window
        public ATCDialog()
        {
            InitializeComponent();

            // Bind Main View Model
            MainModel = new DialogMainViewModel();
            this.DataContext = MainModel;

            // Create new instrument control
            ic = new InstrumentControl();

            // By default select stage 1
            SetDefaultStageSelection();
        }

        internal DialogMainViewModel MainModel { get => _mainModel; set => _mainModel = value; }
    }
}
