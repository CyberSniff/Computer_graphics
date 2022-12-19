using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Polyhedron
    {
        public List<Polygon> Faces { get; set; } = null;
        public Point3D Center { get; set; } = new Point3D(0, 0, 0);
        public Polyhedron(List<Polygon> fs = null)
        {
            if (fs != null)
            {
                Faces = fs.Select(face => new Polygon(face)).ToList();
                find_center();
            }
        }

        public Polyhedron(Polyhedron polyhedron)
        {
            Faces = polyhedron.Faces.Select(face => new Polygon(face)).ToList();
            Center = new Point3D(polyhedron.Center);
        }

        private void find_center()
        {
            Center.X = 0;
            Center.Y = 0;
            Center.Z = 0;
            foreach (Polygon f in Faces)
            {
                Center.X += f.Center.X;
                Center.Y += f.Center.Y;
                Center.Z += f.Center.Z;
            }
            Center.X /= Faces.Count;
            Center.Y /= Faces.Count;
            Center.Z /= Faces.Count;
        }

        public void show(Graphics g, Projection pr = 0, Pen pen = null)
        {
            foreach (Polygon f in Faces)
                f.show(g, pr, pen);
        }

        /*----------------------------- Отражения -----------------------------*/
        public void reflectX()
        {
            if (Faces != null)
                foreach (var f in Faces)
                    f.reflectX();
            find_center();
        }

        public void reflectY()
        {
            if (Faces != null)
                foreach (var f in Faces)
                    f.reflectY();
            find_center();
        }

        public void reflectZ()
        {
            if (Faces != null)
                foreach (var f in Faces)
                    f.reflectZ();
            find_center();
        }

        public void make_hexahedron(float cube_half_size = 50)
        {
            Polygon f = new Polygon(
                new List<Point3D>
                {
                    new Point3D(-cube_half_size, cube_half_size, cube_half_size),
                    new Point3D(cube_half_size, cube_half_size, cube_half_size),
                    new Point3D(cube_half_size, -cube_half_size, cube_half_size),
                    new Point3D(-cube_half_size, -cube_half_size, cube_half_size)
                }
            );


            Faces = new List<Polygon> { f };

            List<Point3D> l1 = new List<Point3D>();
            foreach (var point in f.Points)
            {
                l1.Add(new Point3D(point.X, point.Y, point.Z - 2 * cube_half_size));
            }
            Polygon f1 = new Polygon(
                    new List<Point3D>
                    {
                        new Point3D(-cube_half_size, cube_half_size, -cube_half_size),
                        new Point3D(-cube_half_size, -cube_half_size, -cube_half_size),
                        new Point3D(cube_half_size, -cube_half_size, -cube_half_size),
                        new Point3D(cube_half_size, cube_half_size, -cube_half_size)
                    });

            Faces.Add(f1);

            List<Point3D> l2 = new List<Point3D>
            {
                new Point3D(f.Points[2]),
                new Point3D(f1.Points[2]),
                new Point3D(f1.Points[1]),
                new Point3D(f.Points[3]),
            };
            Polygon f2 = new Polygon(l2);
            Faces.Add(f2);

            List<Point3D> l3 = new List<Point3D>
            {
                new Point3D(f1.Points[0]),
                new Point3D(f1.Points[3]),
                new Point3D(f.Points[1]),
                new Point3D(f.Points[0]),
            };
            Polygon f3 = new Polygon(l3);
            Faces.Add(f3);

            List<Point3D> l4 = new List<Point3D>
            {
                new Point3D(f1.Points[0]),
                new Point3D(f.Points[0]),
                new Point3D(f.Points[3]),
                new Point3D(f1.Points[1])
            };
            Polygon f4 = new Polygon(l4);
            Faces.Add(f4);

            List<Point3D> l5 = new List<Point3D>
            {
                new Point3D(f1.Points[3]),
                new Point3D(f1.Points[2]),
                new Point3D(f.Points[2]),
                new Point3D(f.Points[1])
            };
            Polygon f5 = new Polygon(l5);
            Faces.Add(f5);

            find_center();
        }
        public void make_tetrahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.make_hexahedron();
            }
            Polygon f0 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[0].Points[0]),
                    new Point3D(cube.Faces[1].Points[1]),
                    new Point3D(cube.Faces[1].Points[3])
                }
            );

            Polygon f1 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[1].Points[3]),
                    new Point3D(cube.Faces[1].Points[1]),
                    new Point3D(cube.Faces[0].Points[2])
                }
            );

            Polygon f2 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[0].Points[2]),
                    new Point3D(cube.Faces[1].Points[1]),
                    new Point3D(cube.Faces[0].Points[0])
                }
            );

            Polygon f3 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[0].Points[2]),
                    new Point3D(cube.Faces[0].Points[0]),
                    new Point3D(cube.Faces[1].Points[3])
                }
            );

            Faces = new List<Polygon> { f0, f1, f2, f3 };
            find_center();
        }

        public void make_octahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.make_hexahedron();
            }

            Polygon f0 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[2].Center),
                    new Point3D(cube.Faces[1].Center),
                    new Point3D(cube.Faces[4].Center)
                }
            );

            Polygon f1 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[2].Center),
                    new Point3D(cube.Faces[1].Center),
                    new Point3D(cube.Faces[5].Center)
                }
            );

            Polygon f2 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[2].Center),
                    new Point3D(cube.Faces[5].Center),
                    new Point3D(cube.Faces[0].Center)
                }
            );

            Polygon f3 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[2].Center),
                    new Point3D(cube.Faces[0].Center),
                    new Point3D(cube.Faces[4].Center)
                }
            );

            Polygon f4 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[3].Center),
                    new Point3D(cube.Faces[1].Center),
                    new Point3D(cube.Faces[4].Center)
                }
            );

            Polygon f5 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[3].Center),
                    new Point3D(cube.Faces[1].Center),
                    new Point3D(cube.Faces[5].Center)
                }
            );

            Polygon f6 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[3].Center),
                    new Point3D(cube.Faces[5].Center),
                    new Point3D(cube.Faces[0].Center)
                }
            );

            Polygon f7 = new Polygon(
                new List<Point3D>
                {
                    new Point3D(cube.Faces[3].Center),
                    new Point3D(cube.Faces[0].Center),
                    new Point3D(cube.Faces[4].Center)
                }
            );

            Faces = new List<Polygon> { f0, f1, f2, f3, f4, f5, f6, f7 };
            find_center();
        }

        public void make_icosahedron()
        {
            Faces = new List<Polygon>();

            float size = 100;

            float r1 = size * (float)Math.Sqrt(3) / 4;  
            float r = size * (3 + (float)Math.Sqrt(5)) / (4 * (float)Math.Sqrt(3));

            Point3D up_center = new Point3D(0, -r1, 0);
            Point3D down_center = new Point3D(0, r1, 0);

            double a = Math.PI / 2;
            List<Point3D> up_points = new List<Point3D>();
            for (int i = 0; i < 5; ++i)
            {
                up_points.Add(new Point3D(up_center.X + r * (float)Math.Cos(a), up_center.Y, up_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            a = Math.PI / 2 - Math.PI / 5;
            List<Point3D> down_points = new List<Point3D>();
            for (int i = 0; i < 5; ++i)
            {
                down_points.Add(new Point3D(down_center.X + r * (float)Math.Cos(a), down_center.Y, down_center.Z - r * (float)Math.Sin(a)));
                a += 2 * Math.PI / 5;
            }

            var R = Math.Sqrt(2 * (5 + Math.Sqrt(5))) * size / 4;

            Point3D p_up = new Point3D(up_center.X, (float)(-R), up_center.Z);
            Point3D p_down = new Point3D(down_center.X, (float)R, down_center.Z);

            for (int i = 0; i < 5; ++i)
            {
                Faces.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(p_up),
                        new Point3D(up_points[i]),
                        new Point3D(up_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Faces.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(p_down),
                        new Point3D(down_points[i]),
                        new Point3D(down_points[(i+1) % 5]),
                    })
                    );
            }

            for (int i = 0; i < 5; ++i)
            {
                Faces.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(up_points[i]),
                        new Point3D(up_points[(i+1) % 5]),
                        new Point3D(down_points[(i+1) % 5])
                    })
                    );

                Faces.Add(
                    new Polygon(new List<Point3D>
                    {
                        new Point3D(up_points[i]),
                        new Point3D(down_points[i]),
                        new Point3D(down_points[(i+1) % 5])
                    })
                    );
            }

            find_center();
        }

        public void make_dodecahedron()
        {
            Faces = new List<Polygon>();
            Polyhedron ik = new Polyhedron();
            ik.make_icosahedron();

            List<Point3D> pts = new List<Point3D>();
            foreach (Polygon f in ik.Faces)
            {
                pts.Add(f.Center);
            }

            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[0]),
                new Point3D(pts[1]),
                new Point3D(pts[2]),
                new Point3D(pts[3]),
                new Point3D(pts[4])
            }));

            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[5]),
                new Point3D(pts[6]),
                new Point3D(pts[7]),
                new Point3D(pts[8]),
                new Point3D(pts[9])
            }));

            for (int i = 0; i < 5; ++i)
            {
                Faces.Add(new Polygon(new List<Point3D>
                {
                    new Point3D(pts[i]),
                    new Point3D(pts[(i + 1) % 5]),
                    new Point3D(pts[(i == 4) ? 10 : 2*i + 12]),
                    new Point3D(pts[(i == 4) ? 11 : 2*i + 13]),
                    new Point3D(pts[2*i + 10])
                }));
            }

            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[5]),
                new Point3D(pts[6]),
                new Point3D(pts[13]),
                new Point3D(pts[10]),
                new Point3D(pts[11])
            }));
            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[6]),
                new Point3D(pts[7]),
                new Point3D(pts[15]),
                new Point3D(pts[12]),
                new Point3D(pts[13])
            }));
            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[7]),
                new Point3D(pts[8]),
                new Point3D(pts[17]),
                new Point3D(pts[14]),
                new Point3D(pts[15])
            }));
            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[8]),
                new Point3D(pts[9]),
                new Point3D(pts[19]),
                new Point3D(pts[16]),
                new Point3D(pts[17])
            }));
            Faces.Add(new Polygon(new List<Point3D>
            {
                new Point3D(pts[9]),
                new Point3D(pts[5]),
                new Point3D(pts[11]),
                new Point3D(pts[18]),
                new Point3D(pts[19])
            }));

            find_center();
        }

        public void translate(float x, float y, float z)
        {
            foreach (Polygon f in Faces)
                f.translate(x, y, z);
            find_center();
        }

        public void rotate(double angle, Axis a, Edge line = null)
        {
            foreach (Polygon f in Faces)
                f.rotate(angle, a, line);
            find_center();
        }

        public void scale(float kx, float ky, float kz)
        {
            foreach (Polygon f in Faces)
                f.scale(kx, ky, kz);
            find_center();
        }
    }
}
