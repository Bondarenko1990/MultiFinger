using System.Windows;

namespace MultiFinger.Models.Figures
{
    public class FigurePoint : FigureBase
    {
        public Point A { get; set; }

        public bool Filled { get; set; }

        public override Point CenterPoint
        {
            get
            {
                return A;
            }
        }

        public override Size FigureSize
        {
            get
            {
                return new Size(4, 4);
            }
        }
    }
}
