namespace MeetingsLime.Infrastructure
{
    public class MeetingSlot
    {
        public DateTime StartDateTime { get; }

        public DateTime EndDateTime { get; }

        public MeetingSlot(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
        }
    }
}
