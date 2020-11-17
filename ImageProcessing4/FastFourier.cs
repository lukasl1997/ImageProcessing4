using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing4
{
    public class FastFourier : ConvolutionFilterBase
    {
        public override string FilterName
        {
            get { return "FastFourier"; }
        }

        public override double Factor => throw new NotImplementedException();

        public override double Bias => throw new NotImplementedException();

        public override double[,] FilterMatrix => throw new NotImplementedException();
    }
}
