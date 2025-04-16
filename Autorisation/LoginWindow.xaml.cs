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
using System.Windows.Threading;

namespace Autorisation
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private int _attempts = 0;
        private DateTime? _blockedUntil = null;
        private DispatcherTimer _timer;
        public LoginWindow()
        {
            InitializeComponent();

            _blockedUntil = BlockState.GetBlockTime();

            if (_blockedUntil.HasValue && _blockedUntil > DateTime.Now)
            {
                StartBlockTimer();
            }
        }
        private void StartBlockTimer()
        {
            BlockTimerText.Visibility = Visibility.Visible;
            txtLogin.IsEnabled = false;
            txtPassword.IsEnabled = false;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += UpdateBlockTimer;
            _timer.Start();
        }

        private void UpdateBlockTimer(object sender, EventArgs e)
        {
            if (_blockedUntil.HasValue)
            {
                var timeLeft = _blockedUntil.Value - DateTime.Now;

                if (timeLeft.TotalSeconds > 0)
                {
                    BlockTimerText.Text = $"Система заблокирована. До разблокировки: {timeLeft:mm\\:ss}";
                }
                else
                {
                    // Разблокировка
                    _timer.Stop();
                    BlockTimerText.Visibility = Visibility.Collapsed;
                    txtLogin.IsEnabled = true;
                    txtPassword.IsEnabled = true;
                    _blockedUntil = null;
                    BlockState.ClearBlockTime();
                }
            }
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (_blockedUntil.HasValue && DateTime.Now < _blockedUntil.Value)
            {
                MessageBox.Show($"Система заблокирована до {_blockedUntil.Value:HH:mm:ss}");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLogin.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    var user = await context.Userr
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Login == txtLogin.Text && u.Password == txtPassword.Password);

                    if (user != null)
                    {
                        _attempts = 0;
                        var mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        _attempts++;
                        if (_attempts >= 3)
                        {
                            _blockedUntil = DateTime.Now.AddSeconds(20);
                            BlockState.SetBlockTime(_blockedUntil.Value);
                            StartBlockTimer();
                            MessageBox.Show("Превышено количество попыток. Система заблокирована на 1 минуту.");
                        }
                        else
                        {
                            MessageBox.Show($"Неверный логин или пароль. Осталось попыток: {3 - _attempts}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _timer?.Stop();
        }
    }
}
