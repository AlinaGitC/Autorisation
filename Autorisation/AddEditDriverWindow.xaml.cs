using Autorisation.Models;
using Microsoft.EntityFrameworkCore;
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

namespace Autorisation
{
    /// <summary>
    /// Логика взаимодействия для AddEditDriverWindow.xaml
    /// </summary>
    public partial class AddEditDriverWindow : Window
    {
        private Driver _driver;
        private AppDbContext _context;
        public AddEditDriverWindow(Driver driver = null)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _driver = driver ?? new Driver();

            if (driver != null)
            {
                Title = "Редактирование водителя";
                DataContext = _driver;
            }
            else
            {
                Title = "Добавление нового водителя";
            }
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Заполните обязательные поля (Имя и Фамилия)");
                return;
            }

            try
            {
                if (_driver.ID == 0) // Новый водитель
                {
                    _context.Driver.Add(_driver);
                }
                else // Редактирование
                {
                    _context.Driver.Update(_driver);
                }

                await _context.SaveChangesAsync();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context?.Dispose();
        }
    }
}
