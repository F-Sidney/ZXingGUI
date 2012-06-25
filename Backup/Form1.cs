using System;
using System.Drawing;
using System.Windows.Forms;
using com.google.zxing;
using COMMON = com.google.zxing.common;
namespace zxingGUI
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Image img = Image.FromFile(this.openFileDialog1.FileName);                        
            Bitmap bmap;
            try
            {
                bmap = new Bitmap(img);
            }
            catch (System.IO.IOException ioe)
            {
                MessageBox.Show(ioe.ToString());
                return;
            }

            if (bmap == null)
            {
                MessageBox.Show("Could not decode image");
                return;
            }

            LuminanceSource source = new RGBLuminanceSource(bmap, bmap.Width, bmap.Height);
            com.google.zxing.BinaryBitmap bitmap = new com.google.zxing.BinaryBitmap(new COMMON.HybridBinarizer(source));
            Result result;
            try
            {
                result = new MultiFormatReader().decode(bitmap);
            }
            catch(ReaderException re)
            {
                MessageBox.Show(re.ToString());
                return;
            }
            
            MessageBox.Show(result.Text);        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sFD = new SaveFileDialog();
            sFD.DefaultExt = "*.png|*.png";
            sFD.AddExtension = true;            
            
            try
            {
                if (sFD.ShowDialog() == DialogResult.OK)
                {
                    string content = @"url:http://writeblog.csdn.net/PostEdit.aspx; name:nickwar";
                    COMMON.ByteMatrix byteMatrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, 350, 350);
                    writeToFile(byteMatrix, System.Drawing.Imaging.ImageFormat.Png, sFD.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void writeToFile(COMMON.ByteMatrix matrix, System.Drawing.Imaging.ImageFormat format, string file)
        {
            System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters();            
            eps.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100L);
            Bitmap bmap = toBitmap(matrix);
            bmap.Save(file,format);
        }

        public static Bitmap toBitmap(COMMON.ByteMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }

            return bmap;            
        }
    }
}
