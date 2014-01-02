using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using System.ServiceProcess;
using System.Configuration;
using System.Diagnostics;

namespace AutomicControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DemoServices myServices;

        int rowCount = 2;
        int rowCountR = 2;

        public MainWindow()
        {

            InitializeComponent();

            readConfig();

        }

        private void readConfig()
        {

            myServices = new DemoServices(ConfigurationManager.AppSettings["numBaseServices"], ConfigurationManager.AppSettings["numONEServices"],ConfigurationManager.AppSettings["numARAServices"]);

            for (int i = 0; i <= myServices.count_BaseServices - 1; i++)
            {
                myServices.addBaseService(ConfigurationManager.AppSettings["base" + i]);
            }
            for (int i = 0; i <= myServices.count_OneServices - 1; i++)
            {
                myServices.addOneService(ConfigurationManager.AppSettings["one" + i]);
            }
            for (int i = 0; i <= myServices.count_OneServices - 1; i++)
            {
                myServices.addAraService(ConfigurationManager.AppSettings["ara" + i]);
            }

            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(30);
            MyGrid.RowDefinitions.Add(gridRow1);

            Label lbl = new Label();
            lbl.Content = "Base Services";
            lbl.Margin = new Thickness(10, 0, 0, 0);
            lbl.FontSize = 18;
            Grid.SetRow(lbl, 2);
            Grid.SetColumn(lbl, 0);
            MyGrid.Children.Add(lbl);
            rowCount++;

            for (int i = 0; i <= myServices.count_BaseServices - 1; i++)
            {
                addServiceSection(myServices.getBaseService(i), true, 0);
            }

            RowDefinition gridRow2 = new RowDefinition();
            gridRow2.Height = new GridLength(50);
            MyGrid.RowDefinitions.Add(gridRow2);

            Label lbl2 = new Label();
            lbl2.Content = "ONE Automation";
            lbl2.Margin = new Thickness(10, 15, 0, 0);
            lbl2.FontSize = 18;
            Grid.SetRow(lbl2, rowCount);
            Grid.SetColumn(lbl2, 0);
            MyGrid.Children.Add(lbl2);
            rowCount++;

            for (int i = 0; i <= myServices.count_OneServices - 1; i++)
            {
                addServiceSection(myServices.getOneService(i), true, 0);
            }

            Label lbl3 = new Label();
            lbl3.Content = "ARA";
            lbl3.Margin = new Thickness(10, 0, 0, 0);
            lbl3.FontSize = 18;
            Grid.SetRow(lbl3, rowCountR);
            Grid.SetColumn(lbl3, 1);
            MyGrid.Children.Add(lbl3);
            rowCountR++;

            for (int i = 0; i <= myServices.count_AraServices - 1; i++)
            {
                addServiceSection(myServices.getAraService(i), true, 1);
            }
        }

        private void addServiceSection(string ServiceString, bool left, int i)
        {
            string[] serviceInfo = ServiceString.Split('|');

            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(30);
            MyGrid.RowDefinitions.Add(gridRow1);

            CheckBox chk = new CheckBox();
            chk.Name = serviceInfo[2];
            chk.Style = (Style)FindResource("CheckBoxStyle1");
            chk.Content = serviceInfo[1];
            chk.Tag = serviceInfo[0];
            chk.IsChecked = false;

            chk.Width = 250;
            chk.Margin = new Thickness(20, 0, 0, 0);
            chk.HorizontalAlignment = HorizontalAlignment.Left;
            if (i == 0)
            {
                Grid.SetRow(chk, rowCount);
                rowCount++;
            } else
            {
                Grid.SetRow(chk, rowCountR);
                rowCountR++;
            }
            Grid.SetColumn(chk, i);

            MyGrid.Children.Add(chk);

            checkStatus(chk.Name);
            chk.Checked += CheckBox_Checked;
            chk.Unchecked += CheckBox_Unchecked;

        }


        private void checkStatus(string checkName)
        {

            CheckBox che = FindChild<CheckBox>(MyGrid, checkName);

            ServiceController sc = new ServiceController(che.Tag.ToString());

            switch(sc.Status)
            {
                case ServiceControllerStatus.Running:
                    {
                        che.IsEnabled = true;
                        che.IsChecked = true;
                        break;
                    }
                case ServiceControllerStatus.StartPending:
                    {
                        che.IsEnabled = false;
                        che.IsChecked = true;
                        checkStatus(che.Name);
                        break;
                    }
                case ServiceControllerStatus.StopPending:
                    {
                        che.IsEnabled = false;
                        che.IsChecked = false;
                        checkStatus(che.Name);
                        break;
                    }
                default:
                    {
                        che.IsEnabled = true;
                        che.IsChecked = false;
                        break;
                    }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ServiceController sc = new ServiceController(chk.Tag.ToString());
            if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
            {
                sc.Start();
                chk.IsEnabled = false;
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                chk.IsEnabled = true;
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            ServiceController sc = new ServiceController(chk.Tag.ToString());
            if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
            {
                sc.Stop();
                chk.IsEnabled = false;
                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                chk.IsEnabled = true;
            }
 
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        private void RefreshAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= myServices.count_BaseServices - 1; i++)
            {
                string[] serviceInfo = myServices.getBaseService(i).Split('|');
                checkStatus(serviceInfo[2]);
            }

            for (int i = 0; i <= myServices.count_OneServices - 1; i++)
            {
                string[] serviceInfo = myServices.getOneService(i).Split('|');
                checkStatus(serviceInfo[2]);
            }

            for (int i = 0; i <= myServices.count_AraServices - 1; i++)
            {
                string[] serviceInfo = myServices.getAraService(i).Split('|');
                checkStatus(serviceInfo[2]);
            }
        }
    }
}
