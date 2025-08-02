namespace QuizApi.Models;

public class QuizboardDto
{
    public string? Id { get; set; }
    
    public string? QuizboardTitle { get; set; }
    
    public string? QuizboardDescription { get; set; }
    
    public List<int>? EditorIds { get; set; }
    
    /// <summary>
    /// All possible categories 
    /// </summary>
    public string[]? Categories { get; set; }
    
    /// <summary>
    /// All possible values a question card can have in ascending order
    /// </summary>
    public int[]? ValuesAscending { get; set; }
    
    /// <summary>
    /// A gamecard contains a question with all the related data (value, answers, media, etc.) for the game.
    /// </summary>
    
    public List<GamecardDto>? Gamecards { get; set; }
}

