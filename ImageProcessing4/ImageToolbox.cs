using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing4
{
    class ImageToolbox
    {

        //Folder

            private static String pathResFolder = @"C:";
            private static String pathSaveFolder = @"C:";
            private static String pathRes = null;
        
            public static Image GetRandomImage()
            {
                pathRes = FindFolder("Res", @"C:\Users\letri\OneDrive\Plocha\ImageFolder");
                string[] filesNames = Directory.GetFiles(pathRes);
                Image image;
                if (filesNames.Length != 0)
                {
                    Random random = new Random();
                    image = Image.FromFile(filesNames[random.Next(0, filesNames.Length)]);
                    return image;
                }
                else
                {
                    Console.WriteLine("MISTAKE: THE IMG WAS NOT FINDED!");
                    return null;
                }
            }
            private static String FindFolder(String folderName, String folder)
            {
                //Jesli existuje složka
                if (Directory.Exists(folder + @"\" + folderName))
                {
                    //Zapiš cestu ke složce a vrať
                    String finalPath = folder + @"\" + folderName;
                    return finalPath;
                }
                else if (System.IO.Directory.GetDirectories(folder).Length != 0)
                {
                    //Složka není prázdná
                    
                        //Dej všechny složky v aktuální složce
                        String[] dirs = System.IO.Directory.GetDirectories(folder);

                        //Prohledávací cyklus
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            //rekurence 
                            String path = FindFolder(folderName, dirs[i]);

                            //jestli path není null vrať path
                            if (path != null)
                            {
                                return path;
                            }
                        }
                }
                else
                {
                    //složka je prázdná
                    Console.WriteLine("folder:\t" + folder + "\tis empty");
                }

                //vrať null, protože složka nebyla nalezena
                return null;
            }


        //Image 

            public static Bitmap ResizeImage(Image image, int width, int height)
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            public static Image Grayscale(Image image)
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height);
                ImageAttributes imageAttributes = new ImageAttributes();
                float r = 0.2989f;
                float g = 0.5870f;
                float b = 0.1140f;

                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[]{ r, r, r, 0, 0},
                    new float[]{ g, g, g, 0, 0},
                    new float[]{ b, b, b, 0, 0},
                    new float[]{ 0, 0, 0, 1, 0},
                    new float[]{ 0, 0, 0, 0, 0},
                });
                imageAttributes.SetColorMatrix(colorMatrix);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                graphics.Dispose();
                return bitmap;
            }
            public static Image OpenImage()
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                folderBrowser.Description = @"C:";

                OpenFileDialog oFile = new OpenFileDialog();
                oFile.Filter = "|*.jpg||*.jpeg||*.png||*.bmp||*.tiff||*.wmf||*.gif";
                oFile.InitialDirectory = pathResFolder;
                Image image = null;
                if (DialogResult.OK == oFile.ShowDialog())
                {
                    image = new Bitmap(oFile.FileName);
                }

                return image;
            }
            public static void SaveImage(Image image)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "|*.jpg||*.jpeg||*.png||*.bmp||*.tiff||*.wmf||*.gif";
                saveFileDialog.InitialDirectory = pathSaveFolder;
                ImageFormat imgFormat = GetImageFormat(image);
                Console.WriteLine(imgFormat);
                //image.Save(@"C:\Users\letri\OneDrive\Plocha\ImageFolder\ahoj."+imgFormat., imgFormat);
                //Save(@"D:\Temp\xx.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);

                DialogResult save = saveFileDialog.ShowDialog();
                if (save == DialogResult.OK)
                {
                    if (true)
                    {
                        if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 3).ToLower() == "bmp")
                        {
                            image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                        }
                        if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 3).ToLower() == "jpg")
                        {
                            image.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                        }
                        if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 3).ToLower() == "png")
                        {
                            image.Save(saveFileDialog.FileName, ImageFormat.Png);
                        }
                        if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 4).ToLower() == "tiff")
                        {
                            image.Save(saveFileDialog.FileName, ImageFormat.Tiff);
                        }
                        if (saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length - 3).ToLower() == "gif")
                        {
                            image.Save(saveFileDialog.FileName, ImageFormat.Gif);
                        }
                    }

                }
            }
            public static Image Negative(Image image)
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                 {
                    new float[]{ -1, 0,  0,  0, 0},
                    new float[]{ 0,  -1, 0,  0, 0},
                    new float[]{ 0,  0,  -1, 0, 0},
                    new float[]{ 0,  0,  0,  1, 0},
                    new float[]{ 1,  1,  1,  0, 0},
                 });
                imageAttributes.SetColorMatrix(colorMatrix);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
                graphics.Dispose();
                return bitmap;
            }
            public static ImageFormat GetImageFormat(Image img)
            {
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                    return System.Drawing.Imaging.ImageFormat.Png;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                    return System.Drawing.Imaging.ImageFormat.Emf;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                    return System.Drawing.Imaging.ImageFormat.Exif;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                    return System.Drawing.Imaging.ImageFormat.Gif;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                    return System.Drawing.Imaging.ImageFormat.Icon;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
                    return System.Drawing.Imaging.ImageFormat.MemoryBmp;
                if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                    return System.Drawing.Imaging.ImageFormat.Tiff;
                else
                    return System.Drawing.Imaging.ImageFormat.Wmf;
            }


        //FFT

            public static Bitmap CentredTwoDFFT(Bitmap bitmap, float frequency, int mode, int color_mode)
            {
                Complex[,] complex = ConvertBitmapToComplexAr(bitmap, color_mode);
                complex = TwoDFFT(complex, bitmap.Width, bitmap.Height);

                double[,] db_ar = TwoDFFTShiftToCenter(complex, bitmap.Width, bitmap.Height, mode);
                return BitmapFromDouble(db_ar, bitmap.Width, bitmap.Height, frequency);
            }
            private static Complex[,] TwoDFFT(Complex[,] complex, int width, int height)
            {
                Complex[,] res = new Complex[width, height];
                Complex[] row = new Complex[width];
                Complex[] colum = new Complex[height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        colum[y] = complex[x, y];
                    }
                    Fft.Transform(colum, false);
                    for (int y = 0; y < height; y++)
                    {
                        res[x, y] = colum[y];
                    }
                }
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        row[x] = res[x, y];
                    }
                    Fft.Transform(row, false);
                    for (int x = 0; x < width; x++)
                    {
                        res[x, y] = row[x];
                    }
                }

                return res;
            }
            private static Bitmap BitmapFromDouble(double[,] double_ar, int width, int height, float frequency)
            {
                Bitmap bitmap = new Bitmap(width, height);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int a = (int)(frequency * Math.Pow(double_ar[x, y], 1));
                        if (a > 255)
                        {
                            a = 255;
                        }
                        bitmap.SetPixel(x, y, Color.FromArgb(255, a, a, a));
                    }
                }
                return bitmap;
            }
            private static double[,] TwoDFFTShiftToCenter(Complex[,] complex, int width, int height, int mode)
            {
                double[,] double_ar = new double[width, height];

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        switch (mode)
                        {
                            //Magnitude
                            case 0:
                                double_ar[x, y] = (Math.Log(1 + complex[x, y].Magnitude));
                                break;
                            //Real
                            case 1:
                                double_ar[x, y] = (Math.Log(1 + Math.Abs(complex[x, y].Real)));
                                break;
                            //Imaginary
                            case 2:
                                double_ar[x, y] = (Math.Log(1 + Math.Abs(complex[x, y].Imaginary)));
                                break;
                            //Phase
                            case 3:
                                double_ar[x, y] = (Math.Log(1 + Math.Abs(complex[x, y].Phase)));
                                break;
                        }

                    }
                }

                //double[,] a = Shifting2DArrayToCenter(double_ar, width, height);
                double[,] a = FFTShift(double_ar, width, height);
                WriteToConsoleArray(a, width, height);
                return a;

            }
            private static Complex[,] ConvertBitmapToComplexAr(Bitmap bitmap, int color_mode)
            {
                Complex[,] complex = new Complex[bitmap.Width, bitmap.Height];
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        switch (color_mode)
                        {
                            //red
                            case 0:
                                complex[i, j] = new Complex(bitmap.GetPixel(i, j).R, 0);
                                break;
                            //green
                            case 1:
                                complex[i, j] = new Complex(bitmap.GetPixel(i, j).G, 0);
                                break;
                            //blue
                            case 2:
                                complex[i, j] = new Complex(bitmap.GetPixel(i, j).B, 0);
                                break;

                        }

                    }
                }
                return complex;
            }
            private static double[,] FFTShift(double[,] double_ar_in, int width, int height)
            {
                double[,] res = new double[width, height];
                double[,] double_ar = double_ar_in;

                //hirizontal shift 
                int shift_y, shift_x;
                if (height % 2 == 0)
                {
                    shift_y = height / 2;
                }
                else
                {
                    shift_y = (height + 1) / 2 - 1;
                }
                if (width % 2 == 0)
                {
                    shift_x = width / 2;
                }
                else
                {
                    shift_x = (width + 1) / 2 - 1;
                }

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (shift_y + y >= height)
                        {
                            if (shift_y + x >= width)
                            {
                                res[x + shift_y - width, y + shift_y - height] = double_ar[x, y];
                            }
                            else
                            {
                                res[x + shift_y, y + shift_y - height] = double_ar[x, y];
                            }
                        }
                        else
                        {
                            if (shift_y + x >= width)
                            {
                                res[x + shift_y - width, y + shift_y] = double_ar[x, y];
                            }
                            else
                            {
                                res[x + shift_y, y + shift_y] = double_ar[x, y];
                            }
                        }
                    }
                }

                return res;
            }


        //Write to console

            public static void WriteToConsoleArray(double[,] d, int w, int h)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Console.Write("(" + x + "," + y + ") " + d[x, y] + "\t");
                }
                Console.Write("\n");
            }
        }
        

        //Create GUI

            public static Button CreateBtn(Panel panel, int width, int height, int x, int y)
        {
            Button newButton = new Button();
            panel.Controls.Add(newButton);
            newButton.FlatStyle = FlatStyle.Flat;
            newButton.FlatAppearance.BorderSize = 0;
            newButton.Location = new Point(x, y);
            newButton.Size = new Size(width, height);
            return newButton;
        }

    }
}
