using DotaLass.API.Outputs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DotaLass.API
{
    public class PlayerDisplay
    {
        public DisplayData Data { get; set; }

        public PlayerDisplay()
        {
            Data = new DisplayData();
        }

        public bool ValidData { get; set; }
        public event EventHandler ValidityChanged;

        private Task RetrievalTask { get; set; }
        public bool RetrievingData { get; set; }
        public event EventHandler RetrievalCompleted;

        public void Update(string playerID)
        {
            Data.Clear();

            ValidData = false;
            ValidityChanged?.Invoke(this, null);

            PlayerData playerData = null;
            RecentMatch[] recentMatches = null;

            if (RetrievalTask == null || RetrievalTask.IsCompleted)
            {
                RetrievingData = true;

                RetrievalTask = Task.Factory.StartNew(() =>
                {
                    playerData = OpenDotaAPI.GetPlayerData(playerID);
                    recentMatches = OpenDotaAPI.GetPlayerRecentMatchs(playerID);

                    Data.ConsumeData(playerID, playerData, recentMatches);

                    RetrievingData = false;
                    RetrievalCompleted?.Invoke(this, null);

                    ValidData = true;
                    ValidityChanged?.Invoke(this, null);
                });
            }
        }
        
        public void OpenProfile()
        {
            if (!string.IsNullOrEmpty(Data.ID))
                Process.Start(Settings.Instance.BaseLinkAddress + $"/players/{Data.ID}");
        }

        public class DisplayData : INotifyPropertyChanged
        {
            public string ID { get; private set; }

            public string Name { get; private set; }
            public string SoloMMR { get; private set; }
            public string EstimateMMR { get; private set; }

            public float Winrate { get; private set; }
            public TimeSpan AverageDuration { get; private set; }
            public List<Tuple<int, bool>> HeroesPlayed { get; private set; }
            public float AverageKills { get; private set; }
            public float AverageDeaths { get; private set; }
            public float AverageAssists { get; private set; }
            public float AverageXPM { get; private set; }
            public float AverageGPM { get; private set; }
            public float AverageHeroDamage { get; private set; }
            public float AverageTowerDamage { get; private set; }
            public float AverageHeroHealing { get; private set; }
            public float AverageLastHits { get; private set; }
            
            public event PropertyChangedEventHandler PropertyChanged;

            public void ConsumeData(string id,  PlayerData playerData, RecentMatch[] recentMatches)
            {
                ID = id;

                ConsumePlayerData(playerData);
                ConsumeRecentMatches(recentMatches);

                NotifyPropertiesChanged();
            }

            private void ConsumePlayerData(PlayerData playerData)
            {
                if (playerData == null)
                {
                    Name = "{Network Error}";
                }
                else if (playerData.profile == null)
                {
                    Name = "Anonymous";
                }
                else
                {
                    Name = playerData.profile.personaname;
                    SoloMMR = playerData.solo_competitive_rank ?? "X";
                    EstimateMMR = playerData.mmr_estimate.estimate.HasValue ? playerData.mmr_estimate.estimate.ToString() : "X";
                }

            }
            private void ConsumeRecentMatches(RecentMatch[] recentMatches)
            {
                if (recentMatches != null)
                {
                    HeroesPlayed = new List<Tuple<int, bool>>();

                    float wonMatches = 0;
                    int totalSeconds = 0;
                    float totalKills = 0;
                    float totalDeaths = 0;
                    float totalAssists = 0;
                    float totalXPM = 0;
                    float totalGPM = 0;
                    float totalHeroDamage = 0;
                    float totalTowerDamage = 0;
                    float totalHeroHealing = 0;
                    float totalLastHits = 0;

                    foreach (var match in recentMatches)
                    {
                        if (match.Won)
                            wonMatches++;

                        totalSeconds += match.duration;

                        HeroesPlayed.Add(new Tuple<int, bool>(match.hero_id, match.Won));

                        totalKills += match.kills;
                        totalDeaths += match.deaths;
                        totalAssists += match.assists;
                        totalXPM += match.xp_per_min;
                        totalGPM += match.gold_per_min;
                        totalHeroDamage += match.hero_damage;
                        totalTowerDamage += match.tower_damage;
                        totalHeroHealing += match.hero_healing;
                        totalLastHits += match.last_hits;
                    }


                    Winrate = wonMatches / recentMatches.Length;

                    TimeSpan sumDuration = new TimeSpan(0, 0, totalSeconds);
                    AverageDuration = new TimeSpan(sumDuration.Ticks / recentMatches.Length);

                    AverageKills = totalKills / recentMatches.Length;
                    AverageDeaths = totalDeaths / recentMatches.Length;
                    AverageAssists = totalAssists / recentMatches.Length;
                    AverageXPM = totalXPM / recentMatches.Length;
                    AverageGPM = totalGPM / recentMatches.Length;
                    AverageHeroDamage = totalHeroDamage / recentMatches.Length;
                    AverageTowerDamage = totalTowerDamage / recentMatches.Length;
                    AverageHeroHealing = totalHeroHealing / recentMatches.Length;
                    AverageLastHits = totalLastHits / recentMatches.Length;
                }
            }

            public void Clear()
            {
                ID = "";

                Name = "";
                SoloMMR = "";
                EstimateMMR = "";

                Winrate = 0;
                AverageDuration = TimeSpan.Zero;
                HeroesPlayed = null;
                AverageKills = 0;
                AverageDeaths = 0;
                AverageAssists = 0;
                AverageXPM = 0;
                AverageGPM = 0;
                AverageHeroDamage = 0;
                AverageTowerDamage = 0;
                AverageHeroHealing = 0;
                AverageLastHits = 0;

                NotifyPropertiesChanged();
            }

            private void NotifyPropertiesChanged()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EstimateMMR)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoloMMR)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Winrate)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageDuration)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HeroesPlayed)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageKills)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageDeaths)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageAssists)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageXPM)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageGPM)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageHeroDamage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageTowerDamage)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageHeroHealing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AverageLastHits)));
            }
        }
    }
}
