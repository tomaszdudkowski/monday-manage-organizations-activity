using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class ProjectTask
    {
        public int TaskID { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić nazwę zadania.")]
        //[StringLength(50)]
        public string TaskName { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić datę zakonczenia zadania.")]
        public DateTime TaskDeadline { get; set; }

        // Nawiguje do Użytkownika tworzącego zadanie
        public int? TaskCreatedBy { get; set; }

        // Nawiguje do obiektu Project
        public int? ProjectID { get; set; }
        //[Required(ErrorMessage = "Proszę wybrać projekt do którego ma być przypisane zadanie.")]
        public Project Project { get; set; }

        // Nawiguje do obiektu Employee
        public int? EmployeeID { get; set; }
        //[Required(ErrorMessage = "Proszę wybrać pracownika do którego ma być przypisane zadanie.")]
        public Employee Employee { get; set; }

        // Wskazuje czy zakończono zadanie
        public bool IsEnd { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
