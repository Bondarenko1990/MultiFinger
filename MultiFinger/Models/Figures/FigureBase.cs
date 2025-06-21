using System.Windows;

namespace MultiFinger.Models.Figures
{
    public abstract class FigureBase
    {
        public string ArgbColor { get; set; }

        public string ToolTip { get; set; }

        public bool IsTransformed { get; set; }

        public abstract Point CenterPoint { get; }

        public abstract Size FigureSize { get; }
    }
}
