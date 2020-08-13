using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpLearning
{
    public partial class PathFillPages : ContentPage
    {
        private SKCanvasView canvasView;

        public PathFillPages()
        {
            InitializeComponent();
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPoint center = new SKPoint(info.Width / 2, info.Height / 2);
            float radius = 0.45f * Math.Min(info.Width, info.Height);

            SKPath path = new SKPath
            {
                FillType = SKPathFillType.EvenOdd
            };
            path.MoveTo(info.Width / 2, info.Height / 2 - radius);

            for (int i = 1; i < 5; i++)
            {
                // angle from vertical
                double angle = i * 4 * Math.PI / 5;
                path.LineTo(center + new SKPoint(radius * (float)Math.Sin(angle),
                                                -radius * (float)Math.Cos(angle)));
            }
            path.Close();

            SKPaint strokePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeWidth = 50,
                StrokeJoin = SKStrokeJoin.Round
            };

            SKPaint fillPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Blue
            };

            canvas.DrawPath(path, fillPaint);
            canvas.DrawPath(path, strokePaint);

            SKPaint circlePaint = new SKPaint
            {
                StrokeWidth = 10,
                Color = Color.Red.ToSKColor(),
                Style = SKPaintStyle.Fill
            };

            SKPath path1 = new SKPath();
            path1.FillType = SKPathFillType.EvenOdd;
            path1.AddCircle(250, 250, 100);
            path1.AddCircle(350, 250, 100);
            path1.AddCircle(250, 350, 100);
            

            canvas.DrawPath(path1, circlePaint);
        }
    }
}
