using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class Task
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDeadline { get; set; }
        public Employee TaskCreatedBy { get; set; }
        public Employee TaskEmployeeResponsibleFor { get; set; }

        // Nawiguje do obiektu Project
        public Project Project { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
