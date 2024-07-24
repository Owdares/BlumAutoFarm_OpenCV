using BlumAutoFarm.Models;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    public static class MatDrawingExtension
    {
        public static void DrawingClickPosInfo(this Mat mat, int pressPosX, int pressPosY, int releasePosX, int releasePosY)
        {
            string pressText = $"Press: ({pressPosX}, {pressPosY})";
            string releaseText = $"Release: ({releasePosX}, {releasePosY})";

            double distance = Math.Sqrt(Math.Pow(releasePosX - pressPosX, 2) + Math.Pow(releasePosY - pressPosY, 2));
            string distanceText = $"Distance: {distance:F2}";

            Cv2.PutText(mat, distanceText, new OpenCvSharp.Point(pressPosX, pressPosY - 10), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.White, 1);
            Cv2.PutText(mat, pressText, new OpenCvSharp.Point(pressPosX, pressPosY - 30), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.White, 1);
            Cv2.PutText(mat, releaseText, new OpenCvSharp.Point(pressPosX, pressPosY - 50), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.White, 1);
        }
        public static void DrawingClickPosTags(this Mat mat, int pressPosX, int pressPosY, int releasePosX, int releasePosY)
        {
            Cv2.Line(mat, new OpenCvSharp.Point(pressPosX, pressPosY), new OpenCvSharp.Point(releasePosX, releasePosY), ColorScalars_RGB.Black, 4);
            Cv2.Circle(mat, new OpenCvSharp.Point(releasePosX, releasePosY), 4, ColorScalars_RGB.Blue, -1);
            Cv2.Circle(mat, new OpenCvSharp.Point(pressPosX, pressPosY), 4, ColorScalars_RGB.Red, -1);
        }
        public static void ObjectInfo(this Mat mat, TrackedObjectModel trackedObject)
        {
            string textId = $"ID - {trackedObject.Id}";
            string textIsBaned = $"IsBaned - {trackedObject.IsBanned}";
            string pressText = $"Click";

            Cv2.PutText(mat, textId, new OpenCvSharp.Point(trackedObject.Roi.X, trackedObject.Roi.Y - 10), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.White, 1);
            Cv2.PutText(mat, textIsBaned, new OpenCvSharp.Point(trackedObject.Roi.X, trackedObject.Roi.Y - 30), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.White, 1);
            
            if(trackedObject.IsClicked)
                Cv2.PutText(mat, pressText, new OpenCvSharp.Point(trackedObject.Roi.X, trackedObject.Roi.Y + 20), HersheyFonts.HersheySimplex, 0.5, ColorScalars_RGB.Blue, 2);
        }
        public static void ClickInfo(this Mat mat, int pressPosX, int pressPosY)
        {
            string pressText = $"Click";

            Cv2.PutText(mat, pressText, new OpenCvSharp.Point(pressPosX, pressPosY), HersheyFonts.HersheySimplex, 1, ColorScalars_RGB.Blue, 2);
        }
        public static void ObjectRectagle(this Mat mat, TrackedObjectModel trackedObject)
        {
            var scalar = ColorScalars_RGB.Green;
            if (trackedObject.IsBanned)
            {
                scalar = ColorScalars_RGB.Red;
            }

            Cv2.Rectangle(mat, trackedObject.Roi, scalar, 2);
        }
        public static void ObjectRectagle(this Mat mat, DetectedObjectModel detectedObject)
        {
            var scalar = ColorScalars_RGB.Green;

            Cv2.Rectangle(mat, detectedObject.Roi, scalar, 2);
        }
        public static void DrawClickInfo(this Mat mat, int pressPosX, int pressPosY, int releasePosX, int releasePosY)
        {
            mat.DrawingClickPosTags(pressPosX, pressPosY, releasePosX, releasePosY);
            mat.DrawingClickPosInfo(pressPosX, pressPosY, releasePosX, releasePosY);
        }
    }
}
