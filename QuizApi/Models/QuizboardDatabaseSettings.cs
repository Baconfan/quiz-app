namespace QuizApi.Models;

public class QuizboardsDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    
    public string DatabaseName { get; set; } = null!;
    
    public string QuizboardsCollectionName { get; set; } = null!;
}