using System;
using System.Collections.Generic;

#nullable disable

namespace ITManager.Model
{
    /// <summary>
    /// Класс, интерпретирующий сущность Project
    /// </summary>
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Summary { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
