using CommunityToolkit.Mvvm.ComponentModel;

namespace Core.Web.ViewModels
{
    public partial class ViewModelBase : ObservableObject, IDisposable
    {
        [ObservableProperty]
        private bool _isLoading;

        public ViewModelBase()
        {

        }

        protected void SetIsLoading(bool value)
        {
            IsLoading = value;
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        #region dispose

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
