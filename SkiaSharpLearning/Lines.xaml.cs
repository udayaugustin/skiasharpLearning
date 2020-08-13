using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpLearning
{
    public partial class Lines : ContentPage
    {
        private SKCanvasView canvasView;
        public Lines()
        {
            InitializeComponent();
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;

        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo imageInfo = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPath path = new SKPath();

            path.MoveTo(100, 100);
            path.LineTo(160, 100);
            path.LineTo(160, 150);
            path.LineTo(100, 150);

            var centerX = imageInfo.Width / 2;
            var centerY = imageInfo.Height / 2;
            path.MoveTo(centerX, centerY);
            path.LineTo(centerX - 50, centerY + 50);
            path.LineTo(centerX + 100, centerY + 50);
            path.Close();

            float[] array = { 10, 10 }; 
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Aqua.ToSKColor(),
                StrokeWidth = 40,
                PathEffect = SKPathEffect.CreateDash(array,10)
            };

            SKPaint fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor()
            };

            paint.StrokeJoin = SKStrokeJoin.Miter;
            canvas.DrawPath(path, paint);
            canvas.DrawPath(path, fillPaint);
        }
    }
}
