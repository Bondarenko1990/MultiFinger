using MultiFinger.Enums;
using System.Windows;
using System.Text.Json.Serialization;

namespace MultiFinger.Models.Figures
{
    public abstract class FigureBase
    {
        public FigureType Type { get; set; }

        [JsonPropertyName("color")]
        public string ArgbColor { get; set; }

        public string ToolTip { get; set; }

        public bool IsTransformed { get; set; }

        public abstract Point CenterPoint { get; }

        public abstract Size FigureSize { get; }
    }
}
