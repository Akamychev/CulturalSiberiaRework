using System.IO;
using System.Windows;
using System.Windows.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;
using CulturalSiberiaDiplom.ViewModels;

namespace CulturalSiberiaDiplom.Views.UserMenuViews;

public partial class PersonalCabinet : Window
{
    public PersonalCabinet(CulturalSiberiaContext context, User user)
    {
        InitializeComponent();
        DataContext = new PersonalCabinetViewModel(context, user);
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
            if (DataContext is PersonalCabinetViewModel vm)
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
        if (DataContext is PersonalCabinetViewModel vm)
            vm.ChoseImageCommand.Execute(sender);
    }
}