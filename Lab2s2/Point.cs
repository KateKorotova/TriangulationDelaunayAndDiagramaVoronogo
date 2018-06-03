using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2s2
{
    class Point
    {
        public Point() { }
        public Point(double x, double y, int num)
        {
            this.x = x;

            this.y = y;
            number = num;
        }
        private double x;
        private double y;
        private int number;
        private double normal;
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public int Number { get => number; set => number = value; }
        public double Normal { get => normal; set => normal = value; }
    }

    class Triangle
    {
        public Triangle(Point one, Point two, Point three)
        {
            this.One = one;
            this.Two = two;
            this.Three = three; 
        }
        private Point one;
        private Point two;
        private Point three;

        internal Point One { get => one; set => one = value; }
        internal Point Two { get => two; set => two = value; }
        internal Point Three { get => three; set => three = value; }
    }
}
