
using MeetingsLime.Domain;
using MeetingsLime.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MeetingsLime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingsService _meetingsService;
        public MeetingsController(IMeetingsService meetingsService)
        {
            _meetingsService = meetingsService;
            //Na starcie programu chce za³adowaæ wszystko z pliku i sparsowaæ na dane do Calendarza,
            //Tak jakby mamy wszystko obliczane przy starcie i zapisywane do jakichœ statycznych zmiennych dostepnych publicznie podczas
            //runtim-u aplikacji?
            //Cha ze wzgledu na Thread safety i lazy loading i wstrzykiwanie zaleca Singleton jako rozwi¹zanie do REST API,
            //Dopytaæ czy rozwa¿amy ¿e jakies dane maj¹ dochodziæ czy bierzemy pod uwagê ¿e ten stan aplikacji jest dany pod dany wycinek czasu
        }

        [HttpGet(Name ="GetMeetingSuggestions")] 
        public IActionResult GetMeetingSuggestions(
            [FromQuery, Required] List<string> participants, 
            [FromQuery, Required] int meetingLengthMinutes, 
            [FromQuery, Required] DateTime earliestRequested, 
            [FromQuery, Required] DateTime latestRequested, 
            [FromQuery, Required] int officeStartHour, 
            [FromQuery, Required] int officeEndHour) 
        { 
            //TODO: validation

            var request = new MeetingRequest { 
                EmployeeIds = participants, 
                MeetingLengthMinutes = meetingLengthMinutes, 
                EarliestRequested = earliestRequested, 
                LatestRequested = latestRequested, 
                OfficeStartHour = officeStartHour, 
                OfficeEndHour = officeEndHour
            }; 

            var suggestions = _meetingsService.GetMeetingSuggestions(request);
            return Ok(suggestions); 
        }

    }
}