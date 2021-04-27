using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using SkiaSharp;

namespace WineRecognition
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        byte[] bytes;
        List<DetectedObject> result;
        public ImagePage(Stream stream)
        {
            InitializeComponent();
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
            resultImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
        }
        public ImagePage(MediaFile file)
        {
            InitializeComponent();
            Stream stream = file.GetStreamWithImageRotatedForExternalStorage();
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            bytes = memoryStream.ToArray();
            resultImage.Source = ImageSource.FromStream(() => file.GetStream());
        }
        private void goDetect_Clicked(object sender, EventArgs e)
        {
            int width=0;
            int height=0;
            if (Device.RuntimePlatform == "Android")
            {
                Label_test.Text = "Run on Android";
                result = App.Classifier.identify(bytes);
                Android.Graphics.Bitmap originalImage = Android.Graphics.BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                width = originalImage.Width;
                height = originalImage.Height;              
            }

            if(Device.RuntimePlatform == "iOS")
            {
                Label_test.Text = "Run on iOS";
            }


            if (width != 0 && height != 0)
            {
                SKBitmap skbitmap = getBitmap(bytes, width, height);
                SKCanvas canvas = new SKCanvas(skbitmap);
                // draw a rectangle with a red border
                SKPaint paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Red,
                    StrokeWidth = 5
                };
                foreach(DetectedObject o in result)
                {
                    canvas.DrawRect(SKRect.Create(o.Left * width, o.Top * height, o.Width * width, o.Height * height), paint);
                }
                var image = SKImage.FromBitmap(skbitmap);

                // encode the image
                SKData encodedData = image.Encode();

                // get a stream that can be saved to disk/memory/etc
                var stream = encodedData.AsStream();
                resultImage.Source = ImageSource.FromStream(() => stream);
            }
        }

        private SKBitmap getBitmap(byte[] bytes, int width, int height)
        {
            return SKBitmap.Decode(bytes, new SKImageInfo(width, height));
        }

    }
    

}