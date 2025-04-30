using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using CulturalSiberiaDiplom.Models;
using CulturalSiberiaDiplom.Services;

namespace CulturalSiberiaDiplom.ViewModels;

public class AddNewEventViewModel : NotifyProperty
{
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
    
    public List<Eventstype> EventTypes { get; }
    
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

    private DateTime _startDateProperty;
    public DateTime StartDate
    {
        get => _startDateProperty;
        set
        {
            if (_startDateProperty == value) return;

            _startDateProperty = value;
            OnPropertyChanged(nameof(StartDate));
        }
    }

    private DateTime _endDateProperty;
    public DateTime EndDate
    {
        get => _endDateProperty;
        set
        {
            if (_endDateProperty == value) return;

            _endDateProperty = value;
            OnPropertyChanged(nameof(EndDate));
        }
    }

    private string? _locationProperty;
    public string? LocationProperty
    {
        get => _locationProperty;
        set
        {
            if (_locationProperty == value) return;

            _locationProperty = value;
            OnPropertyChanged(nameof(LocationProperty));
        }
    }

    private int? _capacityProperty;
    public int? CapacityProperty
    {
        get => _capacityProperty;
        set
        {
            if (_capacityProperty == value) return;

            _capacityProperty = value; 
            OnPropertyChanged(nameof(CapacityProperty));
        }
    }

    private string? _descriptionProperty;

    public string? DescriptionProperty
    {
        get => _descriptionProperty;
        set
        {
            if (_descriptionProperty == value) return;

            if (string.IsNullOrEmpty(value) || value.Length > 100) return;
            
            _descriptionProperty = value;
            OnPropertyChanged(nameof(DescriptionProperty));
        }
    }
    
    public string? DescriptionCharacterCount
    {
        get => DescriptionProperty;
        set
        {
            if (DescriptionProperty == value) return;

            DescriptionCharacterCount = "DescriptionProperty.Length" + "/100";
            OnPropertyChanged(nameof(DescriptionCharacterCount));
        }
    }

    public ICommand AddNewEventCommand { get; }
    public ICommand ChoseImageCommand { get; }

    public AddNewEventViewModel()
    {
        EventTypes = Service.GetDbContext().Eventstypes.ToList();
        AddNewEventCommand = new RelayCommand(async () => await AddNewEvent());
        ChoseImageCommand = new RelayCommand(OnChoseImage);
    }
    
    private async Task AddNewEvent()
    {
        if (!InputValidator.ValidateNewEvent(TitleProperty, LocationProperty, PriceProperty, CapacityProperty))
            return;

        if (TypeId <= 0)
        {
            MessageService.ShowError("Выберите тип мероприятия");
            return;
        }

        await using var context = Service.GetDbContext();
        
        var @event = new Event
        {
            Title = TitleProperty,
            TypeId = TypeId,
            StartDate = StartDate,
            EndDate = EndDate,
            Location = LocationProperty,
            Price = PriceProperty,
            Capacity = CapacityProperty,
            Description = DescriptionProperty,
            CreatedBy = CurrentUser.SelectedUser!.Id,
            ImageMediaId = null
        };

        context.Events.Add(@event);
        await context.SaveChangesAsync();

        if (ImageBytes?.Length > 0)
        {
            var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                "Events", 
                @event.Id, 
                $"event_{@event.Id}.png", 
                "image/png",
                ImageBytes,
                DateTime.Now,
                context);

            if (mediaFileId.HasValue)
            {
                @event.ImageMediaId = mediaFileId.Value;
                context.Update(@event);
                await context.SaveChangesAsync(); 
            }
        }
        
        MessageService.ShowSuccess("Мероприятие добавлено");
    }

    private void OnChoseImage()
    {
        var imageBytes = ImageService.ChooseImage();

        ImageBytes = imageBytes ?? null;
    }
    
    public void SetImage(byte[] imageBytes)
    {
        ImageBytes = imageBytes;
        using var stream = new MemoryStream(imageBytes);
        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.StreamSource = stream;
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();

        PreviewImage = bitmap;
        OnPropertyChanged(nameof(PreviewImage));
    }
}