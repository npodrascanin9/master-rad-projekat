using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInsertDataInSeconds
{
    public class PozicijaCsv
    {
        [Name("geografskaSirina")]
        public double GeografskaSirina { get; set; }
        [Name("geografskaSirina")]
        public double GeografskaDuzina { get; set; }
        [Name("brzina")]
        public double Brzina { get; set; }
        [Name("voziloId")]
        public int VoziloId { get; set; }
    }
}
