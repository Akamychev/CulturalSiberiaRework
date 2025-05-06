using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class AddNewMuseumViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;
    
    private string _titleProperty;
    public string TitleProperty
    {
        get => _titleProperty;
        set
        {
            if (_titleProperty == value) return;

            _titleProperty = value;
            OnPropertyChanged(nameof(TitleProperty));
        }
    }

    public List<Museumtype> MuseumTypes { get; }
    
    private int _typeId;
    public int TypeId
    {
        get => _typeId;
        set
        {
            if (_typeId == value) return;

            _typeId = value;
            OnPropertyChanged(nameof(TypeId));
        }
    }

    private decimal? _priceProperty;
    public decimal? PriceProperty
    {
        get => _priceProperty;
        set
        {
            if (_priceProperty == value) return;

            _priceProperty = value;
            OnPropertyChanged(nameof(PriceProperty));
        }
    }

    private byte[]? _imageBytesProperty;
    public byte[]? ImageBytes
    {
        get => _imageBytesProperty;
        set
        {
            if (_imageBytesProperty == value) return;

            _imageBytesProperty = value;
            OnPropertyChanged(nameof(ImageBytes));
        }
    }
    
    private BitmapSource _previewImage;
    public BitmapSource PreviewImage
    {
        get => _previewImage;
        set
        {
            _previewImage = value;
            OnPropertyChanged(nameof(PreviewImage));
        }
    }

    private TimeOnly _startTimeProperty;
    public TimeOnly StartTime
    {
        get => _startTimeProperty;
        set
        {
            if (_startTimeProperty == value) return;
            
            _startTimeProperty = value;
            OnPropertyChanged(nameof(StartTime));
        }
    }

    private TimeOnly _endTimeProperty;
    public TimeOnly EndTime
    {
        get => _endTimeProperty;
        set
        {
            if (_endTimeProperty == value) return;

            _endTimeProperty = value;
            OnPropertyChanged(nameof(EndTime));
        }
    }

    private string _locationProperty;
    public string LocationProperty
    {
        get => _locationProperty;
        set
        {
            if (_locationProperty == value) return;

            _locationProperty = value;
            OnPropertyChanged(nameof(LocationProperty));
        }
    }

    private DateOnly? _foundationDateProperty;
    public DateOnly? FoundationProperty
    {
        get => _foundationDateProperty;
        set
        {
            if (_foundationDateProperty == value) return;

            _foundationDateProperty = value; 
            OnPropertyChanged(nameof(FoundationProperty));
        }
    }

    private string? _architectsProperty;
    public string? ArchitectsProperty
    {
        get => _architectsProperty;
        set
        {
            if (_architectsProperty == value) return;

            _architectsProperty = value;
            OnPropertyChanged(nameof(ArchitectsProperty));
        }
    }

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

    public ICommand AddNewMuseumCommand { get; }
    public ICommand ChoseImageCommand { get; }
    
    public AddNewMuseumViewModel(CulturalSiberiaContext context)
    {
        _context = context;
        MuseumTypes = _context.Museumtypes.ToList();
        AddNewMuseumCommand = new RelayCommand(async () => await AddNewMuseum());
        ChoseImageCommand = new RelayCommand(OnChoseImage);
    }

    private async Task AddNewMuseum()
    {
        try
        {
            if (!InputValidator.ValidateNewMuseum(TitleProperty, LocationProperty, PriceProperty, ArchitectsProperty))
                return;

            if (TypeId < 0)
            {
                MessageService.ShowError("Выберите тип музея");
                return;
            }

            var museum = new Museum
            {
                Name = TitleProperty,
                Location = LocationProperty,
                TypeId = TypeId,
                Price = PriceProperty ?? 0,
                StartWorkingTime = StartTime,
                EndWorkingTime = EndTime,
                Architects = ArchitectsProperty,
                DateOfFoundation = FoundationProperty,
                Description = DescriptionProperty,
                StatusId = 2,
                ImageMediaId = null
            };

            _context.Museums.Add(museum);
            await _context.SaveChangesAsync();

            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Museum",
                    museum.Id,
                    $"museum_{museum.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);

                if (mediaFileId.HasValue)
                {
                    museum.ImageMediaId = mediaFileId.Value;
                    _context.Update(museum);
                    await _context.SaveChangesAsync();
                }
            }

            MessageService.ShowSuccess("Музей добавлен");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка добавления музея " + ex.Message);
            
            if (ex.InnerException != null)
                Console.WriteLine("Внутренняя ошибка добавления музея: " + ex.InnerException.Message);
            
            MessageService.ShowError("Ошибка добавления музея");
        }
    }
    
    private void OnChoseImage()
    {
        try
        {
            var imageBytes = ImageService.ChooseImage();

            ImageBytes = imageBytes ?? null;

            if (imageBytes != null)
                SetImage(imageBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка добавления изображения" + ex.Message);
            
            MessageService.ShowError("Ошибка добавления изображения");
        }
    }
    
    private void SetImage(byte[] imageBytes)
    {
        ImageBytes = imageBytes;
        PreviewImage = ImageService.SetImage(imageBytes);
        OnPropertyChanged(nameof(PreviewImage));
    }
}