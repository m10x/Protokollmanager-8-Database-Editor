using FirebirdSql.Data.FirebirdClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Protokollmanager_8_Database_Editor
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FbConnectionStringBuilder connString;
        FbConnection connection;
        FbCommand cmd;
        FbDataReader myReader;
        CustomerFilterWindow popup;

        public MainWindow()
        {
            InitializeComponent();

            popup = new CustomerFilterWindow();
        }

        private void SendInputButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new FbCommand(inputBox.Text, connection);
            myReader = cmd.ExecuteReader();

            outputBox.Text = "";

            while (myReader.Read())
            {
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    outputBox.Text += myReader.GetName(i) + "\t\t\t" + myReader[i] + "\n";
                }
                outputBox.Text += "----------------------------------------------------------------------\n";
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            connection.Close();
        }

        private void ConsoleBtn_Click(object sender, RoutedEventArgs e)
        {
            consoleGrid.Visibility = Visibility.Visible;
            databaseGrid.Visibility = Visibility.Collapsed;
            customerGrid.Visibility = Visibility.Collapsed;
            deviceGrid.Visibility = Visibility.Collapsed;
            deviceKindGrid.Visibility = Visibility.Collapsed;
            deviceTypeGrid.Visibility = Visibility.Collapsed;
        }

        private void CustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            consoleGrid.Visibility = Visibility.Collapsed;
            databaseGrid.Visibility = Visibility.Collapsed;
            customerGrid.Visibility = Visibility.Visible;
            deviceGrid.Visibility = Visibility.Collapsed;
            deviceKindGrid.Visibility = Visibility.Collapsed;
            deviceTypeGrid.Visibility = Visibility.Collapsed;
        }

        private void DeviceBtn_Click(object sender, RoutedEventArgs e)
        {
            consoleGrid.Visibility = Visibility.Collapsed;
            databaseGrid.Visibility = Visibility.Collapsed;
            customerGrid.Visibility = Visibility.Collapsed;
            deviceGrid.Visibility = Visibility.Visible;
            deviceKindGrid.Visibility = Visibility.Collapsed;
            deviceTypeGrid.Visibility = Visibility.Collapsed;
        }

        private void GithubBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/m10x/Protokollmanager-8-Database-Editor");
        }

        private void CustomerRefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            customerCombo.Items.Clear();

            String sqlString = "SELECT CUST_ID, NAME1 FROM CUSTOMER;";
            cmd = new FbCommand(sqlString, connection);
            myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                customerCombo.Items.Add(myReader[0] + " " + myReader[1]);
            }
        }

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!customerCombo.HasItems == false)
            {
                String comboString = (sender as ComboBox).SelectedItem.ToString();                              // Current selected Customer
                String customerIDString = comboString.Substring(0, comboString.IndexOf(" "));                   // Get ID from the selected Customer
                String sqlString = "SELECT * FROM CUSTOMER WHERE CUST_ID = '" + customerIDString + "';";        // Get all Information of the Selected Customer
                cmd = new FbCommand(sqlString, connection);
                myReader = cmd.ExecuteReader();

                customerBox.Text = "";

                while (myReader.Read())
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        customerBox.Text += String.Format("{0,-30}", myReader.GetName(i)) + myReader[i] + "\n";
                    }
                }
            }
            else
                customerBox.Text = "No customers found!";
        }

        private void DatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                pathBox.Text = openFileDialog.FileName;
            else
                pathBox.Text = "Error while trying to open file!";
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            connString = new FbConnectionStringBuilder();
            connString.Database = pathBox.Text;
            connString.DataSource = "localhost";
            connString.UserID = usernameBox.Text;
            connString.Password = passwordBox.Password;
            connection = new FbConnection(connString.ToString());
            connection.Open();

            if (connection.State.ToString() == "Open")
                outputBox.Text = "Successfully opened database";
            else
                outputBox.Text = "Something went wrong... " + connection.State.ToString();

            consoleGrid.Visibility = Visibility.Visible;
            databaseGrid.Visibility = Visibility.Collapsed;
            customerGrid.Visibility = Visibility.Collapsed;
            deviceGrid.Visibility = Visibility.Collapsed;
            deviceKindGrid.Visibility = Visibility.Collapsed;
            deviceTypeGrid.Visibility = Visibility.Collapsed;
            mainToolbar.IsEnabled = true;
        }

        private void CustomerFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            popup.ShowDialog();

            //((CustomerFilterWindow)Application.Current.MainWindow).

            // FEHLERBEHANDLUNG!!! CONFIG DATEI!!!
        }
    }
}