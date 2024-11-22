using MeetingsLime.Infrastructure;

namespace MeetingsLime.UnitTests
{
    public class MeetingDataStub
    {
        public Meeting Data { get; private set; }

        public MeetingDataStub()
        {
            Data = new Meeting
            {
                UserTimeSlots = new Dictionary<UserDataModel, List<MeetingSlot>>()
            };
        }

        public void SetMockData(Dictionary<UserDataModel, List<MeetingSlot>> mockUserTimeSlots)
        {
            Data = new Meeting
            {
                UserTimeSlots = mockUserTimeSlots
            };
        }

        public void AddMockUserTimeSlots(UserDataModel user, List<MeetingSlot> slots)
        {
            if (Data.UserTimeSlots.ContainsKey(user))
            {
                Data.UserTimeSlots[user].AddRange(slots);
            }
            else
            {
                Data.UserTimeSlots[user] = new List<MeetingSlot>(slots);
            }
        }
    }
}
