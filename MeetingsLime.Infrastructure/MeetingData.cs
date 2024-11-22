using MeetingsLime.Infrastructure.Helpers;

namespace MeetingsLime.Infrastructure
{
    public class MeetingData : IMeetingData
    {
        private readonly string[] _fileLines;
        public MeetingData()
        {
            _fileLines = LoadData();
        }

        private static string[] LoadData()
        {
            //INFO: If you want to run the app locally without Docker, change the file path to the full path where you store the project
            string filePath = @"/app/data/freebusy.txt";
            try
            {
                return File.ReadAllLines(filePath);

            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("An error occurred furing loading the file: " + ex.Message);
            }
        }

        public Meeting CalculateMeetingData()
        {
            List<UserDataModel> usersData = new();
            List<UserCallendar> userCallendarList = new();
            Dictionary<UserDataModel, List<MeetingSlot>> userBusyTimeSlots = new();
            const int userDataPartLength = 2;
            const int userSlotsPartLength = 4;
            const string splitDelimeter = ";";

            foreach (string line in _fileLines)
            {
                string[] parts = line.Split(splitDelimeter);
                if (parts.Length == userDataPartLength)
                {
                    usersData.Add(new UserDataModel { Id = parts[0].Trim(), Name = parts[1] });
                }
                else if (parts.Length == userSlotsPartLength)
                {
                    var busyStartDateTime = DateTimeHelper.ParseToDateTime(parts[1]);
                    var busyEndDateTime = DateTimeHelper.ParseToDateTime(parts[2]);

                    if (!DateTimeHelper.IsValidDateTimeSlot(busyStartDateTime, busyEndDateTime))
                        continue;

                    var userCallendar = new UserCallendar
                    {
                        UserId = parts[0].Trim(),
                        BusyStartDateTime = busyStartDateTime,
                        BusyEndDateTime = busyEndDateTime,
                        CallendarValue = parts[3]
                    };
                    userCallendarList.Add(userCallendar);
                }
            }

            foreach (var item in usersData)
            {
                var userSlots = userCallendarList.Where(x => x.UserId == item.Id);
                userBusyTimeSlots.TryAdd(item, userSlots.Select(x => new MeetingSlot(x.BusyStartDateTime, x.BusyEndDateTime)).ToList());
            }

            return new Meeting()
            {
                UserTimeSlots = userBusyTimeSlots
            };
        }
    }
}
