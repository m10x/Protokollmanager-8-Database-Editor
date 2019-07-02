using FirebirdSql.Data.FirebirdClient;
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

        public MainWindow()
        {
            InitializeComponent();

            connString = new FbConnectionStringBuilder();
            connString.Database = "C:/Users/Public/Documents/Datenbank.FDB";
            connString.DataSource = "localhost";
            connString.UserID = "sysdba";
            connString.Password = "masterkey";
            connection = new FbConnection(connString.ToString());
            connection.Open();

            outputBox.Text = connection.State.ToString();

            sendInputButton.Click += sendInputButton_Click;
        }

        private void sendInputButton_Click(object sender, RoutedEventArgs e)
        {
            cmd = new FbCommand(inputBox.Text, connection);
            myReader = cmd.ExecuteReader();

            while (myReader.Read())
            {
                for (int i = 0; i < myReader.FieldCount; i++)
                {
                    //outputBox.Text += myReader[i] + "\n";
                    outputBox.Text += myReader.GetName(i) + "\t\t\t" + myReader[i] + "\n";
                }
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            connection.Close();
        }
    }
}
