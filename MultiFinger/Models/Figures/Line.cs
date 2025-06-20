using System.Windows;
using System.Windows.Media;

namespace MultiFinger.Models.Figures
{
    public class Line : FigureBase
    {
        public Point A { get; set; }

        public Point B { get; set; }

        public PointCollection Points => new PointCollection()
        {
            new Point(A.X, -A.Y),
            new Point(B.X, -B.Y)
        };

        public override Point CenterPoint
        {
            get
            {
                return GetCenter();
            }
        }

        public override Size FigureSize
        {
            get
            {
                return GetSize();
            }
        }

        private Point GetCenter()
        {
            double X3 = (A.X + B.X) / 2;
            double Y3 = (A.Y + B.Y) / 2;

            return new Point(X3, Y3);
        }

        private Size GetSize()
        {
            // height: Ymax - Ymin
            // width: Xmax - Xmin

            double height = 1;

            if (A.Y > B.Y)
            {
                height = A.Y - B.Y;
            }
            else if (A.Y < B.Y) 
            {
                height = B.Y - A.Y;
            }

            double width = 1;

            if (A.X > B.X)
            {
                width = A.X - B.X;
            }
            else if (A.X < B.X)
            {
                width = B.X - A.X;
            }

            return new Size(width, height);
        }
    }
}
