using EquipmentRepairService.Componentes;
using EquipmentRepairService.Componentes.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using System.Windows.Media;

namespace EquipmentRepairService.Views
{
    /// <summary>
    /// Логика взаимодействия для EquipmentUnitView.xaml
    /// </summary>
    public partial class EquipmentUnitView : Window
    {
        EntityDataGrid<EquipmentUnit> _dataGrid;

        private readonly UP4Entities _db;

        public EquipmentUnitView()
        {
            InitializeComponent();
            _db = new UP4Entities();

            _dataGrid = CreateDataGrid();

            mainGrid.Children.Add(_dataGrid);
        }

        private EntityDataGrid<EquipmentUnit> CreateDataGrid()
        {
            _db.EquipmentUnit.Load();

            var dataGrid = new EntityDataGrid<EquipmentUnit>(_db.EquipmentUnit.Local);
            dataGrid.ValidationErrorOccured += DisplayErrorMessage;

            dataGrid.Headers.Add("Name", "Название");
            dataGrid.Headers.Add("Model", "Модель");
            dataGrid.Headers.Add("ReleaseYear", "Год выпуска");
            dataGrid.Headers.Add("StockNumber", "Инвентарный номер");

            dataGrid.Validators.Add("Год выпуска", new List<Func<object, string>>() { Validators.TypeOf(typeof(int), "Год выпуска не является валидным числом") });
            dataGrid.Validators.Add("Название", new List<Func<object, string>>() { Validators.MinLength(3, "Название должно быть длиннее 3-х символов") });
            dataGrid.Validators.Add("Модель", new List<Func<object, string>>() { Validators.MinLength(3, "Модель должна быть длиннее 3-х символов") });
            dataGrid.Validators.Add("Инвентарный номер", new List<Func<object, string>>() { Validators.MinLength(3, "Инвентарный номер должен быть длиннее 3-х символов") });

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
