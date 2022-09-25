using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;
        private Random _random = new Random(DateTime.Now.Millisecond);
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;

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
/*            SectionsToDictionary();
            PlaceAllParticipants();
            _participantsCounter = Participants.Count;*/

            _timer = new Timer(500);
            //_timer.Elapsed += OnTimedEvent;

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
    }
}
