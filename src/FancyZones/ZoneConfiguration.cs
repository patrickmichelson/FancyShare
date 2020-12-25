using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using FancyShare.Models;

namespace FancyShare.FancyZones
{
    class ZoneConfiguration
    {
        const string fileName = @"%LOCALAPPDATA%\Microsoft\PowerToys\FancyZones\zones-settings.json";
        private FancyZonesConfig Configuration;

        public ZoneConfiguration()
        {
            var configFile = Environment.ExpandEnvironmentVariables(fileName);
            var jsonString = File.ReadAllText(configFile);
            Configuration = JsonSerializer.Deserialize<FancyZonesConfig>(jsonString);
        }


        public ZoneSet GetZones(Monitor screen)
        {
            var fancyZonesId = TrimDeviceId(screen.DeviceId) + "_" + screen.Bounds.Width + "_" + screen.Bounds.Height + "_{" + screen.VirtualDesktopId + "}";

            var deviceConfig = Configuration.Devices.FirstOrDefault(x => x.DeviceId.Equals(fancyZonesId, StringComparison.OrdinalIgnoreCase));
            var spacing = deviceConfig.WithSpacing ? deviceConfig.Spacing : 0;

            switch (deviceConfig.ActiveZoneset.Type)
            {
                case "custom":
                    var customSet = Configuration.CustomZoneSets.FirstOrDefault(x => x.Uuid == deviceConfig.ActiveZoneset.Uuid);
                    if (customSet == null)
                    {
                        throw new InvalidDataException("Did not find custom zone set in settings file.");
                    }

                    return customSet.Type switch
                    {
                        "canvas" => CreateCanvasZones(customSet, screen),
                        "grid" => CreateGridZones(customSet.Info, screen),
                        _ => throw new NotSupportedException($"Custom zone layout type '{customSet.Type}' not is supported."),
                    };
                case "blank":
                case "focus":
                case "columns":
                    return CreateColumnZones((int)deviceConfig.ZoneCount, spacing, screen);
                case "rows":
                    return CreateRowZones((int)deviceConfig.ZoneCount, spacing, screen);
                case "grid":
                    return CreateGridLayout((int)deviceConfig.ZoneCount, screen);
                case "priority-grid":
                default:
                    throw new NotSupportedException($"Zone set type '{deviceConfig.ActiveZoneset.Type}' not is supported.");
            }


        }

        private static ZoneSet CreateGridZones(Info customSet, Monitor monitor)
        {
            var columnsWidth = customSet.ColumnsPercentage.Select(x => (double)x / 10000 * monitor.WorkingArea.Width).ToList();
            var rowsHeight = customSet.RowsPercentage.Select(x => (double)x / 10000 * monitor.WorkingArea.Height).ToList();

            var zones = new List<Zone>();

            double top = 0;
            for (int row = 0; row < customSet.Rows; row++)
            {
                double left = 0;
                for (int col = 0; col < customSet.Columns; col++)
                {
                    var id = customSet.CellChildMap[row][col] + 1;
                    var size = new Rect(left, top, columnsWidth[col], rowsHeight[row]);

                    var existingZone = zones.FirstOrDefault(x => x.Id == id);
                    if (existingZone != null)
                    {
                        existingZone.Bounds = Rect.Union(existingZone.Bounds, size);
                    }
                    else
                    {
                        zones.Add(new Zone
                        {
                            Id = id,
                            Bounds = size,
                        });
                    }

                    left += columnsWidth[col];
                }

                top += rowsHeight[row];
            }

            return new ZoneSet
            {
                Zones = zones,
                ScreenOffset = monitor.WorkingArea.Location,
                ReferenceBounds = monitor.WorkingArea.Size
            };
        }

        private static ZoneSet CreateCanvasZones(CustomZoneSet customSet, Monitor monitor)
        {
            var zones = customSet.Info?.Zones?.Select((x, id) => new Zone
            {
                Id = id + 1,
                Bounds = new Rect(x.X, x.Y, x.Width, x.Height)
            });

            return new ZoneSet
            {
                Zones = zones,
                ScreenOffset = monitor.WorkingArea.Location,
                ReferenceBounds = monitor.WorkingArea.Size
            };
        }

        private static ZoneSet CreateRowZones(int zoneCount, double spacing, Monitor monitor)
        {
            var zoneWidth = monitor.WorkingArea.Width - spacing * 2;
            var zoneHeight = (monitor.WorkingArea.Height - spacing * (zoneCount + 1)) / zoneCount;
            var zoneOffset = (monitor.WorkingArea.Height - spacing) / zoneCount;

            var firstRect = new Rect(spacing, spacing, zoneWidth, zoneHeight);

            var zones = Enumerable.Range(1, zoneCount).Select(id =>
                new Zone
                {
                    Id = id,
                    Bounds = Rect.Offset(firstRect, 0, (id - 1) * zoneOffset)
                }
            );

            return new ZoneSet
            {
                Zones = zones,
                ScreenOffset = monitor.WorkingArea.Location,
                ReferenceBounds = monitor.WorkingArea.Size
            };
        }

        private static ZoneSet CreateColumnZones(int zoneCount, double spacing, Monitor monitor)
        {
            var zoneWidth = (monitor.WorkingArea.Width - spacing * (zoneCount + 1)) / zoneCount;
            var zoneHeight = monitor.WorkingArea.Height - spacing * 2;
            var zoneOffset = (monitor.WorkingArea.Width - spacing) / zoneCount;

            var firstRect = new Rect(spacing, spacing, zoneWidth, zoneHeight);

            var zones = Enumerable.Range(1, zoneCount).Select(id =>
                new Zone
                {
                    Id = id,
                    Bounds = Rect.Offset(firstRect, (id - 1) * zoneOffset, 0)
                }
            );

            return new ZoneSet
            {
                Zones = zones,
                ScreenOffset = monitor.WorkingArea.Location,
                ReferenceBounds = monitor.WorkingArea.Size
            };
        }

        private static ZoneSet CreateFocusLayoutZones(int zoneCount, Monitor monitor)
        {
            Rect focusZoneRect = new Rect(100, 100, monitor.WorkingArea.Width * 0.4, monitor.WorkingArea.Width * 0.4);

            double focusIncrement = 50;
            var zones = new List<Zone>();

            for (int i = 0; i < zoneCount; i++)
            {
                zones.Add(new Zone
                {
                    Id = i + 1,
                    Bounds = focusZoneRect
                });

                focusZoneRect = Rect.Offset(focusZoneRect, focusIncrement, focusIncrement);
            }

            return new ZoneSet
            {
                Zones = zones,
                ScreenOffset = monitor.WorkingArea.Location,
                ReferenceBounds = monitor.WorkingArea.Size
            };
        }


        private static ZoneSet CreateGridLayout(int zoneCount, Monitor monitor)
        {
            int rows = 1;
            while (zoneCount / rows >= rows)
            {
                rows++;
            }

            rows--;
            int cols = zoneCount / rows;
            if (zoneCount % rows == 0)
            {
                // even grid
            }
            else
            {
                cols++;
            }

            var _gridModel = new Info
            {
                Rows = rows,
                Columns = cols,
                RowsPercentage = new int[rows],
                ColumnsPercentage = new int[cols],
                CellChildMap = new int[rows][]
            };
            int _multiplier = 10000;

            // Note: The following are NOT equal to _multiplier divided by rows or columns and is
            // done like this to make the sum of all RowPercents exactly (_multiplier).
            for (int row = 0; row < rows; row++)
            {
                _gridModel.CellChildMap[row] = new int[cols];
                _gridModel.RowsPercentage[row] = _multiplier * (row + 1) / rows - _multiplier * row / rows;
            }

            for (int col = 0; col < cols; col++)
            {
                _gridModel.ColumnsPercentage[col] = _multiplier * (col + 1) / cols - _multiplier * col / cols;
            }

            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    _gridModel.CellChildMap[row][col] = index++;
                    if (index == zoneCount)
                    {
                        index--;
                    }
                }
            }

            return CreateGridZones(_gridModel, monitor);
        }

        private string TrimDeviceId(string input)
        {
            var match = Regex.Match(input, @"#(.*)#");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
