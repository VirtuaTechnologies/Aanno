using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C3D_Anno_Manager.Data
{
    public class Nodes
    {
        public string Note { get; set; }

        public List<NodeValues> NoteValues { get; set; }
    }
    public class NodeValues
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Value { get; set; }

    }
}
