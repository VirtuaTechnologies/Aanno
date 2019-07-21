//Developed by AutoAnno, LLC, Copyright 2010-2018, all rights reserved. www.autoanno.com
//www.autoanno.com


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
using MahApps.Metro;
using MahApps.Metro.Controls;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Xml.Linq;
using System.Xml;

namespace AnnoTesterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //GH.initializeSettings();
            //fetchDATA();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void lBox_labels_zoomto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("lBox_labels_zoomto_Click");

            }
            catch (System.Exception ee)
            {
                ///GH.writeLog(ee.ToString());
            }

        }
        private void lBox_labels_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("lBox_labels_zoomto_Click");

            }
            catch (System.Exception ee)
            {
                //GH.writeLog(ee.ToString());
            }

        }

        public void fetchDATA()
        {
            try
            {
                
            }
            catch (System.Exception ee)
            {
                //GH.writeLog(ee.ToString());
            }
        }

        #region Files
        private void btn_fetchtamplateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //GH.getFiles("template");
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void btn_fetchmapperFiles_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //GH.getFiles("mapper");
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void cBox_template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void cBox_Mapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        #endregion

        private void btn_readTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("DefinitionSample-Space_spl.def"))
                {
                    Dictionary<string, Dictionary<int, string>> notesDict = new Dictionary<string, Dictionary<int, string>>();
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        List<string> vals = new List<string>();
                        vals = currentLine.Split('>').ToList();

                        //check if the key exitst in main dict
                        if(notesDict.ContainsKey(vals[0]))
                        {
                            if (!notesDict[vals[0]].ContainsKey(Convert.ToInt16(vals[1])))
                            {
                                notesDict[vals[0]].Add(Convert.ToInt16(vals[1]), vals[2].Replace("\"", ""));
                            }
                            
                        }
                        else
                        {
                            string dictCheckOut1;

                            Dictionary<int, string> noteItem = new Dictionary<int, string>();
                            string key = vals[0].Replace("\"", "");

                            notesDict.Add(vals[0].Replace("\"", ""), noteItem);

                            var innerdict = notesDict[key];
                            var innerDictKey = Convert.ToInt16(vals[1].Replace("\"", ""));

                            if (!innerdict.TryGetValue(innerDictKey, out dictCheckOut1))
                            {
                                notesDict[vals[0]].Add(Convert.ToInt16(vals[1]), vals[2].Replace("\"", ""));
                            }
                        }

                        

                    }
                }
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void btn_readMapper_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader("MapSample.map"))
                {
                    Dictionary<string, string> mapDict = new Dictionary<string, string>();
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        List<string> vals = new List<string>();
                        vals = currentLine.Split('>').ToList();

                        //check if the key exitst in main dict
                        if (mapDict.ContainsKey(vals[0]))
                        {
                            mapDict[vals[0]] = vals[1];

                        }
                        else
                        {
                            mapDict.Add(vals[0],vals[1]);
                        }
                    }
                }
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void btn_readMap2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TextFieldParser MyReader = new TextFieldParser("CSV FMT.qmap"))
                {
                    Dictionary<string, string> mapDict = new Dictionary<string, string>();
                    MyReader.TextFieldType = FieldType.Delimited;
                    MyReader.SetDelimiters(",");
                    MyReader.HasFieldsEnclosedInQuotes = true;
                    string[] currentRow;
                    DataTable dt = new DataTable();
                    while (!MyReader.EndOfData)
                    {
                        DataRow row = dt.NewRow();
                        currentRow = MyReader.ReadFields();
                        for (int i = 0; i < currentRow.Length; i++)
                        {

                            List<string> vals = new List<string>();
                            vals = currentRow.ToList<string>();
                            //mapDict[i] = vals[i];
                        }
                        dt.Rows.Add(row);
                    }
                }

               
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void btn_readTemp2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_createXML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<string, string> keyvalue = new Dictionary<string, string>();
                keyvalue.Add("IRR", "112");
                keyvalue.Add("SW", "23");
                keyvalue.Add("STR", "2");
                keyvalue.Add("MPR", "45");

                // instantiate XmlDocument and load XML from file
                XDocument d = new XDocument(new XComment("VSHARP XML Writer Library"));
                d.Declaration = new XDeclaration("1.0", "utf-8", "true");

                XElement mainKey_Element = new XElement("STYLE");

                foreach (var item in keyvalue)
                {
                    XElement subKey_Element = new XElement("APP", new XAttribute("name", item.Key), item.Value);
                    mainKey_Element.Add(subKey_Element);
                }

                d.Add(mainKey_Element);

                //if (System.IO.File.Exists(xmlFile))
                //{
                //    System.IO.File.Delete(xmlFile);
                //    d.Save(xmlFile);
                //}



                MessageBox.Show(d.ToString());
            }
            catch(System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_createCSV_Click(object sender, RoutedEventArgs e)
        {
           
            System.IO.StreamReader file = new System.IO.StreamReader(@"R:\GitHub\Aanno\TestFiles\20190630 - SST\TEST.sst");
            //Execute a loop over the rows.  
            string line;

            while ((line = file.ReadLine()) != null)
            {
                string[] linarr = line.Split(Convert.ToChar('|'));

                foreach (string item in linarr)
                {
                    
                    //check the style name if it matches and get rest of the data.
                    if (item == "WP-O-P-Sta & Offset W-LT 2CIRC")
                    {
                        string noteType = linarr[1];
                        string KNLoc = linarr[2];
                    }
                }
            }
            file.Close();
     
        }
    }
}
