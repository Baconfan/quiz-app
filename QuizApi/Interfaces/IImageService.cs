using CloudinaryDotNet.Actions;
using MongoDB.Driver;

namespace QuizApi.Interfaces;

public interface IImageService
{
    Task<ImageUploadResult> UploadImageAsync(Stream fileStream, string fileName);
    Task<DeletionResult> DeletePhotoAsync(string publicId);

    void PrintSettings();
}