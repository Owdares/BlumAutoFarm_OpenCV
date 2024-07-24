using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm.Models
{
    public class DetectedObjectModel
    {
        public Rect Roi { get; set; }
        public OpenCvSharp.Point[]? Countours { get; set; }
    }
}
