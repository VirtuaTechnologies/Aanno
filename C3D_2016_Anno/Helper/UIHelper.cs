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
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.ComponentModel;

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

        public class SortAdorner : Adorner
        {
            private static Geometry ascGeometry =
                    Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

            private static Geometry descGeometry =
                    Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

            public ListSortDirection Direction { get; private set; }

            public SortAdorner(UIElement element, ListSortDirection dir)
                    : base(element)
            {
                this.Direction = dir;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);

                if (AdornedElement.RenderSize.Width < 20)
                    return;

                TranslateTransform transform = new TranslateTransform
                        (
                                AdornedElement.RenderSize.Width - 15,
                                (AdornedElement.RenderSize.Height - 5) / 2
                        );
                drawingContext.PushTransform(transform);

                Geometry geometry = ascGeometry;
                if (this.Direction == ListSortDirection.Descending)
                    geometry = descGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);

                drawingContext.Pop();
            }
        }

        private static GridViewColumnHeader listViewSortCol = null;
        private static SortAdorner listViewSortAdorner = null;
        public static void sortColumn(object sender, ListView listViewControl)
        {
            try
            {
                GridViewColumnHeader column = (sender as GridViewColumnHeader);
                string sortBy = column.Tag.ToString();
                if (listViewSortCol != null)
                {
                    AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                    listViewControl.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                    newDir = ListSortDirection.Descending;

                listViewSortCol = column;
                listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
                AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
                listViewControl.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));

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

        public static void sortColumnASC(ListView listViewControl)
        {
            try
            {
                GridViewHeaderRowPresenter presenter = GetDescendantByType(listViewControl, typeof(GridViewHeaderRowPresenter)) as GridViewHeaderRowPresenter;
                GridView gridView = listViewControl.View as GridView;
                

                for (int i = 1; i < gridView.Columns.Count+1; i++)
                {
                    GridViewColumnHeader header = VisualTreeHelper.GetChild(presenter, i) as GridViewColumnHeader;
                    GridViewColumn colunmn = header.Column;
                    string test = colunmn.Header.ToString();

                    if (colunmn != null && test == "System.Windows.Controls.GridViewColumnHeader: Note Number")
                    {

                        string sortBy = "Note Number";
                        if (listViewSortCol != null)
                        {
                            AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                            listViewControl.Items.SortDescriptions.Clear();
                        }

                        ListSortDirection newDir = ListSortDirection.Ascending;

                        listViewSortCol = header;
                        listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
                        AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
                        listViewControl.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
                    }
                }

                

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

        public static Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
                (element as FrameworkElement).ApplyTemplate();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }
    }
}

