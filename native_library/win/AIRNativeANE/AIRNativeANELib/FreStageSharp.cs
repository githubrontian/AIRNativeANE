﻿using System;
using System.Windows.Interop;
using System.Windows.Media;
using TuaRua.FreSharp;
using TuaRua.FreSharp.Utils;
using Hwnd = System.IntPtr;
using FREObject = System.IntPtr;
using FREContext = System.IntPtr;
using Color = System.Windows.Media.Color;
using System.Windows;

namespace TuaRua.AIRNative {
    /// <summary>
    /// 
    /// </summary>
    public class FreStageSharp {
        private static Rect _viewPort;
        private static bool _visible;

        /// <summary>
        /// 
        /// </summary>
        public static bool Transparent;

        private static Hwnd _childWindow;
        private static Hwnd _airWindow;
        private static Visual _rootView;
        private static HwndSourceParameters _parameters;
        private static bool _isAdded;

        /// <summary>
        /// 
        /// </summary>
        public static Color BackgroundColor;

        /// <summary>
        /// 
        /// </summary>
        public enum FreNativeType {
            /// <summary>
            /// 
            /// </summary>
            Image = 0,

            /// <summary>
            /// 
            /// </summary>
            Button,

            /// <summary>
            /// 
            /// </summary>
            Sprite
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject Init(FREContext ctx, uint argc, FREObject[] argv) {
            var inFre0 = argv[0];
            var inFre1 = argv[1];
            var inFre2 = argv[2];
            var inFre3 = argv[3];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;
            if (inFre1 == FREObject.Zero) return FREObject.Zero;
            if (inFre2 == FREObject.Zero) return FREObject.Zero;
            if (inFre3 == FREObject.Zero) return FREObject.Zero;

            BackgroundColor = inFre3.AsColor();

            _viewPort = inFre0.AsRect();
            _visible = inFre1.AsBool();
            Transparent = inFre2.AsBool();
            _airWindow =
                System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle; //get the reference to the AIR Window


            _parameters = new HwndSourceParameters();
            _parameters.SetPosition(Convert.ToInt32(_viewPort.X), Convert.ToInt32(_viewPort.Y));
            _parameters.SetSize(Convert.ToInt32(_viewPort.Width), Convert.ToInt32(_viewPort.Height));
            _parameters.ParentWindow = _airWindow;
            _parameters.WindowName = "AIRNativeStageWindow";
            
            _parameters.WindowStyle = _visible
                ? (int) (WindowStyles.WS_CHILD | WindowStyles.WS_VISIBLE)
                : (int) WindowStyles.WS_CHILD;

            if (Transparent && WinApi.GetOsVersion().Item1 > 7) {
                _parameters.ExtendedWindowStyle = (int) WindowExStyles.WS_EX_LAYERED;
                _parameters.UsesPerPixelTransparency = true;
            }

            _parameters.AcquireHwndFocusInMenuMode = false;
            return FREObject.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject AddRoot(FREContext ctx, uint argc, FREObject[] argv) {
            if (_isAdded) return FREObject.Zero;
            var nativeRoot = new FreNativeRoot();
            if (!Transparent) {
                nativeRoot.Background = new SolidColorBrush(BackgroundColor);
            }
            _rootView = nativeRoot;
            var source = new HwndSource(_parameters) {RootVisual = _rootView};
            _childWindow = source.Handle;
            WinApi.RegisterTouchWindow(_childWindow, TouchWindowFlags.TWF_WANTPALM);

            nativeRoot.Init();
            _isAdded = true;
            return FREObject.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject OnFullScreen(FREContext ctx, uint argc, FREObject[] argv) {
            return FREObject.Zero;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject Restore(FREContext ctx, uint argc, FREObject[] argv) {
            return FREObject.Zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        public static FREObject Update(FREContext ctx, uint argc, FREObject[] argv) {
            var inFre0 = argv[0];
            if (inFre0 == FREObject.Zero) return FREObject.Zero;

            var inFre1 = argv[1];
            if (inFre1 == FREObject.Zero) return FREObject.Zero;

            var propName = inFre0.AsString();

            if (propName == "visible") {
                SetVisibility(inFre1.AsBool());
            }
            else if (propName == "viewPort") {
                SetViewPort(inFre1.AsRect());
            }

            return FREObject.Zero;
        }

        private static void SetViewPort(Rect rect) {
            const double tolerance = 0.000001;
            var tmpX = rect.X;
            var tmpY = rect.Y;
            var tmpWidth = rect.Width;
            var tmpHeight = rect.Height;

            var updateWidth = false;
            var updateHeight = false;
            var updateX = false;
            var updateY = false;

            if (Math.Abs(tmpWidth - _viewPort.Width) > tolerance) {
                _viewPort.Width = tmpWidth;
                updateWidth = true;
            }

            if (Math.Abs(tmpHeight - _viewPort.Height) > tolerance) {
                _viewPort.Height = tmpHeight;
                updateHeight = true;
            }

            if (Math.Abs(tmpX - _viewPort.X) > tolerance) {
                _viewPort.X = tmpX;
                updateX = true;
            }

            if (Math.Abs(tmpY - _viewPort.Y) > tolerance) {
                _viewPort.Y = tmpY;
                updateY = true;
            }

            if (!updateX && !updateY && !updateWidth && !updateHeight) return;
            var flgs = (WindowPositionFlags) 0;
            if (!updateWidth && !updateHeight) {
                flgs |= WindowPositionFlags.SWP_NOSIZE;
            }
            if (!updateX && !updateY) {
                flgs |= WindowPositionFlags.SWP_NOMOVE;
            }
            WinApi.SetWindowPos(_childWindow, new Hwnd(0), Convert.ToInt32(_viewPort.X), Convert.ToInt32(_viewPort.Y),
                Convert.ToInt32(_viewPort.Width), Convert.ToInt32(_viewPort.Height), flgs);
            WinApi.UpdateWindow(_childWindow);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public static void SetVisibility(bool value) {
            var existing = _visible;
            _visible = value;
            if (existing == _visible) return;
            WinApi.ShowWindow(_childWindow, _visible ? ShowWindowCommands.SW_SHOWNORMAL : ShowWindowCommands.SW_HIDE);
            WinApi.UpdateWindow(_childWindow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Visual GetRootView() {
            return _rootView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsAdded() {
            return _isAdded;
        }
    }
}