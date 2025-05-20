using System.IO;
using System.Windows;
using System.Windows.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views;

public partial class MuseumDetailsWindow : Window
{
    public MuseumDetailsWindow(Museum museum)
    {
        InitializeComponent();

        var currentUser = CurrentUser.SelectedUser;
        var context = Service.GetDbContext();
        
        DataContext = new MuseumDetailsViewModel(museum, context, currentUser, this);
    }

    private void Exhibit_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MuseumDetailsViewModel vm)
            vm.OpenExhibitDetailsCommand.Execute(vm.SelectedExhibit);
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
            if (DataContext is MuseumDetailsViewModel vm)
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
        if (DataContext is MuseumDetailsViewModel vm && vm.IsInEditMode)
            vm.OnChoseImageCommand.Execute(sender);
    }
}