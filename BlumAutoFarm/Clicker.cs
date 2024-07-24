using OpenCvSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    public static class Clicker
    {
        public const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        public const uint MOUSEEVENTF_LEFTUP = 0x04;

        public static void ExecuteClickLogicForLastThreeObjects(Mat mat, Tracker tracker)
        {
            var lastThreeObjects = tracker.TrackedObjects.TakeLast(3);
            foreach (var trackedObject in lastThreeObjects)
            {
                mat.ObjectRectagle(trackedObject);
                if (!trackedObject.IsBanned && !trackedObject.IsClicked && !Program.pause)
                {
                    PerformClickWithMovement(mat, trackedObject.Roi);
                    trackedObject.IsClicked = true;
                    Console.WriteLine($"Clicked on object: {trackedObject.Id}");
                }
            }
        }

        public static void PerformClickWithMovement(Mat mat, Rect rect)
        {
            var (pressPosX, pressPosY) = GetRandomPressPosition(rect);
            PressClick(pressPosX, pressPosY);

            Task.Delay(Program.Random.Next(42, 76)).Wait();

            var (releasePosX, releasePosY) = GetRandomReleasePosition(pressPosX, pressPosY);
            ReleaseClick(releasePosX, releasePosY);

            MatDrawingExtension.DrawClickInfo(mat, pressPosX, pressPosY, releasePosX, releasePosY);
        }

        public static void PerformClickWithoutMovement(Mat mat, Rect rect)
        {
            var (pressPosX, pressPosY) = GetRandomPressPosition(rect);
            PressClick(pressPosX, pressPosY);

            Task.Delay(Program.Random.Next(42, 76)).Wait();

            ReleaseClick(pressPosX, pressPosY);

            MatDrawingExtension.DrawClickInfo(mat, pressPosX, pressPosY, pressPosX, pressPosY);
        }

        private static (int posX, int posY) GetRandomPressPosition(Rect rect)
        {
            var posX = rect.X + Program.Random.Next(rect.Width) + ScreenEngine.CaptureScreenSize_X;
            var posY = rect.Y + rect.Height - 1 + ScreenEngine.CaptureScreenSize_Y;
            return (posX, posY);
        }

        private static (int posX, int posY) GetRandomReleasePosition(int pressPosX, int pressPosY)
        {
            var posX = pressPosX + Program.Random.Next(-5, 5);
            var posY = pressPosY - Program.Random.Next(5);
            return (posX, posY);
        }

        private static void PressClick(int posX, int posY)
        {
            Program.SetCursorPos(posX, posY);
            Program.mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)posX, (uint)posY, 0, UIntPtr.Zero);
        }

        private static void ReleaseClick(int posX, int posY)
        {
            Program.SetCursorPos(posX, posY);
            Program.mouse_event(MOUSEEVENTF_LEFTUP, (uint)posX, (uint)posY, 0, UIntPtr.Zero);
        }
    }
}
