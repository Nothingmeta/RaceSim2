using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {

        public Driver(string name, int points, TeamColors teamColor, IEquipment equipment)
        {
            Name = name;
            TeamColor = teamColor;
            Points = points;
            Equipment = equipment;
        }
    }
}
