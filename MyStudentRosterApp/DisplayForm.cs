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
    public partial class DisplayForm : Form
    {
        private StudentHashTable currentRoster;
        private bool[] sortAscending;
        private Student[] searchResults;
        private int oldColumn;

        public DisplayForm(StudentHashTable theRoster)
        {
            InitializeComponent();
            currentRoster = theRoster;
        }


        private void DisplayForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.ID).ToArray();

            oldColumn = 1;
            searchResults = new Student[0];

            sortAscending = new bool[6];
            sortAscending[0] = true;
            for (int i = 1; i < sortAscending.Length; i++)
            {
                sortAscending[i] = false;
            }
        }

        private void cmbBxRecordFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBxComparers.Items.Clear();

            if (cmbBxRecordFields.SelectedItem is "ID" || cmbBxRecordFields.SelectedItem is "GPA")
            {
                cmbBxComparers.Items.AddRange(new string[] { "is lower than", @"isn't greater than", "is", "is at least", "is greater than" });
                cmbBxValues.Items.Clear();
                cmbBxValues.DropDownStyle = ComboBoxStyle.DropDown;
                return;
            }

            cmbBxComparers.Items.AddRange(new string[] { "is before", @"isn't after", "is", @"isn't before", "is after" });

            if (cmbBxRecordFields.SelectedItem is "Status")
            {
                cmbBxValues.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbBxValues.Items.AddRange(new string[] { "Freshman", "Sophomore", "Junior", "Senior", "Graduate Student" });
                return;
            }

            cmbBxValues.DropDownStyle = ComboBoxStyle.DropDown;
            cmbBxValues.Items.Clear();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbBxValues.SelectedIndex = cmbBxRecordFields.SelectedIndex = cmbBxComparers.SelectedIndex = -1;
            cmbBxValues.Items.Clear();
            cmbBxValues.Text = null;

            dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.ID).ToArray();

            sortAscending = new bool[6];
            sortAscending[0] = true;
            for (int i = 1; i < sortAscending.Length; i++)
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if(cmbBxRecordFields.SelectedIndex is -1 || cmbBxComparers.SelectedIndex is -1 ||
              (cmbBxRecordFields.SelectedItem is "Status" && cmbBxValues.SelectedIndex is -1) ||
              ((string)cmbBxRecordFields.SelectedItem != "Status" && cmbBxValues.Text.Length is 0))
            {
                return;
            }

            Student[] theCurrentTable = (Student[])dataGridView1.DataSource;

            switch (cmbBxRecordFields.SelectedItem)
            {  
                case "ID":
                    if(IDValidator())
                    {
                        uint filterKeyID = uint.Parse(cmbBxValues.Text);

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is less than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.ID < filterKeyID).ToArray();
                                break;
                            case @"isn't greater than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.ID <= filterKeyID).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.ID == filterKeyID).ToArray();
                                break;
                            case "is at least":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.ID >= filterKeyID).ToArray();
                                break;
                            case "is greater than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.ID > filterKeyID).ToArray();
                                break;
                        }
                    }
                    break;
                case "First Name":
                    if(FirstNameValidator())
                    {
                        string filterKeyFName = cmbBxValues.Text.Trim().ToLower();

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.FirstName.ToLower(), filterKeyFName) < 0).ToArray();
                                break;
                            case @"isn't after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.FirstName.ToLower(), filterKeyFName) <= 0).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.FirstName.ToLower(), filterKeyFName) is 0).ToArray();
                                break;
                            case @"isn't before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.FirstName.ToLower(), filterKeyFName) >= 0).ToArray();
                                break;
                            case "is after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.FirstName.ToLower(), filterKeyFName) > 0).ToArray();
                                break;
                        }
                    }
                    break;
                case "Last Name":
                    if (LastNameValidator())
                    {
                        string filterKeyLName = cmbBxValues.Text.Trim().ToLower();

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyLName) < 0).ToArray();
                                break;
                            case @"isn't after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyLName) <= 0).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyLName) is 0).ToArray();
                                break;
                            case @"isn't before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyLName) >= 0).ToArray();
                                break;
                            case "is after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyLName) > 0).ToArray();
                                break;
                        }
                    }
                    break;
                case "Status":
                    int filterKeyStatus = cmbBxValues.SelectedIndex + 1;

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.StatusEnumValue < filterKeyStatus).ToArray();
                                break;
                            case @"isn't after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.StatusEnumValue <= filterKeyStatus).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.StatusEnumValue == filterKeyStatus).ToArray();
                                break;
                            case @"isn't before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.StatusEnumValue >= filterKeyStatus).ToArray();
                                break;
                            case "is after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => pupil.StatusEnumValue > filterKeyStatus).ToArray();
                                break;
                        }
                    break;
                case "Major":
                    if (MajorValidator())
                    {
                        string filterKeyMajor = cmbBxValues.Text.Trim().ToLower();

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyMajor) < 0).ToArray();
                                break;
                            case @"isn't after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyMajor) <= 0).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyMajor) is 0).ToArray();
                                break;
                            case @"isn't before":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyMajor) >= 0).ToArray();
                                break;
                            case "is after":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.LastName.ToLower(), filterKeyMajor) > 0).ToArray();
                                break;
                        }
                    }
                    break;
                case "GPA":
                    if (GPAValidator())
                    {
                        string filterKeyGPA = $"{float.Parse(cmbBxValues.Text):F3}";

                        switch (cmbBxComparers.SelectedItem)
                        {
                            case "is less than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.GPA, filterKeyGPA) < 0).ToArray();
                                break;
                            case @"isn't greater than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.GPA, filterKeyGPA) <= 0).ToArray();
                                break;
                            case "is":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.GPA, filterKeyGPA) is 0).ToArray();
                                break;
                            case "is at least":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.GPA, filterKeyGPA) >= 0).ToArray();
                                break;
                            case "is greater than":
                                dataGridView1.DataSource = theCurrentTable.Where(pupil => string.CompareOrdinal(pupil.GPA, filterKeyGPA) > 0).ToArray();
                                break;
                        }
                    }
                    break;
            }
        }

        private bool IDValidator()
        {
            if (!Regex.IsMatch(cmbBxValues.Text, @"^[0-9]+$"))
            {
                string errorMessage = "ID # must consist of only numbers and must have at least one non-zero digit.";
                MessageBox.Show(errorMessage, "Invalid ID Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Control controlToFocusOn = cmbBxValues;
                return false;
            }

            return true;
        }

        private bool FirstNameValidator()
        {
            string valueToCheck = cmbBxValues.Text.Trim();

            if(valueToCheck.Length < 2 || !Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$"))
            {
                string errorMessage = "First name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.";

                MessageBox.Show(errorMessage, "Issues with the Name you Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBxValues.Focus();
                return false;
            }

            return true;
        }

        private bool LastNameValidator()
        {
            string valueToCheck = cmbBxValues.Text.Trim();

            if (valueToCheck.Length < 2 || !Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$"))
            {
                string errorMessage = "Last name must consist of only letters, spaces, apostrophes, or hyphens"
                             + " and must contain at least two characters, which one must be a non-space character.";

                MessageBox.Show(errorMessage, "Issues with the Name you Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBxValues.Focus();
                return false;
            }

            return true;
        }
        
        private bool MajorValidator()
        {
            string valueToCheck = cmbBxValues.Text.Trim();

            if(valueToCheck.Length < 3 || !Regex.IsMatch(valueToCheck, @"^[a-zA-Z\s\'-]+$"))
            {
                string errorMessage = "Name of major must be 3 characters long and use only "
                                    + "use letters, spaces, apostrophes, and hyphens.";

                MessageBox.Show(errorMessage, "Issues with the Major Entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBxValues.Focus();
                return false;
            }

            return true;
        }

        private bool GPAValidator()
        {
            string valueToCheck = cmbBxValues.Text.Trim();

            if(!float.TryParse(valueToCheck, out float holder) || holder < 0f && holder > 5f)
            {
                string errorMessage = "GPA must be a value between 0.000 and 5.000.";
                MessageBox.Show(errorMessage, "Issues with  the GPA you entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbBxValues.Focus();
                return false;
            }

            return true;
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
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.ID).ToArray();
                        sortAscending[0] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.ID).ToArray();
                        sortAscending[0] = false;
                    }
                    break;
                case 1: //Order by first name
                    if (!sortAscending[1])
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = false;
                    }
                    break;
                case 2: //Order by last name
                    if (!sortAscending[2])
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = false;
                    }
                    break;
                case 3: //Order by status
                    if (!sortAscending[3])
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = false;
                    }
                    break;
                case 4: //Order by major
                    if (!sortAscending[4])
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.Major).ToArray();
                        sortAscending[4] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.Major).ToArray();
                        sortAscending[4] = false;
                    }
                    break;
                case 5: //Order by GPA
                    if (!sortAscending[5])
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderBy(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = currentRoster.DataForDisplay().OrderByDescending(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = false;
                    }
                    break;
            }

            if (oldColumn != clickedColumnHeader)
            {
                sortAscending[oldColumn] = false;
                oldColumn = clickedColumnHeader;
            }
        }
    }
}
