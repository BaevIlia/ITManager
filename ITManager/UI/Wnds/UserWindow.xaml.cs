using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ITManager.Controller;
using ITManager.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>

    public partial class UserWindow : Window
    {


        internal EmployeeInfo CurrentUser { get; set; }
        internal Project CurrentProject { get; set; }
        string Title { get; set; }

        internal UserController controller = new();
        public UserWindow() { }
        public UserWindow(int employeeId)
        {


            CurrentUser = ControllerBase.FindInfoById(employeeId);
            InitializeComponent();
            lblUserName.Content = $"Пользователь: {CurrentUser.FirstName} {CurrentUser.LastName}";
            using (ITOnedbContext db = new())
            {
                Title = db.EmployeeTitle.Where(t => t.Id.Equals(CurrentUser.TitleId)).Select(t => t.Name).FirstOrDefault();
            }
            lblTitle.Content = $"Должность: {Title}";
        }

        private void btnTasksShow_Click(object sender, RoutedEventArgs e)
        {

            ProjectsViewWindow projectsView = new();
            projectsView.ShowDialog();
            if (projectsView.DialogResult == true)
            {
                CurrentProject = projectsView.lvProjectNames.SelectedItem as Project;
                try
                {
                    dgTasksInfo.ItemsSource = controller.ShowAllTasksByProject(CurrentProject.Id);
                }
                catch (NullReferenceException ex)
                {
                    MessageBox.Show("Проект не выбран, пожалуйста выберете проект", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Возникла непредвиденная ошибка, пожалуйста обратитесь к администратору, код ошибки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }




        }


        private void btnMakeReport_Click(object sender, RoutedEventArgs e)
        {
            List<TaskUnion> dataForReprot = controller.MakeDataForReport(CurrentUser.EmployeeId);
            int complitedTasksCount = dataForReprot.Where(t => t.TaskStatus.Equals("Завершена")).Count();
            int inWorkTasksCount = dataForReprot.Where(t => t.TaskStatus.Equals("В работе")).Count();

            controller.MakePdf(CurrentUser, dataForReprot, complitedTasksCount, inWorkTasksCount);

        }

        private void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {


            TaskUnion chosenTask = dgTasksInfo.SelectedItem as TaskUnion;

            if (controller.CheckTask(chosenTask, CurrentUser))
            {
                if (chosenTask.TaskStatus == "Завершена")
                {
                    MessageBox.Show("Вы не можете сменить статус для выполненной задачи", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    controller.ChangeTaskStatus(chosenTask);
                    dgTasksInfo.ItemsSource = controller.ShowAllTasksByUser(CurrentUser.EmployeeId);

                }

            }
            else
            {
                MessageBox.Show("Вы не можете изменять статус чужой задачи", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            List<TaskUnion> searchList = controller.SearchTasks();
            if (searchList.Count > 0)
            {
                dgTasksInfo.ItemsSource = searchList;
            }
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            List<TaskUnion> taskUnions = dgTasksInfo.ItemsSource as List<TaskUnion>;
            taskUnions = taskUnions.OrderBy(t => t.TaskPriority.Equals("Низкий")).ThenBy(t => t.TaskPriority.Equals("Средний")).ToList();

            dgTasksInfo.ItemsSource = taskUnions;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            ControllerBase.LogWriteSession(ControllerBase.FindById(CurrentUser.EmployeeId), CurrentUser, "Выход");
            CurrentUser = null;
            CurrentProject = null;
            Title = null;
            this.Close();
        }



        private void btnEmployeeTasksProject_Click(object sender, RoutedEventArgs e)
        {

            if (controller.ShowAllTasksByUser(CurrentUser.EmployeeId).Count == 0)
            {
                MessageBox.Show("У данного пользователя пока нет задач", "Информация", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
            {
                dgTasksInfo.ItemsSource = controller.ShowAllTasksByUser(CurrentUser.EmployeeId);
            }
        }

        private void btnEmployeePerProjectTasksShow_Click(object sender, RoutedEventArgs e)
        {
            ProjectsViewWindow projectsView = new();
            projectsView.ShowDialog();
            if (projectsView.DialogResult == true)
            {
                CurrentProject = projectsView.lvProjectNames.SelectedItem as Project;
            }
            if (CurrentProject != null)
            {
                if (controller.ShowAllTasksByUserAndProject(CurrentUser.EmployeeId, CurrentProject.Id).Count == 0)
                {
                    MessageBox.Show("На данном проекте у пользователя пока нет задач", "Информация", MessageBoxButton.OK, MessageBoxImage.Question);
                }
                else
                {
                    dgTasksInfo.ItemsSource = controller.ShowAllTasksByUserAndProject(CurrentUser.EmployeeId, CurrentProject.Id);
                }
            }

        }
    }
}
