
using MeetingsLime.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsLime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        public MeetingsController()
        {
        }

        [HttpGet(Name = "Test")]
        public string Get()
        {
            var file = FileManager.LoadFile();
            var callendar = FileManager.ParseToCallendarData(file);
            return "test";
        }
    }
}