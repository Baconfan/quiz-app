namespace QuestionApi.Models;

public class QuestionCatalogueDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    
    public string DatabaseName { get; set; } = null!;
    
    public string QuestionsCollectionName { get; set; } = null!;
}