using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность Task
    /// </summary>
    public partial class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }
        public int StateId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Summary { get; set; }
        public int PriorityId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual TaskPriority Priority { get; set; }
        public virtual Project Project { get; set; }
        public virtual TaskState State { get; set; }
    }
}
