using ITManager.Model;
using ITManager.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ITManager.Controller
{
    /// <summary>
    /// Класс, объединяющий в себе информацию о задаче из сущностей Task, TaskState, Project, TaskPriority, Employee, EmployeeInfo
    /// </summary>
    public class TaskUnion
    {
        public int TaskId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string TaskStatus { get; set; }
        public string TaskPriority { get; set; }
        public string TaskSummary { get; set; }
        public string CreationDate { get; set; }
        public string DeadlineDate { get; set; }
        public string CompleteDate { get; set; }
        public string EmployeeName { get; set; }
    };
    public class UserController
    {

        /// <summary>
        /// Метод вывода всех задач, относящихся к выбранному проекту
        /// </summary>
        /// <param name="currentProjectId">Идентификатор выбранного проекта - int</param>
        /// <returns>Возвращаемое значение: список задач типа List<TaskUnion></returns>
        internal List<TaskUnion> ShowAllTasksByProject(int currentProjectId)
        {
            List<TaskUnion> tasksOnProject = new();
            using (ITOnedbContext db = new())
            {
                tasksOnProject = (from Task in db.Task
                        join TaskState in db.TaskState on Task.StateId equals TaskState.Id
                        join Project in db.Project on Task.ProjectId equals Project.Id
                        join TaskPriority in db.TaskPriority on Task.PriorityId equals TaskPriority.Id
                        join Employee in db.Employee on Task.EmployeeId equals Employee.Id
                        join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                        where Task.ProjectId.Equals(currentProjectId)
                        select new TaskUnion
                        {
                            TaskId = Task.Id,
                            ProjectName = Project.Name,
                            TaskName = Task.Summary,
                            TaskStatus = TaskState.Name,
                            TaskPriority = TaskPriority.Name.ToString(),
                            CreationDate = Task.CreationDate.ToString("dd.MM.yyyy"),
                            DeadlineDate = Task.DeadlineDate.HasValue ? Task.DeadlineDate.Value.ToString("dd.MM.yyyy") : "",
                            CompleteDate = Task.CompleteDate.HasValue ? Task.CompleteDate.Value.ToString("dd.MM.yyyy") : "",
                            EmployeeName = String.Join(" ", EmployeeInfo.FirstName, EmployeeInfo.LastName),
                        }).ToList();
            }
            if (tasksOnProject.Count != 0) 
            {
                return tasksOnProject;
            }
            else
            {
                MessageBox.Show("По данному проекту пока не добавлено ни одной задачи", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                ProjectsViewWindow projectsViewWindow = new();
                projectsViewWindow.ShowDialog();
                return null;
            }
        }



        /// <summary>
        /// Метод вывода всех задач, относящихся к текущему пользователю по выбранному проекту
        /// </summary>
        /// <param name="currentEmployeeId">Идентификатор текущего пользователя - int</param>
        /// <param name="currentProjectId">Идентификатор выбранного проекта - int</param>
        /// <returns>Возвращаемое значение: список задач типа List<TaskUnion></returns>
        internal List<TaskUnion> ShowAllTasksByUserAndProject(int currentEmployeeId, int currentProjectId)
        {
            using (ITOnedbContext db = new())
            {
                return (from Task in db.Task

                        join TaskState in db.TaskState on Task.StateId equals TaskState.Id
                        join Project in db.Project on Task.ProjectId equals Project.Id
                        join TaskPriority in db.TaskPriority on Task.PriorityId equals TaskPriority.Id
                        join Employee in db.Employee on Task.EmployeeId equals Employee.Id
                        join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                        where Task.EmployeeId.Equals(currentEmployeeId) && Task.ProjectId.Equals(currentProjectId)
                        select new TaskUnion
                        {
                            TaskId = Task.Id,
                            ProjectName = Project.Name,
                            TaskName = Task.Summary,
                            TaskStatus = TaskState.Name,
                            TaskPriority = TaskPriority.Name.ToString(),
                            TaskSummary = Task.Summary,
                            CreationDate = Task.CreationDate.ToString("dd.MM.yyyy"),
                            DeadlineDate = Task.DeadlineDate.HasValue ? Task.DeadlineDate.Value.ToString("dd.MM.yyyy") : "",
                            CompleteDate = Task.CompleteDate.HasValue ? Task.CompleteDate.Value.ToString("dd.MM.yyyy") : " ",
                            EmployeeName = String.Join(" ", EmployeeInfo.FirstName, EmployeeInfo.LastName)
                        }).ToList();
            }
        }

        /// <summary>
        /// Метод формирования данных для отчёта по всем задачам текущего пользователя 
        /// </summary>
        /// <param name="currentEmployeeId">Идентификатор текущего пользователя - int</param>
        /// <returns>Возвращаемое значение: список задач типа List<TaskUnion></returns>
        internal List<TaskUnion> MakeDataForReport(int currentEmployeeId)
        {
            using (ITOnedbContext db = new())
            {

                return (from Task in db.Task

                        join TaskState in db.TaskState on Task.StateId equals TaskState.Id
                        join Project in db.Project on Task.ProjectId equals Project.Id
                        join TaskPriority in db.TaskPriority on Task.PriorityId equals TaskPriority.Id
                        join Employee in db.Employee on Task.EmployeeId equals Employee.Id
                        join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                        where Task.EmployeeId.Equals(currentEmployeeId)
                        select new TaskUnion
                        {
                            TaskId = Task.Id,
                            ProjectName = Project.Name,
                            TaskName = Task.Summary,
                            TaskStatus = TaskState.Name,
                            TaskPriority = TaskPriority.Name,
                            TaskSummary = Task.Summary,
                            CreationDate = Task.CreationDate.ToString("dd.MM.yyyy"),
                            DeadlineDate = Task.DeadlineDate.HasValue ? Task.DeadlineDate.Value.ToString("dd.MM.yyyy") : "",
                            CompleteDate = Task.CompleteDate.HasValue ? Task.CompleteDate.Value.ToString("dd.MM.yyyy") : " ",
                            EmployeeName = String.Join(" ", EmployeeInfo.EmployeeId, EmployeeInfo.EmployeeId)
                        }).ToList();
            }
        }

        /// <summary>
        /// Метод, предназначенный для смены статуса задачи пользователем
        /// </summary>
        /// <param name="chosenTask">Выбранная задача - объект класса TaskUnion</param>
        internal void ChangeTaskStatus(TaskUnion chosenTask)
        {
            if (chosenTask != null)
            {
                Model.Task taskForChange = new();
                StatusChangeWindow statusChangeView = new StatusChangeWindow();
                statusChangeView.ShowDialog();
                if (statusChangeView.DialogResult == true)
                {
                    using (ITOnedbContext db = new())
                    {
                        taskForChange = db.Task.Find(chosenTask.TaskId);
                        taskForChange.StateId = db.TaskState.Where(ts => ts.Name.Equals(statusChangeView.cbStatuses.Text)).Select(ts => ts.Id).FirstOrDefault();
                        if (db.TaskState.Where(ts => ts.Id.Equals(taskForChange.StateId)).Select(ts => ts.Name).FirstOrDefault().Equals("Завершена"))
                        {
                            taskForChange.CompleteDate = DateTime.Now.Date;
                        }
                        
                        if (statusChangeView.DialogResult == true)
                        {
                            MessageBox.Show("Статус изменён", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            db.SaveChanges();
                           
                        }
                       
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Задача не выбрана, пожалуйста выберите задачу", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        /// <summary>
        /// Метод поиска задач по их наименованию
        /// </summary>
        /// <returns>Возвращаемое значение: список задач типа List<TaskUnion></returns>
        internal List<TaskUnion> SearchTasks()
        {
            SearchWindow searchView = new();
            searchView.ShowDialog();
            if (searchView.DialogResult == true)
            {
                using (ITOnedbContext db = new())
                {
                    List<TaskUnion> taskUnions = (from Task in db.Task

                                                  join TaskState in db.TaskState on Task.StateId equals TaskState.Id
                                                  join Project in db.Project on Task.ProjectId equals Project.Id
                                                  join TaskPriority in db.TaskPriority on Task.PriorityId equals TaskPriority.Id
                                                  join Employee in db.Employee on Task.EmployeeId equals Employee.Id
                                                  join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                                                  where Task.Name.Contains(searchView.TaskName)
                                                  select new TaskUnion
                                                  {
                                                      TaskId = Task.Id,
                                                      ProjectName = Project.Name,
                                                      TaskName = Task.Name,
                                                      TaskStatus = TaskState.Name,
                                                      TaskPriority = TaskPriority.Name,
                                                      TaskSummary = Task.Summary,
                                                      CreationDate = Task.CreationDate.ToString("dd.MM.yyyy"),
                                                      DeadlineDate = Task.DeadlineDate.HasValue ? Task.DeadlineDate.Value.ToString("dd.MM.yyyy") : "",
                                                      CompleteDate = Task.CompleteDate.HasValue ? Task.CompleteDate.Value.ToString("dd.MM.yyyy") : " ",
                                                      EmployeeName = String.Join(" ", EmployeeInfo.FirstName, EmployeeInfo.LastName)
                                                  }).ToList();
                    if (taskUnions.Count > 0)
                    {
                        return taskUnions;
                    }
                    else
                    {
                        MessageBox.Show("По результатам вашего запроса ничего не найдено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        return new List<TaskUnion>();
                    }
                  
                }
            }
            else
            {
                return new List<TaskUnion>();
            }
        }

        /// <summary>
        /// Метод вывода всех задач, относящихся к текущему пользователю
        /// </summary>
        /// <param name="currentEmployeeId">Идентификатор текущего пользователя - int</param>
        /// <returns>Возвращаемое значение: список задач типа List<TaskUnion></returns>
        internal List<TaskUnion> ShowAllTasksByUser(int currentEmployeeId)
        {

            using (ITOnedbContext db = new())
            {
                List<TaskUnion> taskUnions = (from Task in db.Task
                                              join TaskState in db.TaskState on Task.StateId equals TaskState.Id
                                              join Project in db.Project on Task.ProjectId equals Project.Id
                                              join TaskPriority in db.TaskPriority on Task.PriorityId equals TaskPriority.Id
                                              join Employee in db.Employee on Task.EmployeeId equals Employee.Id
                                              join EmployeeInfo in db.EmployeeInfo on Employee.Id equals EmployeeInfo.EmployeeId
                                              where Task.EmployeeId.Equals(currentEmployeeId)
                                              select new TaskUnion
                                              {
                                                  TaskId = Task.Id,
                                                  ProjectName = Project.Name,
                                                  TaskName = Task.Name,
                                                  TaskStatus = TaskState.Name,
                                                  TaskPriority = TaskPriority.Name,
                                                  TaskSummary = Task.Summary,
                                                  CreationDate = Task.CreationDate.ToString("dd.MM.yyyy"),
                                                  DeadlineDate = Task.DeadlineDate.HasValue ? Task.DeadlineDate.Value.ToString("dd.MM.yyyy") : "",
                                                  CompleteDate = Task.CompleteDate.HasValue ? Task.CompleteDate.Value.ToString("dd.MM.yyyy") : " ",
                                                  EmployeeName = String.Join(" ", EmployeeInfo.FirstName, EmployeeInfo.LastName)
                                              }).ToList();
                return taskUnions;
            }
        }

        /// <summary>
        /// Метод проверки задачи на принадлежность к текущему пользователю, если задача не принадлежит текущему пользователю, текущий пользователь не имеет возможности редактирования статуса данной задачи
        /// </summary>
        /// <param name="chosenTask">Выбранная задача - объект класса TaskUnion</param>
        /// <param name="currentEmployee">Текущий сотрудник - объект класса EmployeeUnion</param>
        /// <returns>Возвращаемое значение: булевый флаг, сигнализирующий о принадлености/не пренадлежности задачи к текущему пользователю</returns>
        internal bool CheckTask(TaskUnion chosenTask, EmployeeInfo currentEmployee)
        {
            using (ITOnedbContext db = new())
            {
                Model.Task taskForCheck = db.Task.Find(chosenTask.TaskId);
                if (taskForCheck.EmployeeId != currentEmployee.EmployeeId)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


        }

        /// <summary>
        /// Метод создания PDF отчёта на основе данных по задачам текущего пользователя
        /// </summary>
        /// <param name="currentEmployee">Информация о текущем пользователе - объект класса EmployeeInfo</param>
        /// <param name="taskForReport">Список задач для отчёта - список типа List<TaskUnion></param>
        /// <param name="completedTasks">Количество выполненных задач - double</param>
        /// <param name="tasksInWork">Количество задач в работе - double</param>
        internal void MakePdf(EmployeeInfo currentEmployee, List<TaskUnion> taskForReport, double completedTasks, double tasksInWork)
        {
            
            string path = Path.Combine(Environment.CurrentDirectory,$@"Documents\UserReports\Отчёт {currentEmployee.LastName} {DateTime.Now.ToShortDateString()}.pdf");
            int allTasks = taskForReport.Count;
            double percentage = (completedTasks / allTasks) * 100;
            Document doc = new();
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font normalFont = new Font(baseFont, 12, Font.NORMAL);
            Font boldFont = new Font(baseFont, 12, Font.BOLD);
            Font boldFontParagraph = new Font(baseFont, 20, Font.BOLD);

            doc.Add(new Paragraph($"Отчёт по задачам сотрудника: {String.Join(" ", currentEmployee.FirstName, currentEmployee.LastName)}", boldFontParagraph));

            PdfPTable table = new PdfPTable(3);
          

            table.AddCell(new Phrase("Проект", boldFont));
            table.AddCell(new Phrase("Задача",boldFont));
            table.AddCell(new Phrase("Статус",boldFont));

            foreach (var task in taskForReport) 
            {
                table.AddCell(new Phrase(task.ProjectName, normalFont));
                table.AddCell(new Phrase(task.TaskName, normalFont));
                table.AddCell(new Phrase(task.TaskStatus, normalFont));
            }
            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(table);

            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(new Paragraph($"Всего задач: {allTasks}", boldFont));

            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(new Paragraph($"Задач выполнено: {completedTasks}", boldFont));

            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(new Paragraph($"Задач в работе: {tasksInWork}", boldFont));

            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(new Paragraph($"Процент выполненных задач: {Convert.ToInt32(percentage)}%", boldFont));

            doc.Add(new Paragraph("") { SpacingBefore = 10, SpacingAfter = 10 });
            doc.Add(new Paragraph($"Дата: {DateTime.Now.ToShortDateString()}", boldFont) {Alignment =2});
            doc.Close();
            MessageBox.Show( $"Отчёт успешно сохранён, путь: {path}", "Информация", MessageBoxButton.OK, icon:MessageBoxImage.Information);
        }
    }
}
