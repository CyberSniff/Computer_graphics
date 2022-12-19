using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public enum Axis { AXIS_X, AXIS_Y, AXIS_Z, OTHER };
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC, ORTHOGR_X, ORTHOGR_Y, ORTHOGR_Z };

    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Axis rotateLineMode = 0;
        Polyhedron figure = null;
        int revertId = -1;


        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (figure == null)
            {
                MessageBox.Show("Неодбходимо выбрать фигуру!", "Ошибка!", MessageBoxButtons.OK);
            }
            else
            {
                //TRANSLATE
                int offsetX = (int)numericUpDown1.Value, offsetY = (int)numericUpDown2.Value, offsetZ = (int)numericUpDown3.Value;
                figure.translate(offsetX, offsetY, offsetZ);

                //ROTATE
                int rotateAngleX = (int)numericUpDown4.Value;
                figure.rotate(rotateAngleX, 0);

                int rotateAngleY = (int)numericUpDown5.Value;
                figure.rotate(rotateAngleY, Axis.AXIS_Y);

                int rotateAngleZ = (int)numericUpDown6.Value;
                figure.rotate(rotateAngleZ, Axis.AXIS_Z);

                //SCALE
                if (checkBox1.Checked)
                {
                    float old_x = figure.Center.X, old_y = figure.Center.Y, old_z = figure.Center.Z;
                    figure.translate(-old_x, -old_y, -old_z);

                    float kx = (float)numericUpDown7.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown9.Value;
                    figure.scale(kx, ky, kz);

                    figure.translate(old_x, old_y, old_z);
                }
                else
                {
                    float kx = (float)numericUpDown7.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown9.Value;
                    figure.scale(kx, ky, kz);
                }
            }

            g.Clear(Color.White);
            figure.show(g, projection);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //Tetrahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.make_tetrahedron();
                    figure.show(g, projection);
                    break;
                case 1:
                    //Hexahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.make_hexahedron();
                    figure.show(g, projection);
                    break;
                case 2:
                    //Oktahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.make_octahedron();
                    figure.show(g, projection);
                    break;
                case 3:
                    //Icosahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.make_icosahedron();
                    figure.show(g, projection);
                    break;
                case 4:
                    //Dodecahedron
                    g.Clear(Color.White);
                    figure = new Polyhedron();
                    figure.make_dodecahedron();
                    figure.show(g, projection);
                    break;
                default:
                    break;
            }
        }

        // Camera projection

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            if (figure != null)
                figure.show(g, projection);
        }

        //Поворот вокруг прямой
        private void button4_Click(object sender, EventArgs e) => RotateAroundLine();

        private void RotateAroundLine()
        {
            Edge rotateLine = new Edge(
                                new Point3D(
                                    (float)numericUpDown11.Value,
                                    (float)numericUpDown12.Value,
                                    (float)numericUpDown13.Value),
                                new Point3D(
                                    (float)numericUpDown14.Value,
                                    (float)numericUpDown15.Value,
                                    (float)numericUpDown16.Value));

            double angle = (double)numericUpDown10.Value;
            figure.rotate(angle, rotateLineMode,rotateLine);

            g.Clear(Color.White);
            figure.show(g, projection);
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            rotateLineMode = (Axis)comboBox4.SelectedIndex;
            if (comboBox4.SelectedIndex != 3)
            {
                numericUpDown11.Enabled = false;
                numericUpDown12.Enabled = false;
                numericUpDown13.Enabled = false;
                numericUpDown14.Enabled = false;
                numericUpDown15.Enabled = false;
                numericUpDown16.Enabled = false;
            }
            else
            {
                numericUpDown11.Enabled = true;
                numericUpDown12.Enabled = true;
                numericUpDown13.Enabled = true;
                numericUpDown14.Enabled = true;
                numericUpDown15.Enabled = true;
                numericUpDown16.Enabled = true;
            }
        }

        // Отражение относительно плоскостей
        private void button3_Click(object sender, EventArgs e)
        {
            if (revertId == 0)
            {
                figure.reflectX();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
            else if (revertId == 1)
            {
                figure.reflectY();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
            else if (revertId == 2)
            {
                figure.reflectZ();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) => revertId = comboBox3.SelectedIndex;
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) => projection = (Projection)comboBox2.SelectedIndex;
    }
}
