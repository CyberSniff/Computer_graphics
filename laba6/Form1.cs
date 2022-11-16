using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laba6
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private Pen pen;
        private ShapePolyhedron current;
        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); // 800; 800
            graphics = Graphics.FromImage(pictureBox1.Image);
            pen = new Pen(Color.Black, 2);
            Clear();
        }
        private void DrawShape()
        {
            Clear();

            foreach (ShapePolygon polygon in current.polygons)
                foreach(ShapeLine line in polygon.lines)
                {
                    ShapePoint first = line.first;
                    ShapePoint second = line.second;
                    graphics.DrawLine(pen, (int)first.x, (int)first.y, (int)second.x, (int)second.y);
                }
            pictureBox1.Invalidate();
        }

        private void Clear()
        {
            /*using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                graphics.FillRectangle(brush, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }*/
            graphics.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        // Нарисовать фигуру
        private void button6_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
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

                current = new ShapePolyhedron(pol_1, pol_2, pol_3, pol_4, pol_5, pol_6);
            }
            else if (radioButton2.Checked)
            {
                double l = 200;
                double h = l * Math.Sqrt(3) / 2;
                double x = 300;
                double y = 600;
                double z = 0;

                ShapePoint p_1 = new ShapePoint(x, y, z);
                /*
                ShapePoint p_2 = new ShapePoint(x + l, y, z + l);
                ShapePoint p_3 = new ShapePoint(x + l, y - l, z);
                ShapePoint p_4 = new ShapePoint(x, y - l, z + l);
                */
                ShapePoint p_2 = new ShapePoint(x + l, y, z);
                ShapePoint p_3 = new ShapePoint(x + l / 2, y - h, z);
                ShapePoint p_4 = new ShapePoint(x + l / 2, y - h / 2, z + h);

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

                current = new ShapePolyhedron(pol_1, pol_2, pol_3, pol_4);
            }
            else if (radioButton3.Checked)
            {

            }
            else if (radioButton4.Checked)
            {

            }
            else if (radioButton5.Checked)
            {

            }
            else
            {
                throw new Exception("Drawing Shape Error");
            }
            DrawShape();
        }

        // Нарисовать тетраэдр
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                


            }
        }

        // Очистить экран
        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        // Выбрано смещение
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрано вращение
        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрано масштабирование
        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Применить аффинные пребразования
        private void button2_Click(object sender, EventArgs e)
        {

        }

        // Применить масштабирование относительно центра
        private void button4_Click(object sender, EventArgs e)
        {

        }

        // Выбрана плоскость OXY
        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана плоскость OXZ
        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана плоскость OYZ
        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Отразить относительно плоскости
        private void button3_Click(object sender, EventArgs e)
        {

        }

        // Выбрана прямая, проходящая через центр многогранника и параллельная OX
        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана прямая, проходящая через центр многогранника и параллельная OY
        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана прямая, проходящая через центр многогранника и параллельная OZ
        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана произвольная прямая
        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Вращать вокруг прямой
        private void button5_Click(object sender, EventArgs e)
        {

        }

        
    }
}
