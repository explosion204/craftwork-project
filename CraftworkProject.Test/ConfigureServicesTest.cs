using System.IO;
using CraftworkProject.Web.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CraftworkProject.Test
{
    public class ConfigureServicesTest
    {
        [Fact]
        public void ConfigureAppServicesTest()
        {
            var services = new ServiceCollection();
            Assert.True(services.Count == 0);
            
            var environmentMock = new Mock<IWebHostEnvironment>();
            environmentMock.SetupGet(x => x.WebRootPath)
                .Returns(Directory.GetCurrentDirectory());
            
            ConfigureAppServices.Configure(services, environmentMock.Object);
            Assert.True(services.Count > 0);
        }

        [Fact]
        public void ConfigureAuthServicesTest()
        {
            var services = new ServiceCollection();
            Assert.True(services.Count == 0);

            ConfigureAuthServices.Configure(services);
            Assert.True(services.Count > 0);
        }
    }
}