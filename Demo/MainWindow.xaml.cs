using System.Windows;
using MvvmLight1.ViewModel;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MvvmLight1.Controls;
using Common;

namespace MvvmLight1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext =  new MainViewModel();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }
        
        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MenuPopupButtonMinimized_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MenuPopupButtonMaximized_OnClick(object sender, RoutedEventArgs e)
        {
            var bt = sender as System.Windows.Controls.Button;
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                bt.Content = SR.GetString("WindowStateMaximized");
            }
            else
            {
                WindowState = WindowState.Maximized;
                bt.Content = SR.GetString("WindowStateNormal");
            }
                
        }
    }
}