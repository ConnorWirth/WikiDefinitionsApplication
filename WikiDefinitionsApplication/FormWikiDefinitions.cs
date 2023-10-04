using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Xml.Linq;

namespace WikiDefinitionsApplication
{
    public partial class FormWikiDefinitions : Form
    {
        public FormWikiDefinitions()
        {
            InitializeComponent();
            textBoxDSN.MouseDown += textBoxDSN_MouseDown;

            // Save button is disabled before program to stop user from saving program with no data in listview
            buttonSave.Enabled = false;
        }

        private void FormWikiDefinitions_Load(object sender, EventArgs e)
        {
            PopulateComboBox();

        }

        // 6.2 Create a global List<T> of type Information called Wiki.
        List<Information> Wiki = new List<Information>();

        /* 
        6.3 Create a button method to ADD a new item to the list. Use a TextBox for the Name input, ComboBox for 
        the Category, Radio group for the Structure and Multiline TextBox for the Definition.
        */
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            string name = textBoxDSN.Text;

            // Check if the name is valid (not a duplicate)
            if (ValidName(name))
            {
                // Add the new information to the list
                Information newInformation = new Information();
                newInformation.SetName(name);
                newInformation.SetCategory(comboBoxCategory.Text);
                newInformation.SetStructure(GetSelectedRadioButtonValue());
                newInformation.SetDefinition(textBoxDefinition.Text);
                buttonSave.Enabled = true;
                Wiki.Add(newInformation);
                ClearInputs();
                DisplayData();
                SetSelectedRadioButton(0); // Select Linear
                SetSelectedRadioButton(1); // Select Non-Linear
                SortAndDisplayData();
            }
            else
            {
                // Handle the case where the name is a duplicate
                MessageBox.Show("Name already exists. Please choose a different name.");
            }
        }

        // Method that displays the Name and Category into the listview
        private void DisplayData()
        {
            listView1.Items.Clear();

            foreach (var data in Wiki)
            {
                ListViewItem item = new ListViewItem(data.GetName());
                item.SubItems.Add(data.GetCategory());
                listView1.Items.Add(item);
            }
        }

        /*
        6.4 Create a custom method to populate the ComboBox when the Form Load method is called. The six categories 
        must be read from a simple text file.
        */
        private void PopulateComboBox()
        {
            string[] comboText = File.ReadAllLines("categories.txt");
            foreach (var category in comboText)
            {
                comboBoxCategory.Items.Add(category);
            }
        }

        /* 
        6.5 Create a custom ValidName method which will take a parameter string value from the Textbox Name and returns 
        a Boolean after checking for duplicates. Use the built in List<T> method “Exists” to answer this requirement.
        */
        private bool ValidName(string name)
        {
            // Use the Exists method to check if a name exists in the list
            bool nameExists = Wiki.Exists(info => info.GetName() == name);

            // Return true if the name does not exist (is valid), otherwise, return false
            return !nameExists;
        }

        /* 
        6.6 Create two methods to highlight and return the values from the Radio button GroupBox. The first method must
        return a string value from the selected radio button (Linear or Non-Linear). The second method must send an integer 
        index which will highlight an appropriate radio button.
        */
        private string GetSelectedRadioButtonValue()
        {
            // Check which radio button is selected and return its value
            if (radioButtonLinear.Checked)
            {
                return "Linear";
            }
            else if (radioButtonNonLin.Checked)
            {
                return "Non-Linear";
            }
            else
            {
                return string.Empty; // No radio button is selected
            }
        }

        private void SetSelectedRadioButton(int index)
        {
            // Use the index to select the appropriate radio button
            switch (index)
            {
                case 0:
                    radioButtonLinear.Checked = true;
                    break;
                case 1:
                    radioButtonNonLin.Checked = true;
                    break;
                default:
                    // Handle invalid index (optional)
                    break;
            }
        }

        /*
        6.7 Create a button method that will delete the currently selected record in the ListView. Ensure the user has 
        the option to backout of this action by using a dialog box. Display an updated version of the sorted list at the 
        end of this process.
        */
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the ListView.
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected item.
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string selectedName = selectedItem.Text; // Assuming the text represents the data.

                // Find the Information object that matches the selected name.
                Information selectedInfo = Wiki.Find(info => info.GetName() == selectedName);

                if (selectedInfo != null)
                {
                    // Ask the user for confirmation before deleting.
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove the selected Information object from the Wiki list.
                        Wiki.Remove(selectedInfo);

                        // Remove the selected item from the ListView.
                        listView1.Items.Remove(selectedItem);
                    }
                }
                else
                {
                    MessageBox.Show("Selected data not found in the list.");
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }

        /*
        6.9 Create a single custom method that will sort and then display the Name and Category from the wiki 
        information in the list.
        */
        private void SortAndDisplayData()
        {
            // Sort the List<T> by the Name property (assuming Information class has a GetName method).
            Wiki.Sort((info1, info2) => info1.GetName().CompareTo(info2.GetName()));

            // Clear the ListView.
            listView1.Items.Clear();

            // Populate the ListView with the sorted data.
            foreach (var data in Wiki)
            {
                ListViewItem item = new ListViewItem(data.GetName());
                item.SubItems.Add(data.GetCategory());
                listView1.Items.Add(item);
            }
        }
        /*
        6.10 Create a button method that will use the builtin binary search to find a Data Structure name. 
        If the record is found the associated details will populate the appropriate input controls and highlight 
        the name in the ListView. At the end of the search process the search input TextBox must be cleared.
        */
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            /*
            // Retrieve the search input from the TextBox.
            string searchName = textBoxSearch.Text;

            // Create a temporary Information object for searching.
            Information searchInfo = new Information(searchName);

            // Perform binary search on the sorted Wiki list.
            int index = Wiki.BinarySearch(searchInfo);

            // Check if the record is found.
            if (index >= 0)
            {
                // Retrieve the found Information object.
                Information foundInformation = Wiki[index];

                // Update the input controls with the associated details.
                textBoxDSN.Text = foundInformation.GetName();
                comboBoxCategory.Text = foundInformation.GetCategory();
                SetSelectedRadioButton(foundInformation.GetStructure() == "Linear" ? 0 : 1);
                textBoxDefinition.Text = foundInformation.GetDefinition();

                // Highlight the name in the ListView.
                listView1.Items[index].Selected = true;
                listView1.Select();

                // Clear the search input TextBox.
                textBoxSearch.Clear();
            }
            else
            {
                MessageBox.Show("Record not found.");
            }
            */
        }

        /*
        6.11 Create a ListView event so a user can select a Data Structure Name from the list of Names and the
        associated information will be displayed in the related text boxes combo box and radio button.
        */
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selectedName = listView1.SelectedItems[0].Text;

                Information selectedData = Wiki.Find(d => d.GetName() == selectedName);
            }
        }

        // 6.12 Create a custom method that will clear and reset the TextBoxes, ComboBox and Radio button
        private void ClearInputs()
        {
            radioButtonNonLin.Checked = false;
            radioButtonLinear.Checked = false;
            textBoxDSN.Clear();
            comboBoxCategory.SelectedItem = null;
            textBoxDefinition.Clear();
        }

        // 6.13 Create a double click event on the Name TextBox to clear the TextBboxes, ComboBox and Radio button.
        private void textBoxDSN_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the left mouse button is clicked twice (double-click)
            if (e.Button == MouseButtons.Left & e.Clicks == 2)
            {
                ClearInputs();
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to let the user choose a file to open.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                openFileDialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a BinaryReader to read data from the selected file.
                        using (BinaryReader reader = new BinaryReader(File.Open(openFileDialog.FileName, FileMode.Open)))
                        {
                            // Clear existing data in the Wiki list.
                            Wiki.Clear();

                            // Read and deserialize the data from the file into the Wiki list.
                            int count = reader.ReadInt32();
                            for (int i = 0; i < count; i++)
                            {
                                string name = reader.ReadString();
                                string category = reader.ReadString();
                                string structure = reader.ReadString();
                                string definition = reader.ReadString();

                                //Information info = new Information(name, category, structure, definition);
                                //Wiki.Add(info);
                            }

                            // Display the updated data.
                            DisplayData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to let the user specify a file for saving.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                saveFileDialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Create a BinaryWriter to write data to the selected file.
                        using (BinaryWriter writer = new BinaryWriter(File.Open(saveFileDialog.FileName, FileMode.Create)))
                        {
                            // Write the count of items in the Wiki list.
                            writer.Write(Wiki.Count);

                            // Write and serialize each item in the Wiki list.
                            foreach (Information info in Wiki)
                            {
                                writer.Write(info.GetName());
                                writer.Write(info.GetCategory());
                                writer.Write(info.GetStructure());
                                writer.Write(info.GetDefinition());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
