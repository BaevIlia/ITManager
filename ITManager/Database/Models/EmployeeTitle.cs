using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность EmployeeTitle
    /// </summary>
    public partial class EmployeeTitle
    {
        public EmployeeTitle()
        {
            EmployeeInfos = new HashSet<EmployeeInfo>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmployeeInfo> EmployeeInfos { get; set; }
    }
}
