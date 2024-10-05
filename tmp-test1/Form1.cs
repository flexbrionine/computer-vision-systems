﻿using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace tmp_test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                    var image = new Image<Bgr, byte>(fileDialog.FileName);
                    pictureBox1.Image = image.ToBitmap();
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
            statusLabel.ForeColor = Color.Black;
            statusLabel.Text = "Image has been removed";
        }

    }
}
