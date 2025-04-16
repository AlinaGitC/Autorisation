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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private int _attempts = 0;
        private DateTime? _blockedUntil = null;
        public LoginWindow()
        {
            InitializeComponent();
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
                            _blockedUntil = DateTime.Now.AddMinutes(0.25);
                            MessageBox.Show("Превышено количество попыток. Система заблокирована на 1 минуту.");
                        }
                        else
                        {
                            MessageBox.Show($"Неверный логин или пароль. Осталось попыток: {3 - _attempts}");
                        }
                    }
                }
            
        }
    }
}
