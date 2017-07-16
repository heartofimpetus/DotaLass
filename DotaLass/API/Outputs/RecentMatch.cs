using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotaLass.API.Outputs
{
    public class RecentMatch
    {
        public long match_id { get; set; }
        public int player_slot { get; set; }
        public bool radiant_win { get; set; }
        public int duration { get; set; }
        public int? game_mode { get; set; }
        public int? lobby_type { get; set; }
        public int hero_id { get; set; }
        public int start_time { get; set; }
        public int? version { get; set; }
        public int kills { get; set; }
        public int deaths { get; set; }
        public int assists { get; set; }
        public int? skill { get; set; }
        public int xp_per_min { get; set; }
        public int gold_per_min { get; set; }
        public int hero_damage { get; set; }
        public int tower_damage { get; set; }
        public int hero_healing { get; set; }
        public int last_hits { get; set; }
        public int? lane { get; set; }
        public int? lane_role { get; set; }
        public bool? is_roaming { get; set; }

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
