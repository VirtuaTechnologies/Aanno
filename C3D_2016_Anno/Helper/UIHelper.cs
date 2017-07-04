using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using System.Windows.Threading;
using Notifications.Wpf;

namespace C3D_2016_Anno.Helper
{
    public class UIHelper
    {
        public static StackPanel sPanelitem_Note;
        public static void getNotItem()
        {
            try
            {
                sPanelitem_Note = new StackPanel();
                sPanelitem_Note.Children.Clear();
                sPanelitem_Note.Orientation = Orientation.Vertical;
            }
            catch (Autodesk.Civil.CivilException ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                GH.errorBox(ee.ToString());
            }

        }

        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        public static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

        public static void toastIT(string message, string title, NotificationType NotificationType)
        {
            //check if the toast category is eanbled

            if (NotificationType == NotificationType.Success && GV.sucessToast != true)
                return;
            if (NotificationType == NotificationType.Error && GV.errorToast != true)
                return;
            if (NotificationType == NotificationType.Information && GV.infoToast != true)
                return;

            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = GV.appName + " | " + title,
                Message = message,
                Type = NotificationType
            });
        }
    }
}
