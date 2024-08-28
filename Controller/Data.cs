using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

public static class Data
{
    public static Competition Competition { get; set; }
    public static Race CurrentRace { get; set; }

    public static void Initialize()
    {
        Competition = new Competition();
        AddParticipants();
        AddTracks();
        NextRace();

    }
    public static void AddParticipants()
    {
        Car Mclaren = new Car(10, 80, 95);
        Driver d1 = new Driver("Lando Norris", 0, TeamColors.Orange, Mclaren);
        Car Redbull = new Car(10, 100, 100);
        Driver d2 = new Driver("Max Verstappen", 0, TeamColors.Blue, Redbull);
        Car AstonMartin = new Car(8, 70, 90);
        Driver d3 = new Driver("Sebastian Vettel", 0, TeamColors.Green, AstonMartin);
        Car Ferrari = new Car(7, 100, 95);
        Driver d4 = new Driver("Carlos Sainz", 0, TeamColors.Red, Ferrari);

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
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
        Track babyPark = new Track("BabyPark", sections1);
        Competition.Tracks.Enqueue(babyPark);

        SectionTypes[] sections2 =
        {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
            };
        Track twisted = new Track("Twisted Curves", sections2);
        Competition.Tracks.Enqueue(twisted);
        
        SectionTypes[] sections3 =
{
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.LeftCorner,

                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,

                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            };
        Track austria = new Track("Redbull Ring", sections3);
        Competition.Tracks.Enqueue(austria);
        
    }
    public static void NextRace()
    {
        Track nextTrack = Competition.NextTrack();
        if (nextTrack != null)
        {
            CurrentRace = new Race(nextTrack, Competition.Participants);
            CurrentRace.NextRace += OnNextRace;
        }
        else
        {
            CurrentRace = null;
        }
    }

    public static void OnNextRace(object sender, EventArgs e)
    {

    }
}

