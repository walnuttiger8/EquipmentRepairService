using EquipmentRepairService.Componentes;
using EquipmentRepairService.Componentes.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;

namespace EquipmentRepairService.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : Window
    {
        EntityDataGrid<Employee> _dataGrid;

        private readonly UP4Entities _db;

        public EmployeeView()
        {
            InitializeComponent();
            _db = new UP4Entities();

            _dataGrid = CreateDataGrid();

            mainGrid.Children.Add(_dataGrid);
        }

        private EntityDataGrid<Employee> CreateDataGrid()
        {
            _db.Employee.Load();

            var dataGrid = new EntityDataGrid<Employee>(_db.Employee.Local);
            dataGrid.ValidationErrorOccured += DisplayErrorMessage;

            dataGrid.Headers.Add("EmployeeCode", "Код сотрудника");
            dataGrid.Headers.Add("LastName", "Фамилия");
            dataGrid.Headers.Add("FirstName", "Имя");
            dataGrid.Headers.Add("MiddleName", "Отчество");

            dataGrid.Validators.Add("Код сотрудника", new List<Func<object, string>>() { Validators.MinLength(3, "Код сотрудника должен быть длиннее 3-х символов") });
            dataGrid.Validators.Add("Фамилия", new List<Func<object, string>>() { Validators.MinLength(2, "Фамилия должна быть длиннее 1-го символа") });
            dataGrid.Validators.Add("Имя", new List<Func<object, string>>() { Validators.MinLength(2, "Имя должно быть длиннее 1-го символа") });

            return dataGrid;
        }

        private List<string> Commit()
        {
            var errorMessages = new List<string>();

            try
            {
                _db.SaveChanges();
            }
            catch
            {
                foreach (var validationError in _db.GetValidationErrors())
                {
                    foreach (var errorMessage in validationError.ValidationErrors)
                    {
                        errorMessages.Add(errorMessage.ErrorMessage.ToString());
                    }
                }
            }

            return errorMessages;
        }

        private void DisplayErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var errorMessages = Commit();
            foreach (var errorMessage in errorMessages)
            {
                MessageBox.Show(errorMessage);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _db.Dispose();
        }
    }
}

