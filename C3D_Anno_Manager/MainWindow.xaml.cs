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
using System.Windows.Forms;
using System.IO;
using C3D_Anno_Manager.Helper;
using System.Xml.Serialization;
using System.Xml;
using Newtonsoft.Json;
using System.Dynamic;

namespace C3D_Anno_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Nodes> masterList = new ObservableCollection<Nodes>();
        Helpers helpers = new Helpers();
        public string currentfile { get; set; }
        public MainWindow()
        {

            InitializeComponent();
        }

        private void listbox_Item_Clicked(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = sender as DataGridRow; ;
            var itemsToDisplay = (Nodes)selectedItem.Item;
            listOfNoteValues.ItemsSource = itemsToDisplay.NoteValues;
        }

        private void Open_FileDialog(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folderPath.Text = fbd.SelectedPath;
                }
            }
        }

        private void Scan_Folders(object sender, RoutedEventArgs e)
        {

            listOfFilesListBox.ItemsSource = helpers.GetFiles(folderPath.Text);
        }
        private void xmlfile_Item_Clicked(object sender, RoutedEventArgs e)
        {

            var selectedItem = sender as DataGridRow;
            var fileToParse = (Files)selectedItem.Item;
            currentfile = fileToParse.FilePath;
            masterList = helpers.ParseXMLFile(fileToParse.FilePath);
            noteTypeListBox.ItemsSource = masterList;
            listOfNoteValues.ItemsSource = new ObservableCollection<NodeValues>();
        }
        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Nodes newNode = new Nodes();
            newNode.Note = addNoteItemTextBox.Text;
            masterList.Add(newNode);
            addNoteItemTextBox.Clear();
        }

        private void deleteNoteItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = noteTypeListBox.SelectedItem;
            var itemToDelete = (Nodes)selectedItem;
            masterList.Remove(itemToDelete);
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            helpers.ImportFromXML(masterList);
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Nodes> exportNodes = new ObservableCollection<Nodes>();
            foreach (var selectedItem in noteTypeListBox.SelectedItems)
            {
                Nodes selectedNode = new Nodes();
                selectedNode = (Nodes)selectedItem;
                exportNodes.Add(selectedNode);
            }
            helpers.ExportToXML(exportNodes);
        }

        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            if (listOfNoteValues.SelectedItem != null)
            {
                var noteitem = (NodeValues)listOfNoteValues.SelectedItem;
                masterList.Where(m => m.Note == noteitem.Name).First().NoteValues.Remove(noteitem);
            }
        }
        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            var result = helpers.XMLToObject(currentfile, masterList);
            listOfFilesListBox.ItemsSource = helpers.GetFiles(folderPath.Text);
            if (result)
            {
                System.Windows.Forms.MessageBox.Show("XML File Updated");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error while updating XML file");
            }
        }
        private void cloneButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = noteTypeListBox.SelectedItem;
            var itemsToDisplay = (Nodes)selectedItem;
            masterList.Add(itemsToDisplay);
            noteTypeListBox.ItemsSource = new ObservableCollection<Nodes>();
            noteTypeListBox.ItemsSource = masterList;
        }
    }
}