using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models.ViewModels
{
    public class ProjectTaskViewModel
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public DateTime TaskDeadline { get; set; }

        public string TaskCreatedBy { get; set; }

        // Wskazuje czy zakończono zadanie
        public bool IsEnd { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
