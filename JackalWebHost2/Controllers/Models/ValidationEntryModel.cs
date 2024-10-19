namespace JackalWebHost2.Controllers.Models;

public class ValidationEntryModel
{
    public string? Property { get; set; }

    public string[] Errors { get; set; } = [];
}