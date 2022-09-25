using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
namespace ControllerTest
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }
        public static void AddParticipants()
        {
            Driver d1 = new Driver("Lando Norris", TeamColors.Orange);
            Driver d2 = new Driver("Max Verstappen", TeamColors.Blue);
            Driver d3 = new Driver("Sebastian Vettel", TeamColors.Green);
            Driver d4 = new Driver("Carlos Sainz", TeamColors.Red);

            Competition.Participants.Add(d1);
            Competition.Participants.Add(d2);
            Competition.Participants.Add(d3);
            Competition.Participants.Add(d4);
        }
        public static void AddTracks()
        {
            SectionTypes[] sections1 =
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Track babyPark = new Track("BabyPark", sections1);
            Competition.Tracks.Enqueue(babyPark);
        }
        public static void NextRace()
        {
            Track nextTrack = Competition.NextTrack();
            if (nextTrack != null)
            {
                CurrentRace = new Race(nextTrack, Competition.Participants);
                //CurrentRace.NextRace += OnNextRace;
            }
            else
            {
                CurrentRace = null;
            }
        }
    }
}
