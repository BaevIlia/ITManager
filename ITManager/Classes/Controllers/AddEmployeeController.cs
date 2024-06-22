using ITManager.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows;

namespace ITManager.Controller
{
    /// <summary>
    /// Контроллер логики добавления/сохранения пользователя
    /// </summary>
    public class AddEmployeeController : ControllerBase
    {
        /// <summary>
        /// Проверка полей окна добавления/сохранения пользователя на пустоту
        /// </summary>
        /// <param name="fields">Список объектов типа TextBox</param>
        /// <param name="comboBoxes">Список объектов типа ComboBox</param>
        /// <returns name="flag">Возвращает булевое значение сигнализируещее о наличии/остутствии пустых полей: true-поля заполнены, false - не заполнены</returns>
        internal bool CheckNullFields(List<TextBox> fields, List<ComboBox> comboBoxes)
        {
            bool flag = true;
            foreach (TextBox field in fields)
            {
                if (string.IsNullOrEmpty(field.Text))
                {
                    field.BorderBrush = Brushes.Red;
                    flag = false;
                }

            }
            foreach (ComboBox comboBox in comboBoxes)
            {
                if (string.IsNullOrEmpty(comboBox.Text))
                {
                    comboBox.BorderBrush = Brushes.Red;
                    flag = false;
                }
            }
            if (flag == false)
            {
                MessageBox.Show("Некоторые данные не введены, пожалуйста введите данные", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return flag;
        }

        /// <summary>
        /// Проверка полей окна добавления/сохранения пользователя под различные форматы: логин в формате: фамилия_ио; пароль: не менее 6 символов; поля имя, фамилия, отчество: только русские буквы;
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="name">Имя пользователя</param>
        /// <param name="surname">Фамилия пользователя</param>
        /// <param name="patronymic">Отчество пользователя</param>
        /// <returns>Возвращает булевое значение, сигнализирующее о наличии или отсутствии ошибок форматов полей</returns>
        internal bool CheckFieldsFormat(string login, string password, string name, string surname, string patronymic)
        {
            bool flag = true;
            if (Regex.IsMatch(login, @"_\S{2}"))
            {
                flag = true;
            }
            else
            {
                MessageBox.Show("Неправильно введён логин, пожалуйста введите логин в формате: фамилия_ио латинскими буквами(ivanov_ii)", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (password.Length >= 6)
            {
                flag = true;
            }
            else
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!Regex.IsMatch(name, @"\P{IsCyrillic}") && !Regex.IsMatch(surname, @"\P{IsCyrillic}") && !Regex.IsMatch(patronymic, @"\P{IsCyrillic}"))
            {
                flag = true;
            }
            else
            {
                MessageBox.Show("Неправильно введены ФИО, пожалуйста заполняйте поля ФИО только русскими буквами", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            return flag;
        }

        /// <summary>
        ///Перегруженный метод проверки формата полей для окна сохранения пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="name">Имя пользователя</param>
        /// <param name="surname">Фамилия пользователя</param>
        /// <param name="patronymic">Отчество пользователя</param>
        /// <returns name="flag">Возвращает булевое значение, сигнализирующее о наличии или отсутствии ошибок форматов полей</returns>
        internal bool CheckFieldsFormat(string login, string name, string surname, string patronymic) 
        {
            bool flag = true;
            if (Regex.IsMatch(login, @"_\S{2}"))
            {
                flag = true;
            }
            else
            {
                MessageBox.Show("Неправильно введён логин, пожалуйста введите логин в формате: фамилия_ио латинскими буквами(ivanov_ii)", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (!Regex.IsMatch(name, @"\P{IsCyrillic}") && !Regex.IsMatch(surname, @"\P{IsCyrillic}") && !Regex.IsMatch(patronymic, @"\P{IsCyrillic}"))
            {
                flag = true;
            }
            else
            {
                MessageBox.Show("Неправильно введены ФИО, пожалуйста заполняйте поля ФИО только русскими буквами", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            return flag;
        }

        /// <summary>
        /// Метод добавления пользователя в базу данных
        /// </summary>
        /// <param name="employeeUnion">Объект класса с объединённой информацией о сотруднике</param>
        /// <returns>Возвращает числовое значение метода SaveChages(), где 1 - успешно сохранено, 0 - ошибка при сохранении</returns>
        public int AddEmployee(EmployeeUnion employeeUnion)
        {
            Employee employeeForAdd = new()
            {
                Login = employeeUnion.Login,
                Password = employeeUnion.Password,
                EmployeeStatus = 1
            };
            EmployeeInfo infoForAdd = new()
            {
                FirstName = employeeUnion.FirstName,
                LastName = employeeUnion.LastName,
                Patronymic = employeeUnion.Patronymic
            };



            using (ITOnedbContext db = new())
            {
                employeeForAdd.RoleId = db.EmployeeRole.Where(r => r.Name.Equals(employeeUnion.RoleName)).Select(r => r.Id).FirstOrDefault();

                try
                {
                    db.Add(employeeForAdd);
                    db.SaveChanges();
                }
                catch (Exception) 
                {
                    MessageBox.Show($"Ошибка сохранения данных пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
                
                infoForAdd.EmployeeId = db.Employee.Where(s => s.Login.Equals(employeeForAdd.Login) && s.Password.Equals(employeeForAdd.Password)).Select(s => s.Id).FirstOrDefault();
                infoForAdd.TitleId = db.EmployeeTitle.Where(t => t.Name.Equals(employeeUnion.Title)).Select(t => t.Id).FirstOrDefault();
                db.Add(infoForAdd);
                return db.SaveChanges();
     


            }

        }
        /// <summary>
        /// Метод записи изменённой информации о пользователе
        /// </summary>
        /// <param name="employeeUnion">Объект класса с объединённой информацией о сотруднике</param>
        /// <returns>Возвращает числовое значение метода SaveChages(), где 1 - успешно сохранено, 0 - ошибка при сохранении</returns>
        internal int ChangeEmployee(EmployeeUnion employeeUnion)
        {
            using (ITOnedbContext db = new())
            {
                Employee employeeForChange = db.Employee.Find(employeeUnion.Id);
                EmployeeInfo employeeInfoForChange = db.EmployeeInfo.Find(employeeUnion.Id);

                employeeForChange.Login = employeeUnion.Login;
                employeeForChange.RoleId = db.EmployeeRole.Where(r => r.Name.Equals(employeeUnion.RoleName)).Select(r => r.Id).FirstOrDefault();
                db.SaveChanges();
                employeeInfoForChange.FirstName = employeeUnion.FirstName;
                employeeInfoForChange.LastName = employeeUnion.LastName;
                employeeInfoForChange.Patronymic = employeeUnion.Patronymic;
                employeeInfoForChange.TitleId = db.EmployeeTitle.Where(t => t.Name.Equals(employeeUnion.Title)).Select(t => t.Id).FirstOrDefault();
                return db.SaveChanges();

            }
        }



    }

}
