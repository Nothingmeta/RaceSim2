using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ConvertSectionTypesToLinkedList(sections);

        }

        private LinkedList<Section> ConvertSectionTypesToLinkedList(SectionTypes[] sections)
        {
            LinkedList<Section> list = new LinkedList<Section>();
            foreach (SectionTypes section in sections)
            {
                list.AddLast(new Section(section));
            }
            return list;
        }
    }
}
