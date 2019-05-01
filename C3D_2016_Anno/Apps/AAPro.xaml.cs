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

namespace C3D_2016_Anno.Apps
{
    /// <summary>
    /// Interaction logic for AAPro.xaml
    /// </summary>
    public partial class AAPro : System.Windows.Controls.UserControl
    {
        public static string appName = "AAnno Pro";
        public AAPro()
        {
            InitializeComponent();
            GV.labelComponentItem_coll.Clear();
            listView_styleComponentMapper.ItemsSource = GV.labelComponentItem_coll;
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
                    GH.getStyleStructureFileDetails(tBox_stylemapperFile.Text);
                }


            }
            catch (System.Exception ex)
            { }
        }

        private void btn_save_styleStructureFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //save the mapping information in XML format to the style mapper file.

            //check if a file selected
            if(File.Exists(tBox_stylemapperFile.Text))
                {
                    //update the existing file.
                    //this will erase whats on the file and update with the current view content.

                }
            else //ask the user to create new file or select and existing file.
                {
                    MessageBox.Show("Unable to save to file, please check if the file exists!");
                }
            }
            catch (System.Exception ex)
            { }
        }
       
        private void btn_learn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //GV.labelComponentItem_coll.Clear();
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
                            Global.labelComponentItem LI = new Global.labelComponentItem();
                            LI.styleName = LCH.getLabelName(objID);
                            LI.objType = LCH.getObjType(objID);

                            GV.ed.WriteMessage("LI.name: " + LI.styleName);
                            GV.ed.WriteMessage("LI.objType: " + LI.objType);

                            //get the component id which has the value 99
                            Dictionary<string, string> CompNameVals = new Dictionary<string, string>();
                            CompNameVals = Helper.LabelTextExtractor.getLabelValsAll(objID);

                            //get the location of the value and store it against the style name and id.
                            int i = 0;
                            LI.KNComponentID = new List<int>();

                            foreach (var item in CompNameVals)
                            {
                                if(item.Value == "99")
                                {
                                    LI.KNComponentID.Add(Convert.ToInt32(item.Key));
                                    i++;
                                }
                            }
                            GV.labelComponentItem_coll.Add(LI);
                            // ask the user to pick the mapper configuration 

                            //store the configuration to settings file

                        }
                    }
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void btn_learn_all_Click(object sender, RoutedEventArgs e)
        {
            GH.errorBox("This function is work in progress! check again later.");
        }

        private void btn_delete_style_item_Click(object sender, RoutedEventArgs e)
        {
            if(GV.labelComponentItem_coll.Contains((Global.labelComponentItem)listView_styleComponentMapper.SelectedItem))
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
    }
}
