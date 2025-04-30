using System;
using System.IO;
using System.Net;
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
}