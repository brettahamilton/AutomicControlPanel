﻿using System;
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
using System.Threading;
using System.ComponentModel;

namespace AutomicControlPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Building Form

        DemoServices myServices;

        int rowCount = 2;
        int rowCountR = 2;
        int rowCountE = 2;
        int rowCountL = 2;

        public MainWindow()
        {

            InitializeComponent();

            readConfig();

        }

        private void readConfig()
        {

            myServices = new DemoServices(ConfigurationManager.AppSettings["numBaseServices"], ConfigurationManager.AppSettings["numONEServices"], ConfigurationManager.AppSettings["numARAServices"], ConfigurationManager.AppSettings["numWSPServices"], ConfigurationManager.AppSettings["numWLSServices"]);

            for (int i = 0; i <= myServices.count_BaseServices - 1; i++)
            {
                myServices.addBaseService(ConfigurationManager.AppSettings["base" + i]);
            }
            for (int i = 0; i <= myServices.count_OneServices - 1; i++)
            {
                myServices.addOneService(ConfigurationManager.AppSettings["one" + i]);
            }
            for (int i = 0; i <= myServices.count_AraServices - 1; i++)
            {
                myServices.addAraService(ConfigurationManager.AppSettings["ara" + i]);
            }
            for (int i = 0; i <= myServices.count_WspServices - 1; i++)
            {
                myServices.addWspService(ConfigurationManager.AppSettings["wsp" + i]);
            }
            for (int i = 0; i <= myServices.count_WlsServices - 1; i++)
            {
                myServices.addWlsService(ConfigurationManager.AppSettings["wls" + i]);
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
            gridRow2.Height = new GridLength(30);
            MyGrid.RowDefinitions.Add(gridRow2);

            Label lbl2 = new Label();
            lbl2.Content = "ONE Automation";
            lbl2.Margin = new Thickness(10, 0, 0, 0);
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
            lbl3.Content = "ARA Base";
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

            Label lbl4 = new Label();
            lbl4.Content = "ARA Websphere";
            lbl4.Margin = new Thickness(10, 0, 0, 0);
            lbl4.FontSize = 18;
            Grid.SetRow(lbl4, rowCountR);
            Grid.SetColumn(lbl4, 1);
            MyGrid.Children.Add(lbl4);
            rowCountR++;

            for (int i = 0; i <= myServices.count_WspServices - 1; i++)
            {
                addServiceSection(myServices.getWspService(i), true, 1);
            }

            Label lbl5 = new Label();
            lbl5.Content = "ARA Weblogic";
            lbl5.Margin = new Thickness(10, 0, 0, 0);
            lbl5.FontSize = 18;
            Grid.SetRow(lbl5, rowCountE);
            Grid.SetColumn(lbl5, 2);
            MyGrid.Children.Add(lbl5);
            rowCountE++;

            for (int i = 0; i <= myServices.count_WlsServices - 1; i++)
            {
                addServiceSection(myServices.getWlsService(i), true, 2);
            }

            Label lbl6 = new Label();
            lbl6.Content = "Links";
            lbl6.Margin = new Thickness(10, 0, 0, 0);
            lbl6.FontSize = 18;
            Grid.SetRow(lbl6, rowCountL);
            Grid.SetColumn(lbl6, 3);
            MyGrid.Children.Add(lbl6);
            rowCountL++;
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

            Viewbox view = new Viewbox();
            view.Name = "view" + serviceInfo[2];
            view.Height = 20;
            view.Width = 20;
            view.HorizontalAlignment = HorizontalAlignment.Right;
            view.Margin = new Thickness(0, 0, 40, 0);
            view.Visibility = Visibility.Hidden;

            CircularProgressBar circ = new CircularProgressBar();
            circ.Name = "prog" + serviceInfo[2];
            view.Child = circ;

            if (i == 0)
            {
                Grid.SetRow(chk, rowCount);
                Grid.SetRow(view, rowCount);
                rowCount++;
            }
            else if (i == 1)
            {
                Grid.SetRow(chk, rowCountR);
                Grid.SetRow(view, rowCountR);
                rowCountR++;
            }
            else
            {
                Grid.SetRow(chk, rowCountE);
                Grid.SetRow(view, rowCountE);
                rowCountE++;
            }
            Grid.SetColumn(chk, i);
            Grid.SetColumn(view, i);

            MyGrid.Children.Add(chk);

            MyGrid.Children.Add(view);

            checkStatus(chk.Name);
            chk.Checked += CheckBox_Checked;
            chk.Unchecked += CheckBox_Unchecked;

        }

        #endregion


        private void checkStatus(string checkName)
        {

            CheckBox che = FindChild<CheckBox>(MyGrid, checkName);
            Viewbox view = FindChild<Viewbox>(MyGrid, "view" + che.Name);

            ServiceController sc = new ServiceController(che.Tag.ToString());

            switch(sc.Status)
            {
                case ServiceControllerStatus.Running:
                    {
                        che.IsEnabled = true;
                        che.IsChecked = true;
                        view.Visibility = Visibility.Hidden;
                        break;
                    }
                case ServiceControllerStatus.StartPending:
                    {
                        che.IsEnabled = false;
                        che.IsChecked = true;
                        view.Visibility = Visibility.Visible;
                        break;
                    }
                case ServiceControllerStatus.StopPending:
                    {
                        che.IsEnabled = false;
                        che.IsChecked = false;
                        view.Visibility = Visibility.Visible;
                        break;
                    }
                default:
                    {
                        che.IsEnabled = true;
                        che.IsChecked = false;
                        view.Visibility = Visibility.Hidden;
                        break;
                    }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            String CheckName = chk.Name;
            ServiceController sc = new ServiceController(chk.Tag.ToString());
            if (sc.Status != ServiceControllerStatus.Running && sc.Status != ServiceControllerStatus.StartPending)
            {
                sc.Start();
                checkStatus(CheckName);

                BackgroundWorker bgWorker;
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

                string[] workerPayload = new string[3] { CheckName, chk.Tag.ToString(), "started" };

                bgWorker.RunWorkerAsync(workerPayload);
            }

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            String CheckName = chk.Name;
            ServiceController sc = new ServiceController(chk.Tag.ToString());
            if (sc.Status != ServiceControllerStatus.Stopped && sc.Status != ServiceControllerStatus.StopPending)
            {
                sc.Stop();
                checkStatus(CheckName);

                BackgroundWorker bgWorker;
                bgWorker = new BackgroundWorker();
                bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

                string[] workerPayload = new string[3] {CheckName,chk.Tag.ToString(),"stopped"};

                bgWorker.RunWorkerAsync(workerPayload);

                //sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                //checkStatus(CheckName);
            }
 
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] args = (string[])e.Argument;
            ServiceController sc = new ServiceController(args[1]);

            ServiceControllerStatus status;

            if (args[2] == "stopped")
            {
                status = ServiceControllerStatus.Stopped;
            }
            else
            {
                status = ServiceControllerStatus.Running;
            }

            sc.WaitForStatus(status, new TimeSpan(0, 1, 0));
            e.Result = args[0];
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            checkStatus((String)e.Result);
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

            for (int i = 0; i <= myServices.count_WspServices - 1; i++)
            {
                string[] serviceInfo = myServices.getWspService(i).Split('|');
                checkStatus(serviceInfo[2]);
            }

            for (int i = 0; i <= myServices.count_WlsServices - 1; i++)
            {
                string[] serviceInfo = myServices.getWlsService(i).Split('|');
                checkStatus(serviceInfo[2]);
            }
        }

        private void ShowLoader_Click(object sender, RoutedEventArgs e)
        {
            //Viewbox Progbar = FindChild<Viewbox>(MyGrid, "ProgBar");
            if (ProgBar.Visibility == System.Windows.Visibility.Visible)
            {
                ProgBar.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                ProgBar.Visibility = System.Windows.Visibility.Visible;
            }
                
        }
    }
}
