using System;
using System.Windows;
using System.Windows.Media;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Utils;
using Image = System.Windows.Controls.Image;
using FREObject = System.IntPtr;

namespace TuaRua.AIRNative.Display {
    /// <summary>
    /// 
    /// </summary>
    public class FreNativeImage : Image {
        /// <summary>
        /// 
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Y { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public FreNativeImage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public void Update(FREObject prop, FREObject value) {
            var propName = Convert.ToString(new FreObjectSharp(prop).Value);
            if (propName == "x") {
                X = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "y") {
                Y = Convert.ToDouble(new FreObjectSharp(value).Value);
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "alpha") {
                Opacity = Convert.ToDouble(new FreObjectSharp(value).Value);
            }
            else if (propName == "visible") {
                Visibility = Convert.ToBoolean(new FreObjectSharp(value).Value)
                    ? Visibility.Visible
                    : Visibility.Hidden;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public FreNativeImage(FREObject freObject) {
            var bitmap = new FreBitmapDataSharp(freObject.GetProp("bitmapData")).GetAsBitmap();
            Width = bitmap.Width;
            Height = bitmap.Height;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Source = BitmapUtils.BitmapToSource(bitmap);

            X = freObject.GetProp("x").AsDouble();
            Y = freObject.GetProp("y").AsDouble();
            Visibility = freObject.GetProp("visible").AsBool()
                ? Visibility.Visible
                : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = freObject.GetProp("alpha").AsDouble();
        }
    }
}