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

public class AddNewEventViewModel : NotifyProperty
{
    private readonly CulturalSiberiaContext _context;

    public event EventHandler? AddEvent;
    
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
    
    private BitmapSource? _previewImage;
    public BitmapSource? PreviewImage
    {
        get => _previewImage;
        set
        {
            _previewImage = value;
            OnPropertyChanged(nameof(PreviewImage));
        }
    }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set => SetField(ref _startDate, value);
    }

    private DateTime _endDate;
    public DateTime EndDate
    {
        get => _endDate;
        set => SetField(ref _endDate, value);
    }

    // private string _editStartDate;
    // public string EditStartDate
    // {
    //     get => _editStartDate;
    //     set
    //     {
    //         if ()
    //     }
    // }
    //
    // private string _editEndDate;
    // public string EditEndDate
    // {
    //     
    // }

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

            if (value?.Length > 100) return;
            
            _descriptionProperty = value;
            OnPropertyChanged(nameof(DescriptionProperty));
            OnPropertyChanged(nameof(DescriptionCharacterCount));
        }
    }
    
    public string DescriptionCharacterCount =>
        $"{DescriptionProperty?.Length ?? 0}/100";

    public ICommand AddNewEventCommand { get; }
    public ICommand ChoseImageCommand { get; }

    public AddNewEventViewModel(CulturalSiberiaContext context)
    {
        _context = context;
        EventTypes = _context.Eventstypes.ToList();
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        AddNewEventCommand = new RelayCommand(async () => await AddNewEvent());
        ChoseImageCommand = new RelayCommand(OnChoseImage);
    }
    
    private async Task AddNewEvent()
    {
        try
        {
            if (!InputValidator.ValidateNewEvent(TitleProperty, LocationProperty, PriceProperty, CapacityProperty,
                    StartDate, EndDate))
                return;

            if (TypeId <= 0)
            {
                MessageService.ShowError("Выберите тип мероприятия");
                return;
            }

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
                CreatedAt = DateTime.Now,
                ImageMediaId = null,
                StatusId = 1
            };

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            if (ImageBytes?.Length > 0)
            {
                var mediaFileId = await MediaFileService.SaveMediaFileAsync(
                    "Events",
                    @event.Id,
                    $"event_{@event.Id}.png",
                    "image/png",
                    ImageBytes,
                    DateTime.Now,
                    _context);

                if (mediaFileId.HasValue)
                {
                    @event.ImageMediaId = mediaFileId.Value;
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
            }
            
            MessageService.ShowSuccess("Мероприятие добавлено");
            AddEvent?.Invoke(this, EventArgs.Empty);

            TitleProperty = "";
            TypeId = 0;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            LocationProperty = null;
            PriceProperty = null;
            CapacityProperty = null;
            DescriptionProperty = null;
            ImageBytes = null;
            PreviewImage = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка добавления мероприятия " + ex.Message);
            
            MessageService.ShowError("Ошибка добавления мероприятия");
        }
    }

    private void OnChoseImage()
    {
        var imageBytes = ImageService.ChooseImage();

        ImageBytes = imageBytes ?? null;

        if (imageBytes != null) 
            SetImage(imageBytes);
    }
    
    private void SetImage(byte[] imageBytes)
    {
        ImageBytes = imageBytes;
        PreviewImage = ImageService.SetImage(imageBytes);
        OnPropertyChanged(nameof(PreviewImage));
    }
}