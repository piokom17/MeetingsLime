
using MeetingsLime.Domain;
using MeetingsLime.Domain.Services;
using MeetingsLime.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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

        //things about thread safety, Make Dictionary the concurrentDictionary, write a unit test for the thread safety
        //I think wha they want is one big endpoint which checks the available times based on:
        //office hours
        //earliest and latest requested meeting date and time
        //desired meeting length (minutes)
        //participants id's (one or multiple)

        //[HttpGet(Name = "GetByUserId")]
        //public IReadOnlyList<MeetingSlot> GetBusyTimeSlotsById()
        //{
        //    var res = _meetingsService.GetMeetingsByUserId("276908764613820584354290536660008166629");

        //    return res;
        //}

        [HttpGet(Name ="GetMeetingSuggestions")] 
        public IActionResult GetMeetingSuggestions(
            [FromQuery] List<string> participants, 
            [FromQuery] int meetingLengthMinutes, 
            [FromQuery] DateTime earliestRequested, 
            [FromQuery] DateTime latestRequested, 
            [FromQuery] int officeStartHour, 
            [FromQuery] int officeEndHour) 
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