using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FancyShare.FancyZones
{

    public class FancyZonesConfig
    {
        [JsonPropertyName("devices")]
        public List<Device> Devices { get; set; }

        [JsonPropertyName("custom-zone-sets")]
        public List<CustomZoneSet> CustomZoneSets { get; set; }
    }

    public partial class CustomZoneSet
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("info")]
        public Info Info { get; set; }
    }

    public partial class Info
    {
        [JsonPropertyName("ref-width")]
        public long? RefWidth { get; set; }

        [JsonPropertyName("ref-height")]
        public long? RefHeight { get; set; }

        [JsonPropertyName("zones")]
        public List<ZoneInfo> Zones { get; set; }

        [JsonPropertyName("rows")]
        public long? Rows { get; set; }

        [JsonPropertyName("columns")]
        public long? Columns { get; set; }

        [JsonPropertyName("rows-percentage")]
        public int[] RowsPercentage { get; set; }

        [JsonPropertyName("columns-percentage")]
        public int[] ColumnsPercentage { get; set; }

        [JsonPropertyName("cell-child-map")]
        public int[][] CellChildMap { get; set; }
    }

    public partial class ZoneInfo
    {
        [JsonPropertyName("X")]
        public long X { get; set; }

        [JsonPropertyName("Y")]
        public long Y { get; set; }

        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }
    }

    public partial class Device
    {
        [JsonPropertyName("device-id")]
        public string DeviceId { get; set; }

        [JsonPropertyName("active-zoneset")]
        public ActiveZoneset ActiveZoneset { get; set; }

        [JsonPropertyName("editor-show-spacing")]
        public bool WithSpacing { get; set; }

        [JsonPropertyName("editor-spacing")]
        public long Spacing { get; set; }

        [JsonPropertyName("editor-zone-count")]
        public long ZoneCount { get; set; }

        [JsonPropertyName("editor-sensitivity-radius")]
        public long EditorSensitivityRadius { get; set; }
    }

    public partial class ActiveZoneset
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

}
