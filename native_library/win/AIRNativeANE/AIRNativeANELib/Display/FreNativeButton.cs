using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Utils;
using Image = System.Windows.Controls.Image;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;

namespace TuaRua.AIRNative.Display {
    /// <summary>
    /// 
    /// </summary>
    public class FreNativeButton : Image {
        /// <summary>
        /// 
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Y { get; set; }

        private readonly BitmapSource _upState;
        private readonly BitmapSource _overState;
        private readonly BitmapSource _downState;

        private readonly string _id;
        private const string AsCallbackEvent = "TRFRESHARP.as.CALLBACK";
        private FREContext _ctx;

        /// <summary>
        /// 
        /// </summary>
        public FreNativeButton() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="freObject"></param>
        /// <param name="id"></param>
        /// <param name="ctx"></param>
        public FreNativeButton(FREObject freObject, string id, ref FREContext ctx) {
            _ctx = ctx;
            _upState = BitmapUtils.BitmapToSource(
                new FreBitmapDataSharp(freObject.GetProp("upStateData")).GetAsBitmap());
            _overState =
                BitmapUtils.BitmapToSource(new FreBitmapDataSharp(freObject.GetProp("overStateData"))
                    .GetAsBitmap());
            _downState =
                BitmapUtils.BitmapToSource(new FreBitmapDataSharp(freObject.GetProp("downStateData"))
                    .GetAsBitmap());
            _id = id;

            Width = _upState.Width;
            Height = _upState.Height;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Source = _upState;
            MouseEnter += Button_MouseEnter;
            MouseLeave += Button_MouseLeave;
            MouseDown += Button_MouseDown;
            MouseUp += Button_MouseUp;

            X = freObject.GetProp("x").AsDouble();
            Y = freObject.GetProp("y").AsDouble();
            Visibility = freObject.GetProp("visible").AsBool() ? Visibility.Visible : Visibility.Hidden;
            RenderTransform = new TranslateTransform(X, Y);
            Opacity = freObject.GetProp("alpha").AsDouble();
        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseUp\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);

            sf = $"{{\"id\": \"{_id}\", \"event\": \"click\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);

            Source = _overState;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseDown\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _downState;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseOver\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _overState;
            Cursor = Cursors.Hand;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            var sf = $"{{\"id\": \"{_id}\", \"event\": \"mouseOut\"}}";
            FreSharpHelper.DispatchEvent(ref _ctx, AsCallbackEvent, sf);
            Source = _upState;
            Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="value"></param>
        public void Update(FREObject prop, FREObject value) {
            var propName = prop.AsString();

            if (propName == "x") {
                X = value.GetProp("x").AsDouble();
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "y") {
                X = value.GetProp("y").AsDouble();
                RenderTransform = new TranslateTransform(X, Y);
            }
            else if (propName == "alpha") {
                Opacity = value.GetProp("alpha").AsDouble();
            }
            else if (propName == "visible") {
                Visibility = value.GetProp("visible").AsBool() ? Visibility.Visible : Visibility.Hidden;
            }
        }
    }
}