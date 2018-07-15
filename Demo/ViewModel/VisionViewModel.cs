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
            SR.ApplicationExit += OnExit;
        }

        private void OnExit(object sender, EventArgs e)
        {
            stop = true;
        }
        
        public Canvas Canvas { get ; set; }
        bool stop = false;
        int mode = 0;

        private RectangleControl selectPath = new RectangleControl();
        public RectangleControl SelectPath
        {
            get { return selectPath; }
            set { SetProperty(ref selectPath, value); }
        }

        private ObservableCollection<UIElement> uiElements = new ObservableCollection<UIElement>();
        public ObservableCollection<UIElement> UIElements
        {
            get { return uiElements; }
            set { uiElements = value; }
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
            set { SetProperty(ref str, value); }
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
            mode = 0;
            if (Canvas==null)
            {
                var ue = new UIElement();
                UIElements.Add(ue);
                Canvas = (Canvas)VisualTreeHelper.GetParent(ue);
                UIElements.Clear();
            }
            

            Canvas.PreviewMouseLeftButtonDown += OnCanvasMouseLeftButtonDown;



        }

        private void OnCanvasMouseLeftButtonDown(object send, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(Canvas);
            RectangleControl rc = new RectangleControl
            {
                RectWidth = 0,
                RectHeight = 0,
                RectX = p.X,
                RectY = p.Y,
            };
            rc.PreviewMouseLeftButtonDown += OnMouseDown;
            UIElements.Add(rc);
            Canvas.PreviewMouseMove += OnCanvasMouseMove;
            Canvas.PreviewMouseLeftButtonUp += OnCanvasMouseLeftButtonUp;
            SelectPath = rc;
        }

        private void OnCanvasMouseMove(object send, MouseEventArgs e)
        {
            Point p = e.GetPosition(Canvas);
            var rc = UIElements.Last() as RectangleControl;
            rc.RectWidth = p.X - rc.RectX;
            rc.RectHeight = p.Y - rc.RectY;
        }

        private void OnCanvasMouseLeftButtonUp(object send, MouseButtonEventArgs e)
        {
            Canvas.PreviewMouseMove -= OnCanvasMouseMove;
            Canvas.PreviewMouseLeftButtonDown -= OnCanvasMouseLeftButtonDown;
            Canvas.PreviewMouseLeftButtonUp -= OnCanvasMouseLeftButtonUp;
        }

        private void OnMouseDown(object send, MouseButtonEventArgs e)
        {
            SelectPath = send as RectangleControl;
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
            stop = false;
            if (Cv.OpenCamera(0))
            {
                while (!stop)
                {
                    
                    DateTime beforDT = System.DateTime.Now;
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        switch (mode)
                        {
                            case 1:
                                ImgSourc = Cv.MatchTemplateFrame();
                                break;
                            default:
                                ImgSourc = Cv.ReadBitmapFrame();
                                break;
                        }
                        
                    }));
                    TimeSpan ts = DateTime.Now.Subtract(beforDT);

                    
                    int sleepDT = (int)(60 - ts.TotalMilliseconds);
                    Thread.Sleep(sleepDT > 0 ? sleepDT : 5);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Str = string.Format("FPS {1} 花费{0}ms ", ts.TotalMilliseconds,1000/ DateTime.Now.Subtract(beforDT).TotalMilliseconds);
                    }));
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
            stop = true;
        }

        public ICommand RoiCmd
        {
            get { return new DelegateCommand(o => ExcuteRoiCmd()); }
        }

        private void ExcuteRoiCmd()
        {
            //var canvas = (Canvas)VisualTreeHelper.GetParent(SelectPath);
            double w = ImgSourc.PixelWidth / Canvas.ActualWidth;
            double h = ImgSourc.PixelHeight / Canvas.ActualHeight;
            Rect rect = new Rect
            {
                X = SelectPath.RectX * w,
                Y = SelectPath.RectY * h,
                Width = SelectPath.RectWidth * w,
                Height = SelectPath.RectHeight * h
            };

            RoiSourc = Cv.ReadBitmapFrameROI(rect);
            
        }

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
            mode = 1;
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
