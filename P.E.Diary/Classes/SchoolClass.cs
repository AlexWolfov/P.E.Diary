using System.Collections.Generic;
using System.Windows.Forms;

namespace P.E._Helper
{
    public class SchoolClass
    {
        public readonly int Grade;
        public readonly string Letter;
        public readonly List<Pupil> Pupils;

        public SchoolClass(int grade, string letter, List<Pupil> pupils)
        {
            Grade = grade;
            Letter = letter;
            Pupils = pupils;
        }
    }
}