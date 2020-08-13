using System;
using System.Collections.Generic;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Net.Http;
using System.IO;

namespace SkiaSharpLearning
{
    public partial class Bitmap : ContentPage
    {
        private SKCanvasView canvasView;
        private SKBitmap webBitmap;

        public Bitmap()
        {
            InitializeComponent();

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += CanvasView_PaintSurface;
            Content = canvasView;
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
            catch(Exception ex)
            {
            }
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo imageInfo = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            if(webBitmap != null)
                canvas.DrawBitmap(webBitmap, new SKRect(0,0, imageInfo.Width, imageInfo.Height));

            var linePaint = new SKPaint
            {
                Color = Color.Green.ToSKColor(),
                StrokeWidth = 40,
                StrokeCap = SKStrokeCap.Round
            };
            canvas.DrawLine(100, 100, 100, 200, linePaint);
        }
    }
}
