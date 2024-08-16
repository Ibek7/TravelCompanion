using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.Tests
{
    [TestClass]
    public class AppUserServiceTests : BaseTests
    {
        [TestMethod]
        public void TestGetByEmailAsync()
        {
            var appUserService = ServiceProvider.GetRequiredService<AppUserService>();
            var email = "max.szczurek@gmail.com";
            var user = appUserService.GetByEmailAsync(email).Result;
            Assert.IsNotNull(user);
            Console.WriteLine(user.ConvertToJson());
        }
    }
}
