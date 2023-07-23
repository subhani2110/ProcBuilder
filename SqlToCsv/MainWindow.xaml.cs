using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace SqlToCsv
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void serverList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverList.SelectedItem != null)
            {
                string server = serverList.SelectedItem.ToString();

                dbList.ItemsSource = new DataAccess.SqlConnectionBuilder().GetDbs(server);
                dbList.IsEnabled = true;
            }
        }
       
        private void dbList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverList.SelectedItem != null && dbList.SelectedItem != null)
            {
                string db = dbList.SelectedItem.ToString();
                string server = serverList.SelectedItem.ToString();

                tableList.ItemsSource = new DataAccess.SqlConnectionBuilder().GetTables(server, db);
                tableList.IsEnabled = true;
            }
        }

        private async void getCsv_Click(object sender, RoutedEventArgs e)
        {
            if (serverList.SelectedItem != null && dbList.SelectedItem != null && tableList.SelectedItem != null)
            {
                try
                {
                    string db = dbList.SelectedItem.ToString();
                    string server = serverList.SelectedItem.ToString();
                    string table = tableList.SelectedItem.ToString();

                    var list = await new DataAccess.SqlDataAccess().LoadData(server, db, table);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "CSV File|*.csv";
                    saveFileDialog1.Title = "Save the Table to CSV";
                    saveFileDialog1.FileName = table;
                    var dg = saveFileDialog1.ShowDialog();

                    if (dg.HasValue && dg.Value)
                    {
                        if (saveFileDialog1.FileName != null)
                        {
                            new Helpers.CsvHandler().CsvWriter(list, saveFileDialog1.FileName);
                            MessageBox.Show(table + ".csv Generated");
                        }
                    }
                    else
                    {
                        MessageBox.Show(table + ".csv Not Generated");

                    }

                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select all values");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            serverList.ItemsSource = await new DataAccess.SqlConnectionBuilder().GetServers();
            serverList.IsEnabled = true;
            serverLbl.Text = "Servers";

        }

        private void connectDb_Click(object sender, RoutedEventArgs e)
        {
            if (serverTb.Text != null && userIdTb.Text != null && passwordTb.Text != null) 
            {
                string server = serverTb.Text;
                string userId = userIdTb.Text;
                string password = passwordTb.Text;

                dbListR.ItemsSource = new DataAccess.RemoteConnectionBuilder().GetDbs(server, userId, password);
                dbListR.IsEnabled = true;
            }
        }

        private void dbListR_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (serverTb.Text != null && userIdTb.Text != null && passwordTb.Text != null && dbListR.SelectedItem != null)
            {
                string db = dbListR.SelectedItem.ToString();
                string server = serverTb.Text;
                string userId = userIdTb.Text;
                string password = passwordTb.Text;

                tableListR.ItemsSource = new DataAccess.RemoteConnectionBuilder().GetTables(server, userId, password, db);
                tableListR.IsEnabled = true;
            }
        }

        private async void getCsvR_Click(object sender, RoutedEventArgs e)
        {
            if (serverTb.Text != null && userIdTb.Text != null && passwordTb.Text != null && dbListR.SelectedItem != null && tableListR.SelectedItem != null)
            {
                try
                {
                    string db = dbListR.SelectedItem.ToString();
                    string server = serverTb.Text;
                    string userId = userIdTb.Text;
                    string password = passwordTb.Text;
                    string table = tableListR.SelectedItem.ToString();

                    var list = await new DataAccess.SqlDataAccess().LoadDataRemote(server,userId,password, db, table);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "CSV File|*.csv";
                    saveFileDialog1.Title = "Save the Table to CSV";
                    saveFileDialog1.FileName = table;
                    var dg = saveFileDialog1.ShowDialog();

                    if (dg.HasValue && dg.Value)
                    { 
                        if (saveFileDialog1.FileName != null)
                        {
                            new Helpers.CsvHandler().CsvWriter(list, saveFileDialog1.FileName);
                            MessageBox.Show(table + ".csv Generated");
                        }
                    }
                    else
                    {
                        MessageBox.Show(table + ".csv Not Generated");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select all values");
            }
        }

        private async void getSp_Click(object sender, RoutedEventArgs e)
        {
            if (serverList.SelectedItem != null && dbList.SelectedItem != null && tableList.SelectedItem != null)
            {
                try
                {
                    string db = dbList.SelectedItem.ToString();
                    string server = serverList.SelectedItem.ToString();
                    string table = tableList.SelectedItem.ToString();

                    var cols = await new DataAccess.SqlConnectionBuilder().GetColumns(server, db, table);

                    var q1 = new Helpers.QueryGenerator().SelectQuery(table);
                    var q2 = new Helpers.QueryGenerator().SelectByIdQuery(table, cols);
                    var q3 = new Helpers.QueryGenerator().InsertQuery(table, cols);
                    var q4 = new Helpers.QueryGenerator().UpdateQuery(table, cols);
                    var q5 = new Helpers.QueryGenerator().DeleteQuery(table, cols);
                    var q6 = new Helpers.QueryGenerator().ClassQuery(table, cols);

                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Script File|*.sql";
                    saveFileDialog1.Title = "Save Stored Procedures Script";
                    saveFileDialog1.FileName = table;
                    var dg = saveFileDialog1.ShowDialog();

                    if (dg.HasValue && dg.Value)
                    {
                        if (saveFileDialog1.FileName != null)
                        {
                            File.WriteAllText(saveFileDialog1.FileName, q1 + q2 + q3 + q4 + q5);

                            File.WriteAllText(saveFileDialog1.FileName.Remove(saveFileDialog1.FileName.Length - 3) + "txt", q6);

                            MessageBox.Show(table + ".sql Generated");
                        }
                    }
                    else
                    {
                        MessageBox.Show(table + ".sql Not Generated");
                    }

                }
                catch { }
            }
        }
    }
}
