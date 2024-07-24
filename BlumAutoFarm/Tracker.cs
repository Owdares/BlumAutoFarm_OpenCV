using BlumAutoFarm.Models;
using OpenCvSharp;
using OpenCvSharp.Tracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    public class Tracker
    {
        public List<TrackedObjectModel> TrackedObjects = new List<TrackedObjectModel>();
        public int ObjectId = 1;

        public void ObjectVerification(List<DetectedObjectModel> detectedObjects)
        {
            foreach (var trackedObject in TrackedObjects)
            {
                trackedObject.MissingFrames = 2;
            }

            foreach (var detectedObject in detectedObjects)
            {
                var newObjectCenter = new Point2f(detectedObject.Roi.X + detectedObject.Roi.Width / 2, detectedObject.Roi.Y + detectedObject.Roi.Height / 2);

                var closestSavedObj = TrackedObjects
                   .Select(x => new { Object = x, Center = new Point2f(x.Roi.X + x.Roi.Width / 2, x.Roi.Y + x.Roi.Height / 2) })
                   .Where(x => Distance(newObjectCenter, x.Center) <= 50 &&
                               Math.Abs(x.Object.Roi.Width - detectedObject.Roi.Width) <= 10 &&
                               Math.Abs(x.Object.Roi.Height - detectedObject.Roi.Height) <= 10)
                   .OrderBy(x => Distance(newObjectCenter, x.Center))
                   .FirstOrDefault();

                if (closestSavedObj == null)
                {
                    Add(detectedObject);
                }
                else
                {
                    Update(closestSavedObj.Object, detectedObject.Roi);
                }
            }

            ObjectsCleaner();
        }

        private double Distance(Point2f p1, Point2f p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public void Add(DetectedObjectModel detectedObject)
        {
            TrackedObjects.Add(new TrackedObjectModel
            {
                Id = ObjectId,
                Roi = detectedObject.Roi,
                MissingFrames = 0,
                Countours = detectedObject.Countours,
                IsBanned = Program.Random.Next(1, 11) <= 3,
            });

            ObjectId++;
        }

        public void Update(TrackedObjectModel trackedObject, Rect newRect)
        {
            trackedObject.Roi = newRect;
            trackedObject.MissingFrames = 0;
        }

        public void ObjectsCleaner()
        {
            TrackedObjects.RemoveAll(x => x.MissingFrames >= 2);
        }
    }
}
