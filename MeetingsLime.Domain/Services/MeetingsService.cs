using MeetingsLime.Infrastructure;

namespace MeetingsLime.Domain.Services
{
    public sealed class MeetingsService : IMeetingsService
    {
        private readonly MeetingData meetingData;

        public MeetingsService()
        {
            meetingData = MeetingData.Instance;
        }

        //TODO: Zmienić user na employee
        public IReadOnlyList<MeetingSlot> GetMeetingsByUserId(string userId)
        {
            var user = meetingData.Data.UserTimeSlots.Keys.First(x => x.Id == userId);

            if (meetingData.Data.UserTimeSlots.TryGetValue(user, out var slots))
            {
                return slots;
            }
            else
            {
                return new List<MeetingSlot>();
            }

        }

        public IReadOnlyList<MeetingResponse> GetMeetingSuggestions(MeetingRequest request)
        {
            //check by the employeeId
            var meetingSuggestions = new List<MeetingResponse>();
            var users = meetingData.Data.UserTimeSlots.Keys.Where(x => request.EmployeeIds.Contains(x.Id));
            if (!users.Any())
            {
                //thing about returning here the Enumerable.Empty<>
                return new List<MeetingResponse>();
            }

            //filter by
            foreach (var user in users)
            {
                meetingData.Data.UserTimeSlots.TryGetValue(user, out var slots);
                //mam listę godzin w których jest zajęty
                var availableSlots = GetAvailableMeetingsSlots(request);
                meetingSuggestions.Add(new MeetingResponse(user, availableSlots));
            }

            return meetingSuggestions;
        }

        private IList<MeetingSlot> GetAvailableMeetingsSlots(MeetingRequest request)
        {
            var meetingLengthStep = CalculateMeetingLengthStep(request.MeetingLengthMinutes);
            var mettingSlotsFromRequest = CalculateAvailableSlots(request, meetingLengthStep.Item1, meetingLengthStep.Item2);
            return mettingSlotsFromRequest;
        }

        //Można to zamienić na calculateStep
        private (int, int) CalculateMeetingLengthStep(int meetingLengthMinutes)
        {
            const int DefaultMeetingLengthInMinutes = 30;

            // Round up to the nearest 30 minutes
            int roundedMinutes = ((meetingLengthMinutes + DefaultMeetingLengthInMinutes - 1) / DefaultMeetingLengthInMinutes) * DefaultMeetingLengthInMinutes;

            // Calculate hours and minutes
            int hours = roundedMinutes / 60;
            int minutes = roundedMinutes % 60;

            return (hours, minutes);
        }


        private IList<MeetingSlot> CalculateAvailableSlots(MeetingRequest request, int hoursStep, int minutesStep)
        {
            var availableSlots = new List<MeetingSlot>();
            DateTime startDateTime;
            if (request.EarliestRequested.Hour < request.OfficeStartHour)
            {
                startDateTime = new DateTime(
                    request.EarliestRequested.Year,
                    request.EarliestRequested.Month,
                    request.EarliestRequested.Day,
                    request.OfficeStartHour, 0, 0);
            }
            else
            {
                startDateTime = request.EarliestRequested;
            }


            var days = (request.LatestRequested - startDateTime).Days;

            for (int i = 0; i <= days; i++)
            {
                DateTime currentDayStart = startDateTime.AddDays(i).Date.AddHours(request.OfficeStartHour);
                DateTime currentDayEnd = startDateTime.AddDays(i).Date.AddHours(request.OfficeEndHour);
                DateTime currentSlotStart = currentDayStart;

                while (currentSlotStart.AddMinutes(request.MeetingLengthMinutes) <= currentDayEnd && currentSlotStart.AddMinutes(request.MeetingLengthMinutes) <= request.LatestRequested)
                {
                    DateTime endSlotDateTime = currentSlotStart.AddMinutes(request.MeetingLengthMinutes);
                    if (endSlotDateTime > request.LatestRequested)
                        break;
                    var slot = new MeetingSlot(currentSlotStart, endSlotDateTime);
                    availableSlots.Add(slot); currentSlotStart = currentSlotStart.AddMinutes(request.MeetingLengthMinutes);
                }
            }
            return availableSlots;
        }
    }
}
