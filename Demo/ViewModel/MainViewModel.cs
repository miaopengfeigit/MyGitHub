
using Common;
using MaterialDesignThemes.Wpf;
using MvvmLight1.Controls;
using MvvmLight1.Model;
using System.Windows;
using System.Windows.Input;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DemoItems = new DemoItem[]
            {
                 new DemoItem("Home", new View.Home(), PackIconKind.Home),
                 new DemoItem("Palette", new View.PaletteSelector(), PackIconKind.Palette),
                 new DemoItem("ConvertFile", new View.ConvertFile(), PackIconKind.Key),
                 new DemoItem("Vision", new View.Vision(), PackIconKind.Video),
            };
            ChangeUI();
            SR.LanguageChanged += (s, e) => ChangeUI();
        }

        public DemoItem[] DemoItems { get; set; }

        public string ShowString { get; set; }

        private void ChangeUI()
        {
            foreach (var item in DemoItems)
            {
                item.DisplayName = SR.GetString(item.Name);
            }
        }

        public ICommand ClosingCommand => new DelegateCommand(ClosingWindows);
        
        private async void ClosingWindows(object o)
        {

            //var sampleMessageDialog = new SampleMessageDialog
            //{
            //    Message = { Text = ((ButtonBase)sender).Content.ToString() }
            //};
            //await DialogHost.Show(sampleMessageDialog, "RootDialog");
            ShowString = "确定关闭程序？";
            var view = new SampleDialog
            {
                DataContext = this
            };
            
            var result = await DialogHost.Show(view, "RootDialog");
            if((bool)result) Application.Current.Shutdown();
        }

        
    }
}