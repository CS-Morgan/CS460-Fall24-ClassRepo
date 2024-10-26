
namespace TeamGenerator.Models;

public class Teams
{
    public IEnumerable<string[]> TeamAssignments { get; private set; } = new List<string[]>();
    // an empty list indicates no assignments have been made

    public int TeamCount => TeamAssignments.Count();
    public int TeamSize {get; private set; } = 0;

    public Teams(string names, int teamSize)
    {
        TeamSize = teamSize;
        List<string> namesList = SplitStringByNewline(names);
        List<string[]> teams = MakeTeams(namesList, teamSize, new Random());
        TeamAssignments = teams;
    }
    
    public static List<string[]> MakeTeams(List<string> namesList, int teamSize, Random random)
    {
        var teams = new List<string[]>();
        var team = new List<string>();
        while (namesList.Count > 0)
        {
            var index = random.Next(namesList.Count);
            team.Add(namesList[index]);
            namesList.RemoveAt(index);
            if (team.Count == teamSize)
            {
                teams.Add(team.ToArray());
                team.Clear();
            }
        }
        if (team.Count > 0)
        {
            teams.Add(team.ToArray());
        }
        return teams;
    }

    // split strings by newline excluding empty entries and trim whitespace
    public static List<string> SplitStringByNewline(string str)
    {
        if (str == null)
        {
            return new List<string>();
        }
        var namesList = str.Split(new string[] { "\n", "\r\n" },StringSplitOptions.RemoveEmptyEntries).ToList();
        namesList = namesList.Select(name => name.Trim()).ToList();
        return namesList;
    }

}