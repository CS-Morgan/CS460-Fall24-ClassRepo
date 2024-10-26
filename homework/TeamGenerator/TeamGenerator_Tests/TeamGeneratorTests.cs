using TeamGenerator.Models;

namespace TeamGenerator_Tests;

public class TeamGeneratorTests
{
    [SetUp]
    public void Setup()
    {
    }

    //public List<string> SplitStringByNewline(string str)

    [Test]
    public void TestGenerator_EmptyStringOfNames_ReturnsEmptyList()
    {
        // Arrange
        var names = "";
        var expected = new List<string>();
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_NullNameString_ReturnsEmptyList()
    {
        // Arrange
        string names = null;
        var expected = new List<string>();
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_SingleName_ReturnsListWithOneName()
    {
        // Arrange
        var names = "John";
        var expected = new List<string>() { "John" };
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_TwoNames_ReturnsListWithTwoNames()
    {
        // Arrange
        var names = "John\nJane";
        var expected = new List<string>() { "John", "Jane" };
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_TwoNamesWithWindowsNewline_ReturnsListWithTwoNames()
    {
        // Arrange
        var names = "John\r\nJane";
        var expected = new List<string>() { "John", "Jane" };
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_NameStringWithExtraSpaces_ReturnsListWithOnlyNames()
    {
        // Arrange
        var names = " John \nJane  \n   Claudia\n   Juanita";
        var expected = new List<string>() { "John", "Jane", "Claudia", "Juanita" };
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_NameStringWithExtraEmptyLines_DoesNotReturnEmptyNames()
    {
        // Arrange
        var names = "John\n\nJane\n\nClaudia\n\nJuanita";
        var expected = new List<string>() { "John", "Jane", "Claudia", "Juanita" };
        // Act
        var actual = Teams.SplitStringByNewline(names);
        // Assert
        Assert.That(expected, Is.EqualTo(actual));
    }

    [Test]
    public void TestGenerator_MakeTeams_MultipleNames_ReturnsListOfTeams()
    {
        // Arrange
        List<string> names = new List<string>() { "John", "Jane", "Claudia", "Juanita" };
        // We cannot test the random number generator, so we will test that the number of teams is correct
        //var expected = new List<string[]>() { new string[] { "John", "Jane" }, new string[] { "Claudia", "Juanita" } };
        int expectedNumberOfTeams = 2;
        int expectedTeamSize = 2;
        var randomGenerator = new Random(42);
        // Act
        var actual = Teams.MakeTeams(names, expectedTeamSize, randomGenerator);
        // Assert
        Assert.Multiple( () =>
        {
            Assert.That(actual.Count, Is.EqualTo(expectedNumberOfTeams));
            Assert.That(actual[0].Length, Is.EqualTo(expectedTeamSize));
            Assert.That(actual[1].Length, Is.EqualTo(expectedTeamSize));
        });
    }
}