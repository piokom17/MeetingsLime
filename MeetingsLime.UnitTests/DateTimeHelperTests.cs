using FluentAssertions;
using MeetingsLime.Infrastructure.Helpers;

public class DateTimeHelperTests
{
    public class ParseToDateTimeTests
    {
        [Theory]
        [InlineData("11/22/2024 5:00:00 PM", 2024, 11, 22, 17, 0, 0)] // Valid date with AM/PM format
        [InlineData("12/31/2023 8:30:00 AM", 2023, 12, 31, 8, 30, 0)] // Valid date with AM
        [InlineData("1/1/2025 12:00:00 PM", 2025, 1, 1, 12, 0, 0)] // Valid date with PM
        public void ParseToDateTime_ShouldParseValidDate(string input, int year, int month, int day, int hour, int minute, int second)
        {
            // Act
            DateTime result = DateTimeHelper.ParseToDateTime(input);

            // Assert
            result.Should().Be(new DateTime(year, month, day, hour, minute, second));
        }

        [Theory]
        [InlineData("invalid date")]
        [InlineData("32/13/2024 5:00:00 PM")]
        [InlineData("2024-02-30 9:00:00 AM")]
        public void ParseToDateTime_ShouldReturnDateTimeMinValueForInvalidDate(string input)
        {
            // Act
            DateTime result = DateTimeHelper.ParseToDateTime(input);

            // Assert
            result.Should().Be(DateTime.MinValue);
        }
    }

    // Test for IsValidDateTimeSlot method
    public class IsValidDateTimeSlotTests
    {
        [Theory]
        [InlineData("2024-11-22 5:00:00 PM", "2024-11-22 6:00:00 PM", true)] // Valid start and end
        [InlineData("2024-11-22 5:00:00 PM", "2024-11-22 4:00:00 PM", true)] // Valid start and end (order doesn't matter)
        [InlineData("2024-11-22 5:00:00 PM", "2024-11-22 5:00:00 PM", true)] // Same start and end
        public void IsValidDateTimeSlot_ShouldReturnTrueForValidDateTimes(string start, string end, bool expected)
        {
            // Arrange
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);

            // Act
            bool result = DateTimeHelper.IsValidDateTimeSlot(startDate, endDate);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("2024-11-22 5:00:00 PM", "0001-01-01 00:00:00", false)] // Invalid end (DateTime.MinValue)
        [InlineData("0001-01-01 00:00:00", "2024-11-22 5:00:00 PM", false)] // Invalid start (DateTime.MinValue)
        [InlineData("0001-01-01 00:00:00", "0001-01-01 00:00:00", false)] // Both are invalid
        public void IsValidDateTimeSlot_ShouldReturnFalseForInvalidDateTimes(string start, string end, bool expected)
        {
            // Arrange
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = DateTime.Parse(end);

            // Act
            bool result = DateTimeHelper.IsValidDateTimeSlot(startDate, endDate);

            // Assert
            result.Should().Be(expected);
        }
    }
}