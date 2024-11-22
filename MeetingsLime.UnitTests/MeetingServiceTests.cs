using FluentAssertions;
using MeetingsLime.Domain;
using MeetingsLime.Domain.Services;
using MeetingsLime.Infrastructure;
using Moq;

namespace MeetingsLime.UnitTests
{
    public class MeetingServiceTests
    {
        private readonly MeetingsService _sut;
        private readonly Mock<IMeetingData> _meetingDataMock = new Mock<IMeetingData>();

        public MeetingServiceTests()
        {
            _sut = new MeetingsService(_meetingDataMock.Object);
        }

        [Fact]
        public void GetMeetingSuggestions_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            var request = new MeetingRequest
            {
                EmployeeIds = new List<string> { "non-existent-id" },
                MeetingLengthMinutes = 30,
                EarliestRequested = new DateTime(2024, 11, 21, 9, 0, 0),
                LatestRequested = new DateTime(2024, 11, 21, 17, 0, 0),
                OfficeStartHour = 9,
                OfficeEndHour = 17
            };

            _meetingDataMock.Setup(x => x.CalculateMeetingData()).Returns(new Meeting()
            {
                UserTimeSlots = new Dictionary<UserDataModel, List<MeetingSlot>>()
                {
                    {
                        new UserDataModel { Id = "user1", Name = "John Doe" },
                        new List<MeetingSlot>
                        {
                            new MeetingSlot(new DateTime(2024, 11, 21, 9, 0, 0), new DateTime(2024, 11, 21, 10, 0, 0)),
                            new MeetingSlot(new DateTime(2024, 11, 21, 14, 0, 0), new DateTime(2024, 11, 21, 15, 0, 0)),
                        }
                    }
                }
            });

            // Act
            var result = _sut.GetMeetingSuggestions(request);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetMeetingSuggestions_UserWithNoBusySlots_ReturnsAllAvailableSlots()
        {
            // Arrange
            const int expectedAvailableSlotsCount = 16;
            var user = new UserDataModel { Id = "user1" };
            var request = new MeetingRequest
            {
                EmployeeIds = new List<string> { "user1" },
                MeetingLengthMinutes = 30,
                EarliestRequested = new DateTime(2024, 11, 21, 9, 0, 0),
                LatestRequested = new DateTime(2024, 11, 21, 17, 0, 0),
                OfficeStartHour = 9,
                OfficeEndHour = 17
            };

            _meetingDataMock.Setup(x => x.CalculateMeetingData()).Returns(new Meeting()
            {
                UserTimeSlots = new Dictionary<UserDataModel, List<MeetingSlot>>()
                {
                    {
                        new UserDataModel { Id = "user1", Name = "John Doe" },
                        new List<MeetingSlot> { }
                    }
                }
            });

            // Act
            var result = _sut.GetMeetingSuggestions(request);

            // Assert
            result.Should().NotBeEmpty();
            result.First().MeetingSlots.Count().Should().Be(expectedAvailableSlotsCount);
        }

        [Fact]
        public void GetMeetingSuggestions_UserWithBusySlots_FiltersOverlappingSlots()
        {
            // Arrange
            const int expectedAvailableSlotsCount = 10;
            var user = new UserDataModel { Id = "user1" };
            var busySlots = new List<MeetingSlot>
            {
                new MeetingSlot(new DateTime(2024, 11, 21, 10, 0, 0), new DateTime(2024, 11, 21, 11, 0, 0)),
                new MeetingSlot(new DateTime(2024, 11, 21, 13, 0, 0), new DateTime(2024, 11, 21, 14, 0, 0)),
            };
            var request = new MeetingRequest
            {
                EmployeeIds = new List<string> { user.Id },
                MeetingLengthMinutes = 30,
                EarliestRequested = new DateTime(2024, 11, 21, 8, 0, 0),
                LatestRequested = new DateTime(2024, 11, 21, 16, 0, 0),
                OfficeStartHour = 9,
                OfficeEndHour = 17
            };

            _meetingDataMock.Setup(x => x.CalculateMeetingData()).Returns(new Meeting()
            {
                UserTimeSlots = new Dictionary<UserDataModel, List<MeetingSlot>>()
                {
                    {
                        new UserDataModel { Id = "user1", Name = "John Doe" },
                        busySlots
                    }
                }
            });

            // Act
            var result = _sut.GetMeetingSuggestions(request);

            // Assert
            result.Should().NotBeEmpty();
            result.First().MeetingSlots.Count().Should().Be(expectedAvailableSlotsCount);
        }

        [Fact]
        public void GetMeetingSuggestions_MultipleUsers_ReturnsMergedAvailableSlots()
        {
            // Arrange
            const int expectedResultCount = 2;
            var user1 = new UserDataModel { Id = "user1" };
            var user2 = new UserDataModel { Id = "user2" };
            var busySlotsUser1 = new List<MeetingSlot>
            {
                new MeetingSlot(new DateTime(2024, 11, 21, 10, 0, 0), new DateTime(2024, 11, 21, 11, 0, 0))
            };
            var busySlotsUser2 = new List<MeetingSlot>
            {
                new MeetingSlot(new DateTime(2024, 11, 21, 14, 0, 0), new DateTime(2024, 11, 21, 15, 0, 0))
            };
            var request = new MeetingRequest
            {
                EmployeeIds = new List<string> { user1.Id, user2.Id },
                MeetingLengthMinutes = 30,
                EarliestRequested = new DateTime(2024, 11, 21, 8, 0, 0),
                LatestRequested = new DateTime(2024, 11, 21, 17, 0, 0),
                OfficeStartHour = 9,
                OfficeEndHour = 17
            };

            _meetingDataMock.Setup(x => x.CalculateMeetingData()).Returns(new Meeting()
            {
                UserTimeSlots = new Dictionary<UserDataModel, List<MeetingSlot>>()
                {
                    {
                        new UserDataModel { Id = "user1", Name = "John Doe" },
                        busySlotsUser1
                    },
                    {
                        new UserDataModel { Id = "user2", Name = "Kenneth Doe" },
                        busySlotsUser2
                    },
                }
            });

            // Act
            var result = _sut.GetMeetingSuggestions(request);

            // Assert
            result.Should().NotBeEmpty();
            result.Count().Should().Be(expectedResultCount);
        }
    }
}
