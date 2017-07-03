using System.Windows;

namespace TuaRua.AIRNative {
    public partial class FreNativeRoot {
        /// <summary>
        /// 
        /// </summary>
        public void Init() {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(UIElement child) {
            MainGrid.Children.Add(child);
        }
    }
}