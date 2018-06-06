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
        int choose = 1; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private double norm(Point point)
        {
            return point.X * point.X + point.Y * point.Y;
        }

        private void numberPoints_GotFocus(object sender, RoutedEventArgs e)
        {
            numberPoints.Clear();
        }

        private void numberPoints_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void numberPoints_Initialized(object sender, EventArgs e)
        {
            numberPoints.Text = "Number of points";
        }


        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = new Point(e.GetPosition(canvas).X, e.GetPosition(canvas).Y, myListPoint.Count);
            p.Normal = norm(p);
            paintPoint(p);
            myListPoint.Add(p);
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
            canvas.Children.Clear();
            myListPoint = new List<Point>();
            Random random = new Random();
            int number;
            bool parser = Int32.TryParse(numberPoints.Text, out number);
            if (parser)
            {
                if (number < 300)
                {
                    for (int iter = 0; iter < number; iter++)
                    {
                        Point p = new Point(random.Next(0, Convert.ToInt32(canvas.Width)), random.Next(0, Convert.ToInt32(canvas.Height)), myListPoint.Count);
                        p.Normal = norm(p);
                        myListPoint.Add(p);
                    }
                }
                else
                    MessageBox.Show("Too many points. Write less number, please", "Attetion");
            }
            else
                MessageBox.Show("Error: Could not read the number of points. Try again", "Error");

            paintPoints();
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            myListPoint = new List<Point>();
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
                Point point = new Point(Int32.Parse(coord[0]), Int32.Parse(coord[1]), myListPoint.Count);
                point.Normal = point.X * point.X + point.Y * point.Y;
                myListPoint.Add(point);
            }
            sr.Close();
        }

        private void writeToFIle(string fileName)
        {
            StreamWriter sw = File.CreateText(fileName);
            sw.WriteLine(myListPoint.Count.ToString());
            foreach(Point point in myListPoint)
            {
                string line = point.X.ToString() + " " + point.Y.ToString();
                sw.WriteLine(line);
            }
            sw.Close();
        }

        private void paintPoint(Point p)
        {
            Ellipse point = new Ellipse();
            point.Width = 4;
            point.Height = 4;
            point.Fill = Brushes.Black;
            Canvas.SetLeft(point, p.X - 2);
            Canvas.SetTop(point, p.Y - 2);

            canvas.Children.Add(point);
        }

        private void paintPoints()
        {
            foreach (Point p in myListPoint)
                paintPoint(p);
        }

        private void paintLine(Point one, Point two, SolidColorBrush color)
        {
            Line edge = new Line();
            edge.X1 = one.X;
            edge.Y1 = one.Y;
            edge.X2 = two.X;
            edge.Y2 = two.Y;

            edge.Stroke = color;
            edge.StrokeThickness = 1;

            canvas.Children.Add(edge);
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

        private void paintTriangl()
        {
            List<Triangle> triangles = delaunaye();
            SolidColorBrush color = Brushes.Black;
            foreach (Triangle tr in triangles)
            {
                paintLine(tr.One, tr.Two, color);
                paintLine(tr.Two, tr.Three, color);
                paintLine(tr.Three, tr.One, color);
            }
        }

        private List<Edge>  edgesInTriangle(Triangle triangle)
        {
            List<Edge> result = new List<Edge>();
            Point[] pointsInTriangle = new Point[] { triangle.One, triangle.Two, triangle.Three, triangle.One };
            Point[] avrPoint = new Point[3];
            for (int i = 0; i < 3; i++)
            {
                double tempX = (pointsInTriangle[i].X + pointsInTriangle[i + 1].X) / 2;
                double tempY = (pointsInTriangle[i].Y + pointsInTriangle[i + 1].Y) / 2;
                avrPoint[i] = new Point(tempX, tempY);
            }

            for (int i = 0; i < 3; i++)
            {
                Edge edge = new Edge(avrPoint[i], triangle.Centre);
                result.Add(edge);
            }


            return result;
        }

        private int compare(Point first, Point second)
        {
            if (first.X < second.X)
                return -1;
            if (first.X > second.X)
                return 1;
            else
            {
                if (first.Y < second.Y)
                    return -1;
                if (first.Y > second.Y)
                    return 1;
            }
            return 0;
        }

        private Point blunt(Triangle triangle)
        {
            double a = (triangle.One.X - triangle.Two.X) * (triangle.One.X - triangle.Two.X) + (triangle.One.Y - triangle.Two.Y) * (triangle.One.Y - triangle.Two.Y);
            double c = (triangle.One.X - triangle.Three.X) * (triangle.One.X - triangle.Three.X) + (triangle.One.Y - triangle.Three.Y) * (triangle.One.Y - triangle.Three.Y);
            double b = (triangle.Three.X - triangle.Two.X) * (triangle.Three.X - triangle.Two.X) + (triangle.Three.Y - triangle.Two.Y) * (triangle.Three.Y - triangle.Two.Y);
            if(a > (b + c))
            {
                return new Point((triangle.One.X + triangle.Two.X) / 2, (triangle.One.Y + triangle.Two.Y) / 2);
            }
            if (b > (a + c))
            {
                return new Point((triangle.Three.X + triangle.Two.X) / 2, (triangle.Three.Y + triangle.Two.Y) / 2);
            }
            if (c > (b + a))
            {
                return new Point((triangle.One.X + triangle.Three.X) / 2, (triangle.One.Y + triangle.Three.Y) / 2);
            }
            return null;
        }

        private Point avrPointInCommonEdge(Triangle trngl1, Triangle trngl2)
        {
            Point result = null;
            List<Point> points = new List<Point> { trngl1.One, trngl1.Two, trngl1.Three, trngl2.One, trngl2.Two, trngl2.Three};
            List<Point> commonPoint = new List<Point>();
            points.Sort((x, y) => compare(x, y));
            for(int i = 0; i < points.Count - 1; i++)
                if(points[i] == points[i + 1])
                    commonPoint.Add(points[i]);
            if(commonPoint.Count == 2)
            {
                double x = (commonPoint[0].X + commonPoint[1].X) / 2;
                double y = (commonPoint[0].Y + commonPoint[1].Y) / 2;
                result = new Point(x,y);
            }
            return result;
        }


        private List<Edge> Voronogo(List<Triangle> triangles)
        {
            List<Edge> avrPerp = new List<Edge>();
            List<Edge> centrePerp = new List<Edge>();
            foreach (Triangle trngl in triangles)
                avrPerp.AddRange(edgesInTriangle(trngl));
            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = i + 1; j < triangles.Count; j++)
                {
                    Point commonPoint = avrPointInCommonEdge(triangles[i], triangles[j]);
                    if (commonPoint != null)
                    {
                        centrePerp.Add(new Edge(triangles[i].Centre, triangles[j].Centre));
                        avrPerp.RemoveAll(item => compare(item.Two, triangles[i].Centre) == 0 && compare(item.One, commonPoint) == 0);
                        avrPerp.RemoveAll(item => compare(item.Two, triangles[j].Centre) == 0 && compare(item.One, commonPoint) == 0);
                    }

                }

                Point bl = blunt(triangles[i]);
                if (bl != null)
                {
                    if(avrPerp.RemoveAll(item => compare(item.Two, triangles[i].Centre) == 0 && compare(item.One, bl) == 0) > 0)
                        avrPerp.Add(new Edge(new Point(2 * triangles[i].Centre.X - bl.X, 2 * triangles[i].Centre.Y - bl.Y),
                                            triangles[i].Centre));

                }
            }
            foreach(Edge edge in avrPerp)
            {
                centrePerp.Add(new Edge(new Point(edge.Two.X + 100 * (edge.One.X - edge.Two.X), edge.Two.Y + 100 * (edge.One.Y - edge.Two.Y)), edge.Two));
            }
            return centrePerp;

        }

        private void paintVoronogo(List<Edge> edges)
        {
            foreach (Edge edge in edges)
                paintLine(edge.One, edge.Two, Brushes.Red);
        }
        

        private void start_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            paintPoints();
            if (choose == 1)
                paintTriangl();
            else
                paintVoronogo(Voronogo(delaunaye()));

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                        writeToFIle(saveFileDialog.FileName);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not write to file. Original error: " + ex.Message);
                }
            }
        }

        private void delaunayeChoose_Checked(object sender, RoutedEventArgs e)
        {
            if(choose == 2)
            {
                canvas.Children.Clear();
                paintPoints();
            }
            choose = 1; 
        }

        private void voronogoChose_Checked(object sender, RoutedEventArgs e)
        {
            if (choose == 1)
            {
                canvas.Children.Clear();
                paintPoints();
            }
            choose = 2;
        }
    }
}

