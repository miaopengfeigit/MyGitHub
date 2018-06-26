using MaterialDesignThemes.Wpf;
using MvvmLight1.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MvvmLight1.Model
{
    public class DemoItem: ViewModelBase
    {
        private PackIconKind _icon;
        private string _name;
        private string _displayName;
        private object _content;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;
        private Thickness _marginRequirement = new Thickness(16);
        
        public DemoItem(string name, object content, PackIconKind icon)
        {
            _icon = icon;
            _name = name;
            Content = content;
        }
        
        public PackIconKind Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }

        public object Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set { SetProperty(ref _horizontalScrollBarVisibilityRequirement, value); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set { SetProperty(ref _verticalScrollBarVisibilityRequirement, value); }
        }

        public Thickness MarginRequirement
        {
            get { return _marginRequirement; }
            set { SetProperty(ref _marginRequirement, value); }
        }

        ////public IEnumerable<DocumentationLink> Documentation { get; }

        //public event PropertyChangedEventHandler PropertyChanged;

        //private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        //{
        //    return args => PropertyChanged?.Invoke(this, args);
        //}
    }
}
