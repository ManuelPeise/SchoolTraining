using System.ComponentModel;
using Core.Web.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Core.Web.Components.Pages.ViewModels
{
    /// <summary>
    /// Generic base component that wires a ViewModel (CommunityToolkit ObservableObject) to Blazor rendering.
    /// It subscribes to PropertyChanged and calls StateHasChanged on changes, and calls InitializeAsync on the ViewModel.
    /// </summary>
    public abstract class ViewModelComponentBase<TViewModel> : ComponentBase, IDisposable
        where TViewModel : ViewModelBase
    {
        [Inject]
        protected TViewModel ViewModel { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            await ViewModel.InitializeAsync();
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Optionally filter by property name here
            InvokeAsync(StateHasChanged);
        }

        public virtual void Dispose()
        {
            try
            {
                ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
            catch { }

            try
            {
                (ViewModel as IDisposable)?.Dispose();
            }
            catch { }
        }
    }
}
