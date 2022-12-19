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
        public ShapePoint Center { get; set; } = new ShapePoint(0, 0, 0);
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
            Center = new ShapePoint(polyhedron.Center);
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

        public void makeHexahedron(float cube_half_size = 50)
        {
            Polygon f = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(-cube_half_size, cube_half_size, cube_half_size),
                    new ShapePoint(cube_half_size, cube_half_size, cube_half_size),
                    new ShapePoint(cube_half_size, -cube_half_size, cube_half_size),
                    new ShapePoint(-cube_half_size, -cube_half_size, cube_half_size)
                }
            );


            Faces = new List<Polygon> { f };

            List<ShapePoint> l1 = new List<ShapePoint>();
            foreach (var point in f.Points)
            {
                l1.Add(new ShapePoint(point.X, point.Y, point.Z - 2 * cube_half_size));
            }
            Polygon f1 = new Polygon(
                    new List<ShapePoint>
                    {
                        new ShapePoint(-cube_half_size, cube_half_size, -cube_half_size),
                        new ShapePoint(-cube_half_size, -cube_half_size, -cube_half_size),
                        new ShapePoint(cube_half_size, -cube_half_size, -cube_half_size),
                        new ShapePoint(cube_half_size, cube_half_size, -cube_half_size)
                    });

            Faces.Add(f1);

            List<ShapePoint> l2 = new List<ShapePoint>
            {
                new ShapePoint(f.Points[2]),
                new ShapePoint(f1.Points[2]),
                new ShapePoint(f1.Points[1]),
                new ShapePoint(f.Points[3]),
            };
            Polygon f2 = new Polygon(l2);
            Faces.Add(f2);

            List<ShapePoint> l3 = new List<ShapePoint>
            {
                new ShapePoint(f1.Points[0]),
                new ShapePoint(f1.Points[3]),
                new ShapePoint(f.Points[1]),
                new ShapePoint(f.Points[0]),
            };
            Polygon f3 = new Polygon(l3);
            Faces.Add(f3);

            List<ShapePoint> l4 = new List<ShapePoint>
            {
                new ShapePoint(f1.Points[0]),
                new ShapePoint(f.Points[0]),
                new ShapePoint(f.Points[3]),
                new ShapePoint(f1.Points[1])
            };
            Polygon f4 = new Polygon(l4);
            Faces.Add(f4);

            List<ShapePoint> l5 = new List<ShapePoint>
            {
                new ShapePoint(f1.Points[3]),
                new ShapePoint(f1.Points[2]),
                new ShapePoint(f.Points[2]),
                new ShapePoint(f.Points[1])
            };
            Polygon f5 = new Polygon(l5);
            Faces.Add(f5);

            find_center();
        }
        public void makeTetrahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.makeHexahedron();
            }
            Polygon f0 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[0].Points[0]),
                    new ShapePoint(cube.Faces[1].Points[1]),
                    new ShapePoint(cube.Faces[1].Points[3])
                }
            );

            Polygon f1 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[1].Points[3]),
                    new ShapePoint(cube.Faces[1].Points[1]),
                    new ShapePoint(cube.Faces[0].Points[2])
                }
            );

            Polygon f2 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[0].Points[2]),
                    new ShapePoint(cube.Faces[1].Points[1]),
                    new ShapePoint(cube.Faces[0].Points[0])
                }
            );

            Polygon f3 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[0].Points[2]),
                    new ShapePoint(cube.Faces[0].Points[0]),
                    new ShapePoint(cube.Faces[1].Points[3])
                }
            );

            Faces = new List<Polygon> { f0, f1, f2, f3 };
            find_center();
        }

        public void makeOctahedron(Polyhedron cube = null)
        {
            if (cube == null)
            {
                cube = new Polyhedron();
                cube.makeHexahedron();
            }

            Polygon f0 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[2].Center),
                    new ShapePoint(cube.Faces[1].Center),
                    new ShapePoint(cube.Faces[4].Center)
                }
            );

            Polygon f1 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[2].Center),
                    new ShapePoint(cube.Faces[1].Center),
                    new ShapePoint(cube.Faces[5].Center)
                }
            );

            Polygon f2 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[2].Center),
                    new ShapePoint(cube.Faces[5].Center),
                    new ShapePoint(cube.Faces[0].Center)
                }
            );

            Polygon f3 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[2].Center),
                    new ShapePoint(cube.Faces[0].Center),
                    new ShapePoint(cube.Faces[4].Center)
                }
            );

            Polygon f4 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[3].Center),
                    new ShapePoint(cube.Faces[1].Center),
                    new ShapePoint(cube.Faces[4].Center)
                }
            );

            Polygon f5 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[3].Center),
                    new ShapePoint(cube.Faces[1].Center),
                    new ShapePoint(cube.Faces[5].Center)
                }
            );

            Polygon f6 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[3].Center),
                    new ShapePoint(cube.Faces[5].Center),
                    new ShapePoint(cube.Faces[0].Center)
                }
            );

            Polygon f7 = new Polygon(
                new List<ShapePoint>
                {
                    new ShapePoint(cube.Faces[3].Center),
                    new ShapePoint(cube.Faces[0].Center),
                    new ShapePoint(cube.Faces[4].Center)
                }
            );

            Faces = new List<Polygon> { f0, f1, f2, f3, f4, f5, f6, f7 };
            find_center();
        }

      

        public void translate(float x, float y, float z)
        {
            foreach (Polygon f in Faces)
                f.translate(x, y, z);
            find_center();
        }

        public void rotate(double angle, Axis a, ShapeLine line = null)
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
