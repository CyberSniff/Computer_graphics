using System;
using System.Collections.Generic;
using System.Text;

namespace laba6
{
    class ShapePoint
    {
        public double x, y, z;
        public ShapePoint(double x, double y, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    class ShapeLine
    {
        public ShapePoint first, second;
        public ShapeLine(ShapePoint first, ShapePoint second)
        {
            this.first = first;
            this.second = second;
        }
    }
    class ShapePolygon
    {
        public List<ShapeLine> lines = new List<ShapeLine>();
        public ShapePolygon(params ShapeLine[]lines)
        {
            foreach (ShapeLine line in lines)
                this.lines.Add(line);
        }

    }
    class ShapePolyhedron
    {
        //public List<ShapePoint> points = new List<ShapePoint>();
        //public List<ShapeLine> lines = new List<ShapeLine>();
        public List<ShapePolygon> polygons = new List<ShapePolygon>();
        public ShapePolyhedron(params ShapePolygon[]polygons)
        {
            foreach (ShapePolygon polygon in polygons)
            {
                this.polygons.Add(polygon);
                /*foreach (ShapeLine line in polygon.lines)
                {
                    lines.Add(line);
                    points.Add(line.first);
                    points.Add(line.second);
                }*/
            }
        }

        public HashSet<ShapeLine>GetLines()
        {
            HashSet<ShapeLine> result = new HashSet<ShapeLine>();

            foreach (ShapePolygon polygon in polygons)
                foreach (ShapeLine line in polygon.lines)
                    result.Add(line);

            return result;
        }
    }
}
