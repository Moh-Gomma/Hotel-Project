using Hotel.Application.Common.Interfaces;
using Hotel.Application.Utility;
using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hotel.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWOrk;

        public BookingController(IUnitOfWork unitOfWOrk)
        {
            _unitOfWOrk = unitOfWOrk;
        }
        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateTime checkInDate,
            int Nights)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWOrk.User.Get(x => x.Id == UserId);


            Booking booking = new Booking()
            {
                VillaId = villaId,
                CheckInDate = checkInDate,

                Nights = Nights
                ,
                Villa = _unitOfWOrk.Villa.Get(u => u.Id == villaId, includeProperties: "VillaAmenity"),
                CheckOutDate = checkInDate.AddDays(Nights),
                UserId = UserId,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            booking.TotalCost = booking.Villa.Price * Nights;

            return View(booking);
        }

        [Authorize]
        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var villa = _unitOfWOrk.Villa.Get(u => u.Id == booking.VillaId);
            booking.TotalCost = villa.Price * booking.Nights;

            booking.BookingNumber = GenerateUniqueBookNumber();// Unique

            booking.status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            _unitOfWOrk.Booking.Add(booking);
            _unitOfWOrk.Save();


            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        [Authorize]
        public IActionResult BookingConfirmation(int bookingId)
        {
            return View(bookingId);
        }

        private string GenerateUniqueBookNumber()
        {
            var now = DateTime.Now;
            string prefix = "BK";
            string dataPart = now.ToString("yyMMdd");
            string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
            string bookingNumber = $"{prefix}{dataPart}-{randomPart}";

            while (_unitOfWOrk.Booking.Any(b => b.BookingNumber == bookingNumber))
            {
                randomPart = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
                bookingNumber = $"{prefix}{dataPart}-{randomPart}";
            }
            return bookingNumber;
        }
    }
}
