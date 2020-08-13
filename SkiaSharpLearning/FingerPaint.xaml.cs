using System;
using System.Collections.Generic;
using SkiaSharp;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using System.Net.Http;
using System.IO;
using System.Linq;
using ColorPicker;

namespace SkiaSharpLearning
{
    public partial class FingerPaint : ContentPage
    {
        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        List<ContourPoints> completedPaths = new List<ContourPoints>();
        private SKBitmap webBitmap;
        private string selectedTool;
        private string enteredText;
        private bool isEntryFocused;

        private float x1;
        private float y1;
        private float x2;
        private float y2;

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Blue,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        SKPaint textPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3,
            TextSize = 34,            
            IsAntialias = true
        };

        public FingerPaint()
        {
            InitializeComponent();

            selectedTool = "Pen";            
            ColorWheel1.PropertyChanged += ColorWheel1_PropertyChanged;
        }

        private void ColorWheel1_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {            
            if(e.PropertyName == "SelectedColor")
            {
                paint.Color = ColorWheel1.SelectedColor.ToSKColor();
            }
        }        

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            HttpClient httpClient = new HttpClient();

            string url = "https://homepages.cae.wisc.edu/~ece533/images/airplane.png";
            try
            {
                using (Stream stream = await httpClient.GetStreamAsync(url))
                using (MemoryStream memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);

                    webBitmap = SKBitmap.Decode(memStream);
                    canvasView.InvalidateSurface();
                };
            }
            catch (Exception ex)
            {
            }
        }

        void canvasView_PaintSurface(System.Object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            var imageInfo = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;            

            canvas.Clear();            

            if (webBitmap != null)
                canvas.DrawBitmap(webBitmap, new SKRect(0, 0, imageInfo.Width, imageInfo.Height));

                     

            foreach (var contour in completedPaths)
            {
                paint.Color = contour.Color;
                if (contour.ToolType == "Pen")
                {                    
                    canvas.DrawPath(contour.Path, paint);
                }                    
                else if(contour.ToolType == "Box")
                {
                    x1 = contour.Path.Points.FirstOrDefault().X;
                    y1 = contour.Path.Points.FirstOrDefault().Y;

                    x2 = contour.Path.Points.Max(x => x.X);
                    y2 = contour.Path.Points.Max(y => y.Y);
                    
                    canvas.DrawRect(new SKRect(x1, y1, x2, y2), paint);
                }
                else if(contour.ToolType == "Text")
                {                    
                    if (contour.Text != null)
                    {                        
                        var x = contour.Path.Points.FirstOrDefault().X;
                        var y = contour.Path.Points.FirstOrDefault().Y;
                        textPaint.Color = contour.Color;
                        canvas.DrawText(contour.Text, x, y, textPaint);
                    }
                    else
                    {
                        if (Device.RuntimePlatform == Device.iOS)
                            Entry.Focus();
                    }                    
                }                
            }

            foreach(var path in inProgressPaths.Values)
            {
                canvas.DrawPath(path, paint);
            }                   
        }

        void TouchEffect_TouchAction(System.Object sender, SkiaSharpLearning.TouchActionEventArgs args)
        {
            paint.Color = ColorWheel1.SelectedColor.ToSKColor();

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if(!inProgressPaths.ContainsKey(args.Id))
                    {
                        if (selectedTool == "Text")
                        {
                            if (Device.RuntimePlatform == Device.iOS)
                                Entry.Unfocus();
                        }
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Moved:
                    if (inProgressPaths.ContainsKey(args.Id))
                    {
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        canvasView.InvalidateSurface();
                    }
                    break;
                case TouchActionType.Released:
                    if(inProgressPaths.ContainsKey(args.Id))
                    {                        
                        SKPath path = inProgressPaths[args.Id];
                        completedPaths.Add(
                            new ContourPoints { Path = path, ToolType = selectedTool,Color = ColorWheel1.SelectedColor.ToSKColor() });
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                        if (selectedTool == "Text")
                        {
                            if(Device.RuntimePlatform == Device.Android)
                                Entry.Focus();
                        }
                            
                    }
                    break;
                case TouchActionType.Cancelled:
                    if(inProgressPaths.ContainsKey(args.Id))
                    {
                        inProgressPaths.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;

            }
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {            
            inProgressPaths = new Dictionary<long, SKPath>();
            completedPaths = new List<ContourPoints>();
            canvasView.InvalidateSurface();
        }

        void SetBox(System.Object sender, System.EventArgs e)
        {
            selectedTool = "Box";
        }

        void SetPen(System.Object sender, System.EventArgs e)
        {
            selectedTool = "Pen";
        }

        void Undo(System.Object sender, System.EventArgs e)
        {
            if (completedPaths.Count == 0)
                return;

            completedPaths?.RemoveAt(completedPaths.Count - 1);
            canvasView.InvalidateSurface();
        }

        void SetText(System.Object sender, System.EventArgs e)
        {
            selectedTool = "Text";
        }

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            isEntryFocused = true;
        }

        void Entry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!isEntryFocused)
                return;

            enteredText = e.NewTextValue;
            var contour = completedPaths.Where(p => p.ToolType == "Text").LastOrDefault();
            if (contour == null)
                return;

            contour.Text = enteredText;

            canvasView.InvalidateSurface();
        }

        void Entry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            isEntryFocused = false;
            (sender as Entry).Text = string.Empty;
        }
    }
;
    public class ContourPoints
    {
        public SKPath Path { get; set; }

        public string ToolType { get; set; }

        public SKColor Color { get; set; }

        public string Text { get; set; }
    }
}
