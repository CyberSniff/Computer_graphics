using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class ShapeLine
    {
        public ShapePoint First { get; set; }
        public ShapePoint Second { get; set; }

        public ShapeLine(ShapePoint p1, ShapePoint p2)
        {
            First = new ShapePoint(p1);
            Second = new ShapePoint(p2);
        }

        private List<PointF> makePerspective(int k = 1000)
        {
            List<PointF> res = new List<PointF>
            {
                First.makePerspective(k),
                Second.makePerspective(k)
            };

            return res;
        }

    

        private List<PointF> makeIsometric()
        {
            List<PointF> res = new List<PointF>
            {
                First.makeIsometric(),
                Second.makeIsometric()
            };
            return res;
        }

        public void show(Graphics g, Projection pr = 0, Pen pen = null)
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
                    pts = makePerspective();
                    break;
            }

            g.DrawLine(pen, pts[0], pts[pts.Count - 1]);
        }
    }
}
