using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SkiaSharpLearning
{
    public partial class AnimationPage : ContentPage
    {
        private SKCanvasView canvasView;
        private SKPaint paint;
        private int noOfCircles;
        Stopwatch stopwatch = new Stopwatch();


        public AnimationPage()
        {
            InitializeComponent();

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;

            paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 20
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            stopwatch.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
            {
                noOfCircles++;

                canvasView.InvalidateSurface();

                if (noOfCircles >= 5)
                    noOfCircles = 0;

                return true;
            });
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo imageInfo = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            var baseRadius = 60;

            for(int i=1; i <= noOfCircles; i++)
            {
                var radius = baseRadius * i;
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, radius, paint);                
            }
        }
    }
}
