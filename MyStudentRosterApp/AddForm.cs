using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyStudentRosterApp
{
    public partial class AddForm : Form
    {
        StudentHashTable currentRoster;

        public AddForm(StudentHashTable theRoster)
        {
            InitializeComponent();
            currentRoster = theRoster;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(EntryValidator()) //Valid entry has been created, so let's add it to the hashtable
            {
                Student newStudent = new Student
                {
                    ID = uint.Parse(txtBxID.Text),
                    FirstName = txtBxFirstName.Text.Trim(),
                    LastName = txtBxLastName.Text.Trim(),
                    Status = cmbBxStatus.SelectedItem.ToString(),
                    Major = txtBxMajor.Text.Trim(),
                    GPA = txtBxGPA.Text.Trim()
                };

                if(currentRoster.Add(newStudent))
                {
                    MessageBox.Show($"{newStudent.FirstName} {newStudent.LastName} has been added to the roster",
                                 "Student Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($@"{newStudent.FirstName} {newStudent.LastName} can't be added to the roster, because it's full.",
                                 "Roster is Full", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

                //Clears the form for another entry
                txtBxID.Clear();
                txtBxFirstName.Clear();
                txtBxLastName.Clear();
                cmbBxStatus.SelectedIndex = -1;
                txtBxMajor.Clear();
                txtBxGPA.Clear();
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            Hide();
            new MainMenu(currentRoster).ShowDialog();
            Dispose();
        }
        
        private bool EntryValidator()
        {
            string errorMessage = null;
            Control controlToFocusOn = null;

            if (!GPAValidator())
            {
                errorMessage = "GPA must be a value between 0.000 and 5.000.";
                controlToFocusOn = txtBxGPA;
            }

            if (!MajorValidator())
            {
                errorMessage = "Name of major must be 3 characters long and use only "
                             + "use letters, spaces, apostrophes, and hyphens.\n\n" + errorMessage;
                controlToFocusOn = txtBxMajor;
            }

            if (!StatusValidator())
            {
                errorMessage = "You must select a status for the student.\n\n" + errorMessage;
                controlToFocusOn = cmbBxStatus;
            }

            if (!LastNameValidator())
            {
                errorMessage = "Last name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.\n\n" + errorMessage;
                controlToFocusOn = txtBxLastName;
            }

            if (!FirstNameValidator())
            {
                errorMessage = "First name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.\n\n" + errorMessage;
                controlToFocusOn = txtBxFirstName;
            }

            if (!IDValidator())
            {
                errorMessage = "ID # must consist of only numbers and must have at least one non-zero digit.\n\n"
                             + errorMessage;
                controlToFocusOn = txtBxID;
            }
            else if(currentRoster.Search(uint.Parse(txtBxID.Text)) != null)
            {
                errorMessage = "ID # entered is currently being used by another student.\n\n" + errorMessage;
                controlToFocusOn = txtBxID;
            }

            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Issues with Your Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                controlToFocusOn.Focus();
                return false;
            }

            return true; //Entry is in proper format
        }
        
        private bool IDValidator()
        {
            return Regex.IsMatch(txtBxID.Text, @"^[0-9]+$");
        }

        private bool FirstNameValidator()
        {
            string valueToCheck = txtBxFirstName.Text.Trim();

            return valueToCheck.Length >= 2 && Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$");
        }

        private bool LastNameValidator()
        {
            string valueToCheck = txtBxLastName.Text.Trim();

            return valueToCheck.Length >= 2 && Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$");
        }

        private bool StatusValidator()
        {
            return cmbBxStatus.SelectedIndex != -1;

        }

        private bool MajorValidator()
        {
            string valueToCheck = txtBxMajor.Text.Trim();

            return Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$");
        }

        private bool GPAValidator()
        {
            string valueToCheck = txtBxGPA.Text.Trim();
            return float.TryParse(valueToCheck, out float holder) && holder >= 0f && holder <= 5f;
        }
    }
}
