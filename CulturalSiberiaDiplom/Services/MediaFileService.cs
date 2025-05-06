using System;
using System.Threading.Tasks;
using CulturalSiberiaDiplom.Models;

namespace CulturalSiberiaDiplom.Services;

public static class MediaFileService
{
    public static async Task<int?> SaveMediaFileAsync(string entityTable, int entityId, string fileName,
        string contentType, byte[] fileData, DateTime createdAt, CulturalSiberiaContext context)
    {
        try
        {
            var mediaFile = new Mediafile
            {
                EntityTable = entityTable,
                EntityId = entityId,
                FileName = fileName,
                ContentType = contentType,
                FileData = fileData,
                CreatedAt = createdAt
            };

            context.Mediafiles.Add(mediaFile);
            await context.SaveChangesAsync();
            
            return mediaFile.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка сохранения медиафайла: " + ex.Message);
            MessageService.ShowError("Ошибка сохранения медиафайла");
        }

        return null;
    }
}