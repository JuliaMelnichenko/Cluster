using System;
using System.Drawing;
using System.Windows.Forms;

//Выделение областей со схожими цветовыми характеристиками

namespace ClusterColor
{
    public partial class Cluster : Form
    {
       Bitmap image;
       int[] mas_color;

        public Cluster()
        {
            InitializeComponent();
            image = new Bitmap(pictureBox2.Width, pictureBox2.Height);
        }
        
        //пиксели с близко стоящими значениями объединяются в группу, где расстояние между группами 4 
        public void Average (Bitmap Im, int length, char canal)
        {
            int middlepixel = 0; //центр
            middlepixel = Im.GetPixel(pictureBox2.Width / 2, pictureBox2.Height / 2).G;
            int count = 0;
            int countofcolor = 0; //кол-во цветов
            countofcolor = Math.Abs(256 / length); //расстояние
            mas_color = new int[countofcolor];

           progressBar1.PerformStep();

            for (int i = 0; i < countofcolor; i++)
                mas_color[i] = 0;

            if (canal == 'A')
            {
                for (int i = 0; i < Im.Width; i++)
                    for (int j = 0; j < Im.Height; j++)
                        if (Im.GetPixel(i, j).A != Color.Black.A)
                        {
                            mas_color[Math.Abs(Im.GetPixel(i, j).A / length)]++;
                            count++;
                        }
                progressBar1.PerformStep();
            }

            if (canal == 'R')
            {
                for (int i = 0; i < Im.Width; i++)
                    for (int j = 0; j < Im.Height; j++)
                        if (Im.GetPixel(i, j).R != Color.Black.R)
                        {
                            mas_color[Math.Abs(Im.GetPixel(i, j).R / length)]++;
                            count++;
                        }
                progressBar1.PerformStep();
            }

            if (canal == 'G')
            {
                for (int i = 0; i < Im.Width; i++)
                    for (int j = 0; j < Im.Height; j++)
                        if (Im.GetPixel(i, j).G != Color.Black.G)
                        {
                            mas_color[Math.Abs(Im.GetPixel(i, j).G / length)]++;
                            count++;
                        }
                progressBar1.PerformStep();
            }

            if (canal == 'B')
            {
                for (int i = 0; i < Im.Width; i++)
                    for (int j = 0; j < Im.Height; j++)
                        if (Im.GetPixel(i, j).B != Color.Black.B)
                        {
                            mas_color[Math.Abs(Im.GetPixel(i, j).B / length)]++;
                            count++;
                        }
                progressBar1.PerformStep();
            }

           progressBar1.PerformStep();
        }

        //при разделении альфа-каналов синий лучше разделяет на группы 
        public void Blue(Bitmap Im)
        {
            int count = 0;
            count = 0;
            Average (Im, 4, 'B');
            int maxColor = mas_color[0];
            int maxIndex = 0;

            progressBar1.PerformStep();

            for (int i = 1; i < 256 / 4; i++)
                if (maxColor < mas_color[i])
                {
                    maxColor = mas_color[i];
                    maxIndex = i;
                }

            progressBar1.PerformStep();

            for (int i = 0; i < Im.Width; i++)
                for (int j = 0; j < Im.Height; j++)
                    if (maxIndex > (Im.GetPixel(i, j).B) / 4)
                    {
                        Im.SetPixel(i, j, Color.Black);
                    }

            progressBar1.PerformStep();

            for (int i = 0; i < Im.Width; i++)
                for (int j = 0; j < Im.Height; j++)
                    if (Im.GetPixel(i, j) == Color.Black)
                        count++;
            progressBar1.PerformStep();
        }
      
        
        //выбор файла
        private void open(ref Bitmap image)
        {
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
          
            if (openfiledialog.ShowDialog() == DialogResult.OK)
            {
                progressBar1.PerformStep();
                try
                {
                    image = new Bitmap(openfiledialog.FileName);
                    pictureBox1.Image = Image.FromFile(openfiledialog.FileName);
                    //pictureBox1.Image = new Bitmap(openfiledialog.FileName);
                    
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                    progressBar1.PerformStep();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            progressBar1.PerformStep();
        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            open(ref image);
            Blue(image);
            pictureBox2.Image = image;
            progressBar1.Value = 0;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            progressBar1.Maximum = 1;
           
          
        }
    }
}
