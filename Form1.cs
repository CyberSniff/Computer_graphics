namespace CG_lab_test1
{
    public partial class Form1 : Form
    {
        private Graphics g;
        float leftX = -100;
        float rightX = 100;
        float step = 0.1f;
        public Form1()
        {
            
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            draw_axis();
           
        

        }
        private void draw_axis()
        {
            Pen p = new Pen(Color.Black);
            g.DrawLine(p, 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2);
            g.DrawLine(p, pictureBox1.Width/2, pictureBox1.Height, pictureBox1.Width/2, 0);
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
    
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            draw_func(choose_fun(textBox1.Text));
            pictureBox1.Image = bmp;

        }
        private Func<float, float> choose_fun(String ind)
        {
            if (ind == "1")
                return x => (float)Math.Sin((double)x);
            if (ind == "2")
                return x => x * x;
            if (ind == "3")
                    return x => (float)Math.Cos((double)x);
                else
                    return x => 0;
            
        }
        private void draw_func(Func<float, float> f)
        {
            List<PointF> points = new List<PointF>();

          
            points.Clear();
          

            float y_max = 0;
            for (float x = leftX; x < rightX; x += step)
            {
               float y = f(x);
                if (y > y_max)
                    y_max = y;
                points.Add(new PointF(x, y * (-1)));
            }
            rightX = (float)(Convert.ToInt32(textBox3.Text));
            leftX = (float)(Convert.ToInt32(textBox2.Text));
            float xScal = pictureBox1.Width / (rightX - leftX);
            float yScal;
            if (y_max != 0)
                yScal = pictureBox1.Height / (y_max * 2);
            else
                yScal = pictureBox1.Height;

          
            g.ScaleTransform(xScal, yScal);

           
            g.TranslateTransform(pictureBox1.Width / xScal - rightX, pictureBox1.Height / yScal / 2);

            Pen p = new Pen(Color.Black, 1 / xScal);
            g.DrawLine(p, new Point(-10000, 0), new Point(10000, 0));
            g.DrawLine(p, new Point(0, 10000), new Point(0, -10000));


            for (int i = 0; i < points.Count() - 1; ++i)
                g.DrawLine(p, points[i], points[i + 1]);
        }
        

    }
}
