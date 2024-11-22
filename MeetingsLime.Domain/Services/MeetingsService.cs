using MeetingsLime.Infrastructure;

namespace MeetingsLime.Domain.Services
{
    public sealed class MeetingsService : IMeetingsService
    {
        private readonly IMeetingData _data;
        public MeetingsService(IMeetingData data)
        {
            _data = data;
        }

        public IReadOnlyList<MeetingResponse> GetMeetingSuggestions(MeetingRequest request)
        {
            var meetingData = _data.CalculateMeetingData();
            var meetingSuggestions = new List<MeetingResponse>();
            var users = meetingData.UserTimeSlots.Keys.Where(x => request.EmployeeIds.Contains(x.Id));

            if (!users.Any())
            {
                return new List<MeetingResponse>();
            }

            foreach (var user in users)
            {
                meetingData.UserTimeSlots.TryGetValue(user, out var busySlots);

                var availableSlotsFromRequest = GetAvailableMeetingsSlotsFromRequest(request);
                if (busySlots is null || busySlots.Count == 0)
                {
                    meetingSuggestions.Add(new MeetingResponse(user, availableSlotsFromRequest));
                }
                else
                {
                    var finalAvailableSlots = GetAvailableSlots(availableSlotsFromRequest, busySlots, request.MeetingLengthMinutes);
                    meetingSuggestions.Add(new MeetingResponse(user, finalAvailableSlots));
                }                
            }

            return meetingSuggestions;
        }

        private IList<MeetingSlot> GetAvailableMeetingsSlotsFromRequest(MeetingRequest request)
        {
            var meetingLengthStep = CalculateMeetingLengthStep(request.MeetingLengthMinutes);
            var mettingSlotsFromRequest = CalculateAvailableSlots(request, meetingLengthStep.Item1, meetingLengthStep.Item2);
            return mettingSlotsFromRequest;
        }

        private IList<MeetingSlot> GetAvailableSlots(IList<MeetingSlot> availableSlots, IList<MeetingSlot> busySlots, int meetingMinutesLength)
        {
            var result = new List<MeetingSlot>();
            var busySlotsForGivenDay = busySlots
                    .Where(x => x.StartDateTime.Month == availableSlots.First().StartDateTime.Month && x.StartDateTime.Day == availableSlots.First().StartDateTime.Day)
                    .OrderBy(bs => bs.StartDateTime);
            foreach (var availableSlot in availableSlots)
            {
                var currentStart = availableSlot.StartDateTime;
                
                foreach (var busySlot in busySlotsForGivenDay)
                {
                    if (busySlot.EndDateTime <= currentStart || busySlot.StartDateTime >= availableSlot.EndDateTime)
                    {
                        // Skip busy slots that don't overlap with the current available slot
                        continue;
                    }

                    if (currentStart < busySlot.StartDateTime)
                    {
                        // Add the free time before the busy slot starts, if it matches the desired duration
                        var potentialSlot = new MeetingSlot(currentStart, busySlot.StartDateTime);
                        if ((potentialSlot.EndDateTime - potentialSlot.StartDateTime).TotalMinutes == meetingMinutesLength)
                        {
                            result.Add(potentialSlot);
                        }
                    }

                    // Adjust the current start to the end of the busy slot
                    if (busySlot.EndDateTime > currentStart)
                    {
                        currentStart = busySlot.EndDateTime;
                    }
                }

                if (currentStart < availableSlot.EndDateTime)
                {
                    // Add the remaining free time, if it matches the desired duration
                    var potentialSlot = new MeetingSlot(currentStart, availableSlot.EndDateTime);
                    if ((potentialSlot.EndDateTime - potentialSlot.StartDateTime).TotalMinutes == meetingMinutesLength)
                    {
                        result.Add(potentialSlot);
                    }
                }
            }

            return result;
        }

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
