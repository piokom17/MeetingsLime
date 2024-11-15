using System.Numerics;

namespace MeetingsLime.Infrastructure
{
    public class CallendarDataModel
    {
        public BigInteger UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BigInteger UserName { get; set; }
    }
}
