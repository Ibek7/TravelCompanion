using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.Tests
{
    /// <summary>
    /// Goes through the SDK to test the AppUserClient. The SDK will communicate through the
    /// web API with connection string defined in Configuration["TravelCompanionBaseAddress"].
    /// </summary>
    [TestClass]
    public class AppUserClientSdkTests : BaseTests
    {
        [TestMethod]
        public void TestGetUser()
        {
            var appUserClient = ServiceProvider.GetRequiredService<AppUserClient>();
            var user = appUserClient.GetUser().Result;
            Assert.IsNotNull(user); 
            Console.WriteLine(user.ConvertToJson());
        }
    }
}
