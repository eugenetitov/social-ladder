using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models
{
    public class BaseTable : INotifyPropertyChanged
    {
        [PrimaryKey]
        public string ID { get; set; }

        [Indexed, NotNull]
        public string UserId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Update(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static bool NotNullOrEmpty(BaseTable item)
        {
            if (item == null)
                return false;
            var propertyInfo = (item.GetType()).GetRuntimeProperty("Object");
            if (propertyInfo == null)
                return false;
            var value = propertyInfo.GetValue(item);
            return value != null; // && !string.IsNullOrEmpty(item.ID);
        }
    }

    public class LocalFile : BaseTable
    {
        public string Url { get; set; } = "";
        public string Path { get; set; } = "";
        public string TempName { get; set; } = "";
        public string Key { get; set; } = "";
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
