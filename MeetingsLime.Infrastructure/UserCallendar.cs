namespace MeetingsLime.Infrastructure
{
    public class UserCallendar
    {
        public string UserId { get; set; }

        public DateTime BusyStartDateTime { get; set; }

        public DateTime BusyEndDateTime { get; set; }

        //I don't what would this value be for, so for now I'm keeping it but it can be removed when finished
        public string CallendarValue { get; set; }

    }
}
