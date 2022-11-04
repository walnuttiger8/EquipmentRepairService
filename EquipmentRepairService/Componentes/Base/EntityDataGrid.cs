using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EquipmentRepairService.Componentes.Base
{
    public class EntityDataGrid<T> : DataGrid
    {
        public delegate void ValidationError(string errorMessage);
        public event ValidationError ValidationErrorOccured;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, List<Func<object, string>>> Validators { get; set; } = new Dictionary<string, List<Func<object, string>>>();

        public EntityDataGrid(IEnumerable<T> entities)
        {
            AutoGeneratingColumn += FilterHeaders;
            CellEditEnding += ValidateCellChange;

            ItemsSource = entities;
        }

        private void FilterHeaders(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var header = e.Column.Header.ToString();
            if (!Headers.ContainsKey(header))
            {
                e.Cancel = true;
                return;
            }

            e.Column.Header = Headers[header];
        }

        private void ValidateCellChange(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!Validators.ContainsKey(e.Column.Header.ToString()))
            {
                return;
            }
            var validators = Validators[e.Column.Header.ToString()];

            var element = (TextBox)e.EditingElement;

            foreach (var validator in validators)
            {
                var errorMessage = validator(element.Text);
                if (errorMessage != null)
                {
                    if (ValidationErrorOccured is null)
                    {
                    } else
                    {
                        ValidationErrorOccured(errorMessage);
                    }
                    e.Cancel = true;
                    return;
                }
            }
        }
    }
}
