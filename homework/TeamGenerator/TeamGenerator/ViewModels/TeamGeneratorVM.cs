using System.ComponentModel.DataAnnotations;

namespace TeamGenerator.ViewModels;

public class TeamGeneratorVM
{
    [Required]
    // Write a Regular Expression to validate the names where the names are words separated by newlines
    [RegularExpression(@"^(\w+\s*\n*)+$", ErrorMessage = "Names must be separated by newlines.")]
    public string Names { get; set; }

    [Required]
    [Range(2, 10, ErrorMessage = "Team size must be a whole number between 2 and 10.")]
    public int TeamSize { get; set; } = 4;
}