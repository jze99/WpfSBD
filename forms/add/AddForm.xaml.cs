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
using System.Windows.Shapes;
using System.Globalization;
using System.Data.OleDb;
using System.Data;


namespace bd5WPF.forms.add
{
    /// <summary>
    /// Логика взаимодействия для AddForm.xaml
    /// </summary>
    public partial class AddForm : Window
    {
        public AddForm()
        {
            InitializeComponent();
        }

        private void data_Initialized(object sender, EventArgs e)
        {
            data.Text = DateTime.Now.ToShortDateString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;
            if (main == null) return;

            string query = "INSERT INTO П340 (Фамилия, Имя, Отчество, Дата) VALUES (@Фамилия, @Имя, @Отчество, @Дата)";

            // Создание команды с параметрами
            OleDbCommand command = new OleDbCommand(query, main.connection);

            // Добавление параметров со значениями
            command.Parameters.AddWithValue("@Фамилия", surname.Text);
            command.Parameters.AddWithValue("@Имя", name.Text);
            command.Parameters.AddWithValue("@Отчество", patronymic.Text);
            command.Parameters.AddWithValue("@Дата", data.Text);

            // Выполнение запроса
            command.ExecuteNonQuery();

            DataRow newRow = main.dataSet.Tables["П340"].NewRow();

            // Заполнение новой строки значениями из текстовых полей
            newRow["Фамилия"] = surname.Text;
            newRow["Имя"] = name.Text;
            newRow["Отчество"] = patronymic.Text;
            newRow["Дата"] = data.Text;

            // Добавление новой строки в таблицу dataSet
            main.dataSet.Tables["П340"].Rows.Add(newRow);

            main.addIdData();

            main.DataGrid1.SelectedIndex = 0;

            surname.Text = "";
            name.Text = "";
            patronymic.Text = "";

        }
    }
}
