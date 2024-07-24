using BlumAutoFarm.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    public static class DetectObjects
    {
        public static List<DetectedObjectModel> DetectBlum(Mat screenMat)
        {
            using var hsvMat = new Mat();
            using var maskOneStunGreen = new Mat();
            using var maskTwoStunGreen = new Mat();
            using var maskCombined = new Mat();

            Cv2.CvtColor(screenMat, hsvMat, ColorConversionCodes.BGR2RGB);

            Cv2.InRange(hsvMat, ColorScalars_RGB.GreenOneStunBlum_Lower, ColorScalars_RGB.GreenOneStunBlum_Upper, maskOneStunGreen);
            Cv2.InRange(hsvMat, ColorScalars_RGB.GreenTwoStunBlum_Lower, ColorScalars_RGB.GreenTwoStunBlum_Upper, maskTwoStunGreen);

            Cv2.BitwiseOr(maskOneStunGreen, maskTwoStunGreen, maskCombined);

            var kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(4, 4));
            Cv2.MorphologyEx(maskCombined, maskCombined, MorphTypes.Close, kernel);
            Cv2.MorphologyEx(maskCombined, maskCombined, MorphTypes.Open, kernel);

            var contours = Cv2.FindContoursAsArray(maskCombined, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            var detectedObjects = new List<DetectedObjectModel>();

            foreach (var contour in contours)
            {
                if (!Program.running) break;

                var rect = Cv2.BoundingRect(contour);

                if (rect.Width >= 15 || rect.Height >= 15)
                {
                    detectedObjects.Add(new DetectedObjectModel()
                    {
                        Countours = contour,
                        Roi = rect
                    });
                }
            }

            return detectedObjects;
        }
        public static DetectedObjectModel? DetectButtonRestart(Mat screenMat)
        {
            using var hsvMat = new Mat();
            using var mask = new Mat();

            Cv2.CvtColor(screenMat, hsvMat, ColorConversionCodes.BGR2RGB);

            Cv2.InRange(hsvMat, ColorScalars_RGB.White, ColorScalars_RGB.White, mask);

            var contours = Cv2.FindContoursAsArray(mask, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            
            foreach (var contour in contours)
            {
                var roi = Cv2.BoundingRect(contour);
                if (roi.Height > 30 && roi.Width > 30)
                {
                    return new DetectedObjectModel { Roi = roi, Countours = contour };
                }
            }

            return null;
        }
    }
}

