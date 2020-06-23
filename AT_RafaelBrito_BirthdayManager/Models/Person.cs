using System;
using System.ComponentModel.DataAnnotations;

namespace AT_RafaelBrito_BirthdayManager.Models
{
    public class Person
    {
        [Required]
        [Display(Name = "ID")]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Nome")]
        public string Firstname { get; set; }
        [Required]
        [Display(Name = "Sobrenome")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Aniversário(Mês/Dia)")]
        public DateTime Birthday { get; set; }

        public int DaysForBirthday()
        {
            DateTime birthday = new DateTime(DateTime.Now.Year, Birthday.Month, Birthday.Day);

            DateTime today = DateTime.Today;
            DateTime next = birthday.AddYears(today.Year - birthday.Year);

            if (next < today)
                next = next.AddYears(1);

            return (next - today).Days;
        }
    }
}
