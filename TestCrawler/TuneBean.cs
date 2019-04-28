using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCrawler
{
    class TuneBean
    {
        public string title{ get; set; }
        public string artist{ get; set; }
        public string arrange{ get; set; }
        public string composer{ get; set; }
        public string voice { get; set; }

        public bool isVoice = false;
        public bool isTurn = true;
        public bool isOrigin = true;
        public bool isArtist = true;
        public bool isOnlyComposer = false;

        public ArrayList origin = new ArrayList();

    }
}
