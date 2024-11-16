using MeetingsLime.Domain;
using System.Globalization;

namespace MeetingsLime.Infrastructure
{
    public static class FileManager
    {
        //static class to load a file into the programs memory and then parse it to some Domain classes

        public static string[] LoadFile()
        {
            string filePath = @"C:\Users\User\DEV\freebusy.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                return lines;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("An error occurred furing loading the file: " + ex.Message);
            }
        }

        public static CallendarDataModel ParseToCallendarData(string[] inputLines)
        {
            //maybe get UserCallendar Data to have only one loop iteration

            //var dataList = new List<CallendarDataModel>();
            //Coś ze StringBuilderem
            //var normalizedFile = input.Replace("\r\n", ";");
            //var records = normalizedFile.Split(';');

            //to jest tak naprawde Nazwa i po r\n UserId
            //jakoś mądrze pozbyć się \r\n z tego pliku
            //var sameNames = records.Where(x => x == "C5CAACCED1B9F361761853A7F995A1D4F16C8BCD0A5001A2DF3EC0D7CD539A09AA7DDA1A5278FA07554B0260880882CCBB30B3399C3C0974C587A8233E5788A81DEAD2921123CB12D13CC11318C38B9679D868145315F1BE24333202D12B3787E51D1BBF97BB25482B0EF7E97DE637BAACEDD74E89E2AC52139EE9369F1D64A6\r\n259939411636051033617118653993975778241");

            //var sameFirstPart = records.Where(x => x == "C5CAACCED1B9F361761853A7F995A1D4F16C8BCD0A5001A2DF3EC0D7CD539A09AA7DDA1A5278FA07554B0260880882CCBB30B3399C3C0974C587A8233E5788A81DEAD2921123CB12D13CC11318C38B9679D868145315F1BE24333202D12B3787E51D1BBF97BB25482B0EF7E97DE637BAACEDD74E89E2AC52139EE9369F1D64A6");

            //var suspicious = records.Where(x => x == "259939411636051033617118653993975778241");
            //czesc po \r\n r\n259939411636051033617118653993975778241

            //1. Ignore irregularities czyli jak nie ma nazwy
            //List to store the parsed data
            
            
            List<UserDataModel> dataList = new List<UserDataModel>();
            List<UserCallendar> userCallendarList = new List<UserCallendar>();
            foreach (string line in inputLines) { 
                // Split the line by ';'
                string[] parts = line.Split(';');
                // Check if the line matches the pattern (ID and Name)
                //&& long.TryParse(parts[0], out _)
                //put 2 into const
                if (parts.Length == 2) { 
                    // Create a new DataModel object and add it to the list
                    dataList.Add(new UserDataModel { Id = parts[0].Trim(), Name = parts[1] }); 
                }
                else if(parts.Length == 4)
                {
                    var busyStartDateTime = ParseToDateTime(parts[1]);
                    var busyEndDateTime = ParseToDateTime(parts[2]);

                    if (!IsValidDateTimeSlot(busyStartDateTime, busyEndDateTime))
                        continue;

                    var userCallendar = new UserCallendar { 
                        UserId = parts[0].Trim(), 
                        BusyStartDateTime = busyStartDateTime,
                        BusyEndDateTime = busyEndDateTime,
                        CallendarValue = parts[3] //I don't even know If I need it
                    };
                    userCallendarList.Add(userCallendar);
                }

            }

            return new CallendarDataModel()
            {
                UsersData = dataList.Distinct().ToList(),
                UsersCallendar = userCallendarList.Distinct().ToList(),
            };
        }

        //TODO: to be moved to another class
        private static DateTime ParseToDateTime(string input)
        {
            const string format = "M/d/yyyy h:mm:ss tt";
            DateTime result;
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
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
