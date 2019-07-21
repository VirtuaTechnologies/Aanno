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
using System.Diagnostics;
using Notifications.Wpf;
using System.Windows.Forms;
using VSharpXMLHelper;
using System.ComponentModel;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using System.IO;

namespace C3D_2016_Anno.Apps.AAnno2
{
    /// <summary>
    /// Interaction logic for AAnnoView.xaml
    /// </summary>
    public partial class AAnnoView : System.Windows.Controls.UserControl
    {
        #region Generic
        public static bool boolRes;
        public BackgroundWorker bw = new BackgroundWorker();
        public static string mTextLabel = "";
        #endregion

        public AAnnoView()
        {
            InitializeComponent();

            //set ui data source
            cBox_template.ItemsSource = GV.templateFiles;
            cBox_template.SelectedIndex = 0;
            cBox_objectType.ItemsSource = GV.NotesCollection_Anno2.Keys;
            lBox_CurrentNotes.ItemsSource = GV.NotesCollection_Anno2.Values;
        }

        #region File Loader
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

        private void btn_browse_templateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create an instance of the open file dialog box.
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Note list XML files (*.XMLNotes)|*.XMLNotes|CSV Notes without double quotes (*.CSVNotes1)|*.CSVNotes1|CSV Notes with double quotes (*.CSVNotes2)|*.CSVNotes2"; //"XML files (*.xml)|*.def";
                ofd.RestoreDirectory = true;
                DialogResult sdResult = ofd.ShowDialog();



                if (sdResult != System.Windows.Forms.DialogResult.OK) return;

                foreach (string file in ofd.FileNames)
                {
                    //if the file dont exits add


                    //extract the data from the selected file.
                    GH.getFileObject(file, "template");
                }
                //set that file to the combobox
                cBox_template.SelectedIndex = cBox_template.Items.Count - 1;
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_template.SelectedItem;
                GH.getTemplateDetails(FI.filePath);
                UIH.toastIT("Note list files read sucessfully!", "File Read", NotificationType.Information);
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        private void btn_browse_SSTFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Filter = "Style Structure Files (*.sst; *.sst)|*.sst;";
                if (openFileDialog.ShowDialog() == true)
                {
                    tBox_StyleStructureFile.Text = openFileDialog.FileName;

                    //load data on the file ot the listview.
                    GV.SST_Coll.Clear();
                    if (GV.SSTfileFormat == "XML")
                    {
                        GV.SST_Coll = xmlParser.getXMLVaulesStrings(tBox_StyleStructureFile.Text, "STYLE", "name");
                    }
                    else
                    {

                    }
                }


            }
            catch (System.Exception ex)
            { }
        }

        private void tBox_StyleStructureFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            GV.sstFile = tBox_StyleStructureFile.Text;
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

        private void btn_fetchSelectedtamplateFile_Click(object sender, RoutedEventArgs e)
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

        public bool checkifDefnitionsSelected()
        {
            try
            {
                if (cBox_template.SelectedIndex < 0 || tBox_StyleStructureFile.Text == string.Empty)
                    boolRes = false;
                else
                    boolRes = true;
            }
            catch (System.Exception ex) { }
            return boolRes;
        }
        #endregion

        #region Selection

        public void clearUIValues()
        {
            try
            {
                lbl_statusCount.Content = "";
                cBox_objectType.ItemsSource = null;
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
                cBox_objectType.ItemsSource = null;
                lBox_CurrentNotes.ItemsSource = null;
                cBox_objectType.ItemsSource = GV.NotesCollection_Anno2.Keys;
                
                
                
                //if more than one objectype present in the cbox then display that
                if (cBox_objectType.Items.Count > 0)
                    cBox_objectType.SelectedIndex = 0;
                //add notes to list view
                lBox_CurrentNotes.ItemsSource = GV.NotesCollection_Anno2[cBox_objectType.SelectedItem.ToString()];
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }
        private void btn_clearCollectionandUI_Click(object sender, RoutedEventArgs e)
        {
            GV.NotesCollection_Anno2.Clear();
            clearUIValues();
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
                    

                    using (GV.Doc.LockDocument())
                    {
                        PromptSelectionResult psRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(new SelectionFilter(LCH.selectionFilter(GV.labelFilterType)));

                        if (psRes.Status == PromptStatus.OK)
                        {
                            SelectionSet acSSet = psRes.Value;
                            GV.selObjects_forProcessing = acSSet.GetObjectIds();

                            //get key notes based on the selection
                            
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
                            Dictionary<string, string> labelVals = new Dictionary<string, string>();
                            Dictionary<string, string> noteItems = new Dictionary<string, string>();
                            List<string> SSTList = new List<string>();
                            foreach (ObjectId objID in GV.selObjects_forProcessing)
                            {
                                // get the style name from the label
                                string styleName = LCH.getLabelName(objID);
                                SSTList = GH.getSST(styleName);
                                if (SSTList != null)
                                {
                                    string notetype = SSTList[1];
                                    labelVals = Helper.LabelTextExtractor.getLabelValsAll(objID);
                                    //get the KN values based on the locations
                                    foreach (var KNloc in SSTList[2].ToString())
                                    {

                                        #region Add the notes to the collection
                                        // add note type if its not there add it to the collection
                                        if (!GV.NotesCollection_Anno2.ContainsKey(notetype))
                                        {
                                            GV.NotesCollection_Anno2.Add(notetype, noteItems);
                                        }

                                        //check if that note number is already in the collection.
                                        noteItems = GV.NotesCollection_Anno2[notetype]; //get the note item for the specific note type

                                        if (labelVals.ContainsKey(KNloc.ToString()))
                                        {
                                            string noteNum = labelVals[KNloc.ToString()]; //get the note number based on the KN location
                                                                                          //check if that note type is available in the template file
                                            if (GV.notesDict.ContainsKey(notetype))
                                            {
                                                if (!noteItems.ContainsKey(noteNum))
                                                {
                                                    noteItems.Add(noteNum, GV.notesDict[notetype][Convert.ToInt32(noteNum)]);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                //get the notes from the notelist files and add it based on the KN values
                            }
                            
                            updateUIdata();

                            UIH.toastIT("All selected labels processed successfully!", "Status", NotificationType.Success);
                        }
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

        private void btn_selectViewport_Click(object sender, RoutedEventArgs e)
        {
            try
            {

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

        #region Preview
        private void cBox_objectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string objTypeSelected = cBox_objectType.SelectedItem.ToString();
                GH.qprint("objTypeSelected : " + objTypeSelected);

                lBox_CurrentNotes.Items.Clear();
                lBox_CurrentNotes.ItemsSource = GV.NotesCollection_Anno2[cBox_objectType.SelectedItem.ToString()];

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

        private void notelistColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            UIH.sortColumn(sender, lBox_CurrentNotes);
        }
        #endregion

        #region Keynote Creator
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

                            if (btn_HeadingUnderline.IsChecked == true)
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

                        foreach (var item in GV.NotesCollection_Anno2[cBox_objectType.SelectedItem.ToString()])
                        {
                            mTextLabel += item.Key + GV.keynoteSeperator + item.Value + @" \P";
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

        
    }
}
