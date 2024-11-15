using System.Globalization;
using System.Numerics;

namespace MeetingsLime.Infrastructure
{
    public static class FileManager
    {
        //static class to load a file into the programs memory and then parse it to some Domain classes
        public static List<CallendarDataModel> ParseData(string input) { 
            var dataList = new List<CallendarDataModel>(); 
            var records = input.Split(';'); 
            
            for (int i = 0; i < records.Length; i += 4) 
            { 
                var data = new CallendarDataModel
                { 
                    UserId = BigInteger.Parse(records[i]), 
                    StartDate = DateTime.Parse(records[i + 1], CultureInfo.InvariantCulture), 
                    EndDate = DateTime.Parse(records[i + 2], CultureInfo.InvariantCulture), 
                    UserName = BigInteger.Parse(records[i + 3], NumberStyles.HexNumber) 
                }; 
                dataList.Add(data); 
            }
            
            //We after adding all of the rows we would like to validate all of the rows if they loaded correctly
            return dataList;
            

        }
    }
}
