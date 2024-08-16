using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Models;

namespace TravelCompanion.Tests
{
    [TestClass]
    public class RepositoryTests : BaseTests
    {
        [TestMethod]
        public void TestGetAppUsers()
        {
            var context = ServiceProvider.GetRequiredService<TravelCompanionContext>();
            var users = context.AppUsers.ToList();
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
            Console.WriteLine(users.ConvertToJson());
        }
    }
}
