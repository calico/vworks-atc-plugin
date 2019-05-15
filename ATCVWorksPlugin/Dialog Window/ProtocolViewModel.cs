using com.apldbio.pcr.protocol;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VworksAtcPlugin
{
    class ProtocolViewModel : INotifyPropertyChanged
    {
        // Setup Logger
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ProtocolModel protocolModel;            // Protocol model to encapsulate cover temperature, block temperature etc
        private RunProtocol originRunProtocol;          // Back up RunProtocol for comparison and revert
        private ObservableCollection<StageModel> stages;    // Stage collection dynamically bound to stage table
        private StageModel selectedStage;               // Selected stage. sub-properties step collection dynamically bound to step table.

        // Define some properties as dynamic binding
        public ProtocolModel ProtocolModel
        {
            get { return protocolModel; }
            set { protocolModel = value; NotifyPropertyChanged("ProtocolModel"); }
        }
        public ObservableCollection<StageModel> Stages
        {
            get { return stages; }
            set { stages = value; NotifyPropertyChanged("Stages"); }
        }
        public StageModel SelectedStage
        {
            get { return selectedStage; }
            set { selectedStage = value; NotifyPropertyChanged("SelectedStage"); }
        }
        public ObservableCollection<RunMode> ProtocolRunModes { get; set; }

        public ProtocolViewModel(ProtocolModel protocolModel)
        {
            ProtocolModel = protocolModel;
            BackupRunProtocol(ProtocolModel.Tcprotocol);
            Stages = AbstractStageModels(ProtocolModel.Tcprotocol.getStages());
            ProtocolRunModes = new ObservableCollection<RunMode>()
            {
                RunMode.FAST, RunMode.STANDARD
            };
        }

        private void BackupRunProtocol(RunProtocol backup)
        {
            originRunProtocol = new RunProtocol(backup);
        }

        private void SetProtocolModel(RunProtocol protocol, String displayName, String path)
        {
            BackupRunProtocol(protocol);
            ProtocolModel.Tcprotocol = protocol;
            ProtocolModel.DisplayName = displayName;
            ProtocolModel.Path = path;
            Stages = AbstractStageModels(ProtocolModel.Tcprotocol.getStages());
        }

        public void UpdateStageSelection(StageModel selectedStage)
        {
            SelectedStage = selectedStage;
        }

        public void InsertStage(int index, StageModel stageModel)
        {
            Stages.Insert(index, stageModel);
            ProtocolModel.InsertStage(index, stageModel);
        }

        public void RemoveStage(int index)
        {
            Stages.RemoveAt(index);
            ProtocolModel.RemoveStage(index);
        }

        private ObservableCollection<StageModel> AbstractStageModels(java.util.List stages)
        {
            ObservableCollection<StageModel> stageModels = new ObservableCollection<StageModel>();
            for (int i = 0; i < stages.size(); i++)
            {
                stageModels.Add(new StageModel(stages.get(i) as Stage));
            }

            return stageModels;
        }

        public RunProtocol GetRunProtocol()
        {
            return ProtocolModel.Tcprotocol;
        }

        public bool AddStageAt(int index, StageModel predecessor)
        {
            try
            {
                InsertStage(index,
                    predecessor != null ? predecessor.Clone() : new StageModel("New Stage", 1));
                return true;
            }
            catch (Exception e)
            {
                log.Error(string.Format($"Failed to add stage {0}. /r/n {e.Message}", index));
            }

            return false;
        }

        public bool DeleteStageAt(int index)
        {
            if (index >= 0 && index < Stages.Count)
            {
                try
                {
                    RemoveStage(index);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove stage {0}. /r/n {e.Message}", index));                    
                }
            }

            return false;
        }

        public bool MoveStageUp(int index)
        {
            if (index > 0)
            {
                try
                {
                    StageModel temp = Stages[index];
                    RemoveStage(index);
                    InsertStage(index - 1, temp);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove stage {0}. /r/n {e.Message}", index));
                }
            }

            return false;
        }

        public bool MoveStageDown(int index)
        {
            if (index < Stages.Count - 1)
            {
                try
                {
                    StageModel temp = Stages[index];
                    RemoveStage(index);
                    InsertStage(index + 1, temp);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove stage {0}. /r/n {e.Message}", index));
                }
            }

            return false;
        }

        public bool AddStepAt(int index, StepModel predecessor)
        {
            try
            {
                SelectedStage.InsertStep(index,
                    predecessor != null ? predecessor.Clone() : new StepModel(90.0f, 10, 100f, RampRateUnit.PERCENTAGE));
                return true;
            }
            catch (Exception e)
            {
                log.Error(string.Format($"Failed to add step {0}. /r/n {e.Message}", index));
            }

            return false;
        }

        public bool DeleteStepAt(int index)
        {
            if (index >= 0 && index < SelectedStage.Steps.Count)
            {
                try
                {
                    SelectedStage.RemoveStep(index);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove step {0}. /r/n {e.Message}", index));
                }
            }

            return false;
        }

        public bool MoveStepUp(int index)
        {
            if (index > 0)
            {
                try
                {
                    StepModel temp = SelectedStage.Steps[index];
                    SelectedStage.RemoveStep(index);
                    SelectedStage.InsertStep(index - 1, temp);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove step {0}. /r/n {e.Message}", index));
                }
            }

            return false;
        }

        public bool MoveStepDown(int index)
        {
            if (index < SelectedStage.Steps.Count - 1)
            {
                try
                {
                    StepModel temp = SelectedStage.Steps[index];
                    SelectedStage.RemoveStep(index);
                    SelectedStage.InsertStep(index + 1, temp);
                    return true;
                }
                catch (Exception e)
                {
                    log.Error(string.Format($"Failed to remove step {0}. /r/n {e.Message}", index));
                }
            }

            return false;
        }

        public void ValidateProtocol()
        {
            // Call ProtocolUtil API to validate protocol
            ProtocolUtil.validate(ProtocolModel.Tcprotocol);
        }

        public bool IsProtocolModified()
        {
            return !this.originRunProtocol.equals(ProtocolModel.Tcprotocol);
        }

        private string GetShortFileName(string fileName)
        {
            return Path.GetFileName(fileName);
        }

        public void ImportProtocol(string fileName)
        {
            // Call ProtocolUtil  API to load protocol xml file
            RunProtocol protocol = ProtocolUtil.load(new java.io.File(fileName));
            SetProtocolModel(protocol, GetShortFileName(fileName), fileName);
        }

        public void ExportProtocol(string fileName)
        {
            java.io.File file = null;
            file = new java.io.File(fileName);
            if (!file.exists())
            {
                file.createNewFile();
            }
            if (file.canWrite())
            {
                // Call ProtocolUtil API to save protocol xml file
                ProtocolUtil.save(ProtocolModel.Tcprotocol, file);

                BackupRunProtocol(ProtocolModel.Tcprotocol);

                ProtocolModel.DisplayName = GetShortFileName(fileName);
                ProtocolModel.Path = fileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public class ProtocolModel : INotifyPropertyChanged
    {
        private String displayName;
        private String path;
        private RunProtocol tcprotocol;

        public String DisplayName
        {
            get { return displayName; }
            set { displayName = value; NotifyPropertyChanged("DisplayName"); }
        }
        public String Path
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged("Path"); }
        }
        public double CoverTemperature
        {
            get { return tcprotocol.getCoverTemperature(); }
            set { tcprotocol.setCoverTemperature(value); NotifyPropertyChanged("CoverTemperature"); }
        }
        public double SampleVolume
        {
            get { return tcprotocol.getSampleVolume(); }
            set { tcprotocol.setSampleVolume(value); NotifyPropertyChanged("SampleVolume"); }
        }
        public RunMode ProtocolRunMode
        {
            get
            {
                return tcprotocol.getRunMode();
            }
            set
            {
                tcprotocol.setRunMode(value);
                NotifyPropertyChanged("ProtocolRunMode");
            }
        }
        public RunProtocol Tcprotocol
        {
            get { return tcprotocol; }
            set
            {
                tcprotocol = value;
                NotifyPropertyChanged("Tcprotocol");
                NotifyPropertyChanged("CoverTemperature");
                NotifyPropertyChanged("SampleVolume");
                NotifyPropertyChanged("ProtocolRunMode");
            }
        }

        public ProtocolModel() :
            this(ProtocolFactory.GetDummyProtocol(), ProtocolFactory.GetDummyProtocolName(),
                ProtocolFactory.GetDummyProtocolName())
        {
        }

        public ProtocolModel(RunProtocol tcprotocol, String displayName, String path)
        {
            Tcprotocol = tcprotocol;
            DisplayName = displayName;
            Path = path;
        }

        public void InsertStage(int index, StageModel stageModel)
        {
            // Call RunProtocol API to insert stage at index.
            Tcprotocol.insertStage(index, stageModel.GetStage());
        }

        public void RemoveStage(int index)
        {
            // Call RunProtocol API to remove stage. First get the stage by index.
            Tcprotocol.removeStage(Tcprotocol.getStage(index));
        }

        public ProtocolModel Clone()
        {
            // Make use of RunProtocol copy constructor to clone the protocol
            return new ProtocolModel(new RunProtocol(tcprotocol), displayName, path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public class StageModel
    {
        private Stage stage;        // Stage object from API to keep stage information

        // Properties for UI binding, call API getter/setter to retrieve and save values.
        public ObservableCollection<StepModel> Steps { get; set; }
        public String Name
        {
            get { return stage.getName(); }
            set { stage.setName(value); }
        }
        public int Cycle
        {
            get { return stage.getNumOfCycles(); }
            set { stage.setNumOfCycles(value); }
        }

        public StageModel(String name, int cycle)
        {
            Steps = new ObservableCollection<StepModel>();
            stage = new Stage(name, cycle);
        }

        public StageModel(Stage stage)
        {
            Steps = AbstractStepModels(stage.getSteps());
            this.stage = stage;
        }

        public void InsertStep(int index, StepModel stepModel)
        {

            Steps.Insert(index, stepModel);
            // Call Stage API to insert step at specific index
            stage.insertStep(index, stepModel.GetStep());
        }

        public void RemoveStep(int index)
        {
            Steps.RemoveAt(index);
            // Call Stage API to remove step
            stage.removeStep(stage.getStep(index));
        }

        public void AddStep(StepModel stepModel)
        {
            Steps.Add(stepModel);
            // Call Stage API to add a step
            stage.addStep(stepModel.GetStep());
        }

        private ObservableCollection<StepModel> AbstractStepModels(java.util.List steps)
        {
            ObservableCollection<StepModel> stepModels = new ObservableCollection<StepModel>();
            for (int i = 0; i < steps.size(); i++)
            {
                stepModels.Add(new StepModel((Step)steps.get(i)));
            }

            return stepModels;
        }

        public Stage GetStage()
        {
            return stage;
        }

        public StageModel Clone()
        {
            StageModel replicate = new StageModel(string.Format("{0}_New", Name), Cycle);
            for (int i = 0; i < Steps.Count; i++)
            {
                replicate.AddStep(Steps[i].Clone());
            }
            return replicate;
        }
    }

    public class StepModel
    {
        private Step step;              // Step object from API to store the step information

        // Properties for UI binding, call API getter/setter to retrieve and save values.
        public double Temperature
        {
            get { return step.getRamp().getTemperature(); }
            set { step.getRamp().setTemperature(value); }
        }
        public int HoldTime
        {
            get { return step.getHold().getDuration(); }
            set { step.getHold().setDuration(value); }
        }
        public double RampRate
        {
            get { return step.getRamp().getRate(); }
            set { step.getRamp().setRate(value, RateUnit); }
        }
        public RampRateUnit RateUnit { get; set; }

        public StepModel(double temperature, int time, double rampRate, RampRateUnit unit)
        {
            step = new Step(temperature, rampRate, unit, time);
            RateUnit = unit;
        }

        public StepModel(Step step)
        {
            this.step = step;
            RateUnit = step.getRamp().getRateUnit();
        }

        public StepModel Clone()
        {
            StepModel replicate = new StepModel(Temperature, HoldTime, RampRate, RateUnit);
            return replicate;
        }

        public Step GetStep()
        {
            return step;
        }
    }

    // Protcol factory static class to demo the use of RunProtocol APIs to create protocols

    public static class ProtocolFactory
    {
        private static string defaultName = "Default Protocol";
        private static string dummyName = "-";
        private static RunMode runMode = RunMode.FAST;
        private static double coverTemperature = 105f;
        private static double sampleVolume = 10f;

        public static string GetDefaultProtocolName()
        {
            return defaultName;
        }

        public static string GetDummyProtocolName()
        {
            return dummyName;
        }

        //// Create a default protocol with proper stage and step definition 
        public static RunProtocol GetDefaultProtocol()
        {
            RunProtocol protocol = new RunProtocol();
            protocol.setCoverTemperature(coverTemperature);
            protocol.setRunMode(runMode);
            protocol.setSampleVolume(sampleVolume);

            // Add preStage with one step
            Stage preStage = new Stage();
            preStage.setName("PreStage");           // Set stage name for user reference
            preStage.addStep(new Step(50.0f, 10));  // Add step to hold 50 degree for 10 seconds
            preStage.addStep(new Step(95.0f, 15));  // Add step to hold 95 degree for 15 seconds
            protocol.addStage(preStage);

            // Add pcrStage with 2 steps and 2 cycles
            Stage pcrStage = new Stage(40);         // Create new step with 40 cycles. Cycle is 1 if not defined 
            pcrStage.setName("PcrStage");
            pcrStage.addStep(new Step(95.0f, 15));
            pcrStage.addStep(new Step(60.0f, 10));
            protocol.addStage(pcrStage);

            return protocol;
        }

        // Define a dummy empty protocol. Note that ProtocolUtil validation will not allow empty protocol
        public static RunProtocol GetDummyProtocol()
        {
            RunProtocol protocol = new RunProtocol();
            protocol.setCoverTemperature(coverTemperature);
            protocol.setRunMode(runMode);
            protocol.setSampleVolume(sampleVolume);

            return protocol;
        }

        public static RunProtocol GetTestProtocol()
        {
            RunProtocol protocol = new RunProtocol();
            protocol.setCoverTemperature(coverTemperature);
            protocol.setRunMode(runMode);
            protocol.setSampleVolume(sampleVolume);

            // Add preStage with one step
            Stage preStage = new Stage();
            preStage.setName("PreStage");
            preStage.addStep(new Step(50.0f, 10));
            preStage.addStep(new Step(95.0f, 15));
            protocol.addStage(preStage);

            // Add pcrStage with 2 steps and 2 cycles
            Stage pcrStage = new Stage(2);
            pcrStage.setName("PcrStage");
            pcrStage.addStep(new Step(95.0f, 15));
            pcrStage.addStep(new Step(60.0f, 10));
            protocol.addStage(pcrStage);

            return protocol;
        }
    }
}
