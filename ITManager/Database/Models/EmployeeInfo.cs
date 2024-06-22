using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность EmployeeInfo
    /// </summary>
    public partial class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int TitleId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual EmployeeTitle Title { get; set; }
    }
}
