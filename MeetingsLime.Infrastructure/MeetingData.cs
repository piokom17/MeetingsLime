using System.Globalization;

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
                    var busyStartDateTime = ParseToDateTime(parts[1]);
                    var busyEndDateTime = ParseToDateTime(parts[2]);

                    if (!IsValidDateTimeSlot(busyStartDateTime, busyEndDateTime))
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

        private static DateTime ParseToDateTime(string input)
        {
            const string format = "M/d/yyyy h:mm:ss tt";
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {

                return result;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private static bool IsValidDateTimeSlot(DateTime start, DateTime end)
        {
            if (start == DateTime.MinValue || end == DateTime.MinValue)
            {
                return false;
            }
            else
                return true;
        }
    }
}
