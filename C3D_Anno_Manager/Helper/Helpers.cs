using C3D_Anno_Manager.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using MahApps.Metro.Controls.Dialogs;

namespace C3D_Anno_Manager.Helper
{
    public class Helpers
    {
        public ObservableCollection<Files> GetFiles(string folderPath, string extension)
        {
            ObservableCollection<Files> files = new ObservableCollection<Files>();
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith(extension))
                {
                    Files file = new Files();
                    file.FileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    file.FilePath = fileName;
                    file.ModifiedDate = File.GetLastWriteTime(fileName).ToString();
                    files.Add(file);
                }
            }
            return files;
        }

        public ObservableCollection<Nodes> ParseXMLFile(string filePath)
        {
            ObservableCollection<Nodes> masterNodes = new ObservableCollection<Nodes>();
            dynamic parser = new DynamicXmlParser(filePath);
            Nodes master = new Nodes();
            ObservableCollection<NodeValues> nodelist = new ObservableCollection<NodeValues>();
            NodeValues notevalues = new NodeValues();
            var Name = (((DynamicXmlParser)parser));
            var name = Name.element.Name.LocalName;
            var node = (XElement)Name.element.FirstNode;

            while (node != null)
            {
                if ((node.Name.ToString() != notevalues.Name) && node.PreviousNode != null)
                {
                    master.Note = notevalues.Name;
                    master.NoteValues = nodelist;
                    masterNodes.Add(master);
                    master = new Nodes();
                    nodelist = new ObservableCollection<NodeValues>();
                }
                notevalues = new NodeValues();
                notevalues.Name = node.Name.ToString();
                if (((XElement)node).FirstAttribute != null)
                notevalues.Number = ((XElement)node).FirstAttribute.Value;
                notevalues.Value = ((XElement)node).FirstNode.ToString();
                node = (XElement)node.NextNode;
                nodelist.Add(notevalues);
                if (node == null)
                {
                    master.Note = notevalues.Name;
                    master.NoteValues = nodelist;
                    masterNodes.Add(master);
                }
            }
            return masterNodes;
        }

        public bool XMLToObject(string currentfile, ObservableCollection<Nodes> masterList)
        {
            var doc = new XmlDocument();

            XElement element = new XElement("Keynotes");

            try
            {
            
            using (var writer = new System.IO.StreamWriter(currentfile))
            {
                foreach (Nodes n in masterList)
                {
                    foreach (NodeValues nv in n.NoteValues)
                    {
                        var newelement = new XElement(nv.Name, nv.Value);
                        newelement.SetAttributeValue("number", nv.Number);
                        element.Add(newelement);
                    }
                }
                element.Save(writer);
            }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ExportToXML(ObservableCollection<Nodes> nodesToExport)
        {
            Microsoft.Win32.SaveFileDialog savefile = new Microsoft.Win32.SaveFileDialog();
            savefile.FileName = "Document";
            savefile.DefaultExt = ".xml";
            savefile.Filter = "XML documents (.xml)|*.xml";
            Nullable<bool> saveResult = savefile.ShowDialog();
            var result = false;
            if (saveResult == true)
            {
                 result = XMLToObject(savefile.FileName, nodesToExport);
            }
            return result;
        }
        public string ExportToMapper(Nodes nodesToExport , string file)
        {
            var status = "error";
            if (file == null)
            {
                Microsoft.Win32.SaveFileDialog savefile = new Microsoft.Win32.SaveFileDialog();
                savefile.FileName = "Document";
                savefile.DefaultExt = ".Mapper";
                savefile.Filter = "Mapper documents (.Mapper)|*.Mapper";
                Nullable<bool> saveResult = savefile.ShowDialog();
                if (saveResult == true)
                {
                    var result = ObjectToMapper(savefile.FileName, nodesToExport);
                    if (result)
                    {
                        status = "mappercreated";
                    }
                    else
                    {
                        status = "createfailed";
                    }
                }
            }
            else
            {
                var result = ObjectToMapper(file, nodesToExport);
                if (result)
                {
                    status = "mapperupdated";
                }
                else
                {
                    status = "updatefailed";
                }
            }
            return status;
        }
    

        public bool ObjectToMapper(string currentfile, Nodes masterList)
        {
            var doc = new XmlDocument();

            XElement element = new XElement("App");

            try
            {

                using (var writer = new System.IO.StreamWriter(currentfile))
                {                   
                        foreach (NodeValues nv in masterList.NoteValues)
                        {
                            var newelement = new XElement(nv.Name, nv.Value);
                            newelement.SetAttributeValue("key", nv.Number);
                            element.Add(newelement);
                        }
                    element.Save(writer);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ImportFromXML(ObservableCollection<Nodes> masterList)
        {
            using (var fbd = new OpenFileDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    ObservableCollection<Nodes> importNodes = new ObservableCollection<Nodes>();
                    importNodes = ParseXMLFile(fbd.FileName);
                    foreach (Nodes newNode in importNodes)
                    {
                        if (!masterList.Any(mlist => mlist.Note == newNode.Note))
                        {
                            masterList.Add(newNode);
                        }
                        else
                        {
                            Nodes matchNode = new Nodes();
                            matchNode = masterList.Where(ml => ml.Note == newNode.Note).First();
                            var index = masterList.IndexOf(matchNode);
                            foreach (NodeValues nodeValue in newNode.NoteValues)
                            {
                                if (!matchNode.NoteValues.Any(nv => nv.Number == nodeValue.Number))
                                {
                                    masterList[index].NoteValues.Add(nodeValue);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
