using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using NHotkey;
using NHotkey.WindowsForms;
using BlumAutoFarm;

class Program
{
    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out OpenCvSharp.Point lpPoint);

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);


    public static bool running = true;
    public static bool pause = true;

    

    public static Random Random = new Random();

    static void Main(string[] args)
    {
        try
        {
            HotkeyManager.Current.AddOrReplace("Exit", Keys.L, HotkeyExitPressed);
            HotkeyManager.Current.AddOrReplace("Pause", Keys.K, HotkeyPausePressed);
        }
        catch (HotkeyAlreadyRegisteredException ex)
        {
            Console.WriteLine("Hotkeys already registered");
            Application.Exit();
        }

        Task.Run(() => CaptureFullScreen());

        Application.Run();
    }

    private static void CaptureFullScreen()
    {
        using var window = new Window("BlumAutoFram");

        var tracker = new BlumAutoFarm.Tracker();

        while (running)
        {
            using (var screanBmp = ScreenEngine.CaptureScreen())
            using (var screenMat = BitmapConverter.ToMat(screanBmp))
            {

                var detectedButtonRestart = DetectObjects.DetectButtonRestart(screenMat);

                if (detectedButtonRestart != null && !pause)
                {
                    Clicker.PerformClickWithoutMovement(screenMat, detectedButtonRestart.Roi);

                    screenMat.ObjectRectagle(detectedButtonRestart);

                    Console.WriteLine("Click on PLAY button: RESTART FARM");

                    ScreenEngine.ShowImage(window, screenMat, ScreenEngine.WindowScreenSize);

                    Thread.Sleep(1000);

                    continue;
                }

                var detectedObjects = DetectObjects.DetectBlum(screenMat);
                tracker.ObjectVerification(detectedObjects);

                Clicker.ExecuteClickLogicForLastThreeObjects(screenMat, tracker);

                ScreenEngine.ShowImage(window, screenMat, ScreenEngine.WindowScreenSize);

                Thread.Sleep(150);
            }
        }

        Application.Exit();
    }
    private static void HotkeyPausePressed(object sender, HotkeyEventArgs e)
    {
        pause = !pause;
        Console.WriteLine("Hot key is press, pause program...");
    }
    private static void HotkeyExitPressed(object sender, HotkeyEventArgs e)
    {
        running = false;
        Console.WriteLine("Hot key is press, stop program...");
    }
}
