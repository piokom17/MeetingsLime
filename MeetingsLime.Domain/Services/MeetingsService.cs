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
            var meetingDataFromFile = _data.CalculateMeetingData();
            var meetingSuggestions = new List<MeetingResponse>();
            var users = meetingDataFromFile.UserTimeSlots.Keys.Where(x => request.EmployeeIds.Contains(x.Id));

            if (!users.Any())
            {
                return new List<MeetingResponse>();
            }

            foreach (var user in users)
            {
                meetingDataFromFile.UserTimeSlots.TryGetValue(user, out var busySlots);

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
            var mettingSlotsFromRequest = CalculateAvailableSlots(request);
            return mettingSlotsFromRequest;
        }

        private static IList<MeetingSlot> GetAvailableSlots(IList<MeetingSlot> availableSlots, IList<MeetingSlot> busySlots, int meetingMinutesLength)
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

        private static IList<MeetingSlot> CalculateAvailableSlots(MeetingRequest request)
        {
            var availableSlots = new List<MeetingSlot>();
            DateTime startDateTime = request.EarliestRequested;

            var days = (request.LatestRequested - startDateTime).Days;
            for (int i = 0; i <= days; i++)
            {
                DateTime currentDayStart = startDateTime.AddDays(i).Date.AddHours(startDateTime.Hour);
                DateTime currentDayEnd = startDateTime.AddDays(i).Date.AddHours(request.LatestRequested.Hour);
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
