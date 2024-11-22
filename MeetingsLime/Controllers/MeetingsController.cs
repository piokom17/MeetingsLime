
using MeetingsLime.Domain;
using MeetingsLime.Domain.Services;
using MeetingsLime.Validators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MeetingsLime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingsService _meetingsService;
        private readonly IMeetingSuggestionsValidator _validator;

        public MeetingsController(IMeetingsService meetingsService, IMeetingSuggestionsValidator validator)
        {
            _meetingsService = meetingsService;
            _validator = validator;
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
            _validator.Validate(meetingLengthMinutes, earliestRequested, latestRequested, officeStartHour, officeEndHour);
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