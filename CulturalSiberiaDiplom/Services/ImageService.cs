using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using Microsoft.Win32;

namespace CulturalSiberiaDiplom.Services;

public static class ImageService
{
    public static byte[]? ChooseImage(string title = "Выберите изображение")
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = title,
            Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*",
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                return File.ReadAllBytes(openFileDialog.FileName); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка чтения файла" + ex.Message);
                MessageService.ShowError("Не удалось прочитать файл");
                return null;
            }
        }

        return null;
    }

    public static bool IsImageFile(string filePath)
    {
        var ext = Path.GetExtension(filePath)?.ToLower();
        return ext is ".png" or ".jpg" or ".jpeg";
    }
    
    public static BitmapImage SetImage(byte[] imageBytes)
    {
        using var stream = new MemoryStream(imageBytes);
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = stream;
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        bitmap.Freeze();

        return bitmap;
    }

    public static BitmapSource GetImageById(int imageMediaId, CulturalSiberiaContext context)
    {
        var mediaFile = context.Mediafiles.FirstOrDefault(mf => mf.Id == imageMediaId);

        if (mediaFile != null)
        {
            return SetImage(mediaFile.FileData);
        }
        
        return new BitmapImage(new Uri("pack://application:,,,/Resources/Images/default_image_for_details.png"));
    }
}