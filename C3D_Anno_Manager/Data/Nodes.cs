using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace C3D_Anno_Manager.Data
{
    [XmlRoot(ElementName = "KeyNotes")]
    public class Nodes
    {
   
        public string Note { get; set; }

        [XmlElement(ElementName = "Sample" )]
        public ObservableCollection<NodeValues> NoteValues { get; set; }
    }
    public class NodeValues
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string Value { get; set; }
    }    
}
