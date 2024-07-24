using OpenCvSharp.Tracking;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm.Models
{
    public class TrackedObjectModel
    {
        public int Id { get; set; }
        public Rect Roi { get; set; }
        public OpenCvSharp.Point[]? Countours { get; set; }
        public int MissingFrames { get; set; }
        public bool IsBanned = false;
        public bool IsClicked = false;
    }
}
