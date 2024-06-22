using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ITManager.Model;

namespace ITManager.Controller
{
    /// <summary>
    /// Стандартный статический класс контроллера содержащий методы, используемые в различных дочерних классах контроллеров приложения
    /// </summary>
    public class ControllerBase
    {

        internal ControllerBase() { }

        /// <summary>
        /// Метод нахождения информации о пользователе по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Возвращаемое значение: объект класса EmployeeInfo</returns>
        public static EmployeeInfo FindInfoById(int id) 
        {
            using (ITOnedbContext db = new()) 
            {
                return db.EmployeeInfo.Find(id);
            }
        }

        /// <summary>
        /// Метод нахождения идентификатора пользователя по его логину и паролю
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Возвращаемое значение: идентификатор типа int</returns>
        public static int FindId(string login, string password) 
        {
            
            using (ITOnedbContext db = new()) 
            {
                return db.Employee.Where(s => s.Login.Equals(login) && s.Password.Equals(PasswordEncryption(password))).Select(s => s.Id).FirstOrDefault();
            }
           
        }

        /// <summary>
        /// Методо получения объекта класса Employee по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Возвращаемое значение: объект класса Employee</returns>
        public static Employee FindById(int id)
        {
            using (ITOnedbContext db = new())
            {
                return db.Employee.Find(id);
            }
        }

        /// <summary>
        /// Запись логов о входах и выходах пользователей в приложение
        /// </summary>
        /// <param name="employee">Сотрудник - объект класса Employee</param>
        /// <param name="employeeInfo">Информация о сотруднике - объект класса EmployeeInfo</param>
        /// <param name="actionType">Действие, выполняемое сотрудником - string</param>
        public static void LogWriteSession(Employee employee, EmployeeInfo employeeInfo, string actionType) 
        {
            LogJournal logJournal = new()
            {
                EmployeeLogin = employee.Login,
                EmployeeName = String.Join(" ", employeeInfo.FirstName, employeeInfo.LastName),
                EnterDate = DateTime.Now.ToString("dd.MM.yyyy"),
                EnterTime = DateTime.Now.ToShortTimeString(),
                EmployeeId = employee.Id,
                ActionType = actionType
            };
            using (ITOnedbContext db = new())
            {
                db.LogJournal.Add(logJournal);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Хэширование пароля методом MD5
        /// </summary>
        /// <param name="password">Пароль пользователя - string</param>
        /// <returns>Возвращаемое значение: преобразованный в стандартную 64 разрядную строку хэш-код</returns>
        public static string PasswordEncryption(string password)
        {
            var cryptAlgorytm = MD5.Create();
            return Convert.ToBase64String(cryptAlgorytm.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
