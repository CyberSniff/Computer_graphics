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
    public enum Projection { PERSPECTIVE = 0, ISOMETRIC };

    public partial class Form1 : Form
    {
        Graphics g;
        Projection projection = 0;
        Axis rotateLineMode = 0;
        Polyhedron figure = null;
       

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
                MessageBox.Show("Выберите фигуру!", "Ошибка!", MessageBoxButtons.OK);
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
                float kx = (float)numericUpDown7.Value, ky = (float)numericUpDown8.Value, kz = (float)numericUpDown9.Value;
                figure.scale(kx, ky, kz);
                
            }

            g.Clear(Color.White);
            figure.show(g, projection);
        }



        // Camera projection

        private void button2_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            if (radioButton4.Checked)
            { projection = Projection.ISOMETRIC; }
            if (radioButton5.Checked)
            { projection = Projection.PERSPECTIVE; }
            if (figure != null)
                figure.show(g, projection);
        }

        //Поворот вокруг прямой
        private void button4_Click(object sender, EventArgs e) => RotateAroundLine();

        private void RotateAroundLine()
        {
            ShapeLine rotateLine = new ShapeLine(
                                new ShapePoint(
                                    (float)numericUpDown11.Value,
                                    (float)numericUpDown12.Value,
                                    (float)numericUpDown13.Value),
                                new ShapePoint(
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
            
            if (radioButton6.Checked)
            {
                rotateLineMode = Axis.AXIS_X;
            }
            if (radioButton7.Checked)
            {
                rotateLineMode = Axis.AXIS_Y;
            }
            if (radioButton8.Checked)
            {
                rotateLineMode = Axis.AXIS_Z;
            }
            
        }

        // Отражение относительно плоскостей
        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton10.Checked)
            {
                figure.reflectX();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
            if (radioButton11.Checked)
            {
                figure.reflectY();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
            if (radioButton12.Checked)
            {
                figure.reflectZ();
                g.Clear(Color.White);
                figure.show(g, projection);
            }
        }

       
        

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                g.Clear(Color.White);
                figure = new Polyhedron();
                figure.makeTetrahedron();
                figure.show(g, projection);
            }
            if (radioButton2.Checked)
            {
                g.Clear(Color.White);
                figure = new Polyhedron();
                figure.makeHexahedron();
                figure.show(g, projection);
            }
            if (radioButton3.Checked)
            {
                g.Clear(Color.White);
                figure = new Polyhedron();
                figure.makeOctahedron();
                figure.show(g, projection);
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
        }

      

        private void button7_Click(object sender, EventArgs e)
        {
            float kx = (float)numericUpDown17.Value, ky = (float)numericUpDown17.Value, kz = (float)numericUpDown17.Value;
            figure.scale(kx, ky, kz);
            g.Clear(Color.White);
            figure.show(g, projection);
        }
    }
}
