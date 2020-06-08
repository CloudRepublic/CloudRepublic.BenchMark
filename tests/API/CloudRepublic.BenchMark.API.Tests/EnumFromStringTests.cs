using CloudRepublic.BenchMark.API.Helpers;
using Xunit;

namespace CloudRepublic.BenchMark.API.Tests
{
    public class EnumFromStringTests
    {
        private enum TestEnum
        {
            value_one = 0,
            Value_two = 1,
        }


        [Theory]
        [InlineData(null)] // null fails 
        [InlineData("")] // Empty fails
        [InlineData(" ")] // only whitespace fails
        [InlineData("-1")] // negative number (not present) fails
        [InlineData("6")] // outside of range fails
        [InlineData("VALUE NOT IN ENUM")] // name not in enum fails
        [InlineData("VALUE_ONE")] // cased up name in enum fails
        [InlineData("value_two")] // cased down name in enum fails
        public void EnumFromString_Should_Not_Parse_InvalidStrings_Succesfull(string invalidInput)
        {
            #region Arrange

            #endregion

            #region Act
            var enumToTest = new EnumFromString<TestEnum>(invalidInput);

            #endregion

            #region Assert

            Assert.False(enumToTest.ParsedSuccesfull);

            #endregion
        }

        [Theory]
        [InlineData("0")] // number in range succeeds
        [InlineData("1")] // number in range succeeds
        [InlineData("value_one")] // name in enum succeeds
        public void EnumFromString_Should_Parse_ValidStrings_Succesfull(string validInput)
        {
            #region Arrange

            #endregion

            #region Act

            var enumToTest = new EnumFromString<TestEnum>(validInput);

            #endregion

            #region Assert

            Assert.True(enumToTest.ParsedSuccesfull);

            #endregion
        }
    }
}
