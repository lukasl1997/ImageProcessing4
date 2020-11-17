using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
namespace ImageProcessing4
{
    public partial class Form1 : Form
    {

        private Image imageOr;
        private Bitmap bitmap;

        private List<ConvolutionFilterBase> filters = new List<ConvolutionFilterBase>();

        public Form1()
        {
            InitializeComponent();

            filters.Add(new Blur3x3Filter());
            filters.Add(new Blur5x5Filter());
            filters.Add(new Gaussian3x3BlurFilter());
            filters.Add(new Gaussian5x5BlurFilter());
            filters.Add(new MotionBlurFilter());
            filters.Add(new MotionBlurLeftToRightFilter());
            filters.Add(new MotionBlurRightToLeftFilter());

            filters.Add(new EdgeDetectionFilter());
            filters.Add(new EdgeDetection45DegreeFilter());
            filters.Add(new HorizontalEdgeDetectionFilter());
            filters.Add(new VerticalEdgeDetectionFilter());
            filters.Add(new EdgeDetectionTopLeftBottomRightFilter());

            filters.Add(new EmbossFilter());
            filters.Add(new Emboss45DegreeFilter());
            filters.Add(new EmbossTopLeftBottomRightFilter());
            filters.Add(new IntenseEmbossFilter());

            filters.Add(new HighPass3x3Filter());

            filters.Add(new SharpenFilter());
            filters.Add(new Sharpen3x3Filter());
            filters.Add(new Sharpen3x3FactorFilter());
            filters.Add(new Sharpen5x5Filter());
            filters.Add(new IntenseSharpenFilter());

            filters.Add(new SoftenFilter());

            filters.Add(new FastFourier());


            this.cmbFilters.DataSource = filters;
            this.cmbFilters.ValueMember = "FilterName";
            this.cmbFilters.SelectedIndex = 0;

            this.btnReset.Enabled = false;
            this.btnSave.Enabled = false;
            this.cmbFilters.Enabled = false;

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            imageOr = ImageToolbox.OpenImage();
            if (imageOr != null || bitmap != null)
            {
                if(imageOr != null)
                {
                    bitmap = ImageToolbox.ResizeImage(imageOr, this.picboxMain.Width, this.picboxMain.Height);
                    this.picboxMain.Image = bitmap;
                    this.btnReset.Enabled = true;
                    this.btnSave.Enabled = true;
                    this.cmbFilters.Enabled = true;
                }
            }
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageOr != null)
            {
                ConvolutionFilterBase filter = (ConvolutionFilterBase)this.cmbFilters.SelectedItem;
                this.picboxMain.Image = bitmap.ConvolutionFilter(filter);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.picboxMain.Image = bitmap;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ImageToolbox.SaveImage((Image)bitmap.ConvolutionFilter((ConvolutionFilterBase)this.cmbFilters.SelectedItem));
        }
    }
}
