using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность Employee
    /// </summary>
    public partial class Employee
    {
        public Employee()
        {
            LogJournals = new HashSet<LogJournal>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int EmployeeStatus { get; set; }

        public virtual EmployeeRole Role { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public virtual ICollection<LogJournal> LogJournals { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
