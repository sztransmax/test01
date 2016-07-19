using System.Linq;

namespace GradeScores
{
    public class StudentData
    {
        public StudentData() { }
        public StudentData(string line)
        {
            Line = line;
        }

        public int Score { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        private string _line = string.Empty;
        public string Line
        {
            get
            {
                return _line;
            }
            set
            {
                _line = value;
                var elements = _line.Split(',').Select(p => p.Trim()).ToList();
                LastName = elements[0];
                FirstName = elements[1];
                Score = int.Parse(elements[2]);
            }
        }

        public string RearrangedLine
        {
            get
            {
                return Score + ", " + LastName + ", " + FirstName;
            }
        }
    }
}
