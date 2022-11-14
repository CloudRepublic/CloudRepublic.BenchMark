using CloudRepublic.BenchMark.Application.Models;
using Shouldly;
using Xunit;

namespace CloudRepublic.BenchMark.Application.Tests
{
    public class EnumFromStringTests
    {
        private enum TestEnum
        {
            Value_one = 0,
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
            //  Arrange



            //  Act
            var enumToTest = new EnumFromString<TestEnum>(invalidInput);



            //  Assert

            enumToTest.ParsedSuccesfull.ShouldBe(false);


        }

        [Theory]
        [InlineData("0")] // number in range succeeds
        [InlineData("1")] // number in range succeeds
        [InlineData("Value_one")] // name in enum succeeds
        public void EnumFromString_Should_Parse_ValidStrings_Succesfull(string validInput)
        {
            // Arrange


            // Act

            var enumToTest = new EnumFromString<TestEnum>(validInput);

            // Assert

            enumToTest.ParsedSuccesfull.ShouldBe(true);

        }
    }
}
