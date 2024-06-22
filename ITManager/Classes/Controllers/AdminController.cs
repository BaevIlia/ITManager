using iTextSharp.text.pdf;
using iTextSharp.text;
using ITManager.Model;
using ITManager.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ITManager.Controller
{
    /// <summary>
    /// Класс, объединяющий в себе информацию о пользователе из сущностей Employee, EmployeeInfo, EmployeeRole, EmployeeTitle
    /// </summary>
    public class EmployeeUnion
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Title { get; set; }
        public string RoleName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int EmployeeStatus { get; set; }
    }
    /// <summary>
    /// Контроллер логики работы окна Администратора
    /// </summary>
    public class AdminController : ControllerBase
    {

        public AdminController() { }


        /// <summary>
        /// Метод, позволяющий обновлять окно вывода информации
        /// </summary>
        /// <returns>Возвращаемое значение: список пользователей типа EmployeeUnion/returns>
        internal List<EmployeeUnion> Refresh()
        {

            using (ITOnedbContext db = new())
            {
                try
                {
                    return (from Employee in db.Employee
                            join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                            join EmployeeRole in db.EmployeeRole on Employee.RoleId equals EmployeeRole.Id
                            join EmployeeTitle in db.EmployeeTitle on EmployeeInfo.TitleId equals EmployeeTitle.Id
                            select new EmployeeUnion
                            {
                                Id = Employee.Id,
                                FirstName = EmployeeInfo.FirstName,
                                LastName = EmployeeInfo.LastName,
                                Patronymic = EmployeeInfo.Patronymic,
                                Title = EmployeeTitle.Name,
                                RoleName = EmployeeRole.Name,
                                Login = Employee.Login,
                                Password = Employee.Password,
                                EmployeeStatus = Employee.EmployeeStatus
                            }).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка вывода пользователей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return new List<EmployeeUnion>();
            }

            /*dgData.ItemsSource = db.Staff.Join(db.StaffData, st => st.Id, sd => sd.StaffId, (st, sd) => 
                new { st.Id, sd.FirstName, sd.LastName, sd.Patronymic, st.Login, st.Password })
                .Join(db.StaffRoles, st => st.Id, sr => sr.Code, (st, sr) => 
                new {st.Id, st.FirstName, st.LastName, st.Patronymic, sr.Name, st.Login, st.Password }).ToList();*/
        }

        /// <summary>
        /// Метод блокировки учётной записи пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя - int</param>
        public void DisableAccount(int id)
        {
            Employee employeeForDelete = new();
            using (ITOnedbContext db = new())
            {
                try
                {
                    employeeForDelete = db.Employee.Find(id);
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла непредвиденная ошибка, код ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                MessageBoxResult result = MessageBox.Show($"Подтвердите блокировку учетной записи {employeeForDelete.Login}", "Информация", MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    employeeForDelete.EmployeeStatus = 0;
                    db.SaveChanges();

                }

            }
        }
        /// <summary>
        /// Метод обновления/изменения данных выбранного пользователя
        /// </summary>
        /// <param name="employeeForChange">Сотрудник, выбранный для изменения - объект класса EmployeeUnion</param>
        /// <returns name="flag">Возвращаемое значение: булевое значение, сигнализирующее о успехе/ошибке при обновлении/изменении данных пользователя</returns>
        public bool UpdateEmployee(EmployeeUnion employeeForChange)
        {
            if (employeeForChange == null)
            {
                MessageBox.Show("Пользователь не выбран, пожалуйста выберете пользователя для изменения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                AddEmployeeWindow updateWindow = new();
                updateWindow.Title = "Изменение данных пользователя";
                updateWindow.btnAddEmployee.Visibility = Visibility.Collapsed;
                updateWindow.lblEmployeePassword.Visibility = Visibility.Collapsed;
                updateWindow.txbEmployeePassword.Visibility = Visibility.Collapsed;
                updateWindow.CurrentEmployeeId = employeeForChange.Id;
                updateWindow.txbEmployeeFirstName.Text = employeeForChange.FirstName;
                updateWindow.txbEmployeeLastName.Text = employeeForChange.LastName;
                updateWindow.txbEmployeePatronymic.Text = employeeForChange.Patronymic;
                updateWindow.cmbEmployeeRoleName.Text = employeeForChange.RoleName;
                updateWindow.txbEmployeeLogin.Text = employeeForChange.Login;
                updateWindow.cmbEmployeeTitle.Text = employeeForChange.Title;
                updateWindow.ShowDialog();
                if (updateWindow.DialogResult == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Метод изменения пароля пользователя
        /// </summary>
        /// <param name="employeeForChange">Сотрудник, выбранный для изменения пароля - объект класса EmployeeUnion</param>
        /// <returns name"flag">Возвращаемое значение: булевое значение, сигнализирующее о успехе/ошибке при изменении пароля пользователя</returns>
        public bool ChangeEmployeePassword(EmployeeUnion employeeForChange)
        {
            if (employeeForChange == null)
            {
                MessageBox.Show("Пользователь не выбран, пожалуйста выберете пользователя для изменения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else
            {
                ChangePasswordWindow changePasswordWindow = new();
                changePasswordWindow.ShowDialog();
                if (changePasswordWindow.DialogResult == true)
                {
                    using (ITOnedbContext db = new())
                    {
                        Employee employeePassChange = db.Employee.Find(employeeForChange.Id);

                        employeePassChange.Password = PasswordEncryption(changePasswordWindow.Password);
                        db.SaveChanges();
                    }
                    return true;
                }
                return false;
            }


        }
        /// <summary>
        /// Метод, предназначенный для получения списка логов из базы данных
        /// </summary>
        /// <returns>Возвращаемое значение: список логов типа List<LogJurnal></returns>
        internal List<LogJournal> ShowLogs()
        {
            using (ITOnedbContext db = new())
            {
                return db.LogJournal.ToList();
            }
        }
        /// <summary>
        /// Метод для формирования PDF файла на основе списка логов
        /// </summary>
        /// <param name="dgLogData">Поле данных - объект класса DataGrid</param>
        internal void MakeLogPdf(DataGrid dgLogData)
        {
            string path = Path.Combine(Environment.CurrentDirectory, $@"Documents\LogReports\Список логов {DateTime.Now.ToShortDateString()}.pdf");


            Document doc = new();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font normalFont = new Font(baseFont, 10, Font.NORMAL);
            Font boldFont = new Font(baseFont, 12, Font.BOLD);
            Font boldFontParagraph = new Font(baseFont, 20, Font.BOLD);

            doc.Add(new iTextSharp.text.Paragraph($"Список логов", boldFontParagraph));

            PdfPTable table = new PdfPTable(5);


            table.AddCell(new Phrase("Дата", boldFont));
            table.AddCell(new Phrase("Время", boldFont));
            table.AddCell(new Phrase("Имя", boldFont));
            table.AddCell(new Phrase("Логин", boldFont));
            table.AddCell(new Phrase("Действие", boldFont));

            foreach (var logString in dgLogData.ItemsSource as List<LogJournal>)
            {
                table.AddCell(new Phrase(logString.EnterDate, normalFont));
                table.AddCell(new Phrase(logString.EnterTime, normalFont));
                table.AddCell(new Phrase(logString.EmployeeName, normalFont));
                table.AddCell(new Phrase(logString.EmployeeLogin, normalFont));
                table.AddCell(new Phrase(logString.ActionType, normalFont));
            }
            doc.Add(new iTextSharp.text.Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(table);

            doc.Add(new iTextSharp.text.Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });

            doc.Add(new iTextSharp.text.Paragraph($"Дата: {DateTime.Now.ToShortDateString()}", boldFont) { Alignment = 2 });
            doc.Close();
            MessageBox.Show($"Список успешно сохранён, путь: {path}", "Информация", MessageBoxButton.OK, icon: MessageBoxImage.Information);
        }
    }



}


