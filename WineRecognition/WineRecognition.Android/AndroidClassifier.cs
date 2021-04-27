using Android.Content.Res;

using Xamarin.Forms;

using Ctie.Dmf.Tf_lite_yolov4_library;
using static Ctie.Dmf.Tf_lite_yolov4_library.YoloV4Classifier;
using Java.Lang;
using Java.Nio;
using Java.IO;
using Java.Nio.Channels;
using Android.Graphics;
using System.Collections.Generic;

namespace WineRecognition.Droid
{
    public class AndroidClassifier : IClassifier
    {
        public List<DetectedObject> identify(byte[] bytes)
        {
            AssetManager assets = Android.App.Application.Context.Assets;
            var model = YoloV4Classifier.Create(assets, getModel(assets, "yolov4-tiny-416.tflite"), "coco.names", true) ;
            IList<ClassifierRecognition> result = model.RecognizeImage(getImage(bytes));
            var s = "";
            List<DetectedObject> detectedObjects = new List<DetectedObject>();
            foreach (ClassifierRecognition cr in result)
            {
                s += cr.Id + "\n";
                s += cr.Title + "\n";
                s += cr.Confidence.ToString() + "\n";
                s += cr.Location.ToShortString()+"\n";
                s += cr.Location.Top + "\n";
                s += cr.Location.Right + "\n";
                s += cr.Location.Left + "\n";
                s += cr.Location.Bottom + "\n";
                s += cr.Location.Width().ToString() + "\n";
                s += cr.Location.Height().ToString() + "\n";
                detectedObjects.Add(new DetectedObject(
                    cr.Title.ToString(),
                    cr.Location.Top/416,
                    cr.Location.Bottom/416,
                    cr.Location.Left/416, 
                    cr.Location.Right/416, 
                    cr.Confidence.FloatValue()/416,
                    cr.Location.Width()/416,
                    cr.Location.Height()/416
                    ));
            }
            return detectedObjects;
        }

        private MappedByteBuffer getModel(AssetManager assets, string modelFilename)
        {
            AssetFileDescriptor fileDescriptor = assets.OpenFd(modelFilename);
            FileInputStream inputStream = new FileInputStream(fileDescriptor.FileDescriptor);
            FileChannel fileChannel = inputStream.Channel;
            long startOffset = fileDescriptor.StartOffset;
            long declaredLength = fileDescriptor.DeclaredLength;
            return fileChannel.Map(FileChannel.MapMode.ReadOnly, startOffset, declaredLength);
        }

        private Bitmap getImage(byte[] bytes)
        {
            var imageBitmap = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(imageBitmap, (int)416, (int)416, true);
            imageBitmap.Recycle();
            return resizedImage;
        }

    }
}