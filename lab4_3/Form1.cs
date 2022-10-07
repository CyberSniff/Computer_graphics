using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace affine_transformations2D
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Graphics g;
        Point pointLocation;
        List<Segment> segments = new List<Segment>();
        List<Point> polygon = new List<Point>();
        Boolean isMouseDown = false;
        Point startPoint, endPoint;
        Point minPolygonCoord, maxPolygonCoord;

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            segmentRB.Checked = true;
            startPoint = Point.Empty;
            endPoint = Point.Empty;
            pointLocation = new Point(0, 0);
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            segments.Clear();
            polygon.Clear();
            pointLocation = Point.Empty;
            posRelativeToSegmentLabel.Text = "Положение точки отн-но ребра";
            posRelativeToPolygonLabel.Text = "Принадлежность точки полигону";
            pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (segmentRB.Checked) 
            {
                isMouseDown = true;
                startPoint = e.Location;
            }
            else if (polygonRB.Checked)
            {
                isMouseDown = true;
                if (polygon.Count == 0)
                {
                    startPoint = e.Location;
                    minPolygonCoord = e.Location;
                    maxPolygonCoord = e.Location;
                    polygon.Add(startPoint);
                }
            }
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (segmentRB.Checked && isMouseDown)
            {
                endPoint = e.Location;
            }
            else if (polygonRB.Checked && isMouseDown)
            {
                endPoint = e.Location;
            }
            pictureBox1.Invalidate();
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (segmentRB.Checked && isMouseDown)
            {
                isMouseDown = false;
                //если нажали и отпустили сразу
                if (endPoint == Point.Empty)
                    return;
                segments.Add(new Segment(startPoint, endPoint));
                startPoint = Point.Empty;
                endPoint = Point.Empty;

            }
            else if (polygonRB.Checked && isMouseDown)
            {
                isMouseDown = false;
                //если нажали и отпустили сразу
                if (endPoint == Point.Empty)
                    return;
                polygon.Add(endPoint);
                if (endPoint.X < minPolygonCoord.X)
                    minPolygonCoord.X = endPoint.X;
                if (endPoint.Y < minPolygonCoord.Y)
                    minPolygonCoord.Y = endPoint.Y;
                if (endPoint.X > maxPolygonCoord.X)
                    maxPolygonCoord.X = endPoint.X;
                if (endPoint.Y > maxPolygonCoord.Y)
                    maxPolygonCoord.Y = endPoint.Y;

                startPoint = endPoint;
                endPoint = Point.Empty;
            }
            else if (pointRB.Checked)
            {
                pointLocation = e.Location;
            }
            pictureBox1.Invalidate();
        }

      
    
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            if (segments.Count > 0)
            {
                foreach (Segment seg in segments)
                    g.DrawLine(Pens.Red, seg.leftP, seg.rightP);
            }

            if (polygon.Count > 1)
            {
                for (int i = 0; i < polygon.Count - 1; ++i)
                {
                    g.DrawLine(Pens.Red, polygon[i], polygon[i + 1]);
                }
                g.DrawLine(Pens.Red, polygon[0], polygon[polygon.Count - 1]);

            }
            //пока тянешь ребро
            if (startPoint != Point.Empty && endPoint != Point.Empty)
                g.DrawLine(Pens.Red, startPoint, endPoint);
            //точка
            g.DrawEllipse(Pens.Blue, pointLocation.X - 1, pointLocation.Y - 1, 3, 3);
            g.FillEllipse(Brushes.Blue, pointLocation.X - 1, pointLocation.Y - 1, 3, 3);

            refresh_labels();
        }

        private void refresh_labels()
        {
            if (segments.Count > 0)
            {              
                int n = segments.Count - 1; //индекс последнего ребра
                int pos = find_where_the_point_is(pointLocation, segments[n].leftP, segments[n].rightP);
                if (pos == 0)
                    posRelativeToSegmentLabel.Text = "Лежит на линии";
                else if (pos > 0)
                    posRelativeToSegmentLabel.Text = "Лежит слева от линии";
                else
                    posRelativeToSegmentLabel.Text = "Лежит справа от линии";
            }

            if (polygon.Count > 2)
            {
                if (is_point_inside(polygon, pointLocation))
                    posRelativeToPolygonLabel.Text = "Принадлежит полигону";
                else
                    posRelativeToPolygonLabel.Text = "Не принадлежит полигону";
            }

         
        }

        // Классифицирует положение точки относительно ребра     
        int find_where_the_point_is(PointF p, Point A, Point B)
        {
            return (int)((p.X - A.X) * (B.Y - A.Y) - (p.Y - A.Y) * (B.X - A.X));
        }

        //определяет принадлежит ли точка полигону
        bool is_point_inside(List<Point> polygon, Point p)
        {
            int n = polygon.Count;
            if (n < 3) return false;

            PointF extreme = new PointF(pictureBox1.Width, p.Y);

            int count = 0, i = 0;
            do
            {
                int next = (i + 1) % n;
                PointF intersection = find_intersection_point(polygon[i], polygon[next], p, extreme);
                if (intersection.X != -1)
                {
                    // If the point 'p' is colinear with line segment 'i-next',
                    // then check if it lies on segment. If it lies, return true,
                    // otherwise false
                    if (orientation(polygon[i], p, polygon[next]) == 0)
                        return on_segment(polygon[i], p, polygon[next]);

                    count++;
                }
                i = next;
            } while (i != 0);

            // Return true if count is odd, false otherwise
            return count % 2 == 1;
        }

        int orientation(PointF p, PointF q, PointF r)
        {
            float val = (q.Y - p.Y) * (r.X - q.X) -
                      (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0;  // colinear
            return (val > 0) ? 1 : 2; // clock or counterclock wise
        }

        private void posRelativeToPolygonLabel_Click(object sender, EventArgs e)
        {

        }

        private void polygonRB_CheckedChanged(object sender, EventArgs e)
        {

        }

        bool on_segment(PointF q, PointF p, PointF r)
        {
            if (q.X <= Math.Max(p.X, r.X) && q.X >= Math.Min(p.X, r.X) &&
                    q.Y <= Math.Max(p.Y, r.Y) && q.Y >= Math.Min(p.Y, r.Y))
                return true;
            return false;
        }

        PointF find_intersection_point(PointF p0, PointF p1, PointF p2, PointF p3)
        {
            PointF i = new PointF(-1, -1);
            PointF s1 = new PointF();
            PointF s2 = new PointF();
            s1.X = p1.X - p0.X;
            s1.Y = p1.Y - p0.Y;
            s2.X = p3.X - p2.X;
            s2.Y = p3.Y - p2.Y;
            float s, t;
            s = (-s1.Y * (p0.X - p2.X) + s1.X * (p0.Y - p2.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
            t = (s2.X * (p0.Y - p2.Y) - s2.Y * (p0.X - p2.X)) / (-s2.X * s1.Y + s1.X * s2.Y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                i.X = p0.X + (t * s1.X);
                i.Y = p0.Y + (t * s1.Y);
            }
            return i;

         
        }
    }
}
