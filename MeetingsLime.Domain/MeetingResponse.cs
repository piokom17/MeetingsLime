using MeetingsLime.Infrastructure;

namespace MeetingsLime.Domain
{
    public sealed class MeetingResponse
    {
        public UserDataModel User { get; }
        public IList<MeetingSlot> MeetingSlots { get; }

        public MeetingResponse(UserDataModel user, IList<MeetingSlot> meetingSlots)
        {
            User = user;
            MeetingSlots = meetingSlots;
        }
    }
}
