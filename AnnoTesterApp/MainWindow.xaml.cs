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
using GV = C3D_2016_Anno.Global.variables;
using GH = C3D_2016_Anno.Helper.GenHelper;
using MahApps.Metro;
using MahApps.Metro.Controls;

namespace AnnoTesterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //GH.initializeSettings();
            //fetchDATA();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }

        private void lBox_labels_zoomto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("lBox_labels_zoomto_Click");

            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }

        }
        private void lBox_labels_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("lBox_labels_zoomto_Click");

            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }

        }

        public void fetchDATA()
        {
            try
            {
                cBox_template.ItemsSource = GV.templateFiles;
                cBox_Mapper.ItemsSource = GV.mapperFiles;
            }
            catch (System.Exception ee)
            {
                GH.writeLog(ee.ToString());
            }
        }

        #region Files
        private void btn_fetchtamplateFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GH.getFiles("template");
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void btn_fetchmapperFiles_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                GH.getFiles("mapper");
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void cBox_template_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_template.SelectedItem;
                GH.getTemplateDetails(FI.filePath);
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }


        private void cBox_Mapper_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                C3D_2016_Anno.Global.fileItem FI = (C3D_2016_Anno.Global.fileItem)cBox_Mapper.SelectedItem;
                GH.getMapper(FI.filePath);
            }
            catch (System.Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }
        #endregion


    }
}
