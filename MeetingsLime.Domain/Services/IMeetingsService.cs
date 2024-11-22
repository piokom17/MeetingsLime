using MeetingsLime.Infrastructure;

namespace MeetingsLime.Domain.Services
{
    public interface IMeetingsService
    {
        IReadOnlyList<MeetingResponse> GetMeetingSuggestions(MeetingRequest request);
    }
}
