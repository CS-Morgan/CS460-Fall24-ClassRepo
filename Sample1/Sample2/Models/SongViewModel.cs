using System.ComponentModel.DataAnnotations;
namespace Sample2.Models;

//View model to take in integers from the user.
//Numbers 2-10 are accepted
//Names with the correct format are also accepted
//Through regular expression validation
public class SongViewModel
{
    [Required]
    [Range(2, 10, ErrorMessage = "Please enter two to ten teams you would like to see:")]
    public int NumberOfTeams { get; set; }

    [Required(ErrorMessage = "Please enter at least one name.")]
    public List<string> UserNames { get; set; } = new List<string>();

    public List<string> SelectedSongs { get; set; } = new List<string>();

    public Dictionary<string, List<string>> Teams { get; set; } = new Dictionary<string, List<string>>();

    public SongViewModel()
    {
        UserNames = new List<string>();
        SelectedSongs = new List<string>();
        Teams = new Dictionary<string, List<string>>();
    }
}