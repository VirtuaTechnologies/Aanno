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
            var selectedItem = sender as ListBoxItem;
            var itemsToDisplay = (Nodes)selectedItem.Content;
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

            var selectedItem = sender as ListBoxItem;
            var fileToParse = (Files)selectedItem.Content;
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

        private void moveDownButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Nodes)noteTypeListBox.SelectedItem;
            var selectedItemIndex = masterList.IndexOf(selectedItem);
            if ((selectedItemIndex + 1) < masterList.Count())
                masterList.Move(selectedItemIndex, selectedItemIndex + 1);
        }

        private void moveUpButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Nodes)noteTypeListBox.SelectedItem;
            var selectedItemIndex = masterList.IndexOf(selectedItem);
            if (selectedItemIndex > 0)
                masterList.Move(selectedItemIndex, selectedItemIndex - 1);
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
            if (result)
            {
                System.Windows.Forms.MessageBox.Show("XML File Updated");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error while updating XML file");
            }
        }
    }
}