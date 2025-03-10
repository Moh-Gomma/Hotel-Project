using Hotel.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Villa>? VillaList { get; set; }

        [Display(Name = "Check-in Date")]
        public DateOnly CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }
        [Range(1, 30, ErrorMessage = "Number of nights must be between 1 and 30.")]
        public int Nights { get; set; }
    }
}
