using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmLight1.Controls
{
    /// <summary>
    /// RectangleControl.xaml 的交互逻辑
    /// </summary>
    public partial class RectangleControl : UserControl
    {
        public RectangleControl()
        {
            InitializeComponent();
        }

        public string MyProperty
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(string), typeof(RectangleControl), new PropertyMetadata(""));

        public double RectX
        {
            get { return (double)GetValue(RectXProperty); }
            set { SetValue(RectXProperty, value); }
        }

        public static readonly DependencyProperty RectXProperty =
            DependencyProperty.Register("RectX", typeof(double), typeof(RectangleControl), new PropertyMetadata(new PropertyChangedCallback(rectXPropertyChangedCallback)));

        private static void rectXPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Canvas.SetLeft((RectangleControl)d, Convert.ToDouble(e.NewValue));
        }

        public double RectY
        {
            get { return (double)GetValue(RectYProperty); }
            set { SetValue(RectYProperty, value); }
        }

        public static readonly DependencyProperty RectYProperty =
            DependencyProperty.Register("RectY", typeof(double), typeof(RectangleControl), new PropertyMetadata(new PropertyChangedCallback(rectYPropertyChangedCallback)));

        private static void rectYPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Canvas.SetTop((RectangleControl)d, Convert.ToDouble(e.NewValue));
        }

        public double RectWidth
        {
            get { return (double)GetValue(RectWidthProperty); }
            set { SetValue(RectWidthProperty, value); }
        }

        public static readonly DependencyProperty RectWidthProperty =
            DependencyProperty.Register("RectWidth", typeof(double), typeof(RectangleControl), new PropertyMetadata(1.0));

        public double RectHeight
        {
            get { return (double)GetValue(RectHeightProperty); }
            set { SetValue(RectHeightProperty, value); }
        }

        public static readonly DependencyProperty RectHeightProperty =
            DependencyProperty.Register("RectHeight", typeof(double), typeof(RectangleControl), new PropertyMetadata(1.0));

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            var thumb = sender as System.Windows.Controls.Primitives.Thumb;
            //RectX = Canvas.GetLeft(this);
            //RectY = Canvas.GetTop(this);
            if (thumb.Cursor == Cursors.SizeAll)
            {
                RectX += e.HorizontalChange;
                RectY += e.VerticalChange;
                //Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                //Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
            }
            else if (thumb.Cursor == Cursors.SizeNWSE && thumb.HorizontalAlignment == HorizontalAlignment.Left)
            {
                this.RectWidth -= e.HorizontalChange;
                this.RectHeight -= e.VerticalChange;
                //Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                //Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
                RectX += e.HorizontalChange;
                RectY += e.VerticalChange;
            }
            else if (thumb.Cursor == Cursors.SizeNWSE && thumb.HorizontalAlignment == HorizontalAlignment.Right)
            {
                this.RectWidth += e.HorizontalChange;
                this.RectHeight += e.VerticalChange;
            }
            else if (thumb.Cursor == Cursors.SizeNS && thumb.VerticalAlignment == VerticalAlignment.Top)
            {
                this.RectHeight -= e.VerticalChange;
                //Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
                RectY += e.VerticalChange;
            }
            else if (thumb.Cursor == Cursors.SizeNS && thumb.VerticalAlignment == VerticalAlignment.Bottom)
            {
                this.RectHeight += e.VerticalChange;
            }
            else if (thumb.Cursor == Cursors.SizeNESW && thumb.HorizontalAlignment == HorizontalAlignment.Left)
            {
                this.RectWidth -= e.HorizontalChange;
                this.RectHeight += e.VerticalChange;
                //Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                RectX += e.HorizontalChange;
            }
            else if (thumb.Cursor == Cursors.SizeNESW && thumb.HorizontalAlignment == HorizontalAlignment.Right)
            {
                this.RectWidth += e.HorizontalChange;
                this.RectHeight -= e.VerticalChange;
                //Canvas.SetTop(this, Canvas.GetTop(this) + e.VerticalChange);
                RectY += e.VerticalChange;
            }
            else if (thumb.Cursor == Cursors.SizeWE && thumb.HorizontalAlignment == HorizontalAlignment.Left)
            {
                this.RectWidth -= e.HorizontalChange;
                //Canvas.SetLeft(this, Canvas.GetLeft(this) + e.HorizontalChange);
                RectX += e.HorizontalChange;
            }
            else if (thumb.Cursor == Cursors.SizeWE && thumb.HorizontalAlignment == HorizontalAlignment.Right)
            {
                this.RectWidth += e.HorizontalChange;
            }


        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {

        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //RectX = Canvas.GetLeft(this);
            //RectY = Canvas.GetTop(this);
        }
    }
}
