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
    public partial class SearchForm : Form
    {
        private StudentHashTable currentRoster;
        private bool[] sortAscending;
        private Student[] searchResults;
        private int oldColumn;

        public SearchForm(StudentHashTable theRoster)
        {
            InitializeComponent();
            currentRoster = theRoster;
            oldColumn = -1;
            searchResults = new Student[0];
            sortAscending = new bool[6];
            for(int i = 0; i < sortAscending.Length; i++)
            {
                sortAscending[i] = false;
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            Hide();
            new MainMenu(currentRoster).ShowDialog();
            Dispose();
        }

        private void cmbBxKeySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbBxKeySelect.SelectedIndex is 0)
            {
                pnlFullName.Visible = false;
                pnlID.Visible = true;
            }
            else if(cmbBxKeySelect.SelectedIndex is 1)
            {
                pnlID.Visible = false;
                pnlFullName.Visible = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            switch (cmbBxKeySelect.SelectedIndex)
            {
                case 0: //Search by student ID
                    if(IDValidator())
                    {
                        uint keyOfSearchedItem = uint.Parse(txtBxID.Text);
                        searchResults = new Student[1] { currentRoster.DataForDisplay().FirstOrDefault(pupil => pupil.ID == keyOfSearchedItem)};
                            
                        if(searchResults[0] is null)
                        {
                            MessageBox.Show(@"Sorry, there isn't any record with that ID #.  Please double check and try again.",
                                            "No Match Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        
                        dataGridView1.DataSource = searchResults;

                        if(searchResults[0] != null)
                        {
                            txtBxID.Clear();
                        } 
                    }
                    break;
                case 1: //Search by full name
                    if(FullNameValidator())
                    {
                        string first = txtBxFirstName.Text.ToLower(),
                               last = txtBxLastName.Text.ToLower();

                        searchResults = currentRoster.DataForDisplay()
                                                     .Where(pupil => pupil.FirstName.ToLower() == first && pupil.LastName.ToLower() == last)
                                                     .ToArray();

                        if(searchResults.Length is 0)
                        {
                            MessageBox.Show(@"Sorry, there isn't any record with that full name.  Please double check and try again.",
                                            "No Match Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else if(searchResults.Length > 1)
                        {
                            MessageBox.Show($"The {searchResults.Length} students named {txtBxFirstName.Text.Trim() + " " + txtBxLastName.Text.Trim()} will be displayed in the table.",
                                            "Multiple Students Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        dataGridView1.DataSource = searchResults;

                        if(searchResults.Length != 0)
                        {
                            txtBxFirstName.Clear();
                            txtBxLastName.Clear();
                        }
                    }
                    break;
                default: //Nothing was selected
                    return;
            }
        }

        private bool IDValidator()
        {
            if(!Regex.IsMatch(txtBxID.Text, @"^[0-9]+$"))
            {
                string errorMessage = "ID # must consist of only numbers and must have at least one non-zero digit.";
                MessageBox.Show(errorMessage, "Invalid ID Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Control controlToFocusOn = txtBxID;
                return false;
            }

            return true;
        }

        private bool FullNameValidator()
        {
            string errorMessage = null;
            Control controlToFocusOn = null;

            if (!LastNameValidator())
            {
                errorMessage = "Last name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.";
                controlToFocusOn = txtBxLastName;
            }

            if (!FirstNameValidator())
            {
                errorMessage = "First name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.\n\n" + errorMessage;
                controlToFocusOn = txtBxFirstName;
            }

            if(errorMessage != null)
            {
                MessageBox.Show(errorMessage, "Issues with the Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                controlToFocusOn.Focus();
                return false;
            }

            return true;
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

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int clickedColumnHeader = e.ColumnIndex;

            //Sorts the table by the selected column header
            switch (clickedColumnHeader)
            {
                case 0: //Order by ID number
                    if (!sortAscending[0])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.ID).ToArray();
                        sortAscending[0] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.ID).ToArray();
                        sortAscending[0] = false;
                    }
                    break;
                case 1: //Order by first name
                    if (!sortAscending[1])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = false;
                    }
                    break;
                case 2: //Order by last name
                    if (!sortAscending[2])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = false;
                    }
                    break;
                case 3: //Order by status
                    if (!sortAscending[3])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = false;
                    }
                    break;
                case 4: //Order by major
                    if (!sortAscending[4])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.Major).ToArray();
                        sortAscending[4] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.Major).ToArray();
                        sortAscending[4] = false;
                    }
                    break;
                case 5: //Order by GPA
                    if (!sortAscending[5])
                    {
                        dataGridView1.DataSource = searchResults.OrderBy(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = searchResults.OrderByDescending(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = false;
                    }
                    break;
            }

            if (oldColumn != -1 && oldColumn != clickedColumnHeader)
            {
                sortAscending[oldColumn] = false;
                oldColumn = clickedColumnHeader;
            }
        }
    }
}
