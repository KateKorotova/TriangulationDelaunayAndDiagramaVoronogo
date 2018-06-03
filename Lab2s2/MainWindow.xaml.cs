﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2s2
{
    //TODO: save
    public partial class MainWindow : Window
    {

        List<Point> myListPoint = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void paintPoint(Point p)
        {
            Ellipse point = new Ellipse();
            point.Width = 5;
            point.Height = 5;
            point.Fill = Brushes.Black;
            Canvas.SetLeft(point, p.X);
            Canvas.SetTop(point, p.Y);

            canvas.Children.Add(point);
        }

        private double norm(Point point)
        {
            return point.X * point.X + point.Y * point.Y;
        }

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = new Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y, myListPoint.Count);
            p.Normal = norm(p);
            paintPoint(p);
            myListPoint.Add(p);
        }

        private void readFromFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            string line = sr.ReadLine();
            int countPoint;
            bool k = Int32.TryParse(line, out countPoint);
            for (int iter = 0; iter < countPoint; iter++)
            {
                string coordTemp = sr.ReadLine();
                string[] coord = coordTemp.Split(' ');
                Point point = new Point(Int32.Parse(coord[0]), Int32.Parse(coord[0]), myListPoint.Count);
                point.Normal = point.X * point.X + point.Y * point.Y;
                myListPoint.Add(point);
            }
            sr.Close();
        }

        private void paintPoints()
        {
            foreach (Point p in myListPoint)
                paintPoint(p);
        }

        private void donwload_Click(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        readFromFile(openFileDialog1.FileName);
                        paintPoints();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }


        private void generation_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int number;
            bool parser = Int32.TryParse(numberPoints.Text, out number);
            if (parser)
            {
                for (int iter = 0; iter < number; iter++)
                {
                    Point p = new Point(random.Next(0, Convert.ToInt32(canvas.ActualWidth)), random.Next(0, Convert.ToInt32(canvas.ActualHeight)), myListPoint.Count);
                    p.Normal = norm(p);
                    myListPoint.Add(p);
                }
            }
            else
            {
                MessageBox.Show("Error: Could not read the number of points. Try again", "Error");

            }
            paintPoints();
        }
        private void numberPoints_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void numberPoints_Initialized(object sender, EventArgs e)
        {
            numberPoints.Text = "Number of points";
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            myListPoint = new List<Point>();
        }

        private void numberPoints_GotFocus(object sender, RoutedEventArgs e)
        {
            numberPoints.Clear();
        }

        private bool isCrossIntersect(Point one, Point two, Point three, Point four)
        {
            double z1 = (three.X - one.X) * (two.Y - one.Y) - (three.Y - one.Y) * (two.X - one.X);
            double z2 = (four.X - one.X) * (two.Y - one.Y) - (four.Y - one.Y) * (two.X - one.X);
            if (z1 < 0 && z2 < 0 || z1 > 0 && z2 > 0 || z1 == 0 || z2 == 0)
                return false;
            double z3 = (one.X - three.X) * (four.Y - three.Y) - (one.Y - three.Y) * (four.X - three.X);
            double z4 = (two.X - three.X) * (four.Y - three.Y) - (two.Y - three.Y) * (four.X - three.X);
            if (z3 < 0 && z4 < 0 || z3 > 0 && z4 > 0 || z3 == 0 || z4 == 0)
                return false;
            return true;
        }


        private List<Triangle> delaunaye()
        {
            List<Triangle> result = new List<Triangle>();
            for (int i = 0; i < myListPoint.Count - 2; i++)
                for (int j = i + 1; j < myListPoint.Count; j++)
                    for (int k = i + 1; k < myListPoint.Count; k++)
                    {
                        if (k == j)
                            continue;
                        Point one = myListPoint[i];
                        Point two = myListPoint[j];
                        Point three = myListPoint[k];

                        double nx = (two.Y - one.Y) * (three.Normal - one.Normal) - (three.Y - one.Y) * (two.Normal - one.Normal);
                        double ny = (three.X - one.X) * (two.Normal - one.Normal) - (two.X - one.X) * (three.Normal - one.Normal);
                        double nz = (two.X - one.X) * (three.Y - one.Y) - (three.X - one.X) * (two.Y - one.Y);
                        if (nz >= 0)
                            continue;
                        bool check = true;
                        for (int m = 0; m < myListPoint.Count; m++)
                        {
                            Point temp = myListPoint[m];
                            double dot = (temp.X - one.X) * nx + (temp.Y - one.Y) * ny + (temp.Normal - one.Normal) * nz;
                            if (dot > 0)
                            {
                                check = false;
                                break;
                            }
                        }
                        if (check == false)
                            continue;
                        Point[] s1 = new Point[] { one, two, three, one };
                        foreach (Triangle triangle in result)
                        {
                            Point[] s2 = new Point[] { triangle.One, triangle.Two, triangle.Three, triangle.One };
                            for (int u = 0; u < 3; u++)
                            {
                                for (int v = 0; v < 3; v++)
                                    if (isCrossIntersect(s1[u], s1[u + 1], s2[v], s2[v + 1]))
                                    {
                                        check = false;
                                        break;
                                    }
                                if (check == false)
                                    break;
                            }
                            if (check == false)
                                break;
                        }
                        if (check == false)
                            continue;
                        result.Add(new Triangle(one, two, three));

                    }
            return result;
        }

        private void paintLine(Point one, Point two)
        {
            Line edge = new Line();
            edge.X1 = one.X;
            edge.Y1 = one.Y;
            edge.X2 = two.X;
            edge.Y2 = two.Y;

            edge.Stroke = Brushes.Black;
            edge.StrokeThickness = 1;
            edge.HorizontalAlignment = HorizontalAlignment.Left;
            edge.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(edge);
        }

        private void paintTriangl()
        {
            List<Triangle> triangles = delaunaye();
            foreach(Triangle tr in triangles)
            {
                paintLine(tr.One, tr.Two);
                paintLine(tr.Two, tr.Three);
                paintLine(tr.Three, tr.One);
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            paintTriangl();
        }
    }
}
