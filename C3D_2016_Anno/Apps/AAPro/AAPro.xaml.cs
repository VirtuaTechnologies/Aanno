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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using LCH = C3D_2016_Anno.Helper.LocalCADHelper;
using UIH = C3D_2016_Anno.Helper.UIHelper;
using Microsoft.Win32;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Civil.DatabaseServices.Styles;
using Autodesk.AutoCAD.EditorInput;
using System.Runtime.InteropServices;
using Autodesk.AutoCAD.Internal;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Notifications.Wpf;

namespace C3D_2016_Anno.Apps
{
    /// <summary>
    /// Interaction logic for AAPro.xaml
    /// </summary>
    public partial class AAPro : System.Windows.Controls.UserControl
    {
        public static ObservableCollection<Global.labelComponentItem> selObjs = new ObservableCollection<Global.labelComponentItem>();
        public static string appName = "AAnno Pro";
        public AAPro()
        {
            try
            {
                InitializeComponent();
                GV.labelComponentItem_coll.Clear();
                listView_styleComponentMapper.ItemsSource = GV.labelComponentItem_coll;
                cBox_styleType.ItemsSource = GV.noteTypeListColl;
            }
            catch (System.Exception ex)
            { GH.writeLog(ex.ToString()); }
        }

        private void btn_browse_styleStructureFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Style Structure Files (*.sst; *.sst)|*.sst;";
                if (openFileDialog.ShowDialog() == true)
                {
                    tBox_stylemapperFile.Text = openFileDialog.FileName;

                    //load data on the file ot the listview.
                    GH.getSSTAll(tBox_stylemapperFile.Text);
                    listView_styleComponentMapper.ItemsSource = GV.labelComponentItem_coll;
                }


            }
            catch (System.Exception ex)
            { }
        }

        private void btn_save_styleStructureFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(tBox_stylemapperFile.Text))
                {
                    writetoFile();
                }
                else //ask the user to create new file or select and existing file.
                {
                    MessageBox.Show("Unable to save to file, please check if the file exists!");
                }

            }
            catch (System.Exception ex)
            { }
        }

        private void btn_saveAS_styleStructureFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Style Structure File (*.sst)|*.sst";
                if (saveFileDialog.ShowDialog() == true)
                {
                    tBox_stylemapperFile.Text = saveFileDialog.FileName;
                    writetoFile();
                }
            }
            catch (System.Exception ex)
            { GH.writeLog(ex.ToString()); }
        }

        private void writetoFile()
        {
            try
            {
                //save the mapping information in XML format to the style mapper file.
                //update the existing file.
                //this will erase whats on the file and update with the current view content.
                Dictionary<string, Dictionary<string, string>> Item = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> keyvalue = new Dictionary<string, string>();
                string CSVstring = "";
                foreach (Global.labelComponentItem item in GV.labelComponentItem_coll)
                {
                    string KNValue = String.Join(",", item.KNComponentID.Select(n => n.ToString()).ToArray());
                    keyvalue.Add(item.labelType, KNValue);
                    Item.Add(item.styleName, keyvalue);
                    CSVstring += item.styleName + GV.SSTfileDelimiter + item.labelType + GV.SSTfileDelimiter + KNValue + Environment.NewLine;
                }

                if (GV.SSTfileFormat == "XML")
                {
                    VSharpXMLHelper.xmlWriter.cleanWritevariableKey(tBox_stylemapperFile.Text, "APP", "type", Item);
                    //check if a file selected
                }
                else //native format
                {
                    if (!File.Exists(tBox_stylemapperFile.Text))
                    {
                        File.Create(tBox_stylemapperFile.Text).Close();
                    }

                    File.WriteAllText(tBox_stylemapperFile.Text, CSVstring);
                }
            }
            catch (System.Exception ex)
            { GH.writeLog(ex.ToString()); }
        }

        private static Global.labelComponentItem LI;

        private void btn_learn_all_Click(object sender, RoutedEventArgs e)
        {
            GH.errorBox("This function is work in progress! check again later.");
        }

        private void btn_delete_style_item_Click(object sender, RoutedEventArgs e)
        {
            if (GV.labelComponentItem_coll.Contains((Global.labelComponentItem)listView_styleComponentMapper.SelectedItem))
            {
                GV.labelComponentItem_coll.Remove((Global.labelComponentItem)listView_styleComponentMapper.SelectedItem);
            }
        }

        private void listView_styleComponentMapper_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (listView_styleComponentMapper.SelectedItem != null)
                {
                    if (GV.labelComponentItem_coll.Contains((Global.labelComponentItem)listView_styleComponentMapper.SelectedItem))
                    {
                        GV.labelComponentItem_coll.Remove((Global.labelComponentItem)listView_styleComponentMapper.SelectedItem);
                    }
                }
            }
        }

        private void btn_learn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                collIndex = 0;
                selObjs.Clear();
                LCH.getCurrentDwgVars();
                using (GV.Doc.LockDocument())
                {
                    //ask the user to pick a style to read about the style
                    //Seleciton options, with single selection
                    PromptSelectionOptions Options = new PromptSelectionOptions();
                    Options.SingleOnly = true;
                    //Options.SinglePickInSpace = true;
                    Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();

                    PromptSelectionResult psRes = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.GetSelection(Options, new SelectionFilter(LCH.selectionFilter(GV.labelFilterType)));

                    if (psRes.Status == PromptStatus.OK)
                    {
                        SelectionSet acSSet = psRes.Value;
                        GV.selObjects_forProcessing = acSSet.GetObjectIds();


                        foreach (ObjectId objID in GV.selObjects_forProcessing)
                        {
                            //get the name of the label
                            LI = new Global.labelComponentItem();
                            LI.styleName = LCH.getLabelName(objID);
                            LI.objType = LCH.getObjType(objID);
                            LI.objID = objID;
                            GV.ed.WriteMessage("LI.name: " + LI.styleName);
                            GV.ed.WriteMessage("LI.objType: " + LI.objType);

                            //get the component id which has the value 99
                            Dictionary<string, string> CompNameVals = new Dictionary<string, string>();
                            CompNameVals = Helper.LabelTextExtractor.getLabelValsAll(objID);

                            //get the location of the value and store it against the style name and id.
                            int i = 0;
                            LI.KNComponentID = new List<int>();
                            string KNLoc = "";

                            foreach (var item in CompNameVals)
                            {
                                if (item.Value == "99")
                                {
                                    LI.KNComponentID.Add(Convert.ToInt32(item.Key));
                                    KNLoc = item.Key + ",";
                                    i++;
                                }
                            }

                            //check if there is a comman at the end of the KN and remove that
                            KNLoc = KNLoc.Remove(KNLoc.Length - 1);

                            if (selObjs.Where(item => item.styleName == LI.styleName).Any() == false)
                            {
                                selObjs.Add(LI);
                            }
                            
                            //show this on the confirmation box - once the user confirms its then add to the list. or discard that item

                            tBox_styleName.Text = LI.styleName;
                            //tBox_styleKNloc.Text = KNLoc;

                            // ask the user to pick the mapper configuration 

                            //store the configuration to settings file

                        }

                        grid_addStyle.Visibility = System.Windows.Visibility.Visible;

                    }
                }
            }
            catch (System.Exception ex)
            { GH.writeLog(ex.ToString()); }
        }

        private void Btn_addStyle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //load the current item
                if (selObjs.Where(item => item.styleName == tBox_styleName.Text).Any() == true)
                {
                    LI = selObjs.Where(item => item.styleName == tBox_styleName.Text).Single();
                }

                //check if that object already exists
                if (GV.labelComponentItem_coll.Where(LabelItem => LabelItem.styleName == LI.styleName).Any() == false)
                {
                    //check if all the values are filled in
                    if (tBox_styleName.Text == string.Empty)
                    {
                        Blink(true, "Style name missing, please enter");
                    }
                    //else if (tBox_styleKNloc.Text == string.Empty)
                    //{
                    //    Blink(true, "Style KN location missing, please enter");
                    //}
                    else if (cBox_styleType.Text == string.Empty)
                    {
                        Blink(true, "Style type missing, please enter");
                    }
                    else
                    {
                        //user approval
                        LI.labelType = cBox_styleType.Text;
                        LI.learnStatus = true;
                        GV.labelComponentItem_coll.Add(LI);

                        //if note type note in the list then add to the list and the file
                        addNoteType();

                        if (selObjs.Where(item => item.styleName == LI.styleName).Any() == true)
                        {
                            selObjs.Remove(selObjs.Where(item => item.styleName == LI.styleName).Single());
                        }

                        //check if next item exsits if so move or else close
                        moveOrClose();
                        Blink(false, "");
                        UIH.toastIT("Style added successfully!", "Status", NotificationType.Success);
                    }
                }
                else // item already exists
                {
                    Blink(true, "Style already exists!");
                    moveOrClose();
                }
            }
            catch (System.Exception ex)
            { GH.writeLog(ex.ToString()); }
        }

        private void moveOrClose()
        {
            if (selObjs.Count <= 0)
            {
                //close the grid
                grid_addStyle.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                //move to next item
                loadStyleItem(1);
            }
        }
        private async void Blink(bool playAnimation, string content)
        {
            lbl_addStyleError.Content = content;
            while (playAnimation)
            {
                await Task.Delay(500);
                lbl_addStyleError.Foreground = lbl_addStyleError.Foreground == Brushes.Red ? Brushes.Black : Brushes.Red;
            }
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //close the grid
                grid_addStyle.Visibility = System.Windows.Visibility.Hidden;
                selObjs.Clear();
                Blink(false, "");
            }
            catch (System.Exception ex)
            { }
        }

        private void btn_clearAll_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                GV.labelComponentItem_coll.Clear();
            }
            catch (System.Exception ex)
            { }
        }

        private void btn_add_styletype_item_Click(object sender, RoutedEventArgs e)
        {
            addNoteType();
        }

        private void btn_del_styletype_item_Click(object sender, RoutedEventArgs e)
        {
            if (GV.noteTypeListColl.Contains(cBox_styleType.Text.ToString()))
            {
                GV.noteTypeListColl.Remove(cBox_styleType.Text.ToString());
                //remove from the ini file
                GH.updateNoteTypeList();
                cBox_styleType.SelectedIndex = 0;
            }
        }

        private void addNoteType()
        {
            if (!GV.noteTypeListColl.Contains(cBox_styleType.Text.ToString()))
            {
                GV.noteTypeListColl.Add(cBox_styleType.Text.ToString());
                //remove from the ini file
                GH.updateNoteTypeList();
            }
        }

        private void lBox_labels_zoomto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if the uesr in paper space if so swithc to model space
                Helper.ViewportExtensions.toggleModelspace();
                //MessageBox.Show("lBox_labels_zoomto_Click");
                GV.selLabels = new ObjectIdCollection();

                if (listView_styleComponentMapper.SelectedItems.Count > 0)
                {
                    List<Global.labelComponentItem> selectedItems = listView_styleComponentMapper.SelectedItems.Cast<Global.labelComponentItem>().ToList();

                    if (selectedItems[0].objID.ObjectClass.DxfName.ToString() == "MULTILEADER")
                    {
                        selectedItems = selectedItems.Reverse<Global.labelComponentItem>().Reverse().ToList();
                    }
                    foreach (Global.labelComponentItem item in listView_styleComponentMapper.SelectedItems)
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

                if (listView_styleComponentMapper.SelectedItems.Count > 0)
                {
                    foreach (Global.labelComponentItem item in listView_styleComponentMapper.SelectedItems)
                    {
                        GV.selLabels.Add(item.objID);
                    }

                    ObjectId[] ids = new ObjectId[listView_styleComponentMapper.SelectedItems.Count];
                    GV.selLabels.CopyTo(ids, 0);
                    Utils.SelectObjects(ids);
                    LCH.ZoomObjects(GV.selLabels);
                }

            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }
        
        private void styleNameHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIH.sortColumn(sender, listView_styleComponentMapper);

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

        private void NoteType_styleAssingmentHeader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIH.sortColumn(sender, listView_styleComponentMapper);
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

        private void btn_previousLabel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadStyleItem(-1);
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

        private void btn_nextLabel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loadStyleItem(1);
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

        int collIndex =0;

        private void loadStyleItem(int moverVal)
        {
            try
            {
                collIndex = collIndex + moverVal;
                if (collIndex <= selObjs.Count)
                {
                    Global.labelComponentItem item = (Global.labelComponentItem)selObjs[collIndex - 1];

                    tBox_styleName.Text = item.styleName;
                    Blink(false, "");
                }
                else
                {
                    loadStyleItem(-2);
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
                //close the grid
                grid_addStyle.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }

}

    

