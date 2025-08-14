using QuizApi.Models;

namespace QuizApi.Facades;

public interface IImageUploadFacade
{
    Task<QuizImageDto?> UploadAndPersistImage(ImageUploadForFacade imageUpload);
    
    Task DeleteAndPersistImage(ImageDeletionForFacade imageDeletion);
}