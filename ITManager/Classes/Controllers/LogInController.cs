using ITManager.Model;
using System;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;
using System.Media;
using System.Windows;
using System.Security.Cryptography;
using ITManager.View;

namespace ITManager.Controller
{
    /// <summary>
    /// Контроллер логики работы окна авторизации
    /// </summary>
    internal class LogInController : ControllerBase
    {
        internal LogInController() { }

        /// <summary>
        /// Метод проверки при авторизации. В первую очередь проверяется наличие пользователя по логину а затем сопоставление введённых логина и пароля
        /// </summary>
        /// <param name="loginTextBox">Поле ввода логина - объект типа TextBox</param>
        /// <param name="passwordBox">Поле ввода пароля - объект типа PasswordBox</param>
        /// <returns>Возвращаемое значение: булевая переменная, сигнализирующая о корректности/некорректности данных, а так же об успешности или ошибке авторизации</returns>
        internal bool CheckLogPass(TextBox loginTextBox, PasswordBox passwordBox) 
        {
            using (ITOnedbContext db = new()) 
            {
                try
                {
                    if (db.Employee.Any(s => s.Login.Equals(loginTextBox.Text)))
                    {
                        if (db.Employee.Any(s => s.Password.Equals(PasswordEncryption(passwordBox.Password)) && s.Login.Equals(loginTextBox.Text)))
                        {
                            return true;
                        }
                        else
                        {
                            passwordBox.BorderBrush = Brushes.Red;
                            MessageBox.Show("Неверный пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                    }
                    else
                    {
                        loginTextBox.BorderBrush = Brushes.Red;
                        MessageBox.Show("Неверный логин или пользователя не существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Возникла непредвиденная ошибка, обратитесь к администратору, код ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            
        }

        /// <summary>
        /// Проверка на пустоту полей окна авторизации
        /// </summary>
        /// <param name="loginBox">Поле ввода логина - объект типа TextBox</param>
        /// <param name="passBox">Поле ввода пароля - объект типа PasswordBox</param>
        /// <returns>Возвращаемое значение: булевая переменная, сигнализирующая о наличии/отсутствии пустых полей</returns>
        internal bool CheckFields(TextBox loginBox, PasswordBox passBox)
        {
            if (string.IsNullOrEmpty(loginBox.Text))
            {
                
                loginBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Поле логин пустое", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else if (string.IsNullOrEmpty(passBox.Password))
            {
                
                passBox.BorderBrush = Brushes.Red;
                MessageBox.Show("Поле пароль пустое", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Метод нахождения кода роли для сопоставления с таблицей ролей
        /// </summary>
        /// <param name="id">Идентификатор пользователя - int</param>
        /// <returns>Возвращаемое значение: код роли типа int</returns>
        internal int FindRoleCode(int id) 
        {
            using (ITOnedbContext db = new()) 
            {
                return db.Employee.Where(s => s.Id == id).Select(s=>s.RoleId).FirstOrDefault();
            }
        }
        /// <summary>
        /// Метод нахождения кода статуса учётной записи сотрудника
        /// </summary>
        /// <param name="id">Идентификатор пользователя int</param>
        /// <returns>Возвращаемое значение: код статуса типа int</returns>
        internal int FindStatus(int id) 
        {
            using (ITOnedbContext db = new()) 
            {
                return db.Employee.Where(s=>s.Id.Equals(id)).Select(s=>s.EmployeeStatus).FirstOrDefault();
            }
        }

        /// <summary>
        /// Метод входа в аккаунт, содержащий алгоритмы проверок и действий для авторизации и отображения окна пользователя
        /// </summary>
        /// <param name="loginTextBox">Поле ввода логина - объект типа TextBox</param>
        /// <param name="passBox">Поле ввода пароля - объект типа PasswordBox</param>
        internal void AccountEnter(TextBox loginTextBox, PasswordBox passBox) 
        {
            int employeeId = ControllerBase.FindId(loginTextBox.Text, passBox.Password);
            int roleCode = FindRoleCode(employeeId);
            int status = FindStatus(employeeId);

            if (status == 0)
            {
                MessageBox.Show("Аккаунт пользователя заблокирован, обратитесь к администратору", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                loginTextBox.Clear();
                passBox.Clear();
            }
            else
            {
                ControllerBase.LogWriteSession(ControllerBase.FindById(employeeId), ControllerBase.FindInfoById(employeeId), "Вход");
                if (roleCode == 1)
                {
                    AdminWindow adminWindow = new(employeeId);
                    MessageBox.Show("Авторизация прошла успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    adminWindow.Show();
                    
                }
                else if (roleCode == 2)
                {
                    UserWindow userWindow = new(employeeId);
                    MessageBox.Show("Авторизация прошла успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    userWindow.Show();
                 
                }
            }
        }


    }
}
