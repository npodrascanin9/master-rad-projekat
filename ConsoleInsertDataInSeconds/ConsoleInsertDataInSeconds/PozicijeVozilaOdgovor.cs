using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleInsertDataInSeconds
{
    public record PozicijeVozilaOdgovor
    {
		public string GeografskaDuzina { get; set; }
		public string GeografskaSirina { get; set; }
		public double Brzina { get; set; }
        public int VoziloId { get; set; }
        public DateTime ModemTime { get; set; }
    }
}
