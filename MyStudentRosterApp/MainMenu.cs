using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyStudentRosterApp
{
    public partial class MainMenu : Form
    {
        private StudentHashTable sampleRoster;

        public MainMenu()
        {
            InitializeComponent();
            sampleRoster = null;
        }

        public MainMenu(StudentHashTable theRoster)
        {
            InitializeComponent();
            sampleRoster = theRoster;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            if(sampleRoster is null)
            {
                Student[] seedStudents = new Student[6]
                {
                    new Student { ID = 1, FirstName = "Terence", LastName = "Lee", Status = "Graduate Student", Major = "Computer Science", GPA = "3.41"},
                    new Student { ID = 2, FirstName = "Terence", LastName = "Hilson", Status = "Senior", Major = "Electrical Engineering", GPA = "2.9"},
                    new Student { ID = 3, FirstName = "Kerri", LastName = "Hilson", Status = "Freshman", Major = "Dance", GPA = "3.2"},
                    new Student { ID = 4, FirstName = "Justin", LastName = "Fellinger", Status = "Freshman", Major = "Marketing", GPA = "1.75"},
                    new Student { ID = 5, FirstName = "Mena", LastName = "Yang", Status = "Junior", Major = "Marketing", GPA = "2.5"},
                    new Student { ID = 6, FirstName = "Ana", LastName = "Stamatiou", Status = "Sophomore", Major = "Education", GPA = "2.5"}
                };

                sampleRoster = new StudentHashTable(seedStudents);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            Hide();
            new RemoveForm(sampleRoster).ShowDialog();
            Dispose();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Hide();
            new AddForm(sampleRoster).ShowDialog();
            Dispose();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Hide();
            new SearchForm(sampleRoster).ShowDialog();
            Dispose();
        }

        private void btnRoster_Click(object sender, EventArgs e)
        {
            Hide();
            new DisplayForm(sampleRoster).ShowDialog();
            Dispose();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
