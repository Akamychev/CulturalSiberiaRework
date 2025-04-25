
namespace CulturalSiberiaDiplom.Services;

public class CheckBoxController : NotifyProperty
{
    private bool _isSelected;
    public string Name { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = true;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}