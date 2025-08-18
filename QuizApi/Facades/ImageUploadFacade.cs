using QuizApi.Dtos.Output;
using QuizApi.Interfaces;
using QuizApi.Models;
using QuizApi.Persistence;

namespace QuizApi.Facades;

public class ImageUploadFacade(
    IImageService imageService, 
    IQuizboardRepository quizboardRepository): IImageUploadFacade
{
    public async Task<QuizImageDto?> UploadAndPersistImage(ImageUploadForFacade imageUpload)
    {
        // Upload into cloud
        var result = await imageService.UploadImageAsync(imageUpload.ToBeUploadedFile, imageUpload.FileName);
        if (result.Error != null)
        {
            return null;
        }

        var image = new QuizImageDto
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        
        // Save as a quizimage in database
        var quizImage = new AddQuizImageToQuizcardModel
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            QuizcardId = imageUpload.QuizcardId,
            Assignment = imageUpload.ImageAssigment,
            Position = imageUpload.Position
        };

        await quizboardRepository.AddImageToQuizcard(quizImage);
        
        return image;
    }

    public async Task DeleteAndPersistImage(ImageDeletionForFacade imageDeletion)
    {
        throw new NotImplementedException();
    }
}