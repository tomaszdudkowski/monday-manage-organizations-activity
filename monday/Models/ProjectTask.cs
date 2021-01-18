using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class ProjectTask
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDeadline { get; set; }

        // Nawiguje do Użytkownika tworzącego zadanie
        public int? TaskCreatedBy { get; set; }

        // Nawiguje do obiektu Project
        public int? ProjectID { get; set; }
        public Project Project { get; set; }

        // Nawiguje do obiektu Employee
        public int? EmployeeID { get; set; }
        public Employee Employee { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
