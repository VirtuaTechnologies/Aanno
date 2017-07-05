//This is an Intelectual Property of Zcodia Technologies and Raghulan Gowthaman.
//www.zcodiatechnologies.com.au


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
using MahApps.Metro.Controls;
using C3D_Anno_Manager.Data;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace C3D_Anno_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Nodes> masterList = new ObservableCollection<Nodes>();
        public MainWindow()
        {
            dynamic parser = new DynamicXmlParser("ProjectA-Template.xml");
            InitializeComponent();
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
                    masterList.Add(master);
                    master = new Nodes();
                    nodelist = new List<NodeValues>();
                }
                notevalues = new NodeValues();
                notevalues.Name = node.Name.ToString();
                notevalues.Number = Convert.ToInt16(((XElement)node).FirstAttribute.Value);
                notevalues.Value = ((XElement)node).FirstNode.ToString();
                node = (XElement)node.NextNode;
                nodelist.Add(notevalues);
                if (node == null)
                {
                    master.Note = notevalues.Name;
                    master.NoteValues = nodelist;
                    masterList.Add(master);
                }
            }
  
            noteTypeListBox.ItemsSource = masterList;  
        }

        private void listbox_Item_Clicked(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = (Nodes)noteTypeListBox.SelectedItem;
            listOfNoteValues.ItemsSource = selectedItem.NoteValues;
        }
    }
}
