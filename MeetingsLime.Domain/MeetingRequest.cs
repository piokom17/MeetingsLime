namespace MeetingsLime.Domain
{
    public sealed class MeetingRequest { 
        public List<string> EmployeeIds { get; set; } 
        public int MeetingLengthMinutes { get; set; } 
        public DateTime EarliestRequested { get; set; } 
        public DateTime LatestRequested { get; set; } 
        public int OfficeStartHour { get; set; } 
        public int OfficeEndHour { get; set; } 
    }
}
