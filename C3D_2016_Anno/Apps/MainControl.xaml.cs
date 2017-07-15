using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using LCH = C3D_2016_Anno.Helper.LocalCADHelper;
using UIH = C3D_2016_Anno.Helper.UIHelper;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using DS = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.Civil.DatabaseServices;
using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Windows.Threading;
using Notifications.Wpf;
using System.Windows.Forms;
using System.Diagnostics;

namespace C3D_2016_Anno.Apps
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : System.Windows.Controls.UserControl
    {
        #region Generic
        public static string mTextLabel = "";
        public static bool boolRes;
        public MainControl()
        {
            try
            {
                InitializeComponent();
                this.Loaded += UserControl1_Loaded;
                fetchDATA();
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fetchDATA();
        }
        public BackgroundWorker bw = new BackgroundWorker();
        public void fetchDATA()
        {
            try
            {
                cBox_template.ItemsSource = GV.templateFiles;
                cBox_template.SelectedIndex = 0;
                cBox_Mapper.ItemsSource = GV.mapperFiles;
                cBox_Mapper.SelectedIndex = 0;
                
                cBox_objectType.ItemsSource = GV.ObtTypes;
                //cBox_objectType.SelectedIndex = 0;
                
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        public void clearUIValues()
        {
            try
            {
                lbl_statusCount.Content = "";
                cBox_objectType.ItemsSource = null;
                lBox_labels.ItemsSource = null;
                lBox_CurrentNotes.ItemsSource = null;
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }
        public void updateUIdata()
        {
            try
            {
                GH.qprint("GV.ObtTypes.Count() ==> 2>  " + GV.ObtTypes.Count());
                foreach (var item in GV.noteTypesCurrent)
                {
                    GH.qprint("GV.noteTypesCurrent > >  " + item.Key + " | " + item.Value);
                }
                GV.noteTypesCurrent.Clear();
                //update selection list
                GH.qprint("GV.Mapper.Count(): " + GV.Mapper.Count());
                GV.ObtTypes.Clear();
                GH.qprint("GV.ObtTypes.Count() ==> 2>  " + GV.ObtTypes.Count());
                foreach (var noteitem in GV.allnotes)
                {
                    GH.qprint("noteitem.Key: " + noteitem.Key);
                    if (GV.Mapper.ContainsKey(noteitem.Key))
                    {
                        GH.qprint("noteitem.Key in Mapper > >  " + noteitem.Key);
                        if (!GV.noteTypesCurrent.ContainsKey(noteitem.Key))
                        {
                            GH.qprint("noteitem.Key adding to current > >  " + noteitem.Key);
                            GV.noteTypesCurrent.Add(noteitem.Key, noteitem.Value);
                        }
                        if (!GV.ObtTypes.Contains(GV.Mapper[noteitem.Key]))
                        {
                            string ObjTypeName = GV.Mapper[noteitem.Key];
                            if (GV.SelectedObjTypes.Contains(ObjTypeName))
                            {
                                GH.qprint("GV.ObtTypes > >  " + GV.Mapper[noteitem.Key]);
                                GV.ObtTypes.Add(ObjTypeName);
                            }
                        }
                    }
                    else
                    {
                        GH.qprint("noteitem.Key not in Mapper > >  " + noteitem.Key);
                        foreach (var item in GV.Mapper)
                        {
                            GH.qprint("GV.Mapper > >  " + item.Key + " | " + item.Key);
                        }
                    }
                }
                cBox_objectType.ItemsSource = GV.ObtTypes;
                //cBox_objectType.SelectedIndex = 0;
                GH.qprint("GV.ObtTypes.Count() ==> 3>  " + GV.ObtTypes.Count());

                lBox_CurrentNotes.ItemsSource = GV.noteTypesCurrent.Values;

                foreach (string ObtTypes in GV.ObtTypes)
                {
                    List<Global.labelItem> filteredLabels = GV.all_label_coll.Where(o => o.objType == ObtTypes).ToList<Global.labelItem>();
                    if (!GV.all_label_coll_Sorted.ContainsKey(ObtTypes))
                    {
                        GV.all_label_coll_Sorted.Add(ObtTypes, filteredLabels);
                    }
                    else
                    {
                        GV.all_label_coll_Sorted[ObtTypes] = filteredLabels;
                    }

                }

                //if more than one objectype present in the cbox then display that
                if (cBox_objectType.Items.Count > 0)
                    cBox_objectType.SelectedIndex = 0;
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        public static void toastIT(string message, string title, NotificationType NotificationType)
        {
            var notificationManager = new NotificationManager();

            notificationManager.Show(new NotificationContent
            {
                Title = GV.appName + " | " +  title,
                Message = message,
                Type = NotificationType
            });
        }
        
        private async void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            MetroWindow window = Window.GetWindow(this) as MetroWindow;
            if (window != null)
            {
                await window.ShowMessageAsync("This is the title", "Some message");
            }
        }
        
        #region ProgressBar
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            proBar.Value = e.ProgressPercentage;
        }

        public void bw_UpdateProgressBar(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                UpdateProgressBar(GV.pBarCurrentVal, GV.pBarMaxVal, GV.pBarStatus);
                //lbl_statusCount.Content = GV.pBarStatus;
                //proBar.Value = ((GV.pBarCurrentVal / GV.pBarMaxVal) * 100);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                GH.writeLog(ex.ToString());
            }
            catch (System.Exception ex)
            {
                GH.writeLog(ex.ToString());
            }
        }

        public void UpdateProgressBar(double val, double Max, string statusCount)
        {
            try
            {
                // Action is delegate (means a function pointer) which is Pointing towards "SetProgress" method
                Action action = () => { SetProgress(val, Max, statusCount); };

                //proBar.Dispatcher.BeginInvoke(action);
                proBar.Dispatcher.BeginInvoke(action);
                //proBar.Dispatcher.Invoke(() => action, DispatcherPriority.Background);
                //listBox_status.Dispatcher.BeginInvoke(action);
            }
            catch (System.Exception ex)
            {
                GH.writeLog("SetProgress : " + ex.ToString());
            }
        }

        public void SetProgress(double val, double Max, string statusCount)
        {
            try
            {
                GH.writeLog("Progress: " + val + " | " + Max + " | " + statusCount);
                lbl_statusCount.Content = statusCount;
                proBar.Value = ((val / Max) * 100);

            }
            catch (System.Exception ex)
            {
                GH.writeLog("SetProgress : " + ex.ToString());
            }
        }
        #endregion
        #endregion

        #region Files Getter
        private void btn_browse_templateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an instance of the open file dialog box.
                Autodesk.AutoCAD.Windows.OpenFileDialog ofd = new Autodesk.AutoCAD.Windows.OpenFileDialog("Select a defnition file(s)", "",
                               "xml;",
                               "Select a defnition file(s)",
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.DefaultIsFolder |
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.ForceDefaultFolder |
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.AllowMultiple
                             );
                DialogResult sdResult = ofd.ShowDialog();

                if (sdResult != System.Windows.Forms.DialogResult.OK) return;

                foreach (string file in ofd.GetFilenames())
                {
                    //if the file dont exits add


                    //extract the data from the selected file.
                    GH.getFileObject(file, "template");
                }
                //set that file to the combobox
                cBox_template.SelectedIndex = cBox_template.Items.Count - 1;

                UIH.toastIT("Defnition files read sucessfully!", "File Read", NotificationType.Information);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_browse_mapperFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an instance of the open file dialog box.
                Autodesk.AutoCAD.Windows.OpenFileDialog ofd = new Autodesk.AutoCAD.Windows.OpenFileDialog("Select a Note list file(s)", "",
                               "Mapper;",
                               "Select a Note list file(s)",
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.DefaultIsFolder |
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.ForceDefaultFolder |
                               Autodesk.AutoCAD.Windows.OpenFileDialog.OpenFileDialogFlags.AllowMultiple
                             );
                DialogResult sdResult = ofd.ShowDialog();

                if (sdResult != System.Windows.Forms.DialogResult.OK) return;

                foreach (string file in ofd.GetFilenames())
                {
                    //extract the data from the selected file.
                    GH.getFileObject(ofd.Filename, "mapper");
                    GH.getMapper(ofd.Filename);
                }
                //set that file to the combobox
                cBox_Mapper.SelectedIndex = cBox_Mapper.Items.Count - 1;

                //set that file to the combobox
                UIH.toastIT("Note list files read sucessfully!", "File Read", NotificationType.Information);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_open_templateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_template.SelectedItem;
                Process.Start("notepad.exe", FI.filePath);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_open_mapperFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_Mapper.SelectedItem;
                Process.Start("notepad.exe", FI.filePath);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_fetchtamplateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = cBox_template.SelectedIndex;
                GH.getFiles("template");
                cBox_template.SelectedIndex = index;
                UIH.toastIT("Defnition files read sucessfully!", "File Read", NotificationType.Information);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_fetchmapperFiles_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int index = cBox_Mapper.SelectedIndex;
                GH.getFiles("mapper");
                cBox_Mapper.SelectedIndex = index;
                UIH.toastIT("Note files read sucessfully!", "File Read", NotificationType.Information);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void cBox_template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_template.SelectedItem;
                GH.getTemplateDetails(FI.filePath);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void cBox_Mapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_Mapper.SelectedItem;
                GH.qprint("Selected Mapper File: " + FI.filePath);
                GH.getMapper(FI.filePath);
                updateUIdata();
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void cBox_objectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string objTypeSelected = cBox_objectType.SelectedItem.ToString();
                GH.qprint("objTypeSelected : " + objTypeSelected);

                //load notes
                List<Global.labelItem> filteredLabels = GV.all_label_coll_Sorted[objTypeSelected];
                //filteredLabels = filteredLabels.GroupBy(n => n.noteNumber).Select(g => g.First()).ToList();
                //filteredLabels = filteredLabels.Where(x => x.note != null).ToList();

                lBox_CurrentNotes.ItemsSource = filteredLabels.GroupBy(n => n.noteNumber).Select(g => g.First()).Where(x => x.note != null && x.noteFound == true).ToList();
                lBox_missingNotes.ItemsSource = filteredLabels.GroupBy(n => n.noteNumber).Select(g => g.First()).Where(x => x.noteFound == false).ToList();
                lBox_labels.ItemsSource = filteredLabels;

                if(filteredLabels.Count > 0)
                    tBox_Heading.Text = objTypeSelected;

            }
            catch (Autodesk.Civil.CivilException ex)
            {
                //GH.errorBox(ex.ToString());
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                //GH.errorBox(ex.ToString());
            }
            catch (System.Exception ee)
            {
                //GH.errorBox(ee.ToString());
            }

        }

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private void notelistColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader column = (sender as GridViewColumnHeader);
                string sortBy = column.Tag.ToString();
                if (listViewSortCol != null)
                {
                    AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                    lBox_CurrentNotes.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                    newDir = ListSortDirection.Descending;

                listViewSortCol = column;
                listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
                AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
                lBox_CurrentNotes.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));

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

        private void labellistColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumnHeader column = (sender as GridViewColumnHeader);
                string sortBy = column.Tag.ToString();
                if (listViewSortCol != null)
                {
                    AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                    lBox_labels.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                    newDir = ListSortDirection.Descending;

                listViewSortCol = column;
                listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
                AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
                lBox_labels.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));

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

        public void updateObjectType()
        {
            try
            {
                
                
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void lBox_labels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }



        #endregion

        #region zoom/ seletion
        private void lBox_labels_zoomto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if the uesr in paper space if so swithc to model space
                Helper.ViewportExtensions.toggleModelspace();
                //MessageBox.Show("lBox_labels_zoomto_Click");
                GV.selLabels = new ObjectIdCollection();

                if (lBox_labels.SelectedItems.Count > 0)
                {
                    List<Global.labelItem> selectedItems = lBox_labels.SelectedItems.Cast<Global.labelItem>().ToList();

                    if (selectedItems[0].objID.ObjectClass.DxfName.ToString() == "MULTILEADER")
                    {
                        selectedItems = selectedItems.Reverse<Global.labelItem>().Reverse().ToList();
                    }
                    foreach (Global.labelItem item in lBox_labels.SelectedItems)
                    {
                        GV.selLabels.Add(item.objID);
                    }
                    LCH.ZoomObjects(GV.selLabels);
                }

            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }

        }

        private void lBox_labels_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if the uesr in paper space if so swithc to model space
                Helper.ViewportExtensions.toggleModelspace();

                GV.selLabels = new ObjectIdCollection();

                if (lBox_labels.SelectedItems.Count > 0)
                {
                    foreach (Global.labelItem item in lBox_labels.SelectedItems)
                    {
                        GV.selLabels.Add(item.objID);
                    }

                    ObjectId[] ids = new ObjectId[lBox_labels.SelectedItems.Count];
                    GV.selLabels.CopyTo(ids, 0);

                    GV.ed.SetImpliedSelection(ids);
                    //LCH.ZoomObjects(GV.selLabels);
                }

            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }

        }
        #endregion

        #region Action buttons
        public bool checkifDefnitionsSelected()
        {
            try
            {
                if (cBox_template.SelectedIndex < 0 || cBox_Mapper.SelectedIndex < 0)
                    boolRes = false;
                else
                    boolRes = true;
            }
            catch(System.Exception ex){ }
            return boolRes;
        }
        private void btn_selectLabels_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //check if defnition and  note list are selected
                if (checkifDefnitionsSelected())
                {
                    //rest progressbar
                    proBar.Value = 0;

                    clearUIValues();
                    GV.clearSelection();
                    tBox_Heading.Text = "";

                    LCH.getCurrentDwgVars();
                    using (GV.Doc.LockDocument())
                    {
                        GH.writeLog("\n Running command : btn_selectLabels_Click");

                        PromptSelectionResult psRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(new SelectionFilter(LCH.selectionFilter(GV.labelFilterType)));

                        if (psRes.Status == PromptStatus.OK)
                        {
                            SelectionSet acSSet = psRes.Value;
                            GV.selObjects_forProcessing = acSSet.GetObjectIds();
                            GH.qprint("Number of objects selected: " + psRes.Value.Count);
                            GH.writeLog("\nNumber of objects selected: " + psRes.Value.Count);

                            //get key notes based on the selection
                            #region process labels Styles
                            bw.WorkerSupportsCancellation = true;
                            bw.WorkerReportsProgress = true;
                            //bw.ProgressChanged += bw_ProgressChanged;
                            bw.DoWork += new DoWorkEventHandler(bw_UpdateProgressBar);
                            //start work
                            if (bw.IsBusy != true)
                            {
                                bw.RunWorkerAsync();
                            }


                            int index = 1;

                            int objCount = GV.selObjects_forProcessing.Count();
                            GV.pBarMaxVal = objCount;

                            //GV.pmeter.Start("Processing Labels");
                            //GV.pmeter.SetLimit(objCount);


                            foreach (ObjectId objID in GV.selObjects_forProcessing)
                            {
                                LCH.getlabelvalueSpecific(objID);

                                #region ProgressBAR

                                GH.printDebug("", "", false, true);
                                GV.pBarStatus = "Labels Processed: " + index + @"/" + objCount;
                                UpdateProgressBar(index, objCount, GV.pBarStatus);
                                //GV.pmeter.MeterProgress();
                                Helper.UIHelper.DoEvents();

                                GV.pBarCurrentVal = index;
                                
                                //assign it work
                                //bw.ReportProgress(index);
                                index++;
                                #endregion
                            }
                            //GV.pmeter.Stop();
                            //LCH.getlabelvalues();
                            //LCH.getlabelvalues(acSSet.GetObjectIds(), trans);
                            updateUIdata();

                            UIH.toastIT("All selected labels processed successfully!", "Status", NotificationType.Success);
                            #endregion
                        }
                    }
                }
                else
                {
                    UIH.toastIT("Check if defintion and note files are selected!", "Missing defintion/note file", NotificationType.Error);
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

        private void btn_selectViewport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clearUIValues();
                if (checkifDefnitionsSelected())
                {
                    cBox_objectType.ItemsSource = null;
                    lBox_labels.ItemsSource = null;
                    GV.clearSelection();
                    tBox_Heading.Text = "";

                    //rest progressbar
                    proBar.Value = 0;

                    //MyCommands.vpshp();

                    //GV.Doc.SendStringToExecute("vpshp", true, false, false);

                    LCH.getCurrentDwgVars();
                    #region old
                    using (GV.Doc.LockDocument())
                    {
                        short val = (short)AcAp.GetSystemVariable("CVPORT");
                        if (val != 1)
                        {
                            GV.processStatus = false;
                            UIH.toastIT("This option works only in Paperspace (Layouts), please swtich to paperspace and try again!", "Viewport Not Preset", NotificationType.Error);
                        }
                        else
                        {
                            Helper.ViewportExtensions.getvPortCoordinatesADV();

                            if (GV.processStatus == true && GV.selObjects_forProcessing != null)
                            {
                                bw.WorkerSupportsCancellation = true;
                                bw.WorkerReportsProgress = true;
                                //bw.ProgressChanged += bw_ProgressChanged;
                                bw.DoWork += new DoWorkEventHandler(bw_UpdateProgressBar);
                                //start work
                                if (bw.IsBusy != true)
                                {
                                    bw.RunWorkerAsync();
                                }
                                int index = 1;
                                int objCount = GV.selObjects_forProcessing.Count();
                                GV.pBarMaxVal = objCount;

                                foreach (ObjectId objID in GV.selObjects_forProcessing)
                                {
                                    LCH.getlabelvalueSpecific(objID);

                                    #region ProgressBAR

                                    GH.printDebug("", "", false, true);
                                    GV.pBarStatus = "Labels Processed: " + index + @"/" + objCount;
                                    UpdateProgressBar(index, objCount, GV.pBarStatus);
                                    //GV.pmeter.MeterProgress();
                                    Helper.UIHelper.DoEvents();

                                    GV.pBarCurrentVal = index;

                                    //assign it work
                                    index++;
                                    #endregion
                                }
                                updateUIdata();

                                UIH.toastIT("All selected labels processed successfully!", "Status", NotificationType.Success);
                            }
                        }

                    }
                    #endregion
                }
                else
                {
                    UIH.toastIT("Check if defintion and note files are selected!", "Missing defintion/note file", NotificationType.Error);
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

        private void btn_CreateKeyNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cBox_objectType.SelectedIndex >= 0)
                {
                    LCH.getCurrentDwgVars();
                    using (GV.Doc.LockDocument())
                    {

                        //get title
                        string objTypeSelected = cBox_objectType.SelectedItem.ToString();
                        GV.textHeight = Convert.ToDouble(tBox_textHeight.Text);
                        //get items
                        mTextLabel = "";
                        
                        //key note heading
                        if (tBox_Heading.Text != "")
                        {
                            LCH.getCurrentFont();
                            if (btn_HeadingBold.IsChecked == true || btn_HeadingUnderline.IsChecked == true)
                            {
                                mTextLabel += "{";
                            }
                                if (btn_HeadingBold.IsChecked == true)
                            {
                                mTextLabel += "\\f" + GV.currentFont + "|b1;";
                            }

                            if(btn_HeadingUnderline.IsChecked == true)
                            {
                                mTextLabel += "\\L";
                            }

                            mTextLabel += tBox_Heading.Text + @"}\P";
                            mTextLabel = mTextLabel.Replace(@"\\", @"\");
                            
                        }
                        else
                        {
                            mTextLabel += tBox_Heading.Text + @" \P";
                        }

                        foreach (Global.labelItem item in lBox_CurrentNotes.Items)
                        {
                            mTextLabel += item.noteNumber + GV.keynoteSeperator + item.note + @" \P";
                        }

                        #region Create Keynote Text
                        if (!mTextLabel.Equals(string.Empty))
                        {
                            switch (GV.keynotetexttype)
                            {
                                case "mtext":
                                    {
                                        LCH.createMtextwithJIG(mTextLabel);
                                    }
                                    break;

                                default:
                                    {
                                        LCH.createMtextwithJIG(mTextLabel);
                                    }
                                    break;
                            }
                            #endregion

                        }
                    }
                }
                else
                {
                    UIH.toastIT("Notes not found.", "No Notes Found", NotificationType.Error);
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

        #endregion

        private void btn_openXMLMan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

                // Draws a circle and zooms to the extents or 
                // limits of the drawing
                acDoc.SendStringToExecute("xmlMan ", true, false, false);
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
}
