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
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
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
            create();
        }
        private Point one;
        private Point two;
        private Point three;
        private Point centre; 

        internal Point One { get => one; set => one = value; }
        internal Point Two { get => two; set => two = value; }
        internal Point Three { get => three; set => three = value; }
        internal Point Centre { get => centre; set => centre = value; }

        private void create()
        {
            double d = 2 * (one.X * (two.Y - three.Y) + two.X * (three.Y - one.Y) + three.X * (one.Y - two.Y));
            double newX = ((one.Normal * (two.Y - three.Y) + two.Normal * (three.Y - one.Y) + three.Normal * (one.Y - two.Y))) / d;
            double newY = ((one.Normal * (three.X - two.X) + two.Normal * (one.X - three.X) + three.Normal * (two.X - one.X))) / d;
            centre = new Point(newX, newY);
        }
    }

    class Edge
    {
        public Edge(Point one, Point two)
        {
            this.One = one;
            this.Two = two;
        }
        private Point one;
        private Point two;

        internal Point Two { get => two; set => two = value; }
        internal Point One { get => one; set => one = value; }
    }
}
