using ITManager.Controller;
using ITManager.Model;
using ITManager.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
   
        public LoginWindow()
        {
            InitializeComponent();
            txbPasswordBox.Visibility = Visibility.Hidden;
        }


        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            LogInController log = new();
            if (log.CheckFields(LogInTextBox, PasswordBox)) 
            {
                if (log.CheckLogPass(LogInTextBox, PasswordBox)) 
                {
                   log.AccountEnter(LogInTextBox, PasswordBox);
                    this.Close();
                    
                   
                }
            }
            
        }

      

        private void LogInTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LogInTextBox.BorderBrush = Brushes.Black;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox.BorderBrush = Brushes.Black;
            txbPasswordBox.Text = PasswordBox.Password;
        }
        

        private void chkbShowPass_Checked(object sender, RoutedEventArgs e)
        {
            txbPasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void chkbShowPass_Unchecked(object sender, RoutedEventArgs e)
        {
            txbPasswordBox.Visibility = Visibility.Collapsed;
            PasswordBox.Password= txbPasswordBox.Text;
            PasswordBox.Visibility = Visibility.Visible;

        }

      
    }
}
