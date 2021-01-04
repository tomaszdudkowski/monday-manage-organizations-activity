using System;

namespace mondayWebApp.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime EmployeeDateOfBirth { get; set; }
        public Address EmployeeAddress { get; set; }
        public string EmployeePhoneNumber { get; set; }
        public string EmployeeRole { get; set; }

        // Nawiguje do obiektu Address
        public virtual Address Address { get; set; }

        // Nawiguje do obiektu Department
        public int? DepartmentID { get; set; }
        public Department Department { get; set; }

        // Nawiguje do obiektu Project
        public int? ProjectID { get; set; }
        public Project Project { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}