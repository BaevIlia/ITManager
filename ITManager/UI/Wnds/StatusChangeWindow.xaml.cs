using ITManager.Model;
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
    /// Логика взаимодействия для StatusChangeView.xaml
    /// </summary>
    public partial class StatusChangeWindow : Window
    {
        internal string Status { get; set; }
        public StatusChangeWindow()
        {
            InitializeComponent();
            using (ITOnedbContext db = new()) 
            {
               cbStatuses.ItemsSource = db.TaskState.Select(ts=>ts.Name).ToList();
              
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Подтвердите смену статуса", "Информация", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                DialogResult = true;
            }
            else 
            {
               
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
