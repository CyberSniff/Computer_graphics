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

            /*foreach (ShapePolygon polygon in current.polygons)
                foreach(ShapeLine line in polygon.lines)
                {
                    ShapePoint first = line.first;
                    ShapePoint second = line.second;
                    graphics.DrawLine(pen, (int)first.x, (int)first.y, (int)second.x, (int)second.y);
                }*/
            var lines = current.GetLines();
            foreach (ShapeLine line in lines)
                graphics.DrawLine(pen, (int)line.first.x, (int)line.first.y, (int)line.second.x, (int)line.second.y);

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
                current = Polyhedrons.CreateTetrahedron();
            }
            else if (radioButton2.Checked)
            {
                current = Polyhedrons.CreateHexahedron();
            }
            else if (radioButton3.Checked)
            {
                current = Polyhedrons.CreateOctahedron();

            }
            else if (radioButton4.Checked)
            {
                current = Polyhedrons.CreateIcosahedron();
            }
            else if (radioButton5.Checked)
            {
                current = Polyhedrons.CreateDodecahedron();
            }
            else
            {
                throw new Exception("Ошибка при рисовании фигуры");
            }
            DrawShape();
        }

        // Очистить экран
        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        // Применить аффинные пребразования
        private void button2_Click(object sender, EventArgs e)
        {
            // Выбрано смещение
            if (radioButton8.Checked)
            {

            }
            // Выбрано вращение
            else if (radioButton9.Checked)
            {

            }
            // Выбрано масштабирование
            else if (radioButton10.Checked)
            {

            }
            else
            {
                throw new Exception("Ошибка при применении афинных преобразований");
            }
        }

        // Маштабировать относительно центра
        private void button4_Click(object sender, EventArgs e)
        {

        }

        // Отразить относительно плоскости
        private void button3_Click(object sender, EventArgs e)
        {
            // Выбрана плоскость OXY
            if (radioButton11.Checked)
            {

            }
            // Выбрана плоскость OXZ
            else if (radioButton12.Checked)
            {

            }
            // Выбрана плоскость OYZ
            else if (radioButton13.Checked)
            {

            }
            else
            {
                throw new Exception("Ошибка при отражении отночительно плоскости");
            }
        }

        // Вращать вокруг прямой
        private void button5_Click(object sender, EventArgs e)
        {
            // Выбрана прямая, проходящая через центр многогранника и параллельная OX
            if (radioButton14.Checked)
            {

            }
            // Выбрана прямая, проходящая через центр многогранника и параллельная OY
            else if (radioButton15.Checked)
            {

            }
            // Выбрана прямая, проходящая через центр многогранника и параллельная OZ
            else if (radioButton16.Checked)
            {

            }
            // Выбрана произвольная прямая
            else if (radioButton17.Checked)
            {

            }
            else
            {
                throw new Exception("Ошибка при вращении вокруг прямой");
            }
        }

        // Выбрана аксонометрическая проекция
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        // Выбрана перспективная проекция
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
