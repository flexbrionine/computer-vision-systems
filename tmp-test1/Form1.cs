using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace tmp_test1
{
    public partial class Form1 : Form
    {
        VideoCapture cam;
        Image<Bgr, byte> image1;
        Image<Bgr, byte> image2;
        Image<Bgr, byte> image1_scope;
        Image<Bgr, byte> image2_scope;
        Image<Bgr, byte> image_lut;

        byte[] lutTab = new byte[256];

        private enum LutOper
        {
            IDENTITY,
            INVERT,
            BRIGHTNESS,
            BIN_P1,
            BIN_P1_P2,
            CONTRAST
        }
        LutOper chosenOperation = LutOper.IDENTITY;

        public Form1()
        {
            InitializeComponent();

            image1 = new Image<Bgr, byte>(320, 230);
            image2 = new Image<Bgr, byte>(320, 230);
            image1_scope = new Image<Bgr, byte>(320, 230);
            image2_scope = new Image<Bgr, byte>(320, 230);
            image_lut = new Image<Bgr, byte>(pictureBox_lut.Size);

            try
            {
                cam = new VideoCapture(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                statusLabel.BackColor = Color.Red;
                statusLabel.Text = "! Error while opening camera usb connection";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp;)|*.jpg; *.png; *.bmp;";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    var originalImage = new Image<Bgr, byte>(fileDialog.FileName);
                    pictureBox1.Image = originalImage.ToBitmap();
                    var changedImage = new Image<Gray, byte>(fileDialog.FileName);
                    pictureBox2.Image = changedImage.ToBitmap();

                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Image has been loaded";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                statusLabel.BackColor = Color.Red;
                statusLabel.Text = "! Error while opening image";
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            statusLabel.ForeColor = Color.Black;
            statusLabel.Text = "Images have been removed";
        }

        private void button_grafika_Click(object sender, EventArgs e)
        {
            // circle
            CvInvoke.Circle(image1, new Point(160, 120), 66, new MCvScalar(150, 100, 0), -1);
            
            //rectangle
            Rectangle rect = new Rectangle(20, 20, 150, 150);
            CvInvoke.Rectangle(image1, rect, new MCvScalar(90, 10, 185), -1);

            // line
            CvInvoke.Line(image1, new Point(20, 20), new Point(50, 50), new MCvScalar(0, 255, 150));
            CvInvoke.Line(image1, new Point(20, 40), new Point(70, 50), new MCvScalar(0, 255, 150));
            CvInvoke.Line(image1, new Point(20, 60), new Point(90, 50), new MCvScalar(0, 255, 150));

            pictureBox1.Image = image1.ToBitmap();
        }

        private void button_czysc_Click(object sender, EventArgs e)
        {
            image1.SetZero();
            pictureBox1.Image = image1.Bitmap;
        }

        private void button_zPliku_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp;)|*.jpg; *.png; *.bmp;";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Mat tmp = new Mat();
                    tmp = CvInvoke.Imread(fileDialog.FileName);
                    //var originalImage = new Image<Bgr, byte>(fileDialog.FileName);
                    CvInvoke.Resize(tmp, tmp, pictureBox1.Size);
                    pictureBox1.Image = tmp.Bitmap;
                    image1 = tmp.ToImage<Bgr, byte>();

                    //var changedImage = new Image<Gray, byte>(fileDialog.FileName);
                    //pictureBox2.Image = changedImage.ToBitmap();

                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Image has been loaded";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                statusLabel.BackColor = Color.Red;
                statusLabel.Text = "! Error while opening image";
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox_x.Text = e.X.ToString();
            textBox_y.Text = e.Y.ToString();

            byte[,,] data = image1.Data;
            textBox_r.Text = data[e.Y, e.X, 2].ToString();
            textBox_g.Text = data[e.Y, e.X, 1].ToString();
            textBox_b.Text = data[e.Y, e.X, 0].ToString();
        }

        private void button_zKamery_Click(object sender, EventArgs e)
        {
            Mat tmp = new Mat();
            cam.Read(tmp);
            CvInvoke.Resize(tmp, tmp, pictureBox1.Size);
            pictureBox1.Image = tmp.Bitmap;
            image1 = tmp.ToImage<Bgr, byte>();
        }

        private void button2_grafika_Click(object sender, EventArgs e)
        {
            // circle
            CvInvoke.Circle(image2, new Point(160, 120), 66, new MCvScalar(10, 100, 230), -1);

            //rectangle
            Rectangle rect = new Rectangle(20, 20, 150, 150);
            CvInvoke.Rectangle(image2, rect, new MCvScalar(0, 150, 185), -1);

            // line
            CvInvoke.Line(image2, new Point(0, 0), new Point(320, 230), new MCvScalar(0, 255, 150));
            CvInvoke.Line(image2, new Point(320, 0), new Point(0, 230), new MCvScalar(0, 255, 150));
            CvInvoke.Line(image2, new Point(60, 60), new Point(120, 60), new MCvScalar(0, 255, 150));

            pictureBox2.Image = image2.ToBitmap();
        }

        private void button2_zPliku_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files (*.jpg; *.png; *.bmp;)|*.jpg; *.png; *.bmp;";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Mat tmp = new Mat();
                    tmp = CvInvoke.Imread(fileDialog.FileName);
                    //var originalImage = new Image<Bgr, byte>(fileDialog.FileName);
                    CvInvoke.Resize(tmp, tmp, pictureBox1.Size);
                    pictureBox2.Image = tmp.Bitmap;
                    image2 = tmp.ToImage<Bgr, byte>();

                    //var changedImage = new Image<Gray, byte>(fileDialog.FileName);
                    //pictureBox2.Image = changedImage.ToBitmap();

                    statusLabel.ForeColor = Color.Green;
                    statusLabel.Text = "Image has been loaded";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                statusLabel.BackColor = Color.Red;
                statusLabel.Text = "! Error while opening image";
            }
        }

        private void button2_czysc_Click(object sender, EventArgs e)
        {
            image2.SetZero();
            pictureBox2.Image = image2.Bitmap;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBox2_x.Text = e.X.ToString();
            textBox2_y.Text = e.Y.ToString();
        }

        private void button2_zKamery_Click(object sender, EventArgs e)
        {
            Mat tmp = new Mat();
            cam.Read(tmp);
            CvInvoke.Resize(tmp, tmp, pictureBox2.Size);
            pictureBox2.Image = tmp.Bitmap;
            image2 = tmp.ToImage<Bgr, byte>();
        }

        private void copySelective(Image<Bgr, byte> src, Image<Bgr, byte> dst)
        {
            byte maskR = (byte)(checkBox_r.Checked ? 0xFF : 0x00);
            byte maskG = (byte)(checkBox_g.Checked ? 0xFF : 0x00);
            byte maskB = (byte)(checkBox_b.Checked ? 0xFF : 0x00);

            byte[,,] srcData = src.Data;
            byte[,,] dstData = dst.Data;
            for (int x = 0; x < src.Width; x++)
            {
                for (global::System.Int32 y = 0; y < src.Height; y++)
                {
                    dstData[y,x,0] = (byte)(maskB & srcData[y,x,0]);
                    dstData[y,x,1] = (byte)(maskG & srcData[y,x,1]);
                    dstData[y,x,2] = (byte)(maskR & srcData[y,x,2]);
                }
            }
        }

        private void copyMono(Image<Bgr, byte> src, Image<Bgr, byte> dst)
        {
            byte[,,] srcData = src.Data;
            byte[,,] dstData = dst.Data;

            for (int x = 0; x < src.Width; x++)
            {
                for (global::System.Int32 y = 0; y < src.Height; y++)
                {
                    int mono = (srcData[y,x,0] + srcData[y, x, 1] + srcData[y, x, 2])/3;
                    dstData[y, x, 0] = (byte)mono;
                    dstData[y, x, 1] = (byte)mono;
                    dstData[y, x, 2] = (byte)mono;
                }
            }
        }

        private void button_toRight_Click(object sender, EventArgs e)
        {
            copySelective(image1, image2);
            pictureBox2.Image = image2.Bitmap;
        }

        private void button_toLeft_Click(object sender, EventArgs e)
        {
            copySelective(image2, image1);
            pictureBox1.Image = image1.Bitmap;
        }

        private void button_motoToRight_Click(object sender, EventArgs e)
        {
            copyMono(image1, image2);
            pictureBox2.Image = image2.Bitmap;
        }

        private void button_motoToLeft_Click(object sender, EventArgs e)
        {
            copyMono(image2, image1);
            pictureBox1.Image = image1.Bitmap;
        }

        private void analyseScope(Image<Bgr, byte> src, Image<Bgr, byte> dst)
        {
            int scopeY = Convert.ToInt32(textBox_y.Text);
            int scopeHeight = pictureBox1_scope.Height;

            double scaleY = ((scopeHeight - 1) / 255.0);

            dst.SetZero();
            byte[,,] srcData = src.Data;
            byte[,,] dstData = dst.Data;

            int calcY;
            for (int x = 0; x < src.Width; x++)
            {
                calcY = (int)(srcData[scopeY, x, 0] * scaleY);
                calcY = (scopeHeight - 1) - calcY;
                dstData[calcY, x, 0] = (byte)calcY;

                calcY = (int)(srcData[scopeY, x, 1] * scaleY);
                calcY = (scopeHeight - 1) - calcY;
                dstData[calcY, x, 1] = (byte)calcY;

                calcY = (int)(srcData[scopeY, x, 2] * scaleY);
                calcY = (scopeHeight - 1) - calcY;
                dstData[calcY, x, 2] = (byte)calcY;
            }
        }

        private void button_wykresLinii_Click(object sender, EventArgs e)
        {
            analyseScope(image1, image2);
            pictureBox1_scope.Image = image2.Bitmap;
        }

        private void button2_wykresLinii_Click(object sender, EventArgs e)
        {
            analyseScope(image2, image1);
            pictureBox2_scope.Image = image1.Bitmap;
        }

        private void radioButton_selection_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_tozsamosc.Checked)
            {
                chosenOperation = LutOper.IDENTITY;
            }
            else if (radioButton_negatyw.Checked)
            {
                chosenOperation = LutOper.INVERT;
            }
            else if (radioButton_jasnoscP1.Checked)
            {
                chosenOperation = LutOper.BRIGHTNESS;
            }
            else if (radioButton_progowanieP1P2.Checked)
            {
                chosenOperation = LutOper.BIN_P1_P2;
            }
            else if (radioButton_progowanieP1.Checked)
            {
                chosenOperation = LutOper.BIN_P1;
            }
            else if (radioButton_kontrast.Checked)
            {
                chosenOperation = LutOper.CONTRAST;
            }
        }

        private void button_init_Click(object sender, EventArgs e)
        {
            initLutTable();
            drawLutTable();
        }

        private void initLutTable()
        {
            switch (chosenOperation)
            {
                case LutOper.IDENTITY:
                    {
                        for (global::System.Int32 i = 0; i < 256; i++)
                        {
                            lutTab[i] = (byte)i;
                        }
                    }
                    break;
                case LutOper.INVERT:
                    {
                        for (global::System.Int32 i = 0; i < 256; i++)
                        {
                            lutTab[i] = (byte)(256-i);
                        }
                    }
                    break;
                case LutOper.BRIGHTNESS:
                    {
                        int brithness = (int)numericUpDown_P1.Value;
                        for (global::System.Int32 i = 0; i < 256; i++)
                        {
                            if (i + brithness > 256)
                            {
                                lutTab[i] = 255;
                            }
                            else
                            {
                                lutTab[i] = (byte)(i + brithness);
                            }
                        }
                    }
                    break;
                case LutOper.BIN_P1:
                    {

                    }
                    break;
                case LutOper.BIN_P1_P2:
                    {

                    }
                    break;
                case LutOper.CONTRAST:
                    {

                    }
                    break;
                default:
                    break;
            }
        }

        private void drawLutTable()
        {
            double sX, sY;
            sX = (pictureBox_lut.Width - 1) / 255.0;
            sY = (pictureBox_lut.Height - 1) / 255.0;
            int height = pictureBox_lut.Height - 1;
            image_lut.SetValue(new Bgr(255, 255, 255));

            byte[,,] data = image_lut.Data;
            for (int x = 0; x < lutTab.Length; x++)
            {
                int Y = (int)(height - lutTab[x] * sY);
                int X = (int)(x * sX);
                data[Y,X,0] = 0;
                data[Y,X,1] = 0;
                data[Y,X,2] = 0;
            }
            pictureBox_lut.Image = image_lut.Bitmap;
        }

        private void numericUpDown_P1_ValueChanged(object sender, EventArgs e)
        {
            button_init_Click(sender, e);
        }
    }
}
