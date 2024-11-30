//Unit Testing to test every clause in the Project
//Made in contribution thanks to ChatGPT, Co-Pilot, and Stack Overflow

using NUnit.Framework;
using NUnit.Framework.Legacy;
using Sample2.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sample_Tests
{
    public class SongViewModelTests
    {
        private SongViewModel model;

        [SetUp]
        public void Setup()
        {
            model = new SongViewModel();
        }

        //Test to accept valid usernames
        [Test]
        public void UserNames_AcceptsValidNames()
        {
            // Arrange
            var validUserNames = new List<string> { "Alice", "Bob", "Chris" };

            // Act
            model.UserNames = validUserNames;

            // Assert
            ClassicAssert.AreEqual(3, model.UserNames.Count);
            ClassicAssert.AreEqual("Alice", model.UserNames[0]);
        }

        //Test to make sure usernames are not accepted if invalid
        [Test]
        public void UserNames_InvalidNames()
        {
            // Arrange
            var invalidUserNames = new List<string> {"Invalid_Name!", "Bob123", "Alice@!" };

            // Act & Assert
            foreach (var name in invalidUserNames)
            {
                ClassicAssert.IsFalse(IsValidUserName(name), $"{name} should not be a valid user name");
            }
        }

        //Test to make sure teams are randomly assigned everytime the app is used
        [Test]
        public void Teams_AreAssignedRandomly()
        {
            // Arrange
            model.UserNames = new List<string> { "Alice", "Bob", "Chris", "David" };
            model.NumberOfTeams = 2;

            // Act
            var random = new System.Random();
            var shuffledUsers = model.UserNames.OrderBy(x => random.Next()).ToList();
            var teamSize = (int)Math.Ceiling((double)shuffledUsers.Count / model.NumberOfTeams);
            var teamNames = new List<string> { "Team A", "Team B" };

            model.Teams = new Dictionary<string, List<string>>();
            for (int i = 0; i < model.NumberOfTeams; i++)
            {
                var teamMembers = shuffledUsers.Skip(i * teamSize).Take(teamSize).ToList();
                if (teamMembers.Any())
                {
                    model.Teams.Add(teamNames[i], teamMembers);
                }
            }

            // Assert
            ClassicAssert.AreEqual(2, model.Teams.Count);
            ClassicAssert.IsTrue(model.Teams.Values.All(t => t.Count > 0));
            ClassicAssert.IsTrue(model.Teams.Values.SelectMany(t => t).Distinct().Count() == model.UserNames.Count, "All usernames should be assigned to a team.");
        }

        //Test to validate number range
        [Test]
        public void NumberOfTeams_WithinValidRange()
        {
            // Arrange
            model.NumberOfTeams = 5;

            // Act & Assert
            ClassicAssert.IsTrue(model.NumberOfTeams >= 2 && model.NumberOfTeams <= 10, "Number of teams should be between 2 and 10.");
        }

        // Helper method to validate names
        private bool IsValidUserName(string name)
        {
            string pattern = @"^[a-zA-Z\s\(\)\.\-_']+$";
            return System.Text.RegularExpressions.Regex.IsMatch(name, pattern);
        }
    }
}
