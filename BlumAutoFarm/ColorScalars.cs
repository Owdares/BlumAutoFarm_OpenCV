using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlumAutoFarm
{
    public static class ColorScalars_RGB
    {
        public static Scalar White { get; } = new Scalar(255, 255, 255);
        public static Scalar Red { get; } = new Scalar(0, 0, 255);
        public static Scalar Green { get; } = new Scalar(0, 255, 0);
        public static Scalar Black { get; } = new Scalar(0, 0, 0);
        public static Scalar Blue { get; } = new Scalar(255, 0, 0);
        public static Scalar GreenOneStunBlum_Lower { get; } = new Scalar(0, 166, 0);
        public static Scalar GreenOneStunBlum_Upper { get; } = new Scalar(147, 255, 82);
        public static Scalar GreenTwoStunBlum_Lower { get; } = new Scalar(0, 225, 0);
        public static Scalar GreenTwoStunBlum_Upper { get; } = new Scalar(245, 255, 221);
    }
}
