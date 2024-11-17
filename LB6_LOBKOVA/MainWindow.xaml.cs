using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LB6_LOBKOVA
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Management> managements;
        private Management? editingRecord = null;

        public MainWindow()
        {
            InitializeComponent();
            managements = new ObservableCollection<Management>();

            // Установка подсказок в текстовые поля
            DepartmentInput.Text = DepartmentInput.Tag.ToString();
            BudgetInput.Text = BudgetInput.Tag.ToString();
            EmployeesInput.Text = EmployeesInput.Tag.ToString();
            ManagerInput.Text = ManagerInput.Tag.ToString();

            RecordList.ItemsSource = managements;
            SearchInput.Text = SearchInput.Tag.ToString();
        }

        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                managements.Clear();
                foreach (var record in Management.GetAll())
                {
                    managements.Add(record);
                }
                InputPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отображении записей: {ex.Message}");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchInput.Text) || SearchInput.Text == SearchInput.Tag.ToString())
            {
                MessageBox.Show("Введите критерий поиска.");
                return;
            }

            try
            {
                managements.Clear();
                foreach (var record in Management.Search(SearchInput.Text))
                {
                    managements.Add(record);
                }
                InputPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DepartmentInput.Text = DepartmentInput.Tag.ToString();
            BudgetInput.Text = BudgetInput.Tag.ToString();
            EmployeesInput.Text = EmployeesInput.Tag.ToString();
            ManagerInput.Text = ManagerInput.Tag.ToString();
            editingRecord = null;
            InputPanel.Visibility = Visibility.Visible;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (RecordList.SelectedItem is Management selected)
            {
                DepartmentInput.Text = string.IsNullOrWhiteSpace(selected.Department) ? DepartmentInput.Tag.ToString() : selected.Department;
                BudgetInput.Text = selected.Budget.HasValue ? selected.Budget.Value.ToString() : BudgetInput.Tag.ToString();
                EmployeesInput.Text = selected.Employees.HasValue ? selected.Employees.Value.ToString() : EmployeesInput.Tag.ToString();
                ManagerInput.Text = string.IsNullOrWhiteSpace(selected.Manager) ? ManagerInput.Tag.ToString() : selected.Manager;
                editingRecord = selected;
                InputPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для изменения.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RecordList.SelectedItem is Management selected)
            {
                try
                {
                    Management.Delete(selected.Id);
                    DisplayAll_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении записи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.");
            }
        }

        private void SaveRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(DepartmentInput.Text) || DepartmentInput.Text == DepartmentInput.Tag.ToString())
                {
                    MessageBox.Show("Поле 'Департамент' обязательно для заполнения.");
                    return;
                }

                double? budget = null;
                if (!string.IsNullOrWhiteSpace(BudgetInput.Text) && BudgetInput.Text != BudgetInput.Tag.ToString())
                {
                    if (double.TryParse(BudgetInput.Text, out double parsedBudget))
                    {
                        budget = parsedBudget;
                    }
                    else
                    {
                        MessageBox.Show("Некорректное значение бюджета.");
                        return;
                    }
                }

                int? employees = null;
                if (!string.IsNullOrWhiteSpace(EmployeesInput.Text) && EmployeesInput.Text != EmployeesInput.Tag.ToString())
                {
                    if (int.TryParse(EmployeesInput.Text, out int parsedEmployees))
                    {
                        employees = parsedEmployees;
                    }
                    else
                    {
                        MessageBox.Show("Некорректное значение количества сотрудников.");
                        return;
                    }
                }

                string manager = string.IsNullOrWhiteSpace(ManagerInput.Text) || ManagerInput.Text == ManagerInput.Tag.ToString()
                    ? null
                    : ManagerInput.Text;

                if (editingRecord == null)
                {
                    var newRecord = new Management
                    {
                        Department = DepartmentInput.Text,
                        Budget = budget,
                        Employees = employees,
                        Manager = manager
                    };
                    newRecord.Insert();
                }
                else
                {
                    editingRecord.Department = DepartmentInput.Text;
                    editingRecord.Budget = budget;
                    editingRecord.Employees = employees;
                    editingRecord.Manager = manager;
                    editingRecord.Update();
                }

                DisplayAll_Click(null, null);
                InputPanel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении записи: {ex.Message}");
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == textBox.Tag.ToString())
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = textBox.Tag.ToString();
                    textBox.Foreground = Brushes.Gray;
                }
            }
        }
    }
}
