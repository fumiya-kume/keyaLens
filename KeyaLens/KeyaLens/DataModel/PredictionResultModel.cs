using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyaLens.DataModel
{
    public class PredictionResultModel
    {
        public string Name { get; set; }
        public string ProfileImageURL { get; set; }
        public string ProfileLinkImage { get; set; }
        public double ProbablyRank { get; set; }
    }
}
