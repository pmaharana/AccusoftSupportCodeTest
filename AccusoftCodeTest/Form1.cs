using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccusoftCodeTest
{
    public partial class Form1 : Form
    {
        private System.Globalization.NumberFormatInfo m_cultNumber = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
        private Accusoft.ImagXpressSdk.Processor m_processorObject;
        private Accusoft.ImagXpressSdk.ImageX m_imagXpressObject;
        private Accusoft.ImagXpressSdk.LoadOptions m_loadOptions;
        private Accusoft.ImagXpressSdk.SaveOptions m_saveOptions;  
        private Size m_scaledSize;
        private string m_currentDir;
        private string m_filePath;
        private string m_fileType;
        private int m_numPages;
        enum e_FileType {  };
    
        public Form1()
        {
            InitializeComponent();
            m_loadOptions = new Accusoft.ImagXpressSdk.LoadOptions();
            imagXpress1.ProgressEvent += new Accusoft.ImagXpressSdk.ImagXpress.ProgressEventHandler(imagXpress1_ProgressEvent);
            
        }

        //Load File button
        private void button1_Click(object sender, EventArgs e)
        {
            ImageLoad(@"testimg.tif", 1);
            imageXView1.Image = m_imagXpressObject;
        }

        //Convert and Scale
        private void button2_Click(object sender, EventArgs e)
        {
            ConvertIX24();
            imageXView1.Image = m_imagXpressObject;
            ScaleIX();
            imageXView1.Image = m_imagXpressObject;
            ConvertIX4GS();
            imageXView1.Image = m_imagXpressObject;
        }

        //Save File format
        private void button3_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.ShowDialog();   // Get file name.
            //string name = saveFileDialog1.FileName;

            SaveIX("");
        }

        //Save Multi-page Tiff file
        private void button4_Click(object sender, EventArgs e)
        {
            SaveIXMultiPage(@"testimg.tif");
        }

        private Accusoft.ImagXpressSdk.ImageX ImageLoad(string p_fileName, int p_pages)
        {
            try
            {
                try
                {
                    m_currentDir = Directory.GetCurrentDirectory();
                    m_filePath = m_currentDir + @"\" + p_fileName;
                    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

                    string file = "";
                    if (result == DialogResult.OK) // Test result.
                    {
                        file = openFileDialog1.FileName;
                    }

                        if (m_imagXpressObject == null)
                    {
                       // m_imagXpressObject = Accusoft.ImagXpressSdk.ImageX.FromFile(imagXpress1, @"C:\testimg.tif", p_pages);
                        m_imagXpressObject = Accusoft.ImagXpressSdk.ImageX.FromFile(imagXpress1, file, p_pages);

                        listBox1.Items.Add("IX Object Loaded");
                        return m_imagXpressObject;
                    }
                    else
                        listBox1.Items.Add("Deleting IX object & reloading file");
                    m_imagXpressObject = null;
                    m_imagXpressObject = Accusoft.ImagXpressSdk.ImageX.FromFile(imagXpress1, @"C:\testimg.tif", p_pages);
                    listBox1.Items.Add("Image Loaded");
                    return m_imagXpressObject;

                }
                catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                {
                    AccusoftError(m_ex, label1);
                    throw;
                }
            }
            catch (Exception p_ex)
            {
                string _msg = String.Format("Loaded file fail. Ex Message: {0}", p_ex.Message);
                listBox1.Items.Add(_msg);
            }
            return null;
        }

        private Accusoft.ImagXpressSdk.ImageX ConvertIX24()
        {
            try
            {
                try
                {
                    m_processorObject = new Accusoft.ImagXpressSdk.Processor(imagXpress1, imageXView1.Image);
                    if (m_imagXpressObject != null && m_processorObject != null)
                    {
                        //Convert IX object to 24-bit Colordepth
                        m_processorObject.ColorDepth(24, Accusoft.ImagXpressSdk.PaletteType.Optimized, Accusoft.ImagXpressSdk.DitherType.BinarizePhotoHalftone);
                        listBox1.Items.Add("IX Object Converted to 24-bit Colordepth");
                    }
                    else
                    {
                        listBox1.Items.Add("IX Convert failed. No IX Object");
                        return m_imagXpressObject;
                    }
                }
                catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                {
                    AccusoftError(m_ex, label1);
                    throw;
                }
            }
            catch (Exception p_ex)
            {
                string _msg = String.Format("Convert failed. Ex Message: {0}", p_ex.Message);
                listBox1.Items.Add(_msg);
            }
            return m_imagXpressObject;
        }

        private Accusoft.ImagXpressSdk.ImageX ScaleIX()
        {
            try
            {
                try
                {
                    if (m_imagXpressObject != null && m_processorObject != null)
                    {
                        //Scale IX Object to 60% from original 
                        m_scaledSize = new System.Drawing.Size((int)imageXView1.Image.Width, (int)imageXView1.Image.Width);
                        m_scaledSize.Width = (int)((float)m_scaledSize.Width * 0.60f);
                        m_scaledSize.Height = (int)((float)m_scaledSize.Width * 0.60f);
                        m_processorObject.Resize(m_scaledSize, Accusoft.ImagXpressSdk.ResizeType.Quality);
                        listBox1.Items.Add("IX Object Scaled to 60%");
                    }
                    else
                    {
                        listBox1.Items.Add("IX Scale failed. No IX Object");
                        return m_imagXpressObject;
                    }
                }
                catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                {
                    AccusoftError(m_ex, label1);
                    throw;
                }
            }
            catch (Exception p_ex)
            {
                string _msg = String.Format("Scale failed. Ex Message: {0}", p_ex.Message);
                listBox1.Items.Add(_msg);
            }
            return m_imagXpressObject;
        }
        
        private Accusoft.ImagXpressSdk.ImageX ConvertIX4GS()
        {
            try
            {
                try
                {
                    m_processorObject = new Accusoft.ImagXpressSdk.Processor(imagXpress1, imageXView1.Image);
                    if (m_imagXpressObject != null && m_processorObject != null)
                    {
                        //Convert IX object to 4-bit Grayscale
                        m_processorObject.ColorDepth(4, Accusoft.ImagXpressSdk.PaletteType.Gray, Accusoft.ImagXpressSdk.DitherType.NoDither);
                        listBox1.Items.Add("IX Object Converted to 4-bit Grayscale");
                    }
                    else
                    {
                        listBox1.Items.Add("IX Convert failed. No IX Object");
                        return m_imagXpressObject;
                    }
                }
                catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                {
                    AccusoftError(m_ex, label1);
                    throw;
                }
            }
            catch (Exception p_ex)
            {
                string _msg = String.Format("Convert failed. Ex Message: {0}", p_ex.Message);
                listBox1.Items.Add(_msg);
            }
            return m_imagXpressObject;
        }

        private Accusoft.ImagXpressSdk.ImageX SaveIX(string p_fileType)
        {
            

            if (p_fileType != "")
            m_fileType = p_fileType;
            else
            m_fileType = comboBox1.Text;
            m_saveOptions = new Accusoft.ImagXpressSdk.SaveOptions();
            //imageXView1 = new Accusoft.ImagXpressSdk.ImageXView();
            switch (m_fileType)
           {
                case "JPEG":
                  try
                   {
                       try
                       {         
                           m_saveOptions.Format = Accusoft.ImagXpressSdk.ImageXFormat.Jpeg;
                           imageXView1.Image.Save(@"D:\test\testing.jpeg", m_saveOptions);
                           listBox1.Items.Add("File saved to JPEG file format.");
                           return m_imagXpressObject;
                       }
                        catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                        {
                            AccusoftError(m_ex, label1);
                            throw;
                        }
                    }
                   catch (Exception p_ex)
                   {
                       string _msg = String.Format("Saving file fail. Ex Message: {0}", p_ex.Message);
                       listBox1.Items.Add(_msg);
                   }
                  break;
                case "BMP":
                   try
                   {
                       try
                       {
                           m_saveOptions.Format = Accusoft.ImagXpressSdk.ImageXFormat.Bmp;
                           imageXView1.Image.Save(@"C:\testimg.bmp", m_saveOptions);
                           listBox1.Items.Add("File saved to BMP file format.");
                           return m_imagXpressObject;
                       }
                       catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                       {
                           AccusoftError(m_ex, label1);
                           throw;
                       }
                   }
                   catch (Exception p_ex)
                   {
                       string _msg = String.Format("Saving file fail. Ex Message: {0}", p_ex.Message);
                       listBox1.Items.Add(_msg);
                   }
                   break;
                case "TIFF":
                   try
                   {
                       try
                       {
                           m_saveOptions.Format = Accusoft.ImagXpressSdk.ImageXFormat.Tiff;
                           imageXView1.Image.Save(@"C:\testimg.tiff", m_saveOptions);
                           listBox1.Items.Add("File saved to TIFF file format.");
                           return m_imagXpressObject;
                       }
                       catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                       {
                           AccusoftError(m_ex, label1);
                           throw;
                       }
                   }
                   catch (Exception p_ex)
                   {
                       string _msg = String.Format("Saving file fail. Ex Message: {0}", p_ex.Message);
                       listBox1.Items.Add(_msg);
                   }
                   break;
                case "PNG":
                   try
                   {
                       try
                       {
                           m_saveOptions.Format = Accusoft.ImagXpressSdk.ImageXFormat.Png;
                           imageXView1.Image.Save(@"C:\testimg.png", m_saveOptions);
                           listBox1.Items.Add("File saved to PNG file format.");
                           return m_imagXpressObject;
                       }
                       catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                       {
                           AccusoftError(m_ex, label1);
                           throw;
                       }
                   }
                   catch (Exception p_ex)
                   {
                       string _msg = String.Format("Saving file fail. Ex Message: {0}", p_ex.Message);
                       listBox1.Items.Add(_msg);
                   }
                   break;
                case "MTIFF":
                   try
                   {
                       try
                       {
                           m_saveOptions.Tiff.MultiPage = true;
                           m_saveOptions.Format = Accusoft.ImagXpressSdk.ImageXFormat.Tiff;    
                           imageXView1.Image.Save(@"C:\testMimg.tiff", m_saveOptions);
                           listBox1.Items.Add("Multi-TIFF file format saved.");
                           return m_imagXpressObject;
                       }
                       catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                       {
                           AccusoftError(m_ex, label1);
                           throw;
                       }
                   }
                   catch (Exception p_ex)
                   {
                       string _msg = String.Format("Saving file fail. Ex Message: {0}", p_ex.Message);
                       listBox1.Items.Add(_msg);
                   }
                   break;
                default:
                   listBox1.Items.Add("Save Failed. Please select a File Type.");
                   break;
           }
            return m_imagXpressObject;
        }

        private Accusoft.ImagXpressSdk.ImageX SaveIXMultiPage(string p_fileName)
        {
            m_filePath = m_currentDir + @"\" + p_fileName;
            m_saveOptions = new Accusoft.ImagXpressSdk.SaveOptions();
            try
            {
                try
                {
                    m_numPages = Accusoft.ImagXpressSdk.ImageX.NumPages(imagXpress1, m_filePath);
                    for (int i = 1; i <= p_fileName.Length; i++)
                    {
                        imageXView1.Image = Accusoft.ImagXpressSdk.ImageX.FromFile(imagXpress1, m_filePath, i);
                        SaveIX("MTIFF");
                    }
                }
                catch (Accusoft.ImagXpressSdk.ImagXpressException m_ex)
                {
                    AccusoftError(m_ex, label1);
                    throw;
                }
            }
            catch (Exception p_ex)
            {
                string _msg = String.Format(" Please load image file. Ex Message: {0}", p_ex.Message);
                listBox1.Items.Add(_msg);
            }
            return null;
        }

        static void AccusoftError(Accusoft.ImagXpressSdk.ImagXpressException ErrorException, System.Windows.Forms.Label ErrorLabel)
        {
            ErrorLabel.Text = ErrorException.Message + "\n" + ErrorException.Source + "\n" + "Error Number: " + ErrorException.Number.ToString(System.Globalization.CultureInfo.CurrentCulture.NumberFormat);
        }

        private void imagXpress1_ProgressEvent(object sender, Accusoft.ImagXpressSdk.ProgressEventArgs e)
        {
            listBox1.Items.Add(e.PercentDone.ToString(m_cultNumber) + "% Process Complete.");
            if (e.IsComplete)
            {
                listBox1.Items.Add(e.TotalBytes + " Bytes Completed.");
            }
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //TODO: Please add at least one function
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
