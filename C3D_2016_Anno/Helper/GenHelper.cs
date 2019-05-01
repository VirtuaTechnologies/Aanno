using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows;
using ZSharpQLogger;
using GV = C3D_2016_Anno.Global.variables;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Drawing;
using VSharpXMLHelper;

namespace C3D_2016_Anno.Helper
{
    //Mapper > XMLMapper, CSVMapper1, CSVMapper2
    //Notes > XMLNotes, CSVNotes1, CSVNotes2, 
    public class GenHelper
    {
        char[] extTrimChar = {'.'};
        public static void initializeSettings()
        {
            try
            {
                GV.clearCollection();
                getSettings();
                setPath();
                setLogFiles();
                //getFiles("all");

                
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        public static void getSettings()
        {
            try
            {

                if (File.Exists(Global.variables.settingsFile))
                {
                    GV.loaddll = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "loaddll"));
                    GV.logSwitch = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "logSwitch"));
                    GV.appDataPath = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "appDataPath");
                    GV.logFile = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "logFile");
                    GV.errorBoxSwitch = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "errorBoxSwitch"));
                    GV.debug = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "debug"));

                    if (bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "templatepathAuto")) == false)
                    {
                        GV.templatepath = xmlParser.getXMLValue(GV.settingsFile, "Settings", "name", "templatepath");
                    }
                    else
                    {
                        GV.templatepath = GV.templatepath = GV.appPath + @"\Templates";
                    }
                    GV.notenumberKey = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "notenumberKey").Split(',').ToList();
                    GV.labelcomponenttypes = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "labelcomponenttypes").Split(',').ToList();
                    GV.IncludeMultiLeader = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "IncludeMultiLeader"));
                    if (GV.IncludeMultiLeader)
                    {
                        GV.labelFilterType = "allLabels";
                        GV.MultiLeaderAtt = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "MultiLeaderAtt");
                        GV.notenumberKey.Add(GV.MultiLeaderAtt);
                    }
                    else
                    {
                        GV.labelFilterType = "civilLabels";
                    }
                    GV.keynotetexttype = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "keynotetexttype");
                    GV.keynoteSeperator = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "keynoteSeperator").Replace("[nbsp]", " ");
                    GV.sucessToast = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "sucessToast"));
                    GV.errorToast = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "errorToast"));
                    GV.infoToast = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "infoToast"));
                    GV.showViewportBoundary = bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "showViewportBoundary"));
                    if(bool.Parse(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "CSVfilecharReplace")))
                    {
                        GV.CSVfilecharReplace = "\"";
                    }
                    else
                    {
                        GV.CSVfilecharReplace = "[noval]";
                    }
                    //GV.xmlManPath = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "xmlManPath");
                    tempalteFiles = new List<string>();
                    mapperFiles = new List<string>();

                    //set the defnition and mapper file format xml or csv
                    GV.templateFileExt = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "templateFileExt");
                    GV.MapperFileExt = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "MapperFileExt");

                    //delimiters
                    GV.templateFileDelimiter = Convert.ToChar(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "templateFileDelimiter"));
                    GV.MapperFileDelimiter = Convert.ToChar(xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "MapperFileDelimiter"));

                    GV.XMLTemplateAtt = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "XMLTemplateAtt");
                    GV.XMLMapperAtt = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "XMLMapperAtt");
                    //remove double q
                    
                }
                
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        public static void setPath()
        {
            try
            {
                //check app path is created if not create it
                createFolder(GV.appDataPath);
                createFolder(Path.GetDirectoryName(GV.logFile));

                
            }
            catch (System.Exception ex) { }
        }
        public static void errorBox(string message)
        {
            if (GV.errorBoxSwitch)
                MessageBox.Show(message);
        }

        private static Global.fileItem FI;
        private static List<string> tempalteFiles;
        private static List<string> mapperFiles;
        public static Global.fileItem getFileObject(string file, string type)
        {
            try
            {
                Global.fileItem FI = new Global.fileItem();
                FI.filePath = file;
                FI.fileName = Path.GetFileNameWithoutExtension(file).Replace(".", string.Empty);
                FI.type = type;
                FI.checkSum = GetMD5HashFromFile(file);

                if (type == "template")
                {
                    GV.templateFiles.Add(FI);
                }
                else
                {
                    GV.mapperFiles.Add(FI);
                }

            }
            catch (System.Exception ex) { }
            return FI;
        }

        public static void valdiateFiles(ObservableCollection<Global.fileItem> fileColl)
        {
            try
            {
                List<Global.fileItem> removeList = new List<Global.fileItem>();
                foreach(var file in fileColl)
                {
                    if (!File.Exists(file.filePath))
                        removeList.Add(file);
                }

                if (removeList.Count > 0)
                {
                    foreach (var file in removeList)
                    {

                    }
                }
            }
            catch (System.Exception ex) { }
        }
        public static void getFiles(string action)
        {
            try
            {
                #region CSV Parser
                //set file extension to ser
                #endregion
                #region XML Parser
                if (action == "template" || action == "all")
                {
                    GV.templateFiles.Clear();
                    //get all the template files
                    
                    foreach (string file in Directory.GetFiles(GV.templatepath, "*." + GV.templateFileExt, System.IO.SearchOption.AllDirectories))
                    {
                        getFileObject(file, "template");
                    }
                }

                if (action == "mapper" || action == "all")
                {
                    GV.mapperFiles.Clear();
                    //get all mapper files
                    foreach (string file in Directory.GetFiles(GV.templatepath, "*." + GV.MapperFileExt, System.IO.SearchOption.AllDirectories))
                    {
                        getFileObject(file, "mapper");
                    }

                    foreach (Global.fileItem mapFile in GV.mapperFiles)
                    {
                        if (mapFile.fileName == "Common")
                        {
                            GV.commonMapper = mapFile;
                        }
                    }
                } 
                #endregion
            }
            catch (System.Exception ex) { }
        }
        public static void createFolder(string path)
        {
            try
            {
                //check app path is created if not create it
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (System.Exception ex) { }
        }

        public static void getStyleStructureFileDetails(string filePath)
        {
            try
            {
                //clear collection
                GV.labelComponentItem_coll.Clear();
                Dictionary<string, string> result = new Dictionary<string, string>();
                result = xmlParser.getXMLVaulesStrings(filePath, "STYLE", "name");

                foreach(var item in result)
                {
                    Global.labelComponentItem LI = new Global.labelComponentItem();
                    LI.styleName = item.Key;
                    LI.KNComponentID = new List<int>(Array.ConvertAll(item.Value.Split(','), int.Parse));
                    GV.labelComponentItem_coll.Add(LI);
                }
            }
            catch (System.Exception ex) { }
        }
        public static void getTemplateDetails(string file)
        {
            try
            {
                GV.NoteColl.Clear();
                GV.notesDict.Clear();

                //GV.notesDict.Clear();
                GV.templateFileExt = Path.GetExtension(file).Replace(".", string.Empty);


                if (GV.templateFileExt == "XMLNotes")
                {
                    #region XML
                    //getFiles();
                    

                    writeLog("\n --------------FILE: " + file);
                    //get the keys
                    List<string> keys = xmlParser.getXMLKeys(file, "KeyNotes");

                    foreach (var key in keys)
                    {
                        extractNotes(file, key, GV.XMLTemplateAtt);//
                    }
                    //List<string> val = xmlParser.getXMLVaules(file, "SEWERNOTES", "name", "note");
                    //print values
                    foreach (var item in GV.notesDict)
                    {
                        writeLog("\n KEY: " + item.Key);
                        Dictionary<int, string> dictfromXML = new Dictionary<int, string>();
                        dictfromXML = item.Value;
                        foreach (var noteitem in dictfromXML)
                        {
                            writeLog("\n ======: " + noteitem.Key + "== " + noteitem.Value);
                        }
                    } 
                    #endregion
                }
                else if (GV.templateFileExt == "CSVNotes1")
                {
                    #region CSV File
                    using (StreamReader sr = new StreamReader(file))
                    {
                        GV.notesDict.Clear();
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            List<string> vals = new List<string>();
                            vals = currentLine.Split(GV.templateFileDelimiter).ToList();

                            //check if the key exitst in main dict
                            var dictCheckOut = new Dictionary<int, string>();
                            string dictCheckOut1;

                            if (GV.notesDict.TryGetValue(vals[0].Replace(GV.CSVfilecharReplace, ""), out dictCheckOut))
                            {

                                if (!GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].TryGetValue(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), out dictCheckOut1))
                                {
                                    GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].Add(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), vals[2].Replace(GV.CSVfilecharReplace, ""));
                                }
                            }
                            else
                            {
                                Dictionary<int, string> noteItem = new Dictionary<int, string>();
                                string key = vals[0].Replace(GV.CSVfilecharReplace, "");
                                GV.notesDict.Add(key, noteItem);
                                var innerdict = GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")];
                                var innerDictKey = Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, ""));
                                if (!innerdict.TryGetValue(innerDictKey, out dictCheckOut1))
                                {
                                    GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].Add(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), vals[2].Replace(GV.CSVfilecharReplace, ""));
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region CSV File 2
                    GV.notesDict.Clear();
                    using (TextFieldParser MyReader = new TextFieldParser(file))
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
                            var dictCheckOut = new Dictionary<int, string>();
                            List<string> vals = new List<string>();
                            vals = currentRow.ToList<string>();
                            string dictCheckOut1;

                            if (GV.notesDict.TryGetValue(vals[0].Replace(GV.CSVfilecharReplace, ""), out dictCheckOut))
                            {

                                if (!GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].TryGetValue(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), out dictCheckOut1))
                                {
                                    GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].Add(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), vals[2].Replace(GV.CSVfilecharReplace, ""));
                                }
                            }
                            else
                            {
                                Dictionary<int, string> noteItem = new Dictionary<int, string>();
                                string key = vals[0].Replace(GV.CSVfilecharReplace, "");
                                GV.notesDict.Add(key, noteItem);
                                var innerdict = GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")];
                                var innerDictKey = Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, ""));
                                if (!innerdict.TryGetValue(innerDictKey, out dictCheckOut1))
                                {
                                    GV.notesDict[vals[0].Replace(GV.CSVfilecharReplace, "")].Add(Convert.ToInt16(vals[1].Replace(GV.CSVfilecharReplace, "")), vals[2].Replace(GV.CSVfilecharReplace, ""));
                                }
                            }

                            dt.Rows.Add(row);
                        }
                    }
                    #endregion
                }
            }
            catch(System.Exception ex) { }
        }
        public static void getMapper(string file)
        {
            try
            {
                GV.Mapper.Clear();
                
                GV.ObtTypes.Clear();
                GV.MapperFileExt = Path.GetExtension(file).Replace(".", string.Empty);

                if (GV.MapperFileExt == "XMLMapper")
                {
                    GV.Mapper = xmlParser.getXMLVaulesStrings(file, "STYLE", GV.XMLMapperAtt);
                }
                else if (GV.MapperFileExt == "CSVMapper1") // CSV file - Format -  "val1", "val 2 - here , this", "val3"
                {
                    string dictCheckOut1;
                    #region Process regular CSV file
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            List<string> vals = new List<string>();
                            vals = currentLine.Split(',').ToList();

                            //check if the key exitst in main dict
                            if (GV.Mapper.TryGetValue(vals[0], out dictCheckOut1))
                            {
                                GV.Mapper[vals[0]] = vals[1].Replace("\"", "");
                            }
                            else
                            {
                                GV.Mapper.Add(vals[0].Replace("\"", ""), vals[1].Replace("\"", ""));
                            }
                        }
                    }
                    #endregion
                }
                else // CSV file
                {
                    string dictCheckOut1;
                    #region Process regular CSV file
                    using (StreamReader sr = new StreamReader(file))
                    {
                        string currentLine;
                        // currentLine will be null when the StreamReader reaches the end of file
                        while ((currentLine = sr.ReadLine()) != null)
                        {
                            List<string> vals = new List<string>();
                            vals = currentLine.Split(GV.MapperFileDelimiter).ToList();

                            //check if the key exitst in main dict
                            if (GV.Mapper.TryGetValue(vals[0], out dictCheckOut1))
                            {
                                GV.Mapper[vals[0]] = vals[1].Replace(GV.CSVfilecharReplace, "");
                            }
                            else
                            {
                                GV.Mapper.Add(vals[0].Replace(GV.CSVfilecharReplace, ""), vals[1].Replace(GV.CSVfilecharReplace, ""));
                            }
                        }
                    } 
                    #endregion
                }

                #region Add obj type
                if (GV.Mapper.Count > 0)
                {
                    foreach (var mapItem in GV.Mapper)
                    {
                        if (!GV.ObtTypes.Contains(mapItem.Value))
                        {
                            GV.ObtTypes.Add(mapItem.Value);
                        }
                        qprint("\n Mapper ======: " + mapItem.Key + "== " + mapItem.Value);
                    }
                } 
                #endregion
            }
            catch (System.Exception ex) { qprint(ex.ToString()); }
        }
        public static void setLogFiles()
        {
            try
            {
                
                if (File.Exists(GV.logFile))
                {
                    File.Delete(GV.logFile);
                    writeLog("Deleting Log");
                }
                File.Create(GV.logFile).Close();
                writeLog("Creating Log");
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
        }

        public static logSetting logSet;
        public static void writeLog(string mess)
        {
            if (GV.logSwitch)
            {
                logSet = new logSetting(GV.appName, GV.devDetails, "", GV.logFile, true);
                LogIT.write(logSet, mess);
            }
        }

        string reg_Install = "Install";
        string reg_Use = "Use";
        string reg_sys = "Lisp";
        string guid = string.Empty;
        public static void regSetup()
        {
            try
            {
                GV.regPath = @"Software\" + GV.globalPath + "\\" + GV.appName;
                RegistryKey regkey = Registry.CurrentUser;
                regkey = regkey.CreateSubKey(GV.regPath); //path
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
        public static ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public static BitmapImage getBitmap(string imageName, int Height, int Width)
        {
            BitmapImage image = new BitmapImage();
            string[] vals = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            image.BeginInit();
            image.StreamSource
                = Assembly.GetExecutingAssembly().GetManifestResourceStream(imageName);
            // image.UriSource = new Uri(imageName);
            image.DecodePixelHeight = Height;
            image.DecodePixelWidth = Width;
            image.EndInit();

            return image;
        }
        private void firstTime()
        {
            GV.regPath = @"Software\" + GV.globalPath + "\\" + GV.appName;
            RegistryKey regkey = Registry.CurrentUser;
            //string encrp_global = EncriptionDecription.Encrypt(globalPath, SetKey());
            //string encrp_install = EncriptionDecription.Encrypt(reg_Install, SetKey());
            //string encrp_use = EncriptionDecription.Encrypt(reg_Use, SetKey());
            //string encrp_tickcad = EncriptionDecription.Encrypt(reg_TICKCAD, SetKey());

            //  string encrp_version = EncriptionDecription.Encrypt(guid, SetKey());
            regkey = regkey.CreateSubKey(@"Software\" + GV.regPath); //path

            DateTime dt = DateTime.Now;
            string onlyDate = dt.ToShortDateString(); // get only date not time
                                                      //  string encrp_onlyDate = EncriptionDecription.Encrypt(dt.ToShortDateString(), SetKey());
            regkey.SetValue(reg_Install, onlyDate); //Value Name,Value Data
            regkey.SetValue(reg_Use, onlyDate); //Value Name,Value Data
            regkey.SetValue(reg_sys, guid);
        }

        public static void qprint(string message)
        {
            if (GV.debug)
            {
                printDebug("", message, false, false);
            }
        }
        public static void printDebug(string prefix, string message, bool topspliter, bool bottomsplitter)
        {
            try
            {
                if (GV.debug)
                {
                    if (topspliter)
                        GV.ed.WriteMessage("\n ================================================");
                    GV.ed.WriteMessage("\n " + prefix + ": " + message);
                    if (bottomsplitter)
                        GV.ed.WriteMessage("\n ================================================ \n");
                }

            }
            catch (System.Exception ex) { GV.ed.WriteMessage("\n ERROR: " + ex); }
        }
        public static void extractNotes(string file, string XMLKey, string att)
        {
            try
            {
                Dictionary<int, string> dictfromXML = new Dictionary<int, string>();
                dictfromXML = xmlParser.getXMLVaulesSpec(file, XMLKey, att);

                Dictionary<int, string> dictCheckOut1 = new Dictionary<int, string>();
                //create a dictionary or use existing.
                //check if the dictionary exists
                if (!GV.notesDict.TryGetValue(XMLKey, out dictCheckOut1))
                {
                    GV.notesDict.Add(XMLKey, dictfromXML);
                }
                else
                {
                    GV.notesDict[XMLKey] = dictfromXML;
                }

            }
            catch (System.Exception ex) { }
        }

        public static void loaddlls(string dll)
        {
            try
            {
                writeLog("Loading dll...." + dll);
                if (File.Exists(dll))
                {
                    writeLog("dll Found (" + dll + ")");
                    using (GV.Doc.LockDocument()) // this is needed from a modeless form
                    {

                        //SendStringToExecute
                        writeLog(dll);
                        Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();
                        string CommandParameter = "(command \"NETLOAD\"" + dll + "\" )";
                        writeLog("COMMNAND: " + CommandParameter);
                        //GV.Doc.SendStringToExecute(CommandParameter, true, false, false);
                        Assembly.LoadFrom(dll);
                    }
                }
                else
                {
                    writeLog("Cannot Find Asembly!" + dll);
                }
                writeLog("Loading dll Complete....");
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                writeLog(ex.ToString());
            }
            catch (System.Exception ex)
            {
                writeLog(ex.ToString());
            }
        }

        private static string res;
        public static string getnumberonlyValuefromList(List<string> list)
        {
            
            foreach(string str in list)
            {
                if (IsDigitsOnly(str))
                    res = str;
            }
            return res;
        }
        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static void updateSeetings(string field, string value)
        {
            xmlWriter.editXML(GV.settingsFile, "App/Settings", "name", field, value);
        }


    }
}
