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

        private void btn_browse_stylemapperFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "DWG files (*.dwg; *.dwt)|*.dwg; *.dwt";
                if (openFileDialog.ShowDialog() == true)
                    tBox_stylemapperFile.Text = openFileDialog.FileName;


            }
            catch (System.Exception ex)
            { }
        }

        private void btn_read_stylemapperFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (System.Exception ex)
            { }
        }
       
        private void btn_learn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GV.labelComponentItem_coll.Clear();
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
                            foreach(var item in CompNameVals)
                            {
                                if(item.Value == "99")
                                {
                                    LI.KNComponentID = Convert.ToInt32(item.Key);
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

       
    }
}
