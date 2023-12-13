using bd5WPF.forms.add;
using bd5WPF.forms.Change;
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
using System.Data.OleDb;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Data.SqlClient;

namespace bd5WPF
{
    public partial class MainWindow : Window
    {

        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\jze9\\Desktop\\programs\\c#\\bd5WPF\\test.accdb";
        public OleDbConnection connection;
        public OleDbDataAdapter adapter;
        public DataSet dataSet;
        public OleDbCommandBuilder commandBuilder;

        public List<int> columnData = new List<int>();

        public int selectedRow;

        public MainWindow()
        {
            InitializeComponent();

            DataGrid1.Loaded += (sender, e) =>
            {
                foreach (var column in DataGrid1.Columns)
                {
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            };
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "SELECT Фамилия, Имя, Отчество, Дата FROM П340";
            OleDbCommand command = new OleDbCommand(sql, connection);
            adapter = new OleDbDataAdapter(command);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "П340");
            DataGrid1.ItemsSource = dataSet.Tables["П340"].DefaultView;
            commandBuilder = new OleDbCommandBuilder(adapter);

            addIdData();

            DataGrid1.SelectedIndex = 0;
        }

        public void addIdData()
        {
            columnData.Clear();
            OleDbCommand command = new OleDbCommand("SELECT Код FROM П340", connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Извлеките данные из столбца и добавьте их в массив
                int data = (int)reader["Код"];
                columnData.Add(data);
            }
            reader.Close();
            command.Dispose();
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            AddForm add = new AddForm();
            add.Owner = this;
            add.ShowDialog();
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            selectedRow = DataGrid1.SelectedIndex;
            Chenge chenge = new Chenge();
            chenge.Owner = this;
            chenge.ShowDialog();
        }

        private void Delite_Data(DataGrid dataGrid, string table)
        {
            selectedRow = dataGrid.SelectedIndex;
            if (selectedRow < 0) return;

            DataRowView selectedRow1 = (DataRowView)DataGrid1.SelectedItem;
            string deleteQuery = $"DELETE FROM {table} WHERE Код = @id";
            OleDbCommand command = new OleDbCommand(deleteQuery, connection);

            command.Parameters.AddWithValue("@id", columnData[selectedRow]);

            command.ExecuteNonQuery();
            DataView dataView = (DataView)dataGrid.ItemsSource;
            dataView.Table.Rows.Remove(selectedRow1.Row);
            addIdData();
            dataGrid.SelectedIndex = 0;
        }

        private void Delite_Button(object sender, RoutedEventArgs e)
        {
            
        }

        private void SwipeLeft(object sender, RoutedEventArgs e)
        {
            DataGrid1.Visibility = Visibility.Collapsed;
            DataGrid2.Visibility = Visibility.Visible;

        }
        private void SwipeRight(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
