using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum SectionTypes
    {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }
    public class Section
    {
        public SectionTypes SectionType { get; set; }
        public static int Length = 100;
        public Section(SectionTypes sectionType)
        {
            SectionType = sectionType;
        }
    }
}
