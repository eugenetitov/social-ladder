using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Challenges
{
    public class LocalPosterModel : INotifyPropertyChanged
    {
        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set => SetField(ref _image, value, nameof(Image));
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value, nameof(Title));
        }

        public double Lat
        {
            get; set;
        }

        public double Lon
        {
            get; set;
        }
         
        public bool HasLocation { get; set; }
        public bool CreatedFromCamera { get; set; } = false;
        public string Position { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
