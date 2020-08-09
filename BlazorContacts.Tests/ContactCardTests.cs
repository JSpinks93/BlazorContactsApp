using BlazorContacts.Components;
using Bunit;
using Bunit.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace BlazorContacts.Tests
{
    public class ContactCardTests : TestContext
    {
        public ContactCardTests()
        {
            Services.AddSingleton(typeof(NavigationManager), typeof(MockNavigationManager));
        }

        [Fact]
        public void Displays_FirstName_LastName()
        {
            // Arrange
            var testContact = new Contact()
            {
                FirstName = "Test",
                LastName = "Contact"
            };

            // Act
            var testComponent = RenderComponent<ContactCard>(("Contact", testContact));

            var nameHeader = testComponent.Find("h5");

            // Assert
            Assert.Equal("Test Contact", nameHeader.TextContent);
        }
    }

    public class MockNavigationManager : NavigationManager, IDisposable
    {
        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }        
    }
}
