using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChampionsLeague
{

    class TeamStatistics
    {

        public TeamStatistics()
        {
            this.Wins = 0;
            this.Opponents = new SortedSet<string>();
        }

        public int Wins { get; set; }
        public SortedSet<string> Opponents { get; set; }


    }


    class Program
    {
        static void Main(string[] args)
        {

            var dict = new Dictionary<string, TeamStatistics>();

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "stop")
                {
                    break;
                }

                var split = input.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

                string team1 = split[0];
                string team2 = split[1];

                int team1GoalsdAsHost = int.Parse(split[2].Split(':')[0]);
                int team1GoalsAsGuest = int.Parse(split[3].Split(':')[1]);
                int team2GoalsdAsHost = int.Parse(split[3].Split(':')[0]);
                int team2GoalsAsGuest = int.Parse(split[2].Split(':')[1]);

                bool doesTeamOneWin = ChecksIfTeam1Won(team1GoalsAsGuest, team1GoalsdAsHost, team2GoalsAsGuest, team2GoalsdAsHost);

                if (!dict.ContainsKey(team1))
                {
                    dict[team1] = new TeamStatistics();
                }
                if (!dict.ContainsKey(team2))
                {
                    dict[team2] = new TeamStatistics();
                }
                dict[team1].Opponents.Add(team2);
                dict[team2].Opponents.Add(team1);

                if (doesTeamOneWin)
                {
                    dict[team1].Wins++;
                }
                else
                {
                    dict[team2].Wins++;
                }
            }

            foreach (var team in dict.OrderByDescending(x => x.Value.Wins).ThenBy(x => x.Key))
            {
                Console.WriteLine(team.Key);
                Console.WriteLine($"- Wins: {team.Value.Wins}");
                Console.WriteLine($"- Opponents: {string.Join(", ", team.Value.Opponents)}");
            }

        }

        private static bool ChecksIfTeam1Won(int team1GoalsAsGuest, int team1GoalsdAsHost, int team2GoalsAsGuest, int team2GoalsdAsHost)
        {
            if ((team1GoalsAsGuest + team1GoalsdAsHost) == (team2GoalsAsGuest + team2GoalsdAsHost))
            {
                if (team1GoalsAsGuest > team2GoalsAsGuest)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((team1GoalsdAsHost + team1GoalsAsGuest) > (team2GoalsAsGuest + team2GoalsdAsHost))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}