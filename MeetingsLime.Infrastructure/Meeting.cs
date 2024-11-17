namespace MeetingsLime.Infrastructure
{
    public class Meeting
    {
        public Dictionary<UserDataModel, List<MeetingSlot>> UserTimeSlots { get; set; }
    }
}
