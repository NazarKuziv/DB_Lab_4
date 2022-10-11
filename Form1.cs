using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DB_lab_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            db.openConnection();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            db.closeConnection();
           
        }


        private void button1_Click(object sender, EventArgs e)
        {
            db.cmd.CommandText = "SELECT * FROM [users] WHERE username = '" + textBox1.Text+"'";
            SqlDataReader reader = db.cmd.ExecuteReader();
            reader.Read();
            textBox2.Text = reader.GetValue(1).ToString();
            textBox3.Text = reader.GetValue(4).ToString();
            textBox4.Text = reader.GetValue(3).ToString();
            byte[] image = (byte[])reader.GetValue(2);
            reader.Close();
            if(image == null)
            {
                pictureBox1.Image = null;
            }
            else
            {
                MemoryStream ms = new MemoryStream(image);
                pictureBox1.Image = Image.FromStream(ms);
            }

        }
        byte[] img;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "ImageFiles(*.BMP; *.JPG; *.GIF; *.PNG)| *.BMP; *.JPG; *.GIF; *.PNG | All files(*.*) | *.* ";
            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    img = GetPhoto(open_dialog.FileName);
                    button2.BackColor = Color.Chartreuse;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromFile(open_dialog.FileName);
                }
                catch (Exception n)
                {
                    DialogResult rezult = MessageBox.Show(n.Message, n.Source,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
           
        }
        public static byte[] GetPhoto(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open,
            FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] photo = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return photo;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            textBox1.Visible = false;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            button2.Visible = true;
            button4.Visible = true;
            pictureBox1.Image = null;

        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox2.Text != " " || textBox3.Text != " " || textBox4.Text != " ")
            {
                try
                {
                    
                    db.cmd.CommandText = "Insert into [users] (username, photo, address, phonenumber) " +
                        "values(@uname, @photo, @address,@phone)";
                    db.cmd.Parameters.Add("@uname", SqlDbType.NVarChar, 100);
                    db.cmd.Parameters["@uname"].Value = textBox2.Text;
                    db.cmd.Parameters.Add("@photo", SqlDbType.Image, img.Length);
                    db.cmd.Parameters["@photo"].Value = img;
                    db.cmd.Parameters.Add("@address", SqlDbType.NVarChar, 255);
                    db.cmd.Parameters["@address"].Value = textBox4.Text;
                    db.cmd.Parameters.Add("@phone", SqlDbType.NVarChar, 10);
                    db.cmd.Parameters["@phone"].Value = textBox3.Text;
                    db.cmd.ExecuteNonQuery();
                    MessageBox.Show("База даних успішно оновлена!");
                    

                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    pictureBox1.Image = null;
                    button2.Visible = false;
                    button4.Visible = false;
                    button3.Visible = true;
                    button1.Visible = true;
                    textBox1.Visible = true;


                }
                catch (Exception n)
                {
                    MessageBox.Show(n.Message, n.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Заповніть порпжні поля!");
            }
            
           
        }

        
    }
}
