using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SkiaSharpLearning
{
    public partial class ColorPicker : ContentPage
    {
        public ColorPicker()
        {
            InitializeComponent();
            var selectedColor = ColorWheel1.SelectedColor;
        }
    }
}
