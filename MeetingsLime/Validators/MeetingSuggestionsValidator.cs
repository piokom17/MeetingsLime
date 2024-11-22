namespace MeetingsLime.Validators
{
    public class MeetingSuggestionsValidator : IMeetingSuggestionsValidator
    {
        const int MinimumMeetingMinutesLength = 1;
        const int MaximumMeetingMinutesLength = 180;
        const int EarliestWorkingHourForHuman = 6;
        const int LatestWorkingHourForHuman = 18;

        public void Validate(
            int meetingLengthMinutes,
            DateTime earliestRequested,
            DateTime latestRequested,
            int officeStartHour,
            int officeEndHour)
        {
            if (meetingLengthMinutes < MinimumMeetingMinutesLength || meetingLengthMinutes > MaximumMeetingMinutesLength)
            {
                throw new InvalidDataException("Meeting length in minutes should be greater than 0, and should not exceed the 180minutes");
            }

            if (officeStartHour < EarliestWorkingHourForHuman)
            {
                throw new InvalidDataException("It's too early for human to work...");
            }

            if (officeEndHour > LatestWorkingHourForHuman)
            {
                throw new InvalidDataException("It's far too late for human to work...");
            }

            if (earliestRequested.Hour < officeStartHour)
            {
                throw new InvalidDataException("Earliest requested hour can't be earlier than office start hour");
            }

            if (latestRequested.Hour > officeEndHour)
            {
                throw new InvalidDataException("Latest requested hour can't be earlier than office latest hour");
            }

            if (earliestRequested.Day != latestRequested.Day)
            {
                throw new InvalidDataException("Meetings can last only one day");
            }

        }        
    }
}
