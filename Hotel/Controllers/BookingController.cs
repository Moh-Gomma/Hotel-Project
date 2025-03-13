using Hotel.Application.Common.Interfaces;
using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public BookingController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;
        }

        public IActionResult FinalizeBooking(int villaId , DateOnly CheckInDate , 
            int Nights)
        {
            Booking booking = new Booking()
            {
                VillaId = villaId,
                CheckInDate = CheckInDate,
                Nights = Nights
                ,Villa = _unitOfWOrk.Villa.Get(u => u.Id == villaId , includeProperties: "VillaAmenity"),
                CheckOutDate = CheckInDate.AddDays(Nights)
            };

            return View(booking);
        }
    }
}
