using System;
using System.Collections;
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
    public partial class RemoveForm : Form
    {
        private bool[] sortAscending;
        private bool checkedAll;
        private StudentHashTable studentHashTable;
        private Student[] theDataSource;
        private int oldColumn;
 
        public RemoveForm(StudentHashTable theRoster)
        {
            InitializeComponent();
            studentHashTable = theRoster;
            theDataSource = studentHashTable.DataForDisplay().OrderBy(pupil => pupil.ID).ToArray();
        }

        private void RemoveForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = theDataSource;

            oldColumn = 1;
            checkedAll = false;

            sortAscending = new bool[6];
            sortAscending[0] = true;
            for (int i = 1; i < sortAscending.Length; i++)
                sortAscending[i] = false;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int clickedColumnHeader = e.ColumnIndex;

            if(clickedColumnHeader is 0) //Clicked on the header for the remove column
            {
                DataGridViewCheckBoxCell chxBox;
                if (!checkedAll)
                {
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        chxBox = (DataGridViewCheckBoxCell)row.Cells[0];
                        chxBox.Value = true;
                    }

                    checkedAll = true;
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        chxBox = (DataGridViewCheckBoxCell)row.Cells[0];
                        chxBox.Value = false;
                    }

                    checkedAll = false;
                }
                return;
            }

            checkedAll = false; //Any sort of the column will unchecked all checkboxes anyway.

            //Sorts the table by the selected column header
            switch(clickedColumnHeader)
            {
                case 1: //Order by ID number
                    if(!sortAscending[0])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.ID).ToArray();
                        sortAscending[0] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.ID).ToArray();
                        sortAscending[0] = false;
                    }
                    break;
                case 2: //Order by first name
                    if (!sortAscending[1])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.FirstName).ToArray();
                        sortAscending[1] = false;
                    }
                    break;
                case 3: //Order by last name
                    if (!sortAscending[2])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.LastName).ToArray();
                        sortAscending[2] = false;
                    }
                    break;
                case 4: //Order by status
                    if (!sortAscending[3])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.StatusEnumValue).ToArray();
                        sortAscending[3] = false;
                    }
                    break;
                case 5: //Order by major
                    if (!sortAscending[4])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.Major).ToArray();
                        sortAscending[4] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.Major).ToArray();
                        sortAscending[4] = false;
                    }
                    break;
                case 6: //Order by GPA
                    if (!sortAscending[5])
                    {
                        dataGridView1.DataSource = theDataSource.OrderBy(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = true;
                    }
                    else
                    {
                        dataGridView1.DataSource = theDataSource.OrderByDescending(pupil => pupil.GPA).ToArray();
                        sortAscending[5] = false;
                    }
                    break;
            }

            if (oldColumn != clickedColumnHeader)
            {
                sortAscending[oldColumn - 1] = false;
                oldColumn = clickedColumnHeader;
            }
        }

        private void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            int[] recordsToDelete = RowsOfCheckboxesSelected();

            if(recordsToDelete.Length is 0)
            {
                return;
            }

            DialogResult confirmation;
            string name = null;

            if(recordsToDelete.Length is 1)
            {
                name = $"{dataGridView1.Rows[recordsToDelete[0]].Cells[2].Value} {dataGridView1.Rows[recordsToDelete[0]].Cells[3].Value}";
                confirmation = MessageBox.Show($"Are you sure you want to delete {name} from the roster?", "Before we take action...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
            else
            {
                confirmation = MessageBox.Show($"Are you sure you want to delete these {recordsToDelete.Length} students from the roster?", "Before we take action...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if(confirmation is DialogResult.Yes)
            {
                DataGridViewRow nextRow;
                uint keyFromNextRow;

                for(int i = 0; i < recordsToDelete.Length; i++)
                {
                    nextRow = dataGridView1.Rows[recordsToDelete[i]];
                    keyFromNextRow = uint.Parse(nextRow.Cells[1].Value.ToString());
                    studentHashTable.Remove(keyFromNextRow);
                }

                if(recordsToDelete.Length is 1)
                {
                    MessageBox.Show($"{name} has deleted from the roster.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"{recordsToDelete.Length} students have been deleted from the roster.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                dataGridView1.DataSource = studentHashTable.DataForDisplay().OrderBy(pupil => pupil.ID).ToArray();
            }
        }

        private int[] RowsOfCheckboxesSelected()
        {
            DataGridViewCheckBoxCell chxBox;
            List<int> indicesOfCheckboxesSelected = new List<int>();
            int i = 0;

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                chxBox = (DataGridViewCheckBoxCell)row.Cells[0];

                if(chxBox.Value is true)
                {
                    indicesOfCheckboxesSelected.Add(i);
                }

                i++;
            }

            return indicesOfCheckboxesSelected.ToArray();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex != 0 || e.RowIndex is -1)
            {
                return;
            }

            DataGridViewCheckBoxCell chxBox = (DataGridViewCheckBoxCell)dataGridView1.Rows[e.RowIndex].Cells[0];

            if(chxBox.Value is true)
            {
                chxBox.Value = false;
            }
            else
            {
                chxBox.Value = true;
            }
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {
            Hide();
            new MainMenu(studentHashTable).ShowDialog();
            Dispose();
        }
    }
}
