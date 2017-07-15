using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Civil.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C3D_2016_Anno.Global
{
    public class variables
    {
        #region - Application Details
        public static string appName = "Anno";
        public static string devDetails = "Raghulan Gowthaman";
        public static string delimiter = "_";
        public static string nameSeperator = "_";
        public static string globalPath = @"VL\Protection";
        public static string regPath = "";
        #endregion

        #region Switch
        public static bool logSwitch;
        public static bool loaddll = false;
        public static bool debug = true;
        public static bool errorBoxSwitch = false;
        public static bool dbPathAuto = false;
        public static int ErrorReportingLevel = 1;
        public static bool sucessToast = false;
        public static bool errorToast = false;
        public static bool infoToast = false;
        public static bool showViewportBoundary = false;
        #endregion

        #region - Path
        public static string tempPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
        public static string appPath = tempPath.Substring(6, tempPath.Length - 6);
        public static string Constr = "Data Source=_dfgdb.s3db;Version=3;";
        public static string settingsFile = appPath + @"\Data\Settings.xml";
        public static string templatepath = appPath + @"\Templates";
        public static string xmlManPath = appPath + @"\XMLMan\C3D_Anno_Manager.exe";
        public static string logFile;
        public static string appDataPath;

        #endregion

        #region App Vars
        public static fileItem commonMapper;
        public static string keynotetexttype = "mtext";
        public static double textHeight = 1;
        public static bool IncludeMultiLeader = false;
        public static string MultiLeaderAtt = "TAGNUMBER";
        public static string labelFilterType = "allLabels";
        public static string currentFont;
        public static double pBarMaxVal;
        public static double pBarCurrentVal;
        public static string pBarStatus;
        public static bool processStatus;
        public static bool dllLoadStatus;
        public static ProgressMeter pmeter = new ProgressMeter();
        public static string keynoteSeperator;
        #endregion

        #region ACAD Vars
        public static Editor ed;
        public static Document Doc;
        public static Database Db;
        public static Viewport SelectedViewport;
        public static PromptEntityResult propmptRes;
        public static ObjectId viewportShpObj;
        #endregion

        #region C3D Vars
        public static CivilDocument CDoc;
        #endregion

        #region Collections
        public static List<string> notenumberKey = new List<string>();
        public static List<string> labelcomponenttypes = new List<string>();
        public static Dictionary<string, Dictionary<int, string>> notesDict = new Dictionary<string, Dictionary<int, string>>();
        public static Dictionary<int, string> PAVINGNOTES = new Dictionary<int, string>();
        public static Dictionary<string, List<NoteRec>> NoteColl = new Dictionary<string, List<NoteRec>>();
        public static Dictionary<string, string> Mapper = new Dictionary<string, string>();
        public static ObservableCollection<fileItem> templateFiles = new ObservableCollection<fileItem>();
        public static ObservableCollection<string> SelectedObjTypes = new ObservableCollection<string>();
        public static ObservableCollection<ObjectId> selObjIds = new ObservableCollection<ObjectId>();
        public static Dictionary<string, Dictionary<string, string>> allnotes= new Dictionary<string, Dictionary<string, string>>();
        public static Dictionary<string, Dictionary<string, string>> noteTypesCurrent = new Dictionary<string, Dictionary<string, string>>();
        public static ObservableCollection<fileItem> mapperFiles = new ObservableCollection<fileItem>();
        public static ObservableCollection<string> ObtTypes = new ObservableCollection<string>();
        public static ObjectIdCollection selLabels;
        public static ObjectId[] selObjects_forProcessing;
        public static Dictionary<string, object> ui_controls_Coll = new Dictionary<string, object>();
        public static ObservableCollection<labelItem> all_label_coll = new ObservableCollection<labelItem>();
        public static Dictionary<string, List<labelItem>> all_label_coll_Sorted = new Dictionary<string, List<labelItem>>();

        public  static void clearCollection()
        {
            noteTypesCurrent.Clear();
            allnotes.Clear();
            commonMapper = null;
            templateFiles.Clear();
            mapperFiles.Clear();
        }

        public static void clearSelection()
        {
            SelectedObjTypes.Clear();
            noteTypesCurrent.Clear();
            ObtTypes.Clear();
            all_label_coll.Clear();
            all_label_coll_Sorted.Clear();
            allnotes.Clear();
        }
        #endregion
    }

    public class NoteRec
    {
        public string noteID { get; set; }
        public string NoteCategory { get; set; }
        public string note { get; set; }
        public string fileName { get; set; }
    }

    public class fileItem
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string type { get; set; }
        public string checkSum { get; set; }
    }

    public class labelItem
    {
        public string name { get; set; }
        public string prefix { get; set; }

        public string objType { get; set; }
        public int noteNumber { get; set; }
        public Dictionary<string, string> properties { get; set; }
        public ObjectId objID { get; set; }
        public string note { get; set; }
        public bool noteFound { get; set; }
    }
}
