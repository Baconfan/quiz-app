using QuizApi.Interfaces;
using QuizApi.Models;

namespace QuizApi.Facades;

public class ImageUploadFacade(IImageService imageService): IImageUploadFacade
{
    public async Task<QuizImageDto?> UploadAndPersistImage(ImageUploadForFacade imageUpload)
    {
        // Upload into cloud
        var result = await imageService.UploadImageAsync(imageUpload.ToBeUploadedFile, imageUpload.FileName);
        if (result.Error != null)
        {
            return null;
        }

        var image = new QuizImageDto()
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };
        
        return image;
        
        // TODO Persist metadata in database
    }

    public async Task DeleteAndPersistImage(ImageDeletionForFacade imageDeletion)
    {
        throw new NotImplementedException();
    }
}