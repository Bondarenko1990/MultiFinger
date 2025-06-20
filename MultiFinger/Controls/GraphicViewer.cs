using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using MultiFinger.Models.Figures;

namespace MultiFinger.Controls
{
    [TemplatePart(Name = "RootCanvas", Type = typeof(Canvas))]
    public class GraphicViewer : Control
    {
        private Canvas _rootCanvas;

        public static readonly DependencyProperty ItemsProperty =
                 DependencyProperty.Register("Items", typeof(IEnumerable<FigureBase>), typeof(GraphicViewer), new PropertyMetadata(null, ItemsChanged));

        public static readonly DependencyProperty ContentControlStyleProperty =
                 DependencyProperty.Register("ContentControlStyle", typeof(Style), typeof(GraphicViewer), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty ShowTransformedDataProperty =
                 DependencyProperty.Register("ShowTransformedData", typeof(bool), typeof(GraphicViewer), new PropertyMetadata(false, OnShowTransformedDataChanged));

        public GraphicViewer()
        {
            DefaultStyleKey = typeof(GraphicViewer);
        }

        public IEnumerable<FigureBase> Items
        {
            get { return (IEnumerable<FigureBase>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public Style ContentControlStyle
        {
            get { return (Style)GetValue(ContentControlStyleProperty); }
            set { SetValue(ContentControlStyleProperty, value); }
        }

        public bool ShowTransformedData
        {
            get { return (bool)GetValue(ShowTransformedDataProperty); }
            set { SetValue(ShowTransformedDataProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _rootCanvas = (Canvas)GetTemplateChild("RootCanvas");
            _rootCanvas.Loaded += _rootCanvas_Loaded;
            this.SizeChanged += GraphicViewer_SizeChanged;

            BuildFigures();
        }

        private static void OnShowTransformedDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var graphicViewer = (GraphicViewer)d;

            graphicViewer.BuildFigures();
        }

        private static void ItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var graphicViewer = (GraphicViewer)dependencyObject;
            var oldAnimations = dependencyPropertyChangedEventArgs.OldValue as INotifyCollectionChanged;
            if (oldAnimations != null)
                oldAnimations.CollectionChanged -= graphicViewer.NewItemsCollectionChanged;

            var newAnimations = dependencyPropertyChangedEventArgs.NewValue as INotifyCollectionChanged;
            if (newAnimations != null)
                newAnimations.CollectionChanged += graphicViewer.NewItemsCollectionChanged;

            graphicViewer.BuildFigures();
        }

        private void NewItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BuildFigures();
        }

        private void BuildFigures()
        {
            if (_rootCanvas == null || Items == null) return;

            var filteredItems = ShowTransformedData
                ? Items.Where(i => i.IsTransformed).ToList()
                : Items.Where(i => !i.IsTransformed).ToList();

            // clear children
            _rootCanvas.Children.Clear();

            // get max height
            var maxHeight = filteredItems.Select(i => ((Math.Abs(i.CenterPoint.Y) * 2) + (i.FigureSize.Height))).Max();
            // get max width
            var maxWidth = filteredItems.Select(i => ((Math.Abs(i.CenterPoint.X) * 2) + (i.FigureSize.Width))).Max();

            // get max size
            var maxSize = Math.Max(maxHeight, maxWidth);

            // set max size
            _rootCanvas.Width = maxSize;
            _rootCanvas.Height = maxSize;

            foreach (var item in filteredItems)
            {
                // create figure control
                var contentControl = new ContentControl
                {
                    Style = ContentControlStyle,
                    Content = item,
                    Width = item.FigureSize.Width,
                    Height = item.FigureSize.Height,
                };

                // add figure control to canvas
                _rootCanvas.Children.Add(contentControl);

                // set Canvas.Left
                Canvas.SetLeft(contentControl, (maxSize / 2) + item.CenterPoint.X - item.FigureSize.Width / 2);
                // set Canvas.Top
                Canvas.SetTop(contentControl, (maxSize / 2) - item.CenterPoint.Y - item.FigureSize.Height / 2);
            }

            // update canvas layout
            _rootCanvas.UpdateLayout();
        }

        private void GraphicViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (double.IsNaN(_rootCanvas.Width)) return;

            var size = Math.Min(e.NewSize.Height, e.NewSize.Width);
            if (size > _rootCanvas.Height)
            {
                _rootCanvas.MaxHeight = double.PositiveInfinity;
                _rootCanvas.MaxWidth = double.PositiveInfinity;
            }
            else
            {
                _rootCanvas.MaxHeight = size;
                _rootCanvas.MaxWidth = size;
            }
        }

        private void _rootCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            if (double.IsNaN(_rootCanvas.Width)) return;
            var minSize = Math.Min(this.ActualHeight, this.ActualWidth);
            if (minSize > _rootCanvas.Height)
            {
                _rootCanvas.MaxHeight = double.PositiveInfinity;
                _rootCanvas.MaxWidth = double.PositiveInfinity;
            }
            else
            {
                _rootCanvas.MaxHeight = minSize;
                _rootCanvas.MaxWidth = minSize;
            }
        }
    }
}
