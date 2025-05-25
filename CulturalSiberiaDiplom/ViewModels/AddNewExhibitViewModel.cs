using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class AddNewExhibitViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    private readonly Museum _museum;
    private readonly Window _window;
    
    public event EventHandler? ExhibitAdded;
    
    public string Title { get; set; }
    public string? Price { get; set; }
    public bool Originality { get; set; }
    public byte[]? ImageBytes { get; set; }
    public BitmapSource? PreviewImage { get; set; }
    
    private string? _descriptionProperty;
    public string? DescriptionProperty
    {
        get => _descriptionProperty;
        set
        {
            if (_descriptionProperty == value) return;

            if (value?.Length > 100) return;
            
            _descriptionProperty = value;
            OnPropertyChanged(nameof(DescriptionProperty));
            OnPropertyChanged(nameof(DescriptionCharacterCount));
        }
    }
    
    public string DescriptionCharacterCount =>
        $"{DescriptionProperty?.Length ?? 0}/100";
    
    public ICommand SetImageCommand { get; }
    public ICommand OnChoseImageCommand { get; }
    public ICommand AddNewExhibitCommand { get; }

    public AddNewExhibitViewModel(CulturalSiberiaContext context, Museum museum)
    {
        _context = context;
        _museum = museum;

        ClearForm();
        
        SetImageCommand = new RelayCommand(() => SetImage(ImageBytes));
        OnChoseImageCommand = new RelayCommand(OnChoseImage);
        AddNewExhibitCommand = new RelayCommand(AddNewExhibit);
    }
    
    private void OnChoseImage()
    {
        var imageBytes = ImageService.ChooseImage();

        ImageBytes = imageBytes ?? null;

        if (imageBytes != null) 
            SetImage(imageBytes);
    }

    private async void AddNewExhibit()
    {
        try
        {
            if (!InputValidator.ValidateNewExhibit(Title, decimal.Parse(Price)))
                return;

            var exhibit = new Exhibit
            {
                Name = Title,
                CreatedAt = null,
                Price = decimal.Parse(Price),
                Description = DescriptionProperty,
                StatusId = 1,
                OriginalityStatusId = Originality ? 1 : 2,
                TimestampOfReceipt = DateTime.Now
            };

            _context.Exhibits.Add(exhibit);
            await _context.SaveChangesAsync();

            _museum.MuseumExhibits.Add(exhibit);
            await _context.SaveChangesAsync();
            
            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Exhibits",
                    exhibit.Id,
                    $"exhibit_{exhibit.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);

                if (mediaFileId.HasValue)
                {
                    exhibit.ImageMediaId = mediaFileId.Value;
                    _context.Update(exhibit);
                    await _context.SaveChangesAsync();
                }
            }
            
            MessageService.ShowSuccess("Экспонат успешно добавлен");
            ExhibitAdded?.Invoke(this, EventArgs.Empty);

            ClearForm();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при добавлении нового экспоната: " + ex.Message);
            
            MessageService.ShowError("Ошибка при добавлении нового экспоната, повторите попытку позже");
        }
    }
    
    private void SetImage(byte[]? imageBytes)
    {
        if (imageBytes == null) return;

        ImageBytes = imageBytes;
        PreviewImage = ImageService.SetImage(imageBytes);
        OnPropertyChanged(nameof(PreviewImage));
    }
    
    private void ClearForm()
    {
        Title = "";
        Price = null;
        Originality = false;
        ImageBytes = null;
        PreviewImage = ImageService.GetImageById(-1, _context);
        DescriptionProperty = null;
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Price));
        OnPropertyChanged(nameof(Originality));
        OnPropertyChanged(nameof(PreviewImage));
        OnPropertyChanged(nameof(DescriptionProperty));
    }
}