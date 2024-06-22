using ITManager.Controller;
using ITManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 


    public partial class AdminWindow : Window
    {

        EmployeeInfo AdminData { get; set; }

        AdminController AdminController = new();
        
        string Title { get; set; }

        public AdminWindow() { }

        public AdminWindow(int employeeId)
        {

            AdminData = ControllerBase.FindInfoById(employeeId);
            using (ITOnedbContext db = new())
            {
                Title = db.EmployeeTitle.Where(t => t.Id.Equals(AdminData.TitleId)).Select(t => t.Name).FirstOrDefault();
            }

            InitializeComponent();
           
            lblUserName.Content = $"Сотрудник: {AdminData.FirstName} {AdminData.LastName}";
            lblTitle.Content = $"Должность: {Title}";

        }

        private void bthAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new();
            addEmployeeWindow.btnSaveEmployee.Visibility = Visibility.Collapsed;
            addEmployeeWindow.ShowDialog();
            if (addEmployeeWindow.DialogResult == true)
            {
                dgData.ItemsSource = AdminController.Refresh();
            }
        }

        private void bthShowAllEmployee_Click(object sender, RoutedEventArgs e)
        {
            
            dgData.ItemsSource = AdminController.Refresh();

        }

        private void bthDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeUnion infoForDelete = dgData.SelectedItem as EmployeeUnion;
            if (infoForDelete.EmployeeStatus == 0)
            {
                MessageBox.Show($"Данная учетная запись заблокирована, выберете другую", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                try
                {
                    AdminController.DisableAccount(infoForDelete.Id);
                    dgData.ItemsSource = AdminController.Refresh();
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show($"Пользователь не выбран, пожалуйста выберете пользователя", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        private void bthUpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
                if (AdminController.UpdateEmployee(dgData.SelectedItem as EmployeeUnion) == true) 
                {
                    dgData.ItemsSource = AdminController.Refresh();
                }
        }

        private void btnPasswordChange_Click(object sender, RoutedEventArgs e)
        {
            if (AdminController.ChangeEmployeePassword(dgData.SelectedItem as EmployeeUnion) == true) 
            {
                dgData.ItemsSource = AdminController.Refresh();
            }
          
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                ControllerBase.LogWriteSession(ControllerBase.FindById(AdminData.EmployeeId), AdminData, "Выход");
                AdminData = null;
                Title = null;
                dgData = null;
                this.Close();
            }
           

          
        }

        private void btnLogInfoView_Click(object sender, RoutedEventArgs e)
        {
            LogShowWindow logShowWindow = new(AdminController.ShowLogs());
            logShowWindow.Show();
        }
    }
}
