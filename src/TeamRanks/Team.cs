namespace TeamRanks
{
    public class Team {
        public string Name {
            get;
        }
        public int Points {
            get;
            private set;
        }
        public int GamesPlayed {
            get;
            private set;
        }
        public Team(string name) {
            Name = name;
            Points = 0;
            GamesPlayed = 0;
        }

        public void SetPoints(int points) {
            Points += points;
            GamesPlayed++;
        }
    }
}
