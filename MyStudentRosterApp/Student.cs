using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentRosterApp
{
    public class Student
    {
        private string gpa;

        public uint ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public short StatusEnumValue
        {
            get
            {
                switch (Status)
                {
                    case "Freshman":
                        return 1;
                    case "Sophomore":
                        return 2;
                    case "Junior":
                        return 3;
                    case "Senior":
                        return 4;
                    case "Graduate Student":
                        return 5;
                    default:
                        return 6;
                }
            }
        }
        public string Major { get; set; }
        public string GPA
        {
            get => string.Format("{0:F3}", float.Parse(gpa));
            set => gpa = value;
        }
    }
}
