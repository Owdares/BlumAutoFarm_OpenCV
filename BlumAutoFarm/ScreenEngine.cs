using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    internal class ScreenEngine
    {
        public static OpenCvSharp.Size CaptureScreenSize = new OpenCvSharp.Size(1920, 400);
        public static OpenCvSharp.Size WindowScreenSize = new OpenCvSharp.Size(1920, 400);

        public static int CaptureScreenSize_X = (Screen.PrimaryScreen.Bounds.Width - CaptureScreenSize.Width) / 2;
        public static int CaptureScreenSize_Y = (Screen.PrimaryScreen.Bounds.Height - CaptureScreenSize.Height) / 2;

        public static Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            int startX = (bounds.Width - ScreenEngine.CaptureScreenSize.Width) / 2;
            int startY = (bounds.Height - ScreenEngine.CaptureScreenSize.Height) / 2;

            Bitmap bitmap = new Bitmap(ScreenEngine.CaptureScreenSize.Width, ScreenEngine.CaptureScreenSize.Height);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(startX, startY, 0, 0, new System.Drawing.Size(ScreenEngine.CaptureScreenSize.Width, ScreenEngine.CaptureScreenSize.Height));
            }

            return bitmap;
        }
        public static void ShowImage(Window window, Mat mat, OpenCvSharp.Size size)
        {
            using var resizedImage = new Mat();
            Cv2.Resize(mat, resizedImage, size);

            window.ShowImage(resizedImage);
            Cv2.WaitKey(1);
        }
    }
}
