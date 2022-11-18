using System;
using System.Collections.Generic;
using System.Text;

namespace laba6
{
    class Polyhedrons
    {
        // Создание тетраэдра
        public static ShapePolyhedron CreateTetrahedron()
        {
            double l = 200;
            double h = l * Math.Sqrt(3) / 2;
            double x = 300;
            double y = 600;
            double z = 0;
            double z_h = h * h - h * h / 9;

            ShapePoint p_1 = new ShapePoint(x, y, z);
            ShapePoint p_2 = new ShapePoint(x + l, y, z);
            ShapePoint p_3 = new ShapePoint(x + l / 2, y - h, z);
            ShapePoint p_4 = new ShapePoint(x + l / 2, y - h / 3, z + z_h);

            ShapeLine l_1 = new ShapeLine(p_1, p_2);
            ShapeLine l_2 = new ShapeLine(p_2, p_3);
            ShapeLine l_3 = new ShapeLine(p_3, p_4);
            ShapeLine l_4 = new ShapeLine(p_4, p_1);
            ShapeLine l_5 = new ShapeLine(p_1, p_3);
            ShapeLine l_6 = new ShapeLine(p_2, p_4);

            ShapePolygon pol_1 = new ShapePolygon(l_1, l_2, l_5);
            ShapePolygon pol_2 = new ShapePolygon(l_1, l_4, l_6);
            ShapePolygon pol_3 = new ShapePolygon(l_5, l_3, l_4);
            ShapePolygon pol_4 = new ShapePolygon(l_2, l_3, l_6);

            return new ShapePolyhedron(pol_1, pol_2, pol_3, pol_4);
        }

        // Создание гексаэдра (куба)
        public static ShapePolyhedron CreateHexahedron()
        {
            double l = 200;
            double x = 300;
            double y = 300;
            double z = 0;

            ShapePoint p_1 = new ShapePoint(x, y, z);
            ShapePoint p_2 = new ShapePoint(x + l, y, z);
            ShapePoint p_3 = new ShapePoint(x + l, y + l, z);
            ShapePoint p_4 = new ShapePoint(x, y + l, z);
            ShapePoint p_5 = new ShapePoint(x, y, z + l);
            ShapePoint p_6 = new ShapePoint(x + l, y, z + l);
            ShapePoint p_7 = new ShapePoint(x + l, y + l, z + l);
            ShapePoint p_8 = new ShapePoint(x, y + l, z + l);

            ShapeLine l_1 = new ShapeLine(p_1, p_2);
            ShapeLine l_2 = new ShapeLine(p_2, p_3);
            ShapeLine l_3 = new ShapeLine(p_3, p_4);
            ShapeLine l_4 = new ShapeLine(p_4, p_1);
            ShapeLine l_5 = new ShapeLine(p_1, p_5);
            ShapeLine l_6 = new ShapeLine(p_2, p_6);
            ShapeLine l_7 = new ShapeLine(p_3, p_7);
            ShapeLine l_8 = new ShapeLine(p_4, p_8);
            ShapeLine l_9 = new ShapeLine(p_5, p_6);
            ShapeLine l_10 = new ShapeLine(p_6, p_7);
            ShapeLine l_11 = new ShapeLine(p_7, p_8);
            ShapeLine l_12 = new ShapeLine(p_8, p_5);

            ShapePolygon pol_1 = new ShapePolygon(l_1, l_2, l_3, l_4);
            ShapePolygon pol_2 = new ShapePolygon(l_5, l_12, l_8, l_4);
            ShapePolygon pol_3 = new ShapePolygon(l_5, l_9, l_6, l_1);
            ShapePolygon pol_4 = new ShapePolygon(l_6, l_10, l_7, l_2);
            ShapePolygon pol_5 = new ShapePolygon(l_8, l_11, l_7, l_3);
            ShapePolygon pol_6 = new ShapePolygon(l_9, l_10, l_11, l_12);

           return new ShapePolyhedron(pol_1, pol_2, pol_3, pol_4, pol_5, pol_6);
        }

        // Создание октаэдра
        public static ShapePolyhedron CreateOctahedron()
        {
            double l = 200;
            double x = 300;
            double y = 300;
            double z = 0;
            double h = l * Math.Sqrt(3) / 2;


            ShapePoint p_1 = new ShapePoint(x, y, z);
            ShapePoint p_2 = new ShapePoint(x + l, y, z);
            ShapePoint p_3 = new ShapePoint(x + l, y, z + l);
            ShapePoint p_4 = new ShapePoint(x, y, z + l);
            ShapePoint p_5 = new ShapePoint(x + l / 2, y - h, z + l / 2);
            ShapePoint p_6 = new ShapePoint(x + l / 2, y + h, z + l / 2);

            ShapeLine l_1 = new ShapeLine(p_1, p_2);
            ShapeLine l_2 = new ShapeLine(p_2, p_3);
            ShapeLine l_3 = new ShapeLine(p_3, p_4);
            ShapeLine l_4 = new ShapeLine(p_4, p_1);
            ShapeLine l_5 = new ShapeLine(p_1, p_5);
            ShapeLine l_6 = new ShapeLine(p_2, p_5);
            ShapeLine l_7 = new ShapeLine(p_3, p_5);
            ShapeLine l_8 = new ShapeLine(p_4, p_5);
            ShapeLine l_9 = new ShapeLine(p_1, p_6);
            ShapeLine l_10 = new ShapeLine(p_2, p_6);
            ShapeLine l_11 = new ShapeLine(p_3, p_6);
            ShapeLine l_12 = new ShapeLine(p_4, p_6);

            ShapePolygon pol_1 = new ShapePolygon(l_1, l_5, l_6);
            ShapePolygon pol_2 = new ShapePolygon(l_2, l_6, l_7);
            ShapePolygon pol_3 = new ShapePolygon(l_3, l_7, l_8);
            ShapePolygon pol_4 = new ShapePolygon(l_4, l_8, l_5);
            ShapePolygon pol_5 = new ShapePolygon(l_1, l_9, l_10);
            ShapePolygon pol_6 = new ShapePolygon(l_2, l_10, l_11);
            ShapePolygon pol_7 = new ShapePolygon(l_3, l_11, l_12);
            ShapePolygon pol_8 = new ShapePolygon(l_4, l_12, l_9);

            return new ShapePolyhedron(pol_1, pol_2, pol_3, pol_4, pol_5, pol_6, pol_7, pol_8);
        }

        // Создание икосаэдра
        public static ShapePolyhedron CreateDodecahedron()
        {
            return null;
        }

        // Создание додекаэдра
        public static ShapePolyhedron CreateIcosahedron()
        {
            return null;
        }
    }
}
