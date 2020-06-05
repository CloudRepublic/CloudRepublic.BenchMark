using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using System.Linq;
using Xunit;


namespace CloudRepublic.BenchMark.Orchestrator.Tests
{
    public class BenchMarkTypeGeneratorTests
    {
        [Fact]
        public void GetAllTypes_Should_Return_BenchMarkType_With_All_Data()
        {
            #region Arrange

            #endregion

            #region Act

            var benchMarkTypes = BenchMarkTypeGenerator.GetAllTypes();

            #endregion

            #region Assert

            Assert.NotNull(benchMarkTypes);

            // we hard coded verify only the first and last(because its so different) entries for now
            var firstTypeForTesting = benchMarkTypes.First();
            Assert.Equal(CloudProvider.Azure, firstTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Windows, firstTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.Csharp, firstTypeForTesting.Runtime);
            Assert.Equal("AzureWindowsCsharp", firstTypeForTesting.Name);
            Assert.Equal("AzureWindowsCsharpClient", firstTypeForTesting.ClientName);
            Assert.Equal("AzureWindowsCsharpKey", firstTypeForTesting.KeyName);
            Assert.Equal("AzureWindowsCsharpUrl", firstTypeForTesting.UrlName);
            Assert.True(firstTypeForTesting.SetXFunctionsKey);


            var lastTypeForTesting = benchMarkTypes.Last();
            Assert.Equal(CloudProvider.Firebase, lastTypeForTesting.CloudProvider);
            Assert.Equal(HostEnvironment.Linux, lastTypeForTesting.HostEnvironment);
            Assert.Equal(Runtime.Nodejs, lastTypeForTesting.Runtime);
            Assert.Equal("FirebaseLinuxNodejs", lastTypeForTesting.Name);
            Assert.Equal("FirebaseLinuxNodejsClient", lastTypeForTesting.ClientName);
            Assert.Equal("FirebaseLinuxNodejsKey", lastTypeForTesting.KeyName);
            Assert.Equal("FirebaseLinuxNodejsUrl", lastTypeForTesting.UrlName);
            Assert.False(lastTypeForTesting.SetXFunctionsKey);

            #endregion
        }
        [Fact]
        public void GetAllTypes_Should_Return_BenchMarkTypes()
        {
            #region Arrange

            #endregion

            #region Act

            var benchMarkTypes = BenchMarkTypeGenerator.GetAllTypes();

            #endregion

            #region Assert

            Assert.NotNull(benchMarkTypes);
            Assert.NotEmpty(benchMarkTypes);

            #endregion
        }
    }
}
