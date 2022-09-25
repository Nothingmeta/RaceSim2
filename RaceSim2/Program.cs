// See https://aka.ms/new-console-template for more information
using Controller;
using ControllerTest;
Data.Initialize();
Data.NextRace();

Console.WriteLine(Data.CurrentRace.Track.Name);

for (; ; )
{
    Thread.Sleep(100);
}
