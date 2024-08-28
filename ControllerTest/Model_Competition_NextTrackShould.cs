using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;
        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
            SectionTypes[] sections1 =
            {
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Track Track1 = new Track("Track1", sections1);

            SectionTypes[] sections2 =
            {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Finish,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner
            };
            Track Track2 = new Track("Track2", sections2);

            //_competition.Tracks.Enqueue(Track1);
            //_competition.Tracks.Enqueue(Track2);

            Car RedBull = new Car(10,10,100);
            Driver Verstappen = new Driver("Verstappen", 0, TeamColors.Blue, RedBull);
            Car Mclaren = new Car(10, 8, 95);
            Driver Norris = new Driver("Norris", 0, TeamColors.Orange, Mclaren);
            _competition.Participants.Add(Verstappen);
            _competition.Participants.Add(Norris);

        }

        [Test]
        public void NextRace_EmptyTrack_ReturnNull()
        {
            Track currentTrack = _competition.NextTrack();
            Assert.IsNull(currentTrack);

        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            SectionTypes[] sections1 =
{
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Track Track1 = new Track("Track1", sections1);
            _competition.Tracks.Enqueue(Track1);
            var currentTrack = _competition.NextTrack();
            Assert.AreEqual(Track1, currentTrack);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] sections1 =
{
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Track Track1 = new Track("Track1", sections1);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] sections1 =
{
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };
            Track Track1 = new Track("Track1", sections1);

            SectionTypes[] sections2 =
            {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Finish,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner
            };
            Track Track2 = new Track("Track2", sections2);

            _competition.Tracks.Enqueue(Track1);
            _competition.Tracks.Enqueue(Track2);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.AreEqual(result, Track2);
        }
    }
}
