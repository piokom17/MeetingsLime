using MeetingsLime.Domain;

namespace MeetingsLime.Infrastructure
{
    public class CallendarDataModel
    {
        public IReadOnlyList<UserDataModel> UsersData { get; set; }

        public IReadOnlyList<UserCallendar> UsersCallendar { get; set; }

    }
}
