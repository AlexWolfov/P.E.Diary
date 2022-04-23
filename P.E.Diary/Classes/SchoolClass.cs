using System.Collections.Generic;

namespace P.E.Diary
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