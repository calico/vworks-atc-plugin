using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.apldbio.pcr.protocol;
using com.apldbio.pcr.exception;

namespace VworksAtcPlugin
{
    /// <summary>
    /// This is the static class to format the PCRException error message
    /// </summary>
    public static class PCRExceptionFormatter
    {
        // Define custom error messages based on PCRErrorCode
        private static Dictionary<PCRErrorCode, String> errorDict = new Dictionary<PCRErrorCode, string>()
        {
            { PCRErrorCode.PROTOCOL_READ_FAILED, "Unable to read protocol." },
            { PCRErrorCode.PROTOCOL_WRITE_FAILED, "Unable to write protocol." },
            { PCRErrorCode.PROTOCOL_EMPTY, "Protocol cannot be empty." },
            { PCRErrorCode.STAGE_EMPTY, "Stage cannot be empty" }
            // Add more if needed
        };

        public static String GetDetailFailureMessage(PCRException exception)
        {
            return GetMessageByErrorCode(exception);
        }

        private static String GetMessageByErrorCode(PCRException exception)
        {
            PCRErrorCode errorCode = exception.getErrorCode();
            String message;
            if (errorCode == PCRErrorCode.PROTOCOL_COVER_TEMPERATURE_OUT_OF_RANGE)
            {
                message = String.Format("Cover temperature {0} is out of range [{1}, {2}].",
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.PROTOCOL_SAMPLE_VOLUME_OUT_OF_RANGE)
            {
                message = String.Format("Sample Volume {0} is out of range [{1}, {2}].",
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.PROTOCOL_TOO_MANY_STAGES)
            {
                message = String.Format("Protocol has too many stages - {0}. Range: [{1}, {2}].",
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.STAGE_NUMBER_OF_CYCLES_OUT_OF_RANGE)
            {
                Stage stage = (Stage)exception.getInfo(PCRErrorInfoKey.STAGE);
                message = String.Format("Stage \"{0}\" cycle number {1} is out of range [{2}, {3}].",
                    stage.getName(),
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.STEP_RAMP_TEMPERATURE_OUT_OF_RANGE)
            {
                Stage stage = (Stage)exception.getInfo(PCRErrorInfoKey.STAGE);
                Step step = (Step)exception.getInfo(PCRErrorInfoKey.STEP);
                message = String.Format("Stage \"{0}\", Step {1} temperature {2} is out of range [{3}, {4}].",
                    stage.getName(),
                    stage.indexOf(step) + 1,
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.STAGE_TOO_MANY_STEPS)
            {
                Stage stage = (Stage)exception.getInfo(PCRErrorInfoKey.STAGE);
                message = String.Format("Stage \"{0}\" has too many steps - {1}. Range: [{2}, {3}].",
                    stage.getName(),
                    exception.getInfo(PCRErrorInfoKey.VALUE),
                    exception.getInfo(PCRErrorInfoKey.MIN_VALUE),
                    exception.getInfo(PCRErrorInfoKey.MAX_VALUE));
            }
            else if (errorCode == PCRErrorCode.STAGE_EMPTY)
            {
                Stage stage = (Stage)exception.getInfo(PCRErrorInfoKey.STAGE);
                message = String.Format("Stage \"{0}\" cannot be empty.", stage.getName());
            }
            else if (errorCode == PCRErrorCode.STEP_INFINITE_HOLD_INVALID)
            {
                Stage stage = (Stage)exception.getInfo(PCRErrorInfoKey.STAGE);
                Step step = (Step)exception.getInfo(PCRErrorInfoKey.STEP);
                message = String.Format("Invalid infinite hold - Stage \"{0}\", Step \"{1}\".\nInfinte hold should only be applied to the last step of protocol\n and cycle number of the stage should be 1.",
                    stage.getName(), stage.indexOf(step) + 1);
            }
            else
            {
                message = errorDict.ContainsKey(errorCode) ? errorDict[errorCode] :
                    String.Format("Error code: {0}", errorCode);
            }

            return message;
        }
    }
}
