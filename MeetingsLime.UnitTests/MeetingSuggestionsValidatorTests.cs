namespace MeetingsLime.UnitTests
{
    using MeetingsLime.Validators;
    using System;
    using System.IO;
    using Xunit;

    public class MeetingSuggestionsValidatorTests
    {
        private readonly MeetingSuggestionsValidator _validator;

        public MeetingSuggestionsValidatorTests()
        {
            _validator = new MeetingSuggestionsValidator();
        }

        [Theory]
        [InlineData(0)] // Less than minimum
        [InlineData(181)] // Greater than maximum
        public void Validate_ThrowsExceptionForInvalidMeetingLength(int invalidLength)
        {
            // Arrange
            DateTime earliestRequested = new DateTime(2024, 11, 20, 9, 0, 0);
            DateTime latestRequested = new DateTime(2024, 11, 20, 17, 0, 0);
            int officeStartHour = 9;
            int officeEndHour = 17;

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                _validator.Validate(invalidLength, earliestRequested, latestRequested, officeStartHour, officeEndHour));
        }

        [Theory]
        [InlineData(5)] // Too early
        [InlineData(19)] // Too late
        public void Validate_ThrowsExceptionForInvalidOfficeHours(int invalidHour)
        {
            // Arrange
            DateTime earliestRequested = new DateTime(2024, 11, 20, 9, 0, 0);
            DateTime latestRequested = new DateTime(2024, 11, 20, 17, 0, 0);

            int validStartHour = invalidHour == 5 ? invalidHour : 9;
            int validEndHour = invalidHour == 19 ? invalidHour : 17;

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                _validator.Validate(60, earliestRequested, latestRequested, validStartHour, validEndHour));
        }

        [Fact]
        public void Validate_ThrowsExceptionIfEarliestRequestedHourIsBeforeOfficeStart()
        {
            // Arrange
            DateTime earliestRequested = new DateTime(2024, 11, 20, 7, 0, 0); // 7:00 AM
            DateTime latestRequested = new DateTime(2024, 11, 20, 17, 0, 0); // 5:00 PM
            int officeStartHour = 9; // 9:00 AM
            int officeEndHour = 17; // 5:00 PM

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                _validator.Validate(60, earliestRequested, latestRequested, officeStartHour, officeEndHour));
        }

        [Fact]
        public void Validate_ThrowsExceptionIfLatestRequestedHourIsAfterOfficeEnd()
        {
            // Arrange
            DateTime earliestRequested = new DateTime(2024, 11, 20, 9, 0, 0); // 9:00 AM
            DateTime latestRequested = new DateTime(2024, 11, 20, 19, 0, 0); // 7:00 PM
            int officeStartHour = 9; // 9:00 AM
            int officeEndHour = 17; // 5:00 PM

            // Act & Assert
            Assert.Throws<InvalidDataException>(() =>
                _validator.Validate(60, earliestRequested, latestRequested, officeStartHour, officeEndHour));
        }

        [Fact]
        public void Validate_DoesNotThrowForValidMeetingRequest()
        {
            // Arrange
            DateTime earliestRequested = new DateTime(2024, 11, 20, 9, 0, 0); // 9:00 AM
            DateTime latestRequested = new DateTime(2024, 11, 20, 17, 0, 0); // 5:00 PM
            int officeStartHour = 9; // 9:00 AM
            int officeEndHour = 17; // 5:00 PM

            // Act & Assert

            Action act = () => _validator.Validate(60, earliestRequested, latestRequested, officeStartHour, officeEndHour);
        }
    }

}
