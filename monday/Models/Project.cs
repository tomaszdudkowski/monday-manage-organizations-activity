using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public string ProjectBrief { get; set; }
        public DateTime ProjectDeadline { get; set; }

        // Nawiguje do zadań projektu
        public ICollection<ProjectTask> ProjectTasks { get; set; }

        // Nawiguje do listy pracwoników
        public ICollection<Employee> Employees { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
