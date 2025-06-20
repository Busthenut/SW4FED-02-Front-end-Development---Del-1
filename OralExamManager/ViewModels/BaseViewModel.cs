using CommunityToolkit.Mvvm.ComponentModel;

namespace OralExamManager.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        // BaseViewModel now inherits from ObservableObject which provides all the INotifyPropertyChanged functionality
        // No need for custom implementation as the toolkit handles everything
    }
} 