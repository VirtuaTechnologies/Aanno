using C3D_Anno_Manager.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace C3D_Anno_Manager.Helper
{
    public class Helpers
    {
        public ObservableCollection<Files> GetFiles(string folderPath)
        {
            ObservableCollection<Files> files = new ObservableCollection<Files>();
            string[] fileEntries = Directory.GetFiles(folderPath);
            foreach (string fileName in fileEntries)
            {
                if (fileName.EndsWith(".xml"))
                {
                    Files file = new Files();
                    file.FileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    file.FilePath = fileName;
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
            List<NodeValues> nodelist = new List<NodeValues>();
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
                    nodelist = new List<NodeValues>();
                }
                notevalues = new NodeValues();
                notevalues.Name = node.Name.ToString();
                if (((XElement)node).FirstAttribute != null)
                notevalues.Number = Convert.ToInt16(((XElement)node).FirstAttribute.Value);
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
    }
}
