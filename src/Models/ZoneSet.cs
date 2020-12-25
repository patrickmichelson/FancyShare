using System.Collections.Generic;
using System.Windows;

namespace FancyShare.Models
{
    class ZoneSet
    {
        public IEnumerable<Zone> Zones { get; set; }
        public Point ScreenOffset { get; set; }
        public Size ReferenceBounds { get; set; }
    }
}
