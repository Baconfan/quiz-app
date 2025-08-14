using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using QuizApi.Helpers;
using QuizApi.Interfaces;

namespace QuizApi.Services;

public class ImageService: IImageService
{
    private readonly Cloudinary _cloudinary;
    
    public ImageService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
        
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<ImageUploadResult> UploadImageAsync(Stream fileStream, string fileName)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(fileName, fileStream),
            Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
            Folder = "quiz-img"
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        
        return await _cloudinary.DestroyAsync(deleteParams);
    }

    // FOR DEBUGGING
    public void PrintSettings()
    {
        Console.WriteLine(_cloudinary.GetConfig().CloudName);
    }
    
}