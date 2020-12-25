using FancyShare.FancyZones;
using FancyShare.Models;
using FanzyShare;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FancyShare.UI
{
    class ZoneOverlayViewModel : INotifyPropertyChanged
    {
        private readonly ZoneConfiguration _config;

        public ZoneOverlayViewModel(ZoneConfiguration config)
        {
            _config = config;
            SelectZone = new RelayCommand((arg) =>
            {
                var selectedZone = Zones.FirstOrDefault(x => x.Id == (int)arg);
                ZoneSelected?.Invoke(this, selectedZone);
            });
        }

        public Monitor Monitor { get; set; }

        public void UpdateZones(Monitor screen)
        {
            Monitor = screen;
            ZoneSet = _config.GetZones(screen);
            OnPropertyChanged("Zones");
        }

        public event EventHandler<Zone> ZoneSelected;

        public ICommand SelectZone { get; }

        public ZoneSet ZoneSet { get; private set; }
        public IEnumerable<Zone> Zones => ZoneSet?.Zones;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
