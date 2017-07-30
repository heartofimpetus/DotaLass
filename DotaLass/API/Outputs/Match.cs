using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLass.API.Outputs
{
    public class Match
    {
        public long match_id { get; set; }
        public int player_slot { get; set; }
        public bool radiant_win { get; set; }
        public int duration { get; set; }
        public int game_mode { get; set; }
        public int lobby_type { get; set; }
        public int hero_id { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int xp_per_min { get; set; }
        public int gold_per_min { get; set; }
        public int hero_damage { get; set; }
        public int tower_damage { get; set; }
        public int hero_healing { get; set; }
        public int last_hits { get; set; }

        public bool Won
        {
            get
            {
                if (player_slot < 5)
                    return radiant_win;
                else
                    return !radiant_win;
            }
        }
    }
}
