using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLass.API.Outputs
{
    public class PlayerData
    {
        public string tracked_until { get; set; }
        public string solo_competitive_rank { get; set; }
        public object competitive_rank { get; set; }
        public MmrEstimate mmr_estimate { get; set; }
        public Profile profile { get; set; }
    }

    public class MmrEstimate
    {
        public int? estimate { get; set; }
        public double? stdDev { get; set; }
        public int? n { get; set; }
    }

    public class Profile
    {
        public int account_id { get; set; }
        public string personaname { get; set; }
        public object name { get; set; }
        public int cheese { get; set; }
        public string steamid { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public string profileurl { get; set; }
        public string last_login { get; set; }
        public object loccountrycode { get; set; }
    }
}
