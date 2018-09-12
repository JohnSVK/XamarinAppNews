using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zadanie1_kanas {
    public class ClankyViewModel {

        public int Id { get; set; }

        public string Image { get; set; }

        public List<int> KategorieList { get; set; }

        public string Kategoria { get; set; }

        public string Nadpis { get; set; }

        public string Obsah { get; set; }

        public ClankyViewModel() {

        }
    }
}
