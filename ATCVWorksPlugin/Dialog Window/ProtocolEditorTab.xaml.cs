using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using com.apldbio.pcr.exception;
using com.apldbio.pcr.protocol;

namespace VworksAtcPlugin
{
    public partial class ATCDialog
    {
        // Load button click handler: display open file dialog and allow user to select XML file
        private void btnImportFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "XML file (*.xml;*.XML)|*.xml;*.XML";
                if (openFileDialog.ShowDialog() == true)
                {
                    MainModel.ProtocolTab.ImportProtocol(openFileDialog.FileName);
                    SetDefaultStageSelection();
                }
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Save button click handler - validate protocol and save to XML file
        private void btnExportFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainModel.ProtocolTab.ValidateProtocol();
                if (SaveProtocolFile())
                {
                    MessageBox.Show("Protocol saved successfully");
                }
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Method to save protocol to local file - Open save file dialog and let user specify File path/name
        private Boolean SaveProtocolFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml;*.XML)|*.xml;*.XML";
            if (!ProtocolFactory.GetDefaultProtocolName().Equals(MainModel.ProtocolTab.ProtocolModel.Path))
            {
                saveFileDialog.InitialDirectory = MainModel.ProtocolTab.ProtocolModel.Path;
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                MainModel.ProtocolTab.ExportProtocol(saveFileDialog.FileName);
                return true;
            }
            return false;
        }

        // Add stage click handler - Duplicate and append to user selected stage. 
        // If no stage is selected, apend default stage to end of protocol
        private void btnAddStage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = ProtocolStages.SelectedItem;
                if (item != null)
                {
                    // Add stage after the selected item
                    int index = ProtocolStages.SelectedIndex;
                    if (MainModel.ProtocolTab.AddStageAt(index + 1, item as StageModel))
                    {
                        // Select the new stage
                        ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[index + 1];
                    }
                }
                else
                {
                    // If no item selected, append to the last
                    StageModel lastItem = null;
                    if (MainModel.ProtocolTab.Stages.Count > 0)
                    {
                        lastItem = MainModel.ProtocolTab.Stages.Last<StageModel>();
                    }
                    int index = MainModel.ProtocolTab.Stages.Count;
                    if (MainModel.ProtocolTab.AddStageAt(MainModel.ProtocolTab.Stages.Count, lastItem))
                    {
                        // Select the new stage
                        ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[index];
                    }
                }

                // Call this to have the row index refreshed. If row index is not applied, this should be avoided
                ProtocolStages.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Delete stage click handler - Delete user selected stage
        private void btnDeleteStage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolStages.SelectedIndex;
                var item = ProtocolStages.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.DeleteStageAt(index))
                    {
                        if (index > 0 && MainModel.ProtocolTab.Stages.Count > 0)
                        {
                            ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[index - 1];
                        }
                        else
                        {
                            // There is no more stage. Clear step table
                            MainModel.ProtocolTab.UpdateStageSelection(null);
                        }
                    }
                }

                ProtocolStages.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Move up stage click handler - Move user selected stage up
        private void btnMoveUpStage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolStages.SelectedIndex;
                var item = ProtocolStages.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.MoveStageUp(index))
                    {
                        if (index > 0)
                        {
                            ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[index - 1];
                        }
                    }
                }

                ProtocolStages.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Move down stage click handler - Move user selected stage down
        private void btnMoveDownStage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolStages.SelectedIndex;
                var item = ProtocolStages.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.MoveStageDown(index))
                    {
                        if (index + 1 < MainModel.ProtocolTab.Stages.Count)
                        {
                            ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[index + 1];
                        }
                    }
                }

                ProtocolStages.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Add step click handler - Duplicate and append to user selected step
        private void btnAddStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = ProtocolSteps.SelectedItem;
                if (item != null)
                {
                    // Add stage after the selected item
                    int index = ProtocolSteps.SelectedIndex;
                    if (MainModel.ProtocolTab.AddStepAt(index + 1, item as StepModel))
                    {
                        ProtocolSteps.SelectedItem = MainModel.ProtocolTab.SelectedStage.Steps[index + 1];
                    }
                }
                else
                {
                    // If no item selected, append to the last
                    StepModel lastItem = null;
                    if (MainModel.ProtocolTab.SelectedStage != null)
                    {
                        if (MainModel.ProtocolTab.SelectedStage.Steps.Count > 0)
                        {
                            lastItem = MainModel.ProtocolTab.SelectedStage.Steps.Last<StepModel>();
                        }
                        int index = MainModel.ProtocolTab.SelectedStage.Steps.Count;
                        if (MainModel.ProtocolTab.AddStepAt(MainModel.ProtocolTab.SelectedStage.Steps.Count, lastItem))
                        {
                            ProtocolSteps.SelectedItem = MainModel.ProtocolTab.SelectedStage.Steps[index];
                        }
                    }

                }

                ProtocolSteps.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Delete step click handler - Delete user selecte step
        private void btnDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolSteps.SelectedIndex;
                var item = ProtocolSteps.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.DeleteStepAt(index))
                    {
                        if (index > 0 && MainModel.ProtocolTab.SelectedStage != null && MainModel.ProtocolTab.SelectedStage.Steps.Count > 0)
                        {
                            ProtocolSteps.SelectedItem = MainModel.ProtocolTab.SelectedStage.Steps[index - 1];
                        }
                    }
                }

                ProtocolSteps.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Move up step click handler - Move user selected step up
        private void btnMoveUpStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolSteps.SelectedIndex;
                var item = ProtocolSteps.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.MoveStepUp(index))
                    {
                        if (index > 0 && MainModel.ProtocolTab.SelectedStage != null)
                        {
                            ProtocolSteps.SelectedItem = MainModel.ProtocolTab.SelectedStage.Steps[index - 1];
                        }
                    }
                }

                ProtocolSteps.Items.Refresh();

            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Move down step click handler - Move user selected step down
        private void btnMoveDownStep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ProtocolSteps.SelectedIndex;
                var item = ProtocolSteps.SelectedItem;
                if (item != null)
                {
                    if (MainModel.ProtocolTab.MoveStepDown(index))
                    {
                        if (MainModel.ProtocolTab.SelectedStage != null && index + 1 < MainModel.ProtocolTab.SelectedStage.Steps.Count)
                        {
                            ProtocolSteps.SelectedItem = MainModel.ProtocolTab.SelectedStage.Steps[index + 1];
                        }
                    }
                }

                ProtocolSteps.Items.Refresh();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Event handler for stage table selection change - update the selected stage so as to refresh step table
        private void protocolStages_Selected(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as DataGrid).SelectedItem;
            if (item != null && MainModel.ProtocolTab.SelectedStage != item)
            {
                MainModel.ProtocolTab.UpdateStageSelection(item as StageModel);
            }
        }

        // Method to select first row in stage table by defaul
        private void SetDefaultStageSelection()
        {
            if (MainModel.ProtocolTab.Stages.Count > 0)
            {
                ProtocolStages.SelectedItem = MainModel.ProtocolTab.Stages[0];
            }
        }

        // Make use of DataGrid LoadingRow event to display row index. Must call "DataGrid.Refresh" after adding/removing row 
        private void ProtocolGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        // Export SiLA click handler - validate protocol and show save file dialog to allow user to select saved destination
        // Call SiLAExporter to save the protocol into SiLA format
        private void btnExportSiLA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainModel.ProtocolTab.ValidateProtocol();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML file (*.xml;*.XML)|*.xml;*.XML";
                if (saveFileDialog.ShowDialog() == true)
                {
                    (new SiLAExporter()).SaveToLocal(MainModel.ProtocolTab.ProtocolModel.Tcprotocol, saveFileDialog.FileName);
                    MessageBox.Show("SiLA Protocol exported successfully");
                }
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (SiLAExportException se)
            {
                MessageBox.Show("Unable to export SiLA: " + se.GetMessage());
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }

        // Done button click handler - Ask user to save protocol if it's modified.
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainModel.ProtocolTab.IsProtocolModified())
                {
                    String message = String.Format("You have unsaved changes.\nDo you want to save it?",
                        MainModel.ProtocolTab.ProtocolModel.DisplayName);
                    MessageBoxResult messageBoxResult = MessageBox.Show(message, "Unsaved protocol", MessageBoxButton.YesNoCancel);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        MainModel.ProtocolTab.ValidateProtocol();
                        if (!SaveProtocolFile())
                        {
                            return;
                        }
                    }
                    else if (messageBoxResult == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                Close();
            }
            catch (PCRException pe)
            {
                MessageBox.Show(PCRExceptionFormatter.GetDetailFailureMessage(pe));
            }
            catch (Exception ge)
            {
                MessageBox.Show(ge.ToString());
            }
        }
    }
}
