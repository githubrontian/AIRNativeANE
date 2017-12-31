using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace TuaRua.AIRNative.Display {
    internal class FreNativeSprite : Canvas {
        public FreNativeSprite(FREObject freObject) {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;

            X = freObject.GetProp("x").AsDouble();
            Y = freObject.GetProp("y").AsDouble();
            Visibility = freObject.GetProp("visible").AsBool()
                ? Visibility.Visible
                : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = freObject.GetProp("alpha").AsDouble();
        }

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
        /// <param name="child"></param>
        public void AddChild(UIElement child) {
            Children.Add(child);
        }

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
    }
}