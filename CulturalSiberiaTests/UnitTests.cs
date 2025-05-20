using System.Windows;
using System.Windows.Media.Imaging;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;
using Moq;

namespace CulturalSiberiaTests;

[Collection("Wpf Collection")]
public class UnitTests
{
    [Fact]
    public void ImageService_SetImage_ReturnsBitmapSource()
    {
        var imageBytes = File.ReadAllBytes("C:\\Users\\Akame\\RiderProjects\\CulturalSiberiaDiplom" +
                                           "\\CulturalSiberiaTests\\Resources\\Images\\default_image_for_details.png");
        
        var result = ImageService.SetImage(imageBytes);
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<BitmapSource>(result);
    }
    
    [Fact]
    public void BooleanToEditSaveConverter_Convert_ReturnsEditOrSave()
    {
        var converter = new BooleanToEditSaveConverter();
        
        var saveResult = converter.Convert(true, null, null, null);
        var editResult = converter.Convert(false, null, null, null);
        
        Assert.Equal("Редактировать", editResult);
        Assert.Equal("Сохранить", saveResult);
    }
    
    [Fact]
    public void RoleToVisibilityConverter_Worker_ReturnsVisible()
    {
        var converter = new RoleToVisibilityConverter();
        
        var workerResult = (Visibility)converter.Convert("Worker", null, "Worker", null);
        var userResult = (Visibility)converter.Convert("User", null, "Worker", null);
        
        Assert.Equal(Visibility.Visible, workerResult);
        Assert.Equal(Visibility.Collapsed, userResult);
    }
    
    
}