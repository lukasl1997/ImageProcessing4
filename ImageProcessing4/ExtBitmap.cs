using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing4
{
    public static class ExtBitmap
    {
        public static Bitmap ConvolutionFilter<T>(this Bitmap bitmap, T filter)
            where T : ConvolutionFilterBase
        {

            //Podmínka kvůli přidání fft
            if (filter.FilterName != "FastFourier")
            {

                //Rozměry convolutio matrix
                int cv_width = filter.FilterMatrix.GetLength(0);
                int cv_height = filter.FilterMatrix.GetLength(1);

                //Výpočet index středu convolution matrix
                int cx_index = ((cv_width - 1) / 2);
                int cy_index = ((cv_height - 1) / 2);

                //Vytvoření nové bitmapy do které se budou zapisovat výsledky
                Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
                
                //Get convolution matrix from filter 
                double[,] convolution = filter.FilterMatrix;

                //Prohledávní původní bitmapy první dva cykly (x,y)
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {

                        //Definování základních 3 barem (r,g,b)
                        double r = 0;
                        double g = 0;
                        double b = 0;

                        //Prohledávání convolution matrix pomocí dvou cyklů
                        for (int j = 0; j < cv_height; j++)
                        {
                            for (int i = 0; i < cv_width; i++)
                            {
                                //Podnímka kdy pvky convolution matrix neleži mimo bitmapu
                                if (!((x - cx_index + i < 0) || (y - cy_index + j < 0) || (x - cx_index + i) >= bitmap.Width || (y - cy_index + j) >= bitmap.Height))
                                {
                                    r += (double)bitmap.GetPixel(x - cx_index + i, y - cy_index + j).R * convolution[j, i];
                                    g += (double)bitmap.GetPixel(x - cx_index + i, y - cy_index + j).G * convolution[j, i];
                                    b += (double)bitmap.GetPixel(x - cx_index + i, y - cy_index + j).B * convolution[j, i];
                                }
                            }
                        }

                        //Přidání filter facto a filter bias k barvám
                        r = filter.Factor * r + filter.Bias;
                        g = filter.Factor * g + filter.Bias;
                        b = filter.Factor * b + filter.Bias;

                        //Hraniční podmínky kde se určuje pro každou baru rozhraní (0 - 255)
                        if (r > 255)
                        { r = 255; }
                        else if (r < 0)
                        { r = 0; }

                        if (g > 255)
                        { g = 255; }
                        else if (g < 0)
                        { g = 0; }

                        if (b > 255)
                        { b = 255; }
                        else if (b < 0)
                        { b = 0; }

                        //Přidělení barev danému pixelu v nové bitmapě
                        newBitmap.SetPixel(x, y, Color.FromArgb(255, (byte)r, (byte)g, (byte)b));
                    }
                }

                //Vrácení výsledné (nové) bitmapy
                return newBitmap;
            }
            else {

                //Vrácení vásledné bitmapy FFT
                return ImageToolbox.CentredTwoDFFT(bitmap,25,0,0);
            }
        }
    }
}
