using System.Numerics;

namespace MeetingsLime.Domain
{
    public sealed record Calendar
    {
        //Pack it into the User record
        public string UserId { get; set; }

        public string UserName { get; set; }


        //Slot
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }


        //Model powinien wyglądac tak:
        //string UserId
        //string UserName
        //IReadOnlyList<Slots> Slots
        //
        //Slot
        //StartDateTime
        //EndDateTime

        //ChatGPT wypluł to:
        //public BigInteger LargeNumber { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public BigInteger HexNumber { get; set; }


    }
}