// See https://aka.ms/new-console-template for more information
using Console_project;

Data.Initialize();
Visualisation.Initialize();
Console.WriteLine(Data.CurrentRace.Track.Name);
Visualisation.DrawTrack(Data.CurrentRace.Track);
for (; ; )
{
    //loop
    Thread.Sleep(100);
    //Visualisation.DrawTrack(Data.CurrentRace.Track);

}
