using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IAlertService
    {
        void ShowSingle(string message);
        void ShowMessage(string title, string message);
        void ShowErrorMessage(Exception ex);
        bool ShowOkCancelMessage(string title, string message, Action onOk, Action onCancel, bool showNegativeButton = true);
        void ShowToast(string message);
        void ShowAlertMessage(string message, List<ConfigurableAlertAction> actions);
        void ShowAlertMessage(string message, ConfigurableAlertAction okAnsver, ConfigurableAlertAction dismissAnswer, ConfigurableAlertAction neutralAnsver);

        Task<bool> ShowAlertConfirmation(string message, string buttonYesTitle = "Yes", string buttonNoTitle = "No");
        Task<string> ShowAlertWithInput(string title, string message = "", string text = "", string buttonYesTitle = "Yes", string buttonNoTitle = "No"); 
    } 

    public class ConfigurableAlertAction
    {
        public string Text;
        public Action OnClickHandler;
    }
}
