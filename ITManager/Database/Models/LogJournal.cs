using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность LogJournal
    /// </summary>
    public partial class LogJournal
    {
        public int Id { get; set; }
        public string EnterDate { get; set; }
        public string EnterTime { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLogin { get; set; }
        public int EmployeeId { get; set; }
        public string ActionType { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
