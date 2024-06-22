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
    /// Логика взаимодействия для SearchView.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        internal string TaskName { get; set; }
        public SearchWindow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txbSearch.Text))
            {
                TaskName = txbSearch.Text;
                DialogResult = true;
            }
            else if (string.IsNullOrEmpty(txbSearch.Text)) 
            {
                MessageBox.Show("Строка поиска пуста", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
   
        
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
