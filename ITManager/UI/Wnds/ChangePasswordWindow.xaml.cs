using ITManager.Controller;
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

namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        internal string Password { get; set; }
        public ChangePasswordWindow()
        {
            InitializeComponent();

        }

     

        private void btnAcceptPasswordChange_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txbPasswordFill.Text)) 
            {
                MessageBox.Show("Введите новый пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                txbPasswordFill.BorderBrush = Brushes.Red;
            }
            else 
            {
                MessageBoxResult result = MessageBox.Show("Подтвердите изменение пароля", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    AddEmployeeController controller = new();
                    Password = txbPasswordFill.Text;
                    DialogResult = true;

                }
                else
                {
                    Password = null;
                    txbPasswordFill.Text = null;
                }
            }
           
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
