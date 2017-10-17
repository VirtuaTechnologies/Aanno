using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZSharpQLogger;
using ZSharpXMLHelper;
using GV = C3D_2016_Anno.Global.variables;

namespace C3D_2016_Anno.Helper
{
    public class GenHelper
    {
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
                    //GV.xmlManPath = xmlParser.getXMLValue(Global.variables.settingsFile, "Settings", "name", "xmlManPath");
                    tempalteFiles = new List<string>();
                    mapperFiles = new List<string>();
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
                FI.fileName = Path.GetFileNameWithoutExtension(file);
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
                if (action == "template" || action == "all")
                {
                    GV.templateFiles.Clear();
                    //get all the template files
                    foreach (string file in Directory.GetFiles(GV.templatepath, "*.xml", SearchOption.AllDirectories))
                    {
                        //Global.fileItem FI = new Global.fileItem();
                        //FI.filePath = file;
                        //FI.fileName = Path.GetFileNameWithoutExtension(file);
                        //FI.type = "mapper";
                        //FI.checkSum = GetMD5HashFromFile(file);
                        getFileObject(file, "template");

                    }


                }


                if (action == "mapper" || action == "all")
                {
                    GV.mapperFiles.Clear();
                    //get all mapper files
                    foreach (string file in Directory.GetFiles(GV.templatepath, "*.Mapper", SearchOption.AllDirectories))
                    {
                        //Global.fileItem FI = new Global.fileItem();
                        //FI.filePath = file;
                        //FI.fileName = Path.GetFileNameWithoutExtension(file);
                        //FI.checkSum = GetMD5HashFromFile(file);
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

                //common mapper


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
        public static void getTemplateDetails(string file)
        {
            try
            {
                //getFiles();
                GV.NoteColl.Clear();

                writeLog("\n --------------FILE: " + file);
                //get the keys
                List<string> keys = xmlParser.getXMLKeys(file, "KeyNotes");

                foreach (var key in keys)
                {
                    extractNotes(file, key, "number");
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

                //get all the template files with ext XML
                //foreach (string file in Directory.GetFiles(GV.templatepath, "*.xml", SearchOption.AllDirectories))
                //{

                //}
            }
            catch(System.Exception ex) { }
        }
        public static void getMapper(string file)
        {
            try
            {
                GV.Mapper.Clear();
                GV.Mapper = xmlParser.getXMLVaulesStrings(file, "STYLE", "key");
                GV.ObtTypes.Clear();

                foreach (var mapItem in GV.Mapper)
                {
                    if(!GV.ObtTypes.Contains(mapItem.Value))
                    {
                        GV.ObtTypes.Add(mapItem.Value);
                    }

                    qprint("\n Mapper ======: " + mapItem.Key + "== " + mapItem.Value);
                }
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
                 

                //create a dictionary or use existing.
                //check if the dictionary exists
                if (!GV.notesDict.ContainsKey(XMLKey))
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
    }
}
