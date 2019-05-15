using System;
using System.IO;
using System.Diagnostics;
using com.apldbio.pcr.protocol;
using com.apldbio.pcr.exception;
using System.Text.RegularExpressions;

namespace VworksAtcPlugin
{
    public class SiLAExporter
    {
        private const string xmlFormat = "<?xml version=\\\"1.0\\\" encoding=\\\"utf-8\\\"?>\n{0}";
        private const string protocolFormat = "<Protocol ProtocolName=\\\"{0}\\\" Volume=\\\"{1}\\\" RunMode=\\\"{2}\\\">\n{3}</Protocol>\n";
        private const string stageFormat = "\t<Stage Label=\\\"{0}\\\" Index=\\\"{1}\\\" Repeat=\\\"{2}\\\">\n{3}\t</Stage>\n";
        private const string stepFormat = "\t\t<Step Identifier=\\\"{0}\\\">\n{1}\t\t</Step>\n";
        private const string rampFormat = "\t\t\t<Ramp Rate=\\\"{0}\\\">\n{1}\t\t\t</Ramp>\n";
        private const string temperatureFormat = "\t\t\t\t<Temperature>{0}</Temperature>\n";
        private const string holdFormat = "\t\t\t<Hold HoldTime=\\\"{0}\\\"/>\n";

        private const string defaultProtocolName = "MyRunProtocol";
        private const string stagePrefix = "Stage_";
        private const int tempZoneRepeat = 3;
        private const double defaultCoverTemperature = 105f;
        private const int defaultInfiniteHoldTime = -1;

        public SiLAExporter()
        {
        }

        public void SaveToLocal(RunProtocol protocol, string fileName)
        {
            string xmlContentString = GetXMLContent(protocol, fileName);
            Debug.WriteLine(xmlContentString);
            byte[] bytes = new byte[xmlContentString.Length * sizeof(char)];
            System.Buffer.BlockCopy(xmlContentString.ToCharArray(), 0, bytes, 0, bytes.Length);
            FileStream fs = File.Create(fileName);

            fs.Write(bytes, 0, bytes.Length);
        }

        private void ValidateCoverTemperature(double coverTemperature)
        {
            if (coverTemperature != defaultCoverTemperature)
            {
                throw new SiLAExportException("Cover temperature is not default.");
            }
        }

        private void ValidateInfiniteHold(int duration)
        {
            if (duration == defaultInfiniteHoldTime)
            {
                throw new SiLAExportException("Infinite hold is not supported");
            }
        }

        private Boolean IsValidateTextString(string textString)
        {
            // Allow alpha numeric, _, white space, and -. Do not allow empty string
            var regexItem = new Regex("^[a-zA-Z0-9_ -]+$");
            return regexItem.IsMatch(textString);
        }

        private void ValidateProtocolName(string name)
        {
            if (!IsValidateTextString(name))
            {
                throw new SiLAExportException(string.Format("Invalid protocol name {0}", name));
            }
        }

        private void ValidateStageLabel(string label)
        {
            if (!IsValidateTextString(label))
            {
                throw new SiLAExportException(string.Format("Invalid stage label {0}", label));
            }
        }

        private string GetRampContet(Step step)
        {
            string rampContent = "";
            for (int i = 0; i < tempZoneRepeat; i++)
            {
                rampContent += string.Format(temperatureFormat, step.getRamp().getTemperature().ToString());
            }
            return string.Format(rampFormat, step.getRamp().getRate(), rampContent);
        }

        private string GetHoldContent(Step step)
        {
            ValidateInfiniteHold(step.getHold().getDuration());

            return string.Format(holdFormat, step.getHold().getDuration().ToString());
        }

        private string GetStepContent(Step step, int index)
        {
            string stepContent = GetRampContet(step) + GetHoldContent(step);
            return string.Format(stepFormat, index + 1, stepContent);
        }

        private string GetStageContent(Stage stage, int index)
        {
            string stageLabel = stage.getName(); // stagePrefix + (index+1)
            ValidateStageLabel(stageLabel);

            string stageContent = "";
            java.util.List steps = stage.getSteps();
            for (int i = 0; i < steps.size(); i++)
            {
                stageContent += GetStepContent(steps.get(i) as Step, i);
            }
            return string.Format(stageFormat, stageLabel, index + 1, stage.getNumOfCycles(), stageContent);
        }

        private string GetProtocolContent(RunProtocol protocol, string fileName)
        {
            string protocolName = Path.GetFileNameWithoutExtension(fileName); // defaultProtocolName
            ValidateProtocolName(protocolName);
            ValidateCoverTemperature(protocol.getCoverTemperature());

            string protocolContent = "";
            java.util.List stages = protocol.getStages();
            for (int i = 0; i < stages.size(); i++)
            {
                protocolContent += GetStageContent(stages.get(i) as Stage, i);
            }
            return string.Format(protocolFormat, protocolName,
                protocol.getSampleVolume(), protocol.getRunMode().toString(), protocolContent);
        }

        private string GetXMLContent(RunProtocol protocol, string fileName)
        {
            string xmlContent = GetProtocolContent(protocol, fileName);
            return string.Format(xmlFormat, xmlContent);
        }
    }

    public class SiLAExportException : Exception
    {
        string exceptionMsg;

        public SiLAExportException(string message) : base(message)
        {
            exceptionMsg = message;
        }

        public string GetMessage()
        {
            return exceptionMsg;
        }
    }
}
