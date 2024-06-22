using iTextSharp.text.pdf;
using iTextSharp.text;
using ITManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using ITManager.Controller;

namespace ITManager.View
{
    /// <summary>
    /// Логика взаимодействия для LogShowWindow.xaml
    /// </summary>
    public partial class LogShowWindow : Window
    {
        public LogShowWindow(List<LogJournal> logJournal)
        {
            InitializeComponent();
            dgLogData.ItemsSource = logJournal;
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            AdminController adminController = new();
            adminController.MakeLogPdf(dgLogData);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            dgLogData.ItemsSource = null;
            this.Close();
        }
    }
}
