using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChampionWinRate
{
    // JSON structure for champion info API request. Created using
    // json2csharp.com.
    public class ChampionInfo
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public string key { get; set; }
    }
}
