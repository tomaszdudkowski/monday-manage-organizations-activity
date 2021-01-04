namespace mondayWebApp.Models
{
    public class Address
    {
        public string PostCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }

        // Nawiguje do objektu Employee
        public virtual Employee Employee { get; set; }

        // Blokuje rekord przed edycją
        public bool IsEdited { get; set; }

        // Wskazuje czy zaznazono rekord
        public bool IsChecked { get; set; }
    }
}