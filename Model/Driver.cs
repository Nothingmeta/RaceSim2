using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Driver : IParticipant
    {

        public Driver(string name, TeamColors teamColor)
        {
            Name = name;
            TeamColor = teamColor;
            Points = 0;
            Equipment = new Car();
        }
    }
}
