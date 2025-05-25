using System.IO;
using System.Windows;
using System.Windows.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.WorkerOperationsWithMuseumExhibits;

public partial class AddNewMuseumExhibitWindow : Window
{
    public AddNewMuseumExhibitWindow(CulturalSiberiaContext context, Museum museum)
    {
        InitializeComponent();
        DataContext = new AddNewExhibitViewModel(context, museum);
    }
    
    private void Image_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) is false) return;

        var files = e.Data.GetData(DataFormats.FileDrop) as string[];

        if (files is not { Length: > 0 }) return;

        var path = files[0];

        if (ImageService.IsImageFile(path))
        {
            var data = File.ReadAllBytes(path);
            if (DataContext is AddNewExhibitViewModel vm)
            {
                vm.ImageBytes = data;
                vm.SetImageCommand.Execute(data);
            }
        }
        else
        {
            MessageService.ShowError("Поддерживаются только файлы изображений");
        }
    }

    private void Image_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is AddNewExhibitViewModel vm)
            vm.OnChoseImageCommand.Execute(sender);
    }
}