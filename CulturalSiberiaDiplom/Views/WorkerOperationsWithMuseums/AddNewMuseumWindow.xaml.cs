using System.IO;
using System.Windows;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseums;

public partial class AddNewMuseumWindow : Window
{
    public AddNewMuseumWindow()
    {
        InitializeComponent();
        DataContext = new AddNewMuseumViewModel(Service.GetDbContext());
    }
    
    private void ImageButton_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) is false) return;

        var files = e.Data.GetData(DataFormats.FileDrop) as string[];

        if (files is not { Length: > 0 }) return;

        var path = files[0];

        if (ImageService.IsImageFile(path))
        {
            var data = File.ReadAllBytes(path);
            
            if (DataContext is AddNewEventViewModel vm)
            {
                vm.ImageBytes = data;
                vm.PreviewImage = ImageService.SetImage(data);
            }
            
        }
        else
        {
            MessageService.ShowError("Поддерживаются только файлы изображений");
        }
    }
}