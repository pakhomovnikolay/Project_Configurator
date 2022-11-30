using System.Windows;
using System.Windows.Input;

namespace Project_Сonfigurator.Views.Windows
{
    public partial class SettingWindow
    {
        public SettingWindow() => InitializeComponent();

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Top = e.GetPosition(this).Y / 2;
                    DragMove();
                }
                else
                    DragMove();
            }
        }
    }
}
