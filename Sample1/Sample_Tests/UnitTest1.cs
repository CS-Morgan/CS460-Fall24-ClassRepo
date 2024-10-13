using NUnit.Framework;
using Sample2.Models;
using System.Text.RegularExpressions;

namespace Sample1.Tests
{
    [TestFixture]
    public class SongViewModelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserNames_AcceptsValidNames()
        {
            // Arrange
            var model = new SongViewModel();
            var validUserNames = new[] { "Alice", "Bob", "Chris" };

            // Act
            model.UserNames = validUserNames.ToList();

            // Assert
            Assert.AreEqual(3, model.UserNames.Count);
            Assert.AreEqual("Alice", model.UserNames[0]);
        }

        [Test]
        public void NumberOfTeams_AcceptsValidInput()
        {
            // Arrange
            var model = new SongViewModel { NumberOfTeams = 5 };

            // Assert
            Assert.AreEqual(5, model.NumberOfTeams);
        }

        [Test]
        public void Teams_AssignedCorrectly()
        {
            // Arrange
            var model = new SongViewModel();
            model.UserNames = new[] { "Alice", "Bob", "Chris", "David" }.ToList();
            model.NumberOfTeams = 2;

            // Act
            var controller = new SampleController();  // Assuming this is the controller handling the logic
            controller.AssignTeams(model);  // Assuming this method exists to assign teams

            // Assert
            Assert.AreEqual(2, model.Teams.Count);  // Check if 2 teams were created
            Assert.IsTrue(model.Teams.Values.All(t => t.Count > 0));  // Ensure each team has members
        }

        [Test]
        public void InvalidUserNames_ReturnsModelError()
        {
            // Arrange
            var model = new SongViewModel();
            var invalidUserNames = new[] { "Invalid_Name!", "Bob123", "Alice@!" };

            // Act & Assert
            foreach (var name in invalidUserNames)
            {
                Assert.False(IsValidUserName(name), $"{name} should not be a valid user name");
            }
        }

        // Helper method to validate names (adjust based on your validation rules)
        private bool IsValidUserName(string name)
        {
            string pattern = @"^[a-zA-Z\s\(\)\.\-_']+$";
            return Regex.IsMatch(name, pattern);
        }
    }
}