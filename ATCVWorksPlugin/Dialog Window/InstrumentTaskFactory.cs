using com.apldbio.pcr.exception;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace VworksAtcPlugin
{
    /// <summary>
    /// This is static class to wrap the instrument related task into new thread,
    /// using Task.Factory.StartNew() and handle the exception
    /// </summary>
    /// 
    public static class InstrumentTaskFactory
    {
        // Setup Logger        
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void StartTask(Action action)
        {
            StartTask(action, (string)null);
        }

        public static void StartTask(Action action, Action failureCallback)
        {
            StartTask(action, null, failureCallback);
        }

        public static void StartTask(Action action, string failureMessage)
        {
            Task.Factory.StartNew(() => {
                try
                {
                    action();
                }
                catch (PCRException pe)
                {
                    log.Error(PCRExceptionFormatter.GetDetailFailureMessage(pe));
                    MessageBox.Show((failureMessage ?? "") + PCRExceptionFormatter.GetDetailFailureMessage(pe));
                }
                catch (Exception ge)
                {
                    log.Error(failureMessage + ge.ToString());
                    MessageBox.Show((failureMessage ?? "") + ge.ToString());
                }
            });
        }

        public static void StartTask(Action action, string failureMessage, Action failureCallback)
        {
            Task.Factory.StartNew(() => {
                try
                {
                    action();
                }
                catch (PCRException pe)
                {
                    log.Error(PCRExceptionFormatter.GetDetailFailureMessage(pe));
                    failureCallback();
                    MessageBox.Show((failureMessage == null ? "" : failureMessage) + PCRExceptionFormatter.GetDetailFailureMessage(pe));
                }
                catch (Exception ge)
                {
                    log.Error(ge.ToString());
                    failureCallback();
                    MessageBox.Show((failureMessage == null ? "" : failureMessage) + ge.ToString());
                }
            });
        }

        public static void UpdateUITask(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
