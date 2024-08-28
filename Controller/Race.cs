using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        public event EventHandler DriversChanged;
        public event EventHandler NextRace;

        private Random _random = new Random(DateTime.Now.Millisecond);
        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();
        private Timer _timer;
        private int _rounds = 1; //Total rounds is _rounds + 1
        private int _participantsCounter;
        private Dictionary<IParticipant, int> _finishes = new Dictionary<IParticipant, int>();


        public SectionData GetSectionData(Section section)
        {
            if (!_positions.ContainsKey(section))
            {
                _positions.Add(section, new SectionData());
            }
            return _positions[section];
        }
        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            SectionsToDictionary();
            PlaceParticipants();
            _participantsCounter = Participants.Count;

            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;

            Start();
        }

        public void Start()
        {
            _timer.Start();
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void RandomizeEquipment()
        {
            foreach (IParticipant participant in Participants)
            {
                participant.Equipment.Quality = _random.Next(5, 10);
                participant.Equipment.Performance = _random.Next(60, 100);
            }
        }

        public void SectionsToDictionary()
        {
            foreach (Section section in Track.Sections)
            {
                _positions.Add(section, new SectionData());
            }
        }

        public void PlaceParticipants()
        {
            RandomizeEquipment();
            Stack<Section> startSections = new Stack<Section>();
            //int amountParticipants = Participants.Count;
            int participantCounter = 0;
            int totalParticipants = Participants.Count;

            // There are 4 drivers in a race so don't have catch alot of errors
            foreach (Section section in Track.Sections)
            {
                if(section.SectionType == SectionTypes.StartGrid)
                {
                    /*SectionData sectionData =GetSectionData(section);
                    sectionData.Left = Participants[participantCounter];
                    sectionData.Right = Participants[participantCounter + 1];
                    participantCounter += 2;
                    if(participantCounter == amountParticipants)
                    {
                        break;
                    }*/
                    if (section.SectionType == SectionTypes.StartGrid)
                    {
                        startSections.Push(section);
                    }
                }
            }
            while (startSections.Count > 0)
            {
                Section section = startSections.Pop();
                SectionData sectionData = GetSectionData(section);

                // Keeps adding 2 till there is 1 left and after that it reaches the end of the while loop.
                if (totalParticipants > participantCounter + 1)
                {
                    sectionData.Left = Participants[participantCounter];
                    sectionData.Right = Participants[participantCounter + 1];

                    _finishes.Add(Participants[participantCounter], 0);
                    _finishes.Add(Participants[participantCounter + 1], 0);
                    participantCounter += 2;
                }
                else if (totalParticipants > participantCounter)
                {
                    sectionData.Left = Participants[participantCounter];
                    _finishes.Add(Participants[participantCounter], 0);
                    participantCounter++;
                }
            }
        }

        //If the participant gets move from the end to the start of the track, hide the participant
        //the hidden section does not count as an extra length, so it makes it up by adding the
        //section length to the distance
        public void hideParticipant(SectionData nextSection, SectionData hiddenSectionData, IParticipant participant1, IParticipant participant2)
        {
            //Left has to be the participant has been moved, so either participant1 or participant2
            if (nextSection.Left != null && (nextSection.Left == participant1 || nextSection.Left == participant2))
            {
                hiddenSectionData.Left = nextSection.Left;
                hiddenSectionData.DistanceLeft = nextSection.DistanceLeft + Section.Length;
                //Reset
                nextSection.Left = null;
                nextSection.DistanceLeft = 0;
            }
            //Same principle as in the if condition before, but instead check right.
            if (nextSection.Right != null && (nextSection.Right == participant1 || nextSection.Right == participant2))
            {
                hiddenSectionData.Right = nextSection.Right;
                hiddenSectionData.DistanceRight = nextSection.DistanceRight + Section.Length;
                //Reset
                nextSection.Right = null;
                nextSection.DistanceRight = 0;
            }
        }


        public void MoveParticipants()
        {
            //Initialize
            SectionData nextSection = _positions.First().Value;
            SectionData hiddenSectionData = new SectionData();
            bool enteredOnce = false;
            IParticipant participant1 = _positions.Last().Value.Left;
            IParticipant participant2 = _positions.Last().Value.Right;

            //Iterate through all the positions, reverse the positions because otherwise it's checking the wrong way
            foreach (KeyValuePair<Section, SectionData> kvPair in _positions.Reverse())
            {
                SectionData currentSectionSD = kvPair.Value;
                if (currentSectionSD.Left != null)
                {
                    //SaveSectionTimes(kvPair.Key, kvPair.Value, DateTime.Now, "left");
                    if (!currentSectionSD.Left.Equipment.IsBroken)
                    {
                        if (Section.Length > currentSectionSD.DistanceLeft)
                        {
                            currentSectionSD.DistanceLeft += currentSectionSD.Left.Equipment.Speed * currentSectionSD.Left.Equipment.Performance;
                        }
                        if (currentSectionSD.DistanceLeft >= Section.Length)
                        {
                            if (kvPair.Key.SectionType == SectionTypes.Finish)
                            {
                                CheckFinishes(kvPair.Value, "left");
                            }

                            MoveParticipantsLeftSection(nextSection, currentSectionSD);
                        }
                    }
                }
                if (currentSectionSD.Right != null)
                {
                    //SaveSectionTimes(kvPair.Key, kvPair.Value, DateTime.Now, "right");
                    if (!currentSectionSD.Right.Equipment.IsBroken)
                    {
                        if (Section.Length > currentSectionSD.DistanceRight)
                        {
                            currentSectionSD.DistanceRight += currentSectionSD.Right.Equipment.Speed * currentSectionSD.Right.Equipment.Performance;
                        }
                        if (currentSectionSD.DistanceRight >= Section.Length)
                        {
                            if (kvPair.Key.SectionType == SectionTypes.Finish)
                            {
                                CheckFinishes(kvPair.Value, "right");
                            }
                            MoveParticipantsRightSection(nextSection, currentSectionSD);
                        }
                    }
                }
                //If you moved it, don't move it again.
                if (!enteredOnce)
                {
                    //Check if any of them has moved from the end of the track to the start
                    if (participant1 == _positions.First().Value.Left ||
                        participant1 == _positions.First().Value.Right ||
                        participant2 == _positions.First().Value.Left ||
                        participant2 == _positions.First().Value.Right)
                    {
                        hideParticipant(_positions.First().Value, hiddenSectionData, participant1, participant2);
                    }
                    //So the if condition only executes once at the start of the foreach
                    enteredOnce = true;
                }

                //Compare next two sections, backwards
                nextSection = kvPair.Value;
            }

            //No point on revealing if there is nothing on the hiddensection
            if (hiddenSectionData.Left != null || hiddenSectionData.Right != null)
            {
                RevealParticipant(_positions.First().Value, hiddenSectionData);
            }
        }
        public void MoveParticipantsLeftSection(SectionData nextSection, SectionData currentSection)
        {
            if (nextSection.Left == null)
            {
                nextSection.Left = currentSection.Left;
                nextSection.DistanceLeft = currentSection.DistanceLeft - Section.Length;
                //Reset
                currentSection.Left = null;
                currentSection.DistanceLeft = 0;
            }
            //if left is full, try right
            else if (nextSection.Right == null)
            {
                nextSection.Right = currentSection.Left;
                nextSection.DistanceRight = currentSection.DistanceLeft - Section.Length;
                //Reset
                currentSection.Left = null;
                currentSection.DistanceLeft = 0;
            }
        }
        public void MoveParticipantsRightSection(SectionData nextSection, SectionData currentSection)
        {
            if (nextSection.Right == null)
            {
                nextSection.Right = currentSection.Right;
                nextSection.DistanceRight = currentSection.DistanceRight - Section.Length;
                //Reset
                currentSection.Right = null;
                currentSection.DistanceRight = 0;

            }
            //if right is full, try left
            else if (nextSection.Left == null)
            {
                nextSection.Left = currentSection.Right;
                nextSection.DistanceRight = currentSection.DistanceRight - Section.Length;
                //Reset
                currentSection.Right = null;
                currentSection.DistanceRight = 0;
            }
        }

        public void RevealParticipant(SectionData nextSection, SectionData hiddenSectionData)
        {
            if (hiddenSectionData.Left != null)
            {
                nextSection.Left = hiddenSectionData.Left;
                nextSection.DistanceLeft = hiddenSectionData.DistanceLeft;

                hiddenSectionData.Left = null;
                hiddenSectionData.DistanceLeft = 0;
            }
            if (hiddenSectionData.Right != null)
            {
                nextSection.Right = hiddenSectionData.Right;
                nextSection.DistanceRight = hiddenSectionData.DistanceRight;

                hiddenSectionData.Right = null;
                hiddenSectionData.DistanceRight = 0;
            }
        }

        public void CheckFinishes(SectionData finish, string side)
        {
            if (finish.Left != null && side == "left" && !finish.Left.Equipment.IsBroken)
            {
                if (_finishes[finish.Left] >= _rounds)
                {
                    RemoveParticipant(finish, side);
                    _participantsCounter--;
                }
                else
                {
                    _finishes[finish.Left]++;
                }
            }

            if (finish.Right != null && side == "right" && !finish.Right.Equipment.IsBroken)
            {
                if (_finishes[finish.Right] >= _rounds)
                {
                    RemoveParticipant(finish, side);
                    _participantsCounter--;
                }
                else
                {
                    _finishes[finish.Right]++;
                }
            }
        }

        public void RemoveParticipant(SectionData finish, string side)
        {
            if (side == "left")
            {
                //AddToFinalScore(finish.Left);
                finish.Left = null;
                finish.DistanceLeft = 0;
            }
            else if (side == "right")
            {
                //AddToFinalScore(finish.Right);
                finish.Right = null;
                finish.DistanceRight = 0;
            }
        }
        private void RepairParticipant(IParticipant participant)
        {
            if (_random.Next(0, 100) > 35 + participant.Equipment.Quality)
            {
                participant.Equipment.IsBroken = false;
            }
        }

        private void BreakDownParticipant(IParticipant participant)
        {
            if (_random.Next(0, 100) < 25 - participant.Equipment.Quality)
            {
                participant.Equipment.IsBroken = true;

                if(participant.Equipment.Quality > 0)
                {
                    participant.Equipment.Quality -= 1;
                }
            }
        }

        private void BreakDownOrRepairParticipant(IParticipant participant)
        {
            if (participant.Equipment.IsBroken)
            {
                RepairParticipant(participant);
            }
            else
            {
                BreakDownParticipant(participant);
            }
        }

        public void ReliabilityCheck()
        {
            foreach (IParticipant participant in Participants)
            {
                BreakDownOrRepairParticipant(participant);
            }
        }

        public void CleanupEvents()
        {
            Delegate[] delegates = DriversChanged?.GetInvocationList();
            if (delegates != null)
            {
                foreach (var d in delegates)
                {
                    DriversChanged -= (EventHandler)d;
                }
            }
            delegates = NextRace?.GetInvocationList();
            if (delegates != null)
            {
                foreach (var d in delegates)
                {
                    NextRace -= (EventHandler)d;
                }
            }
        }

        public void CheckRaceFinished(Track track)
        {
            if (_participantsCounter == 0 && track != null)
            {
                NextRace?.Invoke(this, new EventArgs());
            }
        }

        public void OnTimedEvent(object o, ElapsedEventArgs e)
        {
            ReliabilityCheck();
            MoveParticipants();
            DriversChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
            CheckRaceFinished(this.Track);

        }

    }
}
