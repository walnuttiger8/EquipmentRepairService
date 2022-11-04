using EquipmentRepairService.Views;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace EquipmentRepairService
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenEquipmentUnitView()
        {
            var view = new EquipmentUnitView();
            view.Show();
        }

        private void OpenEmployeeView()
        {
            var view = new EmployeeView();
            view.Show();
        }

        private void equipmentUnitViewButton_Click(object sender, RoutedEventArgs e)
        {
            OpenEquipmentUnitView();
        }

        private void employeeViewButton_Click(object sender, RoutedEventArgs e)
        {
            OpenEmployeeView();
        }

        private void employeePdfView_Click(object sender, RoutedEventArgs e)
        {
            var yOffset = 30;
            var row = 1;
            var filename = "Сотрудники__" + DateTime.Today.ToString("dd-MM-yyyy");

            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Comic Sans", 16);
            var pen = new XPen(XColors.Black);
            var fontBold = new XFont("Comic Sans", 15, XFontStyle.Bold);

            var textBrush = new XSolidBrush(XColor.FromArgb(0, 186, 0, 238));
            var backgroundBrush = new XSolidBrush(XColor.FromArgb(0, 139, 103, 2));

            gfx.DrawRectangle(backgroundBrush, new XRect(0, 0, page.Width, page.Height));

            gfx.DrawString("Код сотрудника", fontBold, textBrush, new XPoint(10, row * yOffset));
            gfx.DrawString("Имя", fontBold, textBrush, new XPoint(160, row * yOffset));
            gfx.DrawString("Фамилия", fontBold, textBrush, new XPoint(260, row * yOffset));
            gfx.DrawString("Отчесто", fontBold, textBrush, new XPoint(360, row * yOffset));

            gfx.DrawLine(pen, new XPoint(0, row * yOffset + 5), new XPoint(page.Width, row * yOffset));

            gfx.DrawLine(pen, new XPoint(158, 0), new XPoint(158, page.Height));
            gfx.DrawLine(pen, new XPoint(258, 0), new XPoint(258, page.Height));
            gfx.DrawLine(pen, new XPoint(358, 0), new XPoint(358, page.Height));

            row++;

            using (var db = new UP4Entities())
            {
                var employees = db.Employee.ToList();
                
                foreach (var employee in employees)
                {
                    gfx.DrawString(employee.EmployeeCode, font, textBrush, new XPoint(10, row * yOffset));
                    gfx.DrawString(employee.FirstName, font, textBrush, new XPoint(160, row * yOffset));
                    gfx.DrawString(employee.LastName, font, textBrush, new XPoint(260, row * yOffset));
                    gfx.DrawString(employee.MiddleName, font, textBrush, new XPoint(360, row * yOffset));
                    row++;
                }
            }

            var dlg = new SaveFileDialog();
            dlg.FileName = filename;
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF documents (.pdf)|*.pdf";

            var result = dlg.ShowDialog();

            if (result == true)
            {
                document.Save(dlg.FileName);
                Process.Start(dlg.FileName);
            }
        }
    }
}
