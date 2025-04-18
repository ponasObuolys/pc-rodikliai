using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PC_Rodikliai.Models
{
    public class Metric : INotifyPropertyChanged
    {
        public required string Title { get; set; } = string.Empty;
        public double Value { get; set; }
        public required string Unit { get; set; } = string.Empty;
        public required string Icon { get; set; } = string.Empty;

        public string Name
        {
            get => Title;
            set => Title = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 