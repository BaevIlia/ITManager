using ITManager.Controller;
using ITManager.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для AddStaffWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        List<TextBox> Fields { get; set; }
        List<ComboBox> ComboFields { get; set; }


        internal int CurrentEmployeeId { get; set; }
        


        public AddEmployeeWindow()
        {
            
            InitializeComponent();
            Fields = new() { txbEmployeeFirstName, txbEmployeeLastName, txbEmployeePatronymic, txbEmployeeLogin, txbEmployeePassword };
            ComboFields = new() { cmbEmployeeRoleName, cmbEmployeeTitle };
            using (ITOnedbContext db = new()) 
            {
                cmbEmployeeRoleName.ItemsSource = db.EmployeeRole.Select(r => r.Name).ToList();
                cmbEmployeeTitle.ItemsSource = db.EmployeeTitle.Select(r => r.Name).ToList();
            }

        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeController controller = new();
            if (controller.CheckNullFields(Fields, ComboFields) && controller.CheckFieldsFormat(txbEmployeeLogin.Text, txbEmployeePassword.Text, txbEmployeeFirstName.Text, txbEmployeeLastName.Text, txbEmployeePatronymic.Text))
            {
                MessageBoxResult result = MessageBox.Show("Добавить пользователя?", "Подтверждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {

                            EmployeeUnion employeeForAdd = new()
                            {
                                FirstName = txbEmployeeFirstName.Text,
                                LastName = txbEmployeeLastName.Text,
                                Patronymic = txbEmployeePatronymic.Text,
                                Title = cmbEmployeeTitle.Text,
                                RoleName = cmbEmployeeRoleName.Text,
                                Login = txbEmployeeLogin.Text,
                                Password = ControllerBase.PasswordEncryption(txbEmployeePassword.Text),
                            };
                            controller.AddEmployee(employeeForAdd);
                            DialogResult = true;
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            this.Close();
                            break;
                        }
                };



            }
      
        }

        private void btnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
           
            Fields = new() { txbEmployeeFirstName, txbEmployeeLastName, txbEmployeePatronymic, txbEmployeeLogin };
            AddEmployeeController controller = new();
            if (controller.CheckNullFields(Fields, ComboFields) && controller.CheckFieldsFormat(txbEmployeeLogin.Text, txbEmployeeFirstName.Text, txbEmployeeLastName.Text, txbEmployeePatronymic.Text))
            {
                MessageBoxResult result = MessageBox.Show("Сохранить изменения?", "Подтверждение", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        {

                            EmployeeUnion employeeForChange = new()
                            {
                                Id = CurrentEmployeeId,
                                FirstName = txbEmployeeFirstName.Text,
                                LastName = txbEmployeeLastName.Text,
                                Patronymic = txbEmployeePatronymic.Text,
                                Title = cmbEmployeeTitle.Text,
                                RoleName = cmbEmployeeRoleName.Text,
                                Login = txbEmployeeLogin.Text,
                            };
                            controller.ChangeEmployee(employeeForChange);
                            DialogResult = true;
                            break;
                        }
                    case MessageBoxResult.No:
                        {
                            break;
                        }
                    case MessageBoxResult.Cancel:
                        {
                            this.Close();
                            break;
                        }
                };

              
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
