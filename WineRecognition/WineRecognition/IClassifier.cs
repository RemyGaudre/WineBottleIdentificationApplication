using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace WineRecognition
{
    public interface IClassifier
    {
        List<DetectedObject> identify(byte[] bytes);
    }

    public class DetectedObject
    {
        public DetectedObject(String Type, float Top, float Bottom, float Left, float Right, float Confidence, float Width, float Height)
        {
            this.Type = Type;
            this.Top = Top;
            this.Bottom = Bottom;
            this.Left = Left;
            this.Right = Right;
            this.Confidence = Confidence;
            this.Width = Width;
            this.Height = Height;
        }
        public String Type { set; get; }
        public float Top { set; get; }
        public float Bottom { set; get; }
        public float Left { set; get; }
        public float Right { set; get; }
        public float Confidence { set; get; }
        public float Width { set; get; }
        public float Height { set; get; }

        override public String ToString()
        {
            return "[" + Type + "," + Top + "," + Bottom + "," + Left + "," + Right + "," + Confidence + "," + Width + "," + Height + "," + "]";
        }
    }
}
