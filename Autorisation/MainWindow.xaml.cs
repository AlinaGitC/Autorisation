using Autorisation.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Autorisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppDbContext _context;
        private ObservableCollection<Driver> _drivers;
        private DateTime _lastActivityTime;
        private DispatcherTimer _inactivityTimer;
        private DispatcherTimer _saveTimer;
        public MainWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _lastActivityTime = DateTime.Now;

            // Настройка таймера неактивности
            _inactivityTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _inactivityTimer.Tick += InactivityTimer_Tick;
            _inactivityTimer.Start();
            // Таймер для сохранения времени активности (каждую секунду)
            /*_saveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _saveTimer.Tick += SaveActivityTime;
            _saveTimer.Start();
            */
            LoadDrivers();
        }
        private async void LoadDrivers()
        {
            try
            {
                var driversList = await _context.Driver.ToListAsync();
                _drivers = new ObservableCollection<Driver>(driversList);
                DriversListBox.ItemsSource = _drivers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                // Логирование ошибки
                Debug.WriteLine(ex.ToString());
            }

        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - _lastActivityTime).TotalSeconds >= 3)
            {
                _inactivityTimer.Stop();
                MessageBox.Show("Сеанс завершен из-за неактивности");
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            _lastActivityTime = DateTime.Now;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            _lastActivityTime = DateTime.Now;
        }

        

        private void AddDriver_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditDriverWindow();
            if (addWindow.ShowDialog() == true)
            {
                LoadDrivers();
            }
        }

        private void EditDriver_Click(object sender, RoutedEventArgs e)
        {
            
            /*if (DriversListBox.SelectedItem is Driver selectedDriver)
            {
                var editWindow = new AddEditDriverWindow(selectedDriver);
                if (editWindow.ShowDialog() == true)
                {
                    LoadDrivers();
                }
            }
            else
            {
                MessageBox.Show("Выберите водителя для редактирования");
            }*/
        }

        private async void DeleteDriver_Click(object sender, RoutedEventArgs e)
        {
            if (DriversListBox.SelectedItem is Driver selectedDriver)
            {
                if (MessageBox.Show("Удалить выбранного водителя?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Driver.Remove(selectedDriver);
                    await _context.SaveChangesAsync();
                    LoadDrivers();
                }
            }
            else
            {
                MessageBox.Show("Выберите водителя для удаления");
            }
        }
               

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _context?.Dispose();
        }
    }
}