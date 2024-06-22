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
using ITManager.Model;

namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для ProjectInfoWindow.xaml
    /// </summary>
    public partial class ProjectInfoWindow : Window
    {
        Project ChosenProject { get;set; }
        public ProjectInfoWindow(int projectId)
        {
            InitializeComponent();
            using (ITOnedbContext db = new()) 
            {
                ChosenProject = db.Project.Find(projectId);


                lbProjectName.Content = $"{ChosenProject.Name}";
                lbCreationDate.Content = $"{ChosenProject.CreationDate.ToString()}";
                lbDeadlineDate.Content = $"{ChosenProject.DeadlineDate.ToString()}";
                txbProjectDescription.Text = $"{ChosenProject.Summary}";
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
