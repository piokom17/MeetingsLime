namespace MeetingsLime.Validators
{
    public interface IMeetingSuggestionsValidator
    {
        void Validate(
            int meetingLengthMinutes,
            DateTime earliestRequested,
            DateTime latestRequested,
            int officeStartHour,
            int officeEndHour);
    }
}
