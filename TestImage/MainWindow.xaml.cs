using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TestImage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ZoomOutScaleValue = 1.1;
        private const double ZoomInScaleValue = 1 / ZoomOutScaleValue;
        private Point start;
        private Point origin;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                start = e.GetPosition(img);
                origin.X = img.RenderTransform.Value.OffsetX;
                origin.Y = img.RenderTransform.Value.OffsetY;
            }
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                Matrix m = img.RenderTransform.Value;

                m.M11 = m.M22 = 1;
                m.M12 = m.M21 = 0;
                m.OffsetX = m.OffsetY = 0;

                img.RenderTransform = new MatrixTransform(m);
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;
            if (img != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var control = img.Parent as FrameworkElement;

                Point p = e.GetPosition(img);
                Matrix m = img.RenderTransform.Value;

                SetImageBorder(ref m, p, img, control);

                img.RenderTransform = new MatrixTransform(m);
            }
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                var control = img.Parent as FrameworkElement;

                Point p = e.GetPosition(img);
                Matrix m = img.RenderTransform.Value;
                if (e.Delta > 0)
                    m.ScaleAtPrepend(ZoomOutScaleValue, ZoomOutScaleValue, p.X, p.Y);
                else
                    m.ScaleAtPrepend(ZoomInScaleValue, ZoomInScaleValue, p.X, p.Y);

                img.RenderTransform = new MatrixTransform(m);
            }
        }

        private void SetImageBorder(ref Matrix m, Point p, Image img, FrameworkElement control)
        {
            double diffX = (p.X - start.X) * m.M11;
            double diffY = (p.Y - start.Y) * m.M22;
            if (img.ActualWidth * m.M11 > control.ActualWidth)
            {
                m.OffsetX = origin.X + diffX;
            }
            if (img.ActualHeight * m.M22 > control.ActualHeight)
            {
                m.OffsetY = origin.Y + diffY;
            }
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.SizeAll;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

    }
}
