using System.Threading.Tasks;

namespace Core.Web.ViewModels
{
    public class CounterViewModel : ViewModelBase
    {
        private int _count;

        public int Count
        {
            get => _count;
            private set => SetProperty(ref _count, value);
        }

        public CounterViewModel()
        {
        }

        public Task IncrementAsync()
        {
            Count++;
            return Task.CompletedTask;
        }

        public override Task InitializeAsync()
        {
            // initialize default state if needed
            return Task.CompletedTask;
        }
    }
}
