using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace Lab6
{
    class Polygon
    {
        public List<ShapePoint> Points { get; }
        public ShapePoint Center { get; set; } = new ShapePoint(0, 0, 0);
        public List<float> Normal { get; set; }
        public bool IsVisible { get; set; }
        public Polygon(Polygon face)
        {
            Points = face.Points.Select(pt => new ShapePoint(pt.X, pt.Y, pt.Z)).ToList();
            Center = new ShapePoint(face.Center);
            if (Normal != null)
                Normal = new List<float>(face.Normal);
            IsVisible = face.IsVisible;
        }

        public Polygon(List<ShapePoint> pts = null)
        {
            if (pts != null)
            {
                Points = new List<ShapePoint>(pts);
                find_center();
            }
        }

        private void find_center()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (ShapePoint p in Points)
            {
                Center.X += p.X;
                Center.Y += p.Y;
                Center.Z += p.Z;
            }
            Center.X /= Points.Count;
            Center.Y /= Points.Count;
            Center.Z /= Points.Count;
        }

        
        public void reflectX()
        {
            Center.X = -Center.X;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectX();
        }
        public void reflectY()
        {
            Center.Y = -Center.Y;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectY();
        }
        public void reflectZ()
        {
            Center.Z = -Center.Z;
            if (Points != null)
                foreach (var p in Points)
                    p.reflectZ();
        }

        public List<PointF> makePerspective(float k = 1000, float z_camera = 1000)
        {
            List<PointF> res = new List<PointF>();

            foreach (ShapePoint p in Points)
            {
                res.Add(p.makePerspective(k));
            }
            return res;
        }

        public List<PointF> makeIsometric()
        {
            List<PointF> res = new List<PointF>();

            foreach (ShapePoint p in Points)
                res.Add(p.makeIsometric());

            return res;
        }

        

        public void show(Graphics g, Projection pr = 0, Pen pen = null, ShapeLine camera = null, float k = 1000)
        {
            if (pen == null)
                pen = Pens.Black;

            List<PointF> pts;

            switch (pr)
            {
                case Projection.ISOMETRIC:
                    pts = makeIsometric();
                    break;
         
                default:
                    if (camera != null)
                        pts = makePerspective(k, camera.First.Z);
                    else pts = makePerspective(k);
                    break;
            }

            if (pts.Count > 1)
            {
                g.DrawLines(pen, pts.ToArray());
                g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
            }
            else if (pts.Count == 1)
                g.DrawRectangle(pen, pts[0].X, pts[0].Y, 1, 1);
        }

        public void translate(float x, float y, float z)
        {
            foreach (ShapePoint p in Points)
                p.translate(x, y, z);
            find_center();
        }

        public void rotate(double angle, Axis a, ShapeLine line = null)
        {
            foreach (ShapePoint p in Points)
                p.rotate(angle, a, line);
            find_center();
        }

        public void scale(float kx, float ky, float kz)
        {
            foreach (ShapePoint p in Points)
                p.scale(kx, ky, kz);
            find_center();
        }
    }
}
