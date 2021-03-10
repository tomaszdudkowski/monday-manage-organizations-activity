using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace mondayWebApp.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwę działu firmy.")]
        [StringLength(100)]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić opis działu firmy.")]
        [StringLength(400)]
        public string DepartmentDesc { get; set; }
        //[Required(ErrorMessage = "Proszę wprowadzić datę powstania działu firmy.")]
        public DateTime DepartmentEstablishmentDate { get; set; }

        // Nawiguje do DepartmentManager
        public int? DepartmentManagerID { get; set; }
        //[Required(ErrorMessage = "Proszę wybrać kierownika działu firmy.")]
        public Employee DepartmentManager { get; set; }

        // Pracownicy działu
        public ICollection<Employee> Employees { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}
