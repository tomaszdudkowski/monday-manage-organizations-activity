using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mondayWebApp.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeUserID { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić adres email pracownika.")]
        [StringLength(50)]
        [EmailAddress]
        public string EmployeeEmail { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić hasło pracownika.")]
        [StringLength(50)]
        [NotMapped]
        public string EmployeePassword { get; set; }
        [Required(ErrorMessage = "Proszę ponownie wprowadzić hasło pracownika.")]
        [StringLength(50)]
        [NotMapped]
        public string EmployeeCurrentPassword { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić imię pracownika.")]
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić nazwisko pracownika.")]
        [StringLength(50)]
        public string EmployeeSurname { get; set; }
        public string EmployeeNameSurname { get; set; }
        [Required(ErrorMessage = "Proszę wprowadzić datę urodzenia pracownika.")]
        [StringLength(50)]
        public DateTime EmployeeDateOfBirth { get; set; }
        public Address EmployeeAddress { get; set; }
        [Phone]
        [Required(ErrorMessage = "Proszę wprowadzić numer telefonu pracownika.")]
        public string EmployeePhoneNumber { get; set; }
        public string EmployeeRole { get; set; }

        // Nawiguje do obiektu Address
        public virtual Address Address { get; set; }

        // Nawiguje do DepartmentManager
        public Department DepartmentManager { get; set; }

        // Nawiguje do ProjectManager
        public Project ProjectManager { get; set; }

        // Nawiguje do obiektu Department
        public int? DepartmentID { get; set; }
        [Required(ErrorMessage = "Proszę przypisać pracownika do wybranego działu firmy.")]
        public Department Department { get; set; }

        // Nawiguje do obiektu Project
        public int? ProjectID { get; set; }
        [Required(ErrorMessage = "Proszę przypisać pracownika do wybranego projektu.")]
        public Project Project { get; set; }

        // Nawiguje do listy zadań
        public ICollection<ProjectTask> ProjectTasks { get; set; }

        // Wskazuje czy kierownik działu
        public bool IsDepartmentManager { get; set; }

        // Wskazuje czy kierownik projektu
        public bool IsProjectManager { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}