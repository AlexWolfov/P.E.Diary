namespace P.E.Diary.Classes
{
    public class TestParticipator
    {
        public TestParticipator(Pupil pupil, double result)
        {
            Pupil = pupil;
            Фамилия = pupil.Surname;
            Имя = pupil.Name;
            Результат = result;
        }

        public Pupil Pupil;
        public string Фамилия { get; private set; }
        public string Имя { get; private set; }
        public double Результат { get; set; }
    }
}
