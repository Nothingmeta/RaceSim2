using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Console_project
{
    public static class Visualisation
    {
        private static int _compass;
        private static int trueX, trueY;
        public static void Initialize()
        {
            _compass = 1;
            //Race.DriversChanged += OnDriversChanged;
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.NextRace += OnNextRace;
            
        }

        #region graphics
        /*
        private static string[] _finishHorizontal = { "----", " # ", " # ", "----" };
        private static string[] _finishVertical = { "|    |", "|    |", "|#  #|", "|    |" };
                                                           
        private static string[] _straightHorizontal = { "----", "    ", "    ", "----" };
        private static string[] _straightVertical = { "|  |", "|  |", "|  |", "|  |" };

        private static string[] _rightHorizontalR = { "----", "   |", "   |", "   |" };
        private static string[] _rightHorizontalL = { "|    |", "|  \\", "\\    ", "\\--" };
        
        // private static string[] _rightHorizontalL = { "\\--", "\\    ", "|  \\", "|    |" };

        private static string[] _leftHorizontalR = { "|    |", "/   |", "   /", "--/" };
        private static string[] _leftHorizontalL = { "|    |", "|  \\", "\\    ", "\\--" };

        //private static string[] _leftHorizontalL = { "\\--", "\\    ", "|  \\", "|    |" };

        private static string[] _startGridHorizontal = { "----", "]  ]", "]  ]", "----" };
        private static string[] _startGridVertical = { "|-  -|", "|-  -|", "|-  -|", "|-  -|" };
        */

        // _CornerAllignment(Direction)

        /*        private static string[] _finishHorizontal = { "----", " #1#", " #2#", "----" };
                private static string[] _finishVertical = { "|  |", "|12|", "|##|", "|  |" };

                private static string[] _straightHorizontal = { "----", " 1  ", " 2  ", "----" };
                private static string[] _straightVertical = { "|  |", "|12|", "|  |", "|  |" };

                private static string[] _rightHorizontalR = { "----", " 1 |", " 2 |", "   |" }; //B
                private static string[] _rightHorizontalL = { "|   ", "| 2 ", "| 1 ", "----" };


                private static string[] _leftHorizontalR = { "   |", " 2 |", " 1 |", "----" }; //A
                private static string[] _leftHorizontalL = { "----", "| 1 ", "| 2 ", "|   " };

                private static string[] _rightVerticalU = { "----", "|   ", "|1 2", "|   " };
                private static string[] _rightVerticalD = { "|   ", "|2 1", "|   ", "----" };


                private static string[] _leftVerticalU = { "----", "   |", "1 2|", "   |" }; // B
                private static string[] _leftVerticalD = { "   |", "2 1|", "   |", "----" };  //A



                private static string[] _startGridHorizontal = { "----", "] 1]", "] 2]", "----" };
                private static string[] _startGridVertical = { "|12|", "|--|", "|--|", "|--|" };

                private static string[] _dirt = { "    ", "    ", "    ", "    " };*/

        private static string[] _startN = { "|##|", "|1 |", "| 2|", "|  |" };
        private static string[] _startE = { "----", "  1#", " 2# ", "----" };
        private static string[] _startS = { "|  |", "|2 |", "|  1|", "|##|" };
        private static string[] _startW = { "----", "#1  ", " #2 ", "----" };

        private static string[] _finishN = { "|  |", "|==|", "|1 |", "| 2|" };
        private static string[] _finishE = { "----", " 1[]", "2 []", "----" };
        private static string[] _finishS = { "|2 |", "| 1|", "|==|", "|  |" };
        private static string[] _finishW = { "----", "[] 2", "[]1 ", "----" };

        private static string[] _straightN = { "|  |", "|1 |", "| 2|", "|  |" };
        private static string[] _straightE = { "----", "  1 ", " 2  ", "----" };
        private static string[] _straightS = { "|  |", "|2 |", "| 1|", "|  |" };
        private static string[] _straightW = { "----", "  2 ", " 1  ", "----" };

        private static string[] _leftN = { "--\\ ", " 1 \\", "  2|", "|  |" };
        private static string[] _leftE = { "|  |", " 1 |", "  2/", "--/ " };
        private static string[] _leftS = { "|  |", "|2  ", "\\ 1 ", " \\--" };
        private static string[] _leftW = { " /--", "/2  ", "| 1 ", "|  |" };

        private static string[] _rightN = { " /--", "/1  ", "| 2 ", "|  |" };
        private static string[] _rightE = { "--\\ ", "  1\\", " 2 |", "|  |" };
        private static string[] _rightS = { "|  |", " 2 |", "  1/", "--/ " };
        private static string[] _rightW = { "|  |", "|  2", "\\1  ", " \\--" };
        #endregion
        public static void DrawTrack(Track track)
        {
            if (!track.Sections.Any()) return;
            TrackCursorPosition(track);
            foreach(Section section in track.Sections)
            {
                switch (section.SectionType)
                {
                    case SectionTypes.StartGrid:
                        PrintUsingCompass(_startN, _startE, _startS, _startW, section);
                        break;
                    case SectionTypes.Finish:
                        PrintUsingCompass(_finishN, _finishE, _finishS, _finishW, section);
                        break;
                    case SectionTypes.LeftCorner:
                        PrintUsingCompass(_leftN, _leftE, _leftS, _leftW, section);
                        _compass = RotateCompass(_compass, "Left");
                        break;
                    case SectionTypes.RightCorner:
                        PrintUsingCompass(_rightN, _rightE, _rightS, _rightW, section);
                        _compass = RotateCompass(_compass, "Right");
                        break;
                    case SectionTypes.Straight:
                        PrintUsingCompass(_straightN, _straightE, _straightS, _straightW, section);
                        break;
                        default:
                        throw new ArgumentOutOfRangeException();
                }
                switch (_compass)
                {
                    case 0:
                        trueY -= 4;
                        break;
                    case 1:
                        trueX += 4;
                        break;
                    case 2:
                        trueY += 4;
                        break;
                    case 3:
                        trueX -= 4;
                        break;
                }
                Console.SetCursorPosition(trueX, trueY);
            }
        }

        public static void TrackCursorPosition(Track track)
        {
            int x, y, globalX, globalY;
            x = 0;
            y = 0;
            globalX = 0;
            globalY = 0;

            foreach(Section section in track.Sections)
            {
                /*if (section.SectionType == SectionTypes.LeftCorner)
                {
                    _compass = RotateCompass(_compass, "Left");
                }
                else if (section.SectionType == SectionTypes.RightCorner)
                {
                    _compass = RotateCompass(_compass, "Right");
                }*/
                switch (section.SectionType)
                {
                    case SectionTypes.LeftCorner:
                        _compass = RotateCompass(_compass, "Left");
                        break;
                    case SectionTypes.RightCorner:
                        _compass = RotateCompass(_compass, "Right");
                        break;
                }
                switch (_compass)
                {
                    case 0:
                        y -= 4;
                        break;
                    case 1:
                        x += 4;
                        break;
                    case 2:
                        y += 4;
                        break;
                    case 3:
                        x -= 4;
                        break;
                }
                if(globalX > x)
                {
                    globalX = x;
                }
                if(globalY > y)
                {
                    globalY = y;
                }
            }
            Console.SetCursorPosition(-globalX, -globalY);
            _compass = 1;
            trueX = -globalX;
            trueY = -(-globalY - 5);
        }
        public static int RotateCompass(int compass, string direction)
        {
            if(direction == "Left")
            {
                compass = (compass < 1) ? 3: compass -= 1;
            } else if (direction == "Right")
            {
                compass = (compass > 2) ? 0: compass += 1;
            } else
            {
                return -1;
            }
            return compass;
        }
        public static void PrintSection(string[] section, Section Section)
        {
            trueY -= 4;
            foreach(string s in section)
            {
                IParticipant left = Data.CurrentRace.GetSectionData(Section).Left;
                IParticipant right = Data.CurrentRace.GetSectionData(Section).Right;
                string replacedString = ReplaceString(s, left, right);
                Console.SetCursorPosition(trueX, trueY);
                Console.WriteLine(replacedString);
                trueY++;
            }
        }
        
        public static void PrintUsingCompass(string[] north, string[] east, string[] south, string[] west, Section section)
        {
            switch (_compass)
            {
                case 0:
                    PrintSection(north, section);
                    break;
                case 1:
                    PrintSection(east, section);
                    break;
                case 2:
                    PrintSection(south, section);
                    break;
                case 3:
                    PrintSection(west, section);
                    break;
            }
        }
        public static string ReplaceString(string text, IParticipant first, IParticipant second)
        {
            if (first != null)
            {
                if (!first.Equipment.IsBroken)
                {
                    text = text.Replace('1', first.Name[0]);
                }
                else
                {
                    text = text.Replace('1', char.ToLower(first.Name[0]));
                }
            }
            else
            {
                text = text.Replace('1', ' ');
            }
            if (second != null)
            {
                if (!second.Equipment.IsBroken)
                {
                    text = text.Replace('2', second.Name[0]);
                }
                else
                {
                    text = text.Replace('2', char.ToLower(second.Name[0]));
                }
            }
            else
            {
                text = text.Replace('2', ' ');
            }
            return text;
        }
        public static void OnDriversChanged(object sender, EventArgs e)
        {
            DriversChangedEventArgs dE = (DriversChangedEventArgs)e;
            DrawTrack(dE.Track);
        }
        public static void OnNextRace(object sender, EventArgs e)
        {
            Console.Clear();
            Data.CurrentRace.CleanupEvents();
            Data.NextRace();
            //Visualizer itself
            if (Data.CurrentRace != null)
            {
                Console.Clear();
                Initialize();
            }
            else
            {
                Console.WriteLine("All races are finished.");
                //Console.WriteLine($"The winner is: {Data.Competition.ParticipantPoints.bestParticipant()}");
                //Console.WriteLine($"The participant with the best car is: {Data.Competition.ParticipantSpeedPerTrack.bestParticipant()}");
            }
        }
    }
}
