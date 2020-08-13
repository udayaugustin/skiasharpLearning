using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Threading.Tasks;

namespace SkiaSharpLearning
{
    public partial class TappToFill : ContentPage
    {
        private bool showFill = true;
        private SKCanvasView canvasView;

        public TappToFill()
        {
            InitializeComponent();

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += TapGesture_Tapped;

            canvasView.GestureRecognizers.Add(tapGesture);

            Content = canvasView;
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            showFill = !showFill;
            (sender as SKCanvasView).InvalidateSurface();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            while (true)
            {
                showFill = !showFill;
                canvasView.InvalidateSurface();
                await Task.Delay(2000);
            }
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo imageInfo = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint outerLine = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 10
            };
            canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, 100, outerLine);

            if (!showFill)
                return;

            SKPaint innerLine = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Blue.ToSKColor()              
            };
            canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, 95, innerLine);

            var font = new SKPaint
            {
                Color = SKColors.Red,
                TextSize = 45
            };
            canvas.DrawText("Test", imageInfo.Width/2+100, imageInfo.Height/2, font);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Blue,
                StrokeWidth = 20
            };
            canvas.DrawOval(imageInfo.Width / 2, imageInfo.Height / 2, imageInfo.Width / 2-10, imageInfo.Height / 2 -10, paint);
        }
    }
}
