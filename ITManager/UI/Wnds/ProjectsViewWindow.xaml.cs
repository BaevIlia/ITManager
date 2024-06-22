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
    /// Логика взаимодействия для ProjectsView.xaml
    /// </summary>
    public partial class ProjectsViewWindow : Window
    {
        internal Project ChosenProject { get; set; }

        public ProjectsViewWindow()
        {
            InitializeComponent();
           
            List<Project> projectsNames = new();
            using (ITOnedbContext db = new()) 
            {
                projectsNames = db.Project.ToList();
                lvProjectNames.ItemsSource = projectsNames;
            }
            
        }

        private void btnChooseProject_Click(object sender, RoutedEventArgs e)
        {
            
            DialogResult = true;
            this.Close();
           
        }

        private void btnInfoAboutProject_Click(object sender, RoutedEventArgs e)
        {
            using (ITOnedbContext db = new()) 
            {
                ChosenProject = lvProjectNames.SelectedItem as Project;
            }
            ProjectInfoWindow projectInfoWindow = new(ChosenProject.Id);
            projectInfoWindow.Show();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
