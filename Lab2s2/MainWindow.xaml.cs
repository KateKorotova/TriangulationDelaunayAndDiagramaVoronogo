using Microsoft.Win32;
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
            if (parser) {
                for (int iter = 0; iter < number; iter++)
                {
                    Point p = new Point(random.Next(0,Convert.ToInt32(canvas.ActualWidth)), random.Next(0, Convert.ToInt32(canvas.ActualHeight)), myListPoint.Count);
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
        //private Point searchNear()
        //{
        //    Point point = myListPoint[0];
        //    double min = Double.MaxValue;
        //    Point result = null; 
        //    for(int i = 1; i < myListPoint.Count; i++)
        //    {
        //        Point temp = myListPoint[i];
        //        double len = Math.Sqrt((point.X - temp.X)* (point.X - temp.X) + (point.Y - temp.Y)*(point.Y - temp.Y));
        //        if (len < min)
        //        {
        //            min = len;
        //            result = temp; 
        //        }
        //    }
        //    return result;
        //}

        private List<Triangle> delone()
        {
            List<Triangle> result = new List<Triangle>();














            return result;
        }
    }
}
