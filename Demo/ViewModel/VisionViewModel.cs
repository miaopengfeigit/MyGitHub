using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignThemes.Wpf;
using MaterialDesignColors;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Common;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Documents;
using MvvmLight1.Controls;

namespace MvvmLight1.ViewModel
{
    public class VisionViewModel : ViewModelBase
    {
        //#endregion
        public VisionViewModel()
        {
            //Application.Current.Exit += OnExit;
            SR.ApplicationExit += OnExit;
        }

        private void OnExit(object sender, EventArgs e)
        {
            stop = false;
        }


        bool stop = false;

        private RectangleControl selectPath = new RectangleControl();
        public RectangleControl SelectPath
        {
            get { return selectPath; }
            set { SetProperty(ref selectPath, value); }
        }

        private ObservableCollection<RectangleControl> canvasRectangle = new ObservableCollection<RectangleControl>();
        public ObservableCollection<RectangleControl> CanvasRectangle
        {
            get { return canvasRectangle; }
            set { canvasRectangle = value; }
        }
        private const string LOG_IDENTITY = "Vision";

        #region 属性



        private BitmapSource imgSourc;
        public BitmapSource ImgSourc
        {
            get { return imgSourc; }
            set { SetProperty(ref imgSourc, value); }
        }

        private BitmapSource roiSourc;
        public BitmapSource RoiSourc
        {
            get { return roiSourc; }
            set { SetProperty(ref roiSourc, value); }
        }

        private string str;
        public string Str
        {
            get { return str; }
            set { str = value; SetProperty(ref str, value); }
        }
        #endregion

        #region 命令
        public ICommand SourcePathCmd
        {
            get { return new DelegateCommand(o => ExcuteSourcePathCmd()); }
        }

        private void ExcuteSourcePathCmd()
        {
            string path = OpenFileDialog();
            if (!string.IsNullOrEmpty(path))
            {
                ImgSourc = Cv.ReadBitmapImage(path);
                NLog.Info(LOG_IDENTITY, "打开图片 {0}", path);
            }
        }

        public ICommand AddPathCmd
        {
            get { return new DelegateCommand(o => ExcuteAddPathCmd()); }
        }

        private void ExcuteAddPathCmd()
        {
            RectangleControl rc = new RectangleControl();
            rc.RectWidth = 100;
            rc.RectHeight = 100;
            rc.RectX = 100;
            rc.RectY = 100;
            CanvasRectangle.Add(rc);
            SelectPath = rc;
        }

        public ICommand OpenCameraCmd
        {
            get { return new DelegateCommand(o => ExcuteOpenCameraCmd()); }
        }

        private void ExcuteOpenCameraCmd()
        {
            Thread td = new Thread(ReadFrame);
            td.IsBackground = true;
            td.Start();
        }

        private void ReadFrame()
        {
            stop = true;
            if (Cv.OpenCamera(0))
            {
                while (stop)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ImgSourc = Cv.ReadBitmapFrame();
                    }));
                    Thread.Sleep(30);
                }
                Cv.ReleaseCamera();
            }
        }

        public ICommand StopCameraCmd
        {
            get { return new DelegateCommand(o => ExcuteStopCameraCmd()); }
        }

        private void ExcuteStopCameraCmd()
        {
            stop = false;
        }

        public ICommand RoiCmd
        {
            get { return new DelegateCommand(o => ExcuteRoiCmd()); }
        }

        private void ExcuteRoiCmd()
        {
            var canvas = (Canvas)VisualTreeHelper.GetParent(SelectPath);
            double w = ImgSourc.PixelWidth / canvas.ActualWidth;
            double h = ImgSourc.PixelHeight / canvas.ActualHeight;
            Rect rect = new Rect();
            rect.X = SelectPath.RectX * w;
            rect.Y = SelectPath.RectY * h;
            rect.Width = SelectPath.RectWidth * w;
            rect.Height = SelectPath.RectHeight * h;

            RoiSourc = Cv.ReadBitmapFrameROI(rect);
            CanvasRectangle.Clear();
        }

        public double Wi { get; set; }

        public double He { get; set; }

        public ICommand MatchTemplateCmd
        {
            get { return new DelegateCommand(o => ExcuteMatchTemplateCmd()); }
        }

        private void ExcuteMatchTemplateCmd()
        {
            Thread td = new Thread(ReadMatchTemplateFrame);
            td.IsBackground = true;
            td.Start();
        }

        private void ReadMatchTemplateFrame()
        {
            stop = true;
            if (Cv.OpenCamera(0))
            {
                while (stop)
                {
                    Thread.Sleep(30);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        ImgSourc = Cv.MatchTemplateFrame();
                    }));
                }
                Cv.ReleaseCamera();
            }
        }

        #endregion

        #region 方法

        private string OpenFileDialog()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Title = SR.GetString("Language");
            //dialog.IsFolderPicker = true;
            if (CommonFileDialogResult.Ok == dialog.ShowDialog())
                return dialog.FileName;
            else
                return string.Empty;
        }


        #endregion
    }


}
