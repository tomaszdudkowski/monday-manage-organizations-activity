using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić nazwę projektu.")]
        [StringLength(50)]
        public string ProjectName { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić opis projektu.")]
        [StringLength(250)]
        public string ProjectDesc { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić założenia projektu.")]
        [StringLength(250)]
        public string ProjectBrief { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić datę zakończenia projektu.")]
        public DateTime ProjectDeadline { get; set; }

        // Nawiguje do ProjectManager
        public int? ProjectManagerID { get; set; }
        //[Required(ErrorMessage = "Proszę wybrać kierownika projektu.")]
        public Employee ProjectManager { get; set; }

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
