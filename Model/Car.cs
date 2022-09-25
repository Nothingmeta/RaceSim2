using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }


        public Car()
        {
            Performance = 0;
            Quality = 0;
            Speed = 0;
            IsBroken = false;
        }
    }
}
