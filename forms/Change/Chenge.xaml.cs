using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
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

namespace bd5WPF.forms.Change
{
    public partial class Chenge : Window
    {
        public Chenge()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;
            if (main == null) return;

            if (main.DataGrid1.SelectedCells.Count > 0)
            {
                // Получаем выбранную ячейку
                DataGridCellInfo selectedCell = main.DataGrid1.SelectedCells[0];

                // Проверяем, что ячейка содержит значение
                if (selectedCell.Item is DataRowView)
                {
                    // Получаем значение из выбранной ячейки
                    DataRowView rowView = (DataRowView)selectedCell.Item;
                    surname.Text = rowView[0].ToString();
                    name.Text = rowView[1].ToString();
                    patronymic.Text = rowView[2].ToString();
                    data.Text = rowView[3].ToString();
                }
            }
        }

        private void data_Initialized(object sender, EventArgs e)
        {
            
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void chenge_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;
            if (main == null) return;
            if (main.DataGrid1.SelectedIndex < 0) return;
            DataRow rowToUpdate = main.dataSet.Tables["П340"].Rows[main.selectedRow];

            // Обновление значений столбцов
            rowToUpdate["Фамилия"] = surname.Text;
            rowToUpdate["Имя"] = name.Text;
            rowToUpdate["Отчество"] = patronymic.Text;
            rowToUpdate["Дата"] = data.Text;

            // Создание команды обновления данных
            string updateCommand = "UPDATE П340 SET Фамилия=?, Имя=?, Отчество=?, Дата=? WHERE Код=?";
            OleDbCommand command = new OleDbCommand(updateCommand, main.connection);
            
            // Параметры команды
            command.Parameters.AddWithValue("@Фамилия", surname.Text);
            command.Parameters.AddWithValue("@Имя", name.Text);
            command.Parameters.AddWithValue("@Отчество", patronymic.Text);
            command.Parameters.AddWithValue("@Дата", data.Text);
            command.Parameters.AddWithValue("@Код", main.columnData[main.selectedRow]);

            // Выполнение команды обновления
            command.ExecuteNonQuery();

            Close();
        }
    }
}
