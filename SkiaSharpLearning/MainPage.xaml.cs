using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpLearning
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo imageInfo = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25,
                IsAntialias = true
            };

            canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, 100, paint);

            paint.Color = Color.Blue.ToSKColor();
            paint.Style = SKPaintStyle.StrokeAndFill;
            canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, 75, paint);
        }
    }
}
