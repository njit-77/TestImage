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
        private Point StartPosition;
        private Point EndPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                StartPosition = e.GetPosition(img);
            }
        }

        private void Image_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                var tg = img.RenderTransform as TransformGroup;

                if (tg != null)
                {
                    var translateTransform = tg.Children[0] as TranslateTransform;
                    if (translateTransform != null)
                    {
                        translateTransform.X = 0;
                        translateTransform.Y = 0;
                    }

                    var scaleTransform = tg.Children[1] as ScaleTransform;
                    if (scaleTransform != null)
                    {
                        scaleTransform.ScaleX = 1;
                        scaleTransform.ScaleY = 1;
                    }
                }
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;
            if (img != null && e.LeftButton == MouseButtonState.Pressed)
            {
                EndPosition = e.GetPosition(img);

                TransformGroup tg = img.RenderTransform as TransformGroup;
                if (tg != null)
                {
                    var translateTransform = tg.Children[0] as TranslateTransform;
                    var scaleTransform = tg.Children[1] as ScaleTransform;
                    if (scaleTransform != null && translateTransform != null)
                    {
                        var control = img.Parent as FrameworkElement;

                        var matrix = ((MatrixTransform)scaleTransform.Inverse).Value;

                        if (img.ActualWidth * scaleTransform.ScaleX > control.ActualWidth)
                        {
                            translateTransform.X += EndPosition.X - StartPosition.X;

                            translateTransform.X = (translateTransform.X > 0 ? 1 : -1) *
                                Math.Min(Math.Abs(matrix.OffsetX - (control.ActualWidth - img.ActualWidth) * 0.5 / scaleTransform.ScaleX), Math.Abs(translateTransform.X));
                        }
                        if (img.ActualHeight * scaleTransform.ScaleY > control.ActualHeight)
                        {
                            translateTransform.Y += EndPosition.Y - StartPosition.Y;

                            translateTransform.Y = (translateTransform.Y > 0 ? 1 : -1) *
                                Math.Min(Math.Abs(matrix.OffsetY - (control.ActualHeight - img.ActualHeight) * 0.5 / scaleTransform.ScaleY), Math.Abs(translateTransform.Y));
                        }
                    }
                }
            }
        }

        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Image img = sender as Image;
            if (img != null)
            {
                double scaleValue = 0.5;
                if (e.Delta > 0)
                {
                    scaleValue *= 1;
                }
                else if (e.Delta < 0)
                {
                    scaleValue *= -1;
                }

                var tg = img.RenderTransform as TransformGroup;
                if (tg != null)
                {
                    var translateTransform = tg.Children[0] as TranslateTransform;
                    var scaleTransform = tg.Children[1] as ScaleTransform;
                    if (scaleTransform != null)
                    {
                        var control = img.Parent as FrameworkElement;

                        var matrix = ((MatrixTransform)scaleTransform.Inverse).Value;

                        scaleTransform.CenterX = img.ActualWidth * 0.5;
                        scaleTransform.CenterY = img.ActualHeight * 0.5;

                        scaleTransform.ScaleX += scaleValue;
                        scaleTransform.ScaleY += scaleValue;

                        scaleTransform.ScaleX = Math.Max(0.5, scaleTransform.ScaleX);
                        scaleTransform.ScaleY = Math.Max(0.5, scaleTransform.ScaleY);
                        //scaleTransform.ScaleX = Math.Min(21, scaleTransform.ScaleX);
                        //scaleTransform.ScaleY = Math.Min(21, scaleTransform.ScaleY);

                        if (img.ActualWidth * scaleTransform.ScaleX > control.ActualWidth)
                        {
                            translateTransform.X += EndPosition.X - StartPosition.X;

                            translateTransform.X = (translateTransform.X > 0 ? 1 : -1) *
                                Math.Min(Math.Abs(matrix.OffsetX - (control.ActualWidth - img.ActualWidth) * 0.5 / scaleTransform.ScaleX), Math.Abs(translateTransform.X));
                        }
                        if (img.ActualHeight * scaleTransform.ScaleY > control.ActualHeight)
                        {
                            translateTransform.Y += EndPosition.Y - StartPosition.Y;

                            translateTransform.Y = (translateTransform.Y > 0 ? 1 : -1) *
                                Math.Min(Math.Abs(matrix.OffsetY - (control.ActualHeight - img.ActualHeight) * 0.5 / scaleTransform.ScaleY), Math.Abs(translateTransform.Y));
                        }

                        if (Math.Abs(scaleTransform.ScaleX - 0.5) < 1e-7)
                        {
                            translateTransform.X = 0;
                            translateTransform.Y = 0;
                        }
                    }
                }
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
