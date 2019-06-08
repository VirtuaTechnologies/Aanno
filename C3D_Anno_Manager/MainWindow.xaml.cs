//This is an Intelectual Property of Virtua Technologies and Raghulan Gowthaman.
//www.Virtuatechnologies.com.au


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
using MahApps.Metro.Native;
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
using GV = C3D_Anno_Manager.Global.variables;
using C3D_Anno_Manager.Properties;
using MahApps.Metro.Controls.Dialogs;

namespace C3D_Anno_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public ObservableCollection<Nodes> masterList = new ObservableCollection<Nodes>();
        public ObservableCollection<Nodes> mapperList = new ObservableCollection<Nodes>();
        Helpers helpers = new Helpers();
        public string currentfile { get; set; }
        public string currentmapperfile { get; set; }
        public MainWindow()
        {

            InitializeComponent();
            if (Properties.Settings.Default["folderpath"] != null)
                folderPath.Text = Properties.Settings.Default["folderpath"].ToString();
            if (Properties.Settings.Default["mapperpath"].ToString() != null)
                mapperFolderPath.Text = Properties.Settings.Default["mapperpath"].ToString();
        }

        private void listbox_Item_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = sender as DataGridRow; ;
                var itemsToDisplay = (Nodes)selectedItem.Item;
                listOfNoteValues.ItemsSource = itemsToDisplay.NoteValues;
                valueTextBox.Clear();
                numberTextBox.Clear();
            }
            catch
            {

            }
        }
        private void mapping_listbox_Item_Clicked(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedItem = sender as DataGridRow;
                var item = (IGrouping<String, NodeValues>)selectedItem.DataContext;
                listOfMappingKeys.ItemsSource = mapperList[0].NoteValues.Where(mp => mp.Value == item.Key);
                noteKeyTextBox.Clear();
            }
            catch
            {

            }
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
            try
            {
                listOfFilesListBox.ItemsSource = helpers.GetFiles(folderPath.Text, ".xml");
                Properties.Settings.Default["folderpath"] = folderPath.Text;
                Properties.Settings.Default.Save();
            }
            catch
            {

            }
        }
        private void xmlfile_Item_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = sender as DataGridRow;
                var fileToParse = (Files)selectedItem.Item;
                currentfile = fileToParse.FilePath;
                masterList = helpers.ParseXMLFile(fileToParse.FilePath);
                noteTypeListBox.ItemsSource = masterList;
                listOfNoteValues.ItemsSource = new ObservableCollection<NodeValues>();
            }
            catch
            {

            }
        }

        private void mapperfile_Item_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = sender as DataGridRow;
                var fileToParse = (Files)selectedItem.Item;
                currentmapperfile = fileToParse.FilePath;
                mapperList = helpers.ParseXMLFile(fileToParse.FilePath);
                mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
                listOfMappingKeys.ItemsSource = new ObservableCollection<NodeValues>();
            }
            catch
            {

            }
        }

        private void listOfNoteValuesSingleClick(object sender, RoutedEventArgs e)
        {
            try
            {
            var selectedItem = sender as DataGridRow;
            var nodeValue = (NodeValues)selectedItem.Item;
            numberTextBox.Text = nodeValue.Number.ToString();
            valueTextBox.Text = nodeValue.Value;
            }
            catch
            {

            }
        }

        private void listOfMappingKeysSingleClick(object sender, RoutedEventArgs e)
        {
            try
            {
            var selectedItem = sender as DataGridRow;
            var nodeValue = (NodeValues)selectedItem.Item;
            noteKeyTextBox.Text = nodeValue.Number;
            }
            catch
            {

            }
        }
        private void addNoteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            Nodes newNode = new Nodes();
            newNode.Note = addNoteItemTextBox.Text;
            masterList.Add(newNode);
            addNoteItemTextBox.Clear();
            }
            catch
            {

            }
        }

        private void deleteNoteItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            var selectedItem = noteTypeListBox.SelectedItem;
            var itemToDelete = (Nodes)selectedItem;
            masterList.Remove(itemToDelete);
            }
            catch
            {

            }
        }

        private void importButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                helpers.ImportFromXML(masterList);
            }
            catch
            {

            }
        }

        private void exportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
            ObservableCollection<Nodes> exportNodes = new ObservableCollection<Nodes>();
            foreach (var selectedItem in noteTypeListBox.SelectedItems)
            {
                Nodes selectedNode = new Nodes();
                selectedNode = (Nodes)selectedItem;
                exportNodes.Add(selectedNode);
            }
            var status = helpers.ExportToXML(exportNodes);
            if (status)
            {
                this.ShowMessageAsync("Success", "XML File Created");
            }
            else
            {
                this.ShowMessageAsync("Error", "Error while Exporting XML file");
            }
            }
            catch
            {

            }
        }

        private void MenuItemRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (listOfNoteValues.SelectedItem != null)
            {
                var noteitem = (NodeValues)listOfNoteValues.SelectedItem;
                masterList.Where(m => m.Note == noteitem.Name).First().NoteValues.Remove(noteitem);
            }
            }
            catch
            {

            }
        }
        private void ApplyXmlchanges()
        {
            try
            {
                var result = helpers.XMLToObject(currentfile, masterList);
                listOfFilesListBox.ItemsSource = helpers.GetFiles(folderPath.Text, ".xml");
                if (result)
                {
                    this.ShowMessageAsync("Success", "XML File Updated");
                }
                else
                {
                    this.ShowMessageAsync("Error", "Error while updating XML file");
                }
            }
            catch
            {

            }
        }
        private void cloneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            var selectedItem = noteTypeListBox.SelectedItem;
            var itemsToDisplay = (Nodes)selectedItem;
            masterList.Add(itemsToDisplay);
            noteTypeListBox.ItemsSource = new ObservableCollection<Nodes>();
            noteTypeListBox.ItemsSource = masterList;
            }
            catch
            {

            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((numberTextBox.Text != null) && (valueTextBox.Text != null))
                {
                    ObservableCollection<NodeValues> nodeValues = new ObservableCollection<NodeValues>();
                    Nodes nodes = new Nodes();
                    nodes = (Nodes)noteTypeListBox.SelectedItem;
                    bool numberCheck = true;
                    bool valueCheck = true;
                    if (nodes.NoteValues != null)
                    { 
                    numberCheck = nodes.NoteValues.Any(n => n.Number == numberTextBox.Text);
                    valueCheck = nodes.NoteValues.Any(n => n.Value == valueTextBox.Text);
                    }
                    else
                    {
                        numberCheck = false;
                        valueCheck = false;
                        nodes.NoteValues = new ObservableCollection<NodeValues>();
                        listOfNoteValues.ItemsSource = nodes.NoteValues;
                    }
                    if (!numberCheck && !valueCheck)
                    {
                        NodeValues nodeValue = new NodeValues();
                        nodeValue.Name = nodes.Note;
                        nodeValue.Number = numberTextBox.Text;
                        nodeValue.Value = valueTextBox.Text;
                        nodes.NoteValues.Add(nodeValue);
                    }
                    else
                    {
                        this.ShowMessageAsync("Error", "Duplicates Not Allowed");
                    }
                    valueTextBox.Clear();
                    numberTextBox.Clear();
                }
            }
            catch
            {

            }
            ApplyXmlchanges();
        }

        private void addKeyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (noteKeyTextBox != null)
                {
                    NodeValues nodeValues = new NodeValues();
                    var keyCheck = mapperList[0].NoteValues.Any(mp => mp.Number == noteKeyTextBox.Text);
                    if (!keyCheck)
                    {
                        nodeValues.Number = noteKeyTextBox.Text;
                        nodeValues.Name = mapperList[0].Note;
                        var selectedNote = mappingNoteTypeListBox.SelectedItem;
                        var itemToAdd = (IGrouping<String, NodeValues>)selectedNote;
                        nodeValues.Value = itemToAdd.Key;
                        mapperList[0].NoteValues.Add(nodeValues);
                        mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
                        listOfMappingKeys.ItemsSource = mapperList[0].NoteValues.Where(mp => mp.Value == itemToAdd.Key);
                    }
                    else
                    {
                        this.ShowMessageAsync("Error", "Duplicates Not Allowed");
                    }
                    noteKeyTextBox.Clear();
                }
            }
            catch
            {

            }
            ApplyMapperChanges();
        }

        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nodeValue = (NodeValues)listOfNoteValues.SelectedItem;

                if ((numberTextBox.Text != null) && (valueTextBox.Text != null))
                {

                    ObservableCollection<NodeValues> nodeValues = new ObservableCollection<NodeValues>();
                    Nodes nodes = new Nodes();
                    nodes = (Nodes)noteTypeListBox.SelectedItem;
                    var numberCheck = nodes.NoteValues.Where(n => n.Number != nodeValue.Number);
                    var valueCheck = nodes.NoteValues.Where(n => n.Value != nodeValue.Value);

                    var numberDuplicate = numberCheck.Any(n => n.Number == numberTextBox.Text);
                    var valueDuplicate = valueCheck.Any(n => n.Value == valueTextBox.Text);
                    if (!numberDuplicate && !valueDuplicate)
                    {
                        NodeValues updateNodeValue = new NodeValues();
                        updateNodeValue.Name = nodes.Note;
                        updateNodeValue.Number = numberTextBox.Text;
                        updateNodeValue.Value = valueTextBox.Text;
                        var index = nodes.NoteValues.IndexOf(nodeValue);
                        nodes.NoteValues[index] = updateNodeValue;
                    }
                    else
                    {
                        this.ShowMessageAsync("Error", "Duplicates Not Allowed");
                    }

                    valueTextBox.Clear();
                    numberTextBox.Clear();
                }
            }
            catch
            {

            }
            ApplyXmlchanges();
        }

        private void updateKeyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nodeValue = (NodeValues)listOfMappingKeys.SelectedItem;
                if (noteKeyTextBox != null && nodeValue != null)
                {
                    NodeValues nodeValues = new NodeValues();
                    var keyCheck = mapperList[0].NoteValues.Where(mp => mp.Number != nodeValue.Number);
                    var keyPresent = keyCheck.Any(mp => mp.Number == noteKeyTextBox.Text);
                    if (!keyPresent)
                    {
                        nodeValues.Number = noteKeyTextBox.Text;
                        nodeValues.Name = mapperList[0].Note;
                        var selectedNote = mappingNoteTypeListBox.SelectedItem;
                        var itemToUpdate = (IGrouping<String, NodeValues>)selectedNote;
                        nodeValues.Value = itemToUpdate.Key;
                        var index = mapperList[0].NoteValues.IndexOf(nodeValue);
                        mapperList[0].NoteValues[index] = nodeValues;
                        listOfMappingKeys.ItemsSource = mapperList[0].NoteValues.Where(mp => mp.Value == itemToUpdate.Key);
                    }
                    else
                    {
                        this.ShowMessageAsync("Error", "Duplicates Not Allowed");
                    }
                    noteKeyTextBox.Clear();
                }
            }
            catch
            {

            }
            ApplyMapperChanges();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            var nodeValue = (NodeValues)listOfNoteValues.SelectedItem;
            masterList.Where(m => m.Note == nodeValue.Name).First().NoteValues.Remove(nodeValue);
            valueTextBox.Clear();
            numberTextBox.Clear();
            }
            catch
            {

            }
            ApplyXmlchanges();
        }
        private void deleteKeyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nodeValue = (NodeValues)listOfMappingKeys.SelectedItem;
                mapperList[0].NoteValues.Remove(nodeValue);
                if (!mapperList[0].NoteValues.Any(mp => mp.Value == nodeValue.Value))
                {
                    mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
                    listOfMappingKeys.ItemsSource = new ObservableCollection<NodeValues>();
                }
                else
                {
                    var selectedItem = mappingNoteTypeListBox.SelectedItem;
                    var item = (IGrouping<String, NodeValues>)selectedItem;
                    listOfMappingKeys.ItemsSource = mapperList[0].NoteValues.Where(mp => mp.Value == item.Key);
                }
            }
            catch
            {

            }
            ApplyMapperChanges();
        }

        private void buttonMapperBrowse_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    mapperFolderPath.Text = fbd.SelectedPath;
                }
            }

        }
        private void buttonMapperScan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                listOfMappingFilesListBox.ItemsSource = helpers.GetFiles(mapperFolderPath.Text, ".Mapper");
                Properties.Settings.Default["mapperpath"] = mapperFolderPath.Text;
                Properties.Settings.Default.Save();
            }
            catch
            {

            }
        }
        private void buttonAddMappingNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NodeValues newNodeValue = new NodeValues();
                newNodeValue.Value = addMappingNoteTextBox.Text;
                newNodeValue.Name = mapperList[0].Note;
                newNodeValue.Number = "***";
                mapperList[0].NoteValues.Add(newNodeValue);
                mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
                addMappingNoteTextBox.Clear();
            }
            catch
            {

            }
        }
        private void buttonDeleteMappingNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = mappingNoteTypeListBox.SelectedItem;
                var itemToDelete = (IGrouping<String, NodeValues>)selectedItem;
                var delete = itemToDelete.ToList();
                foreach (NodeValues nv in delete)
                {
                    mapperList[0].NoteValues.Remove(nv);
                }
                mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
                listOfMappingKeys.ItemsSource = new ObservableCollection<NodeValues>();
            }
            catch
            {

            }
        }
        private void importMapping_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                helpers.ImportFromXML(mapperList);
                mappingNoteTypeListBox.ItemsSource = mapperList[0].NoteValues.GroupBy(m => m.Value);
            }
            catch
            {

            }
        }
        private void exportMapping_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Nodes exportNodes = new Nodes();
                exportNodes.NoteValues = new ObservableCollection<NodeValues>();
                foreach (var selectedItem in mappingNoteTypeListBox.SelectedItems)
                {
                    var itemToExport = (IGrouping<String, NodeValues>)selectedItem;
                    foreach (NodeValues nv in itemToExport)
                    {
                        exportNodes.NoteValues.Add(nv);
                    }
                }
                var status = helpers.ExportToMapper(exportNodes, null);
                if (status == "mappercreated")
                {
                    this.ShowMessageAsync("Success", "Mapper Created Succesfully");
                }
                else if (status == "createfailed")
                {
                    this.ShowMessageAsync("Error", "Mapper Creation Failed");
                }
            }
            catch
            {

            }
        }

        private void ApplyMapperChanges()
        {
            try
            {
                var mapperFile = (Files)listOfMappingFilesListBox.SelectedItem;
                var status = helpers.ExportToMapper(mapperList[0], mapperFile.FilePath);
                if (status == "mapperupdated")
                {
                    this.ShowMessageAsync("Success", "Mapper Updated Succesfully");
                }
                else if (status == "updatefailed")
                {
                    this.ShowMessageAsync("Error", "Mapper Update Failed");
                }
            }
            catch
            {

            }
        }
    }
}