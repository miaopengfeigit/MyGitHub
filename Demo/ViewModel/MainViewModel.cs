
using Common;
using MaterialDesignThemes.Wpf;
using MvvmLight1.Controls;
using MvvmLight1.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            var view = new SampleDialog{ Message = { Text = "确定关闭程序？" }};
            
            await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();
            SR.ApplicationShutdown();
            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler
            
            //lets run a fake operation for 3 seconds then close this baby.
            Task.Delay(TimeSpan.FromSeconds(1))
                .ContinueWith((t, _) => { eventArgs.Session.Close(false); Application.Current.Shutdown(); }, null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}