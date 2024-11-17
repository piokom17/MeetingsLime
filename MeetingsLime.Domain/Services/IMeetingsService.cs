using MeetingsLime.Infrastructure;

namespace MeetingsLime.Domain.Services
{
    public interface IMeetingsService
    {
        IReadOnlyList<MeetingSlot> GetMeetingsByUserId(string userId);

        IReadOnlyList<MeetingResponse> GetMeetingSuggestions(MeetingRequest request);
    }
}
