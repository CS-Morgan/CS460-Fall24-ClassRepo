using System.ComponentModel.DataAnnotations;
//namespace Sample2.Models

//View model to take in integers from the user.
//Numbers 1-10 are accepted,
public class SongViewModel
{
    [Required]
    [Range(1, 10, ErrorMessage = "Please enter a number between 1 and 10.")]
    public int NumberOfSongs { get; set; }

    public List<string> SelectedSongs { get; set; }
}
