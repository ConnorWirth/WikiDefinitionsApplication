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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Reflection;
using System.Diagnostics;

namespace WikiDefinitionsApplication
{
    public partial class FormWikiDefinitions : Form
    {
        public FormWikiDefinitions()
        {
            InitializeComponent();
            // Attach an event handler to the MouseDown event of the "textBoxDSN" control.
            textBoxDSN.MouseDown += textBoxDSN_MouseDown;
            // Attach an event handler to the FormClosing event of the form itself.
            this.FormClosing += FormWikiDefinitions_FormClosing;
            // Save button is disabled before program to stop user from saving program with no data in listview
            buttonSave.Enabled = false;
        }

        private void FormWikiDefinitions_Load(object sender, EventArgs e)
        {
            // Populate the ComboBox with data when the form is loaded.
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
            // Get the name entered in the TextBox.
            string name = textBoxDSN.Text;

            // Check if the name is valid (not a duplicate)
            if (ValidName(name))
            {
                // Check for empty input fields or unselected radio buttons.

                if (string.IsNullOrEmpty(textBoxDSN.Text) ||
                    comboBoxCategory.SelectedIndex == -1 ||
                    (!radioButtonLinear.Checked && !radioButtonNonLin.Checked) ||
                    string.IsNullOrEmpty(textBoxDefinition.Text))
                {
                    // Display a message in the status strip indicating empty fields.
                    statusStrip1.Items.Clear();
                    statusStrip1.Items.Add("Empty textbox/es or buttons");
                }
                else
                {
                    // Create a new Information object and populate it with input values.
                    Information newInformation = new Information();
                    newInformation.SetName(name);
                    newInformation.SetCategory(comboBoxCategory.Text);
                    newInformation.SetStructure(GetSelectedRadioButtonValue());
                    newInformation.SetDefinition(textBoxDefinition.Text);

                    // Enable the "Save" button, add the new information to the "Wiki" list, and clear input fields.
                    buttonSave.Enabled = true;
                    Wiki.Add(newInformation);
                    ClearInputs();

                    // Refresh the ListView and sort data, ensuring the new item is displayed appropriately.
                    DisplayData();
                    GetSelectedRadioButtonValue();
                    SortAndDisplayData();

                    // Display a success message in the status strip.
                    statusStrip1.Items.Clear();
                    statusStrip1.Items.Add("Data successfully added");
                }
            }
            else
            {
                // Handle the case where the name is a duplicate and display an error message.
                MessageBox.Show("Name already exists. Please choose a different name.");
                statusStrip1.Items.Clear();
            }
        }

        // Method that displays the Name and Category into the listview
        private void DisplayData()
        {
            // Clear the existing items in the ListView.
            listView1.Items.Clear();

            // Iterate through each data item in the "Wiki" data source.
            foreach (var data in Wiki)
            {
                // Create a new ListViewItem and set its primary text to the name of the data item.
                ListViewItem item = new ListViewItem(data.GetName());

                // Add a subitem to the ListViewItem with the category of the data item.
                item.SubItems.Add(data.GetCategory());

                // Add the ListViewItem to the ListView, effectively displaying the data in the ListView.
                listView1.Items.Add(item);
            }
        }

        /*
        6.4 Create a custom method to populate the ComboBox when the Form Load method is called. The six categories 
        must be read from a simple text file.
        */
        private void PopulateComboBox()
        {
            // Read the lines from a text file named "categories.txt" and store them in an array of strings.
            string[] comboText = File.ReadAllLines("categories.txt");

            // Iterate through each category in the array and add them to the ComboBox.
            foreach (var category in comboText)
            {
                // Add the current category to the items of the ComboBox.
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

                        // Clear input controls
                        ClearInputs();

                        // Display a success message in the status strip
                        statusStrip1.Items.Clear();
                        statusStrip1.Items.Add("Data successfully deleted");

                        // Disable the "Save" button if the ListView is empty
                        if (listView1.Items.Count == 0)
                        {
                            buttonSave.Enabled = false;
                        }
                    }
                }
                else
                {
                    // Display a message indicating that the selected data was not found in the list.
                    MessageBox.Show("Selected data not found in the list.");
                }
            }
            // If listview is empty, tell user that listbox is empty so nothing can be deleted
            else if (listView1.Items.Count == 0)
            {
                MessageBox.Show("There is no items in listbox to delete.");
            }      
            else
            {
                // Display a message indicating that no item is selected for deletion.
                MessageBox.Show("Please select an item to delete.");
            }
        }

        /*
        6.8 Create a button method that will save the edited record of the currently selected item in the ListView. All the 
        changes in the input controls will be written back to the list. Display an updated version of the sorted list at the 
        end of this process.
        */
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            // Get the name entered in the TextBox.
            string name = textBoxDSN.Text;

            // Check if any item is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Check if the name or definition TextBox is empty
                if (string.IsNullOrEmpty(textBoxDSN.Text) || string.IsNullOrEmpty(textBoxDefinition.Text))
                {
                    // Display a message in the status strip indicating empty textboxes
                    statusStrip1.Items.Clear();
                    statusStrip1.Items.Add("Empty textbox/es or buttons");
                }

                // Check is name being edited is a duplicate
                else if (ValidName(name))
                {
                    // Get the index of the selected item
                    int index = listView1.SelectedItems[0].Index;

                    // Update the properties of the selected item in the "Wiki" data source
                    Wiki[index].SetName(textBoxDSN.Text);
                    Wiki[index].SetCategory(comboBoxCategory.Text);
                    Wiki[index].SetStructure(GetSelectedRadioButtonValue());
                    Wiki[index].SetDefinition(textBoxDefinition.Text);

                    // Refresh the ListView with updated data
                    DisplayData();

                    // Reorganises the data so it's sorted in alphabetical order by Name
                    SortAndDisplayData();

                    // Clear input controls
                    ClearInputs();

                    // Display a success message in the status strip
                    statusStrip1.Items.Clear();
                    statusStrip1.Items.Add("Data successfully edited");
                }
                else
                {
                    // Handle the case where the name is a duplicate and display an error message.
                    MessageBox.Show("Name already exists. Please choose a different name.");
                    statusStrip1.Items.Clear();
                }
            }
            // If listview is empty, tell user that listbox is empty so nothing can be edited
            else if (listView1.Items.Count == 0)
            {
                MessageBox.Show("There is no items in listbox to edit.");
            }
            else
            {
                // Display a message in the status strip indicating no item is selected in the ListView
                statusStrip1.Items.Clear();
                statusStrip1.Items.Add("Item in listview not selected");
            }
        }

        /*
        6.9 Create a single custom method that will sort and then display the Name and Category from the wiki 
        information in the list.
        */
        private void SortAndDisplayData()
        {
            // Sort the "Wiki" data source by the Name property (assumes Information class has a GetName method).
            Wiki.Sort((info1, info2) => info1.GetName().CompareTo(info2.GetName()));

            // Clear the ListView to remove existing items.
            listView1.Items.Clear();

            // Populate the ListView with the sorted data. Iterate through each item in the "Wiki" data source.
            foreach (var data in Wiki)
            {
                // Create a new ListViewItem with the name of the data item.
                ListViewItem item = new ListViewItem(data.GetName());

                // Add a subitem to the ListViewItem with the category of the data item.
                item.SubItems.Add(data.GetCategory());

                // Add the ListViewItem to the ListView.
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
            // Get the search string from the TextBox control
            string searchName = textBoxSearch.Text;

            // Use LINQ to find the first item in the "Wiki" data source whose name matches the search string (case-insensitive)
            Information foundInfo = Wiki.FirstOrDefault(info =>
                string.Equals(info.GetName(), searchName, StringComparison.OrdinalIgnoreCase));

            if (foundInfo != null)
            {
                // Name found, update input controls and highlight ListView item

                // Set the text of a TextBox control to the name of the found item
                textBoxDSN.Text = foundInfo.GetName();

                // Set the text of a ComboBox control to the category of the found item
                comboBoxCategory.Text = foundInfo.GetCategory();

                // Check if the "Linear" radio button is checked and set the selected radio button accordingly
                if (radioButtonLinear.Checked)
                {
                    SetSelectedRadioButton(0);
                }
                else
                {
                    SetSelectedRadioButton(1);
                }

                // Set the text of a TextBox control to the definition of the found item
                textBoxDefinition.Text = foundInfo.GetDefinition();

                // Get the index of the found item in the "Wiki" data source
                int index = Wiki.IndexOf(foundInfo);

                // Highlight the corresponding item in the ListView
                listView1.Items[index].Selected = true;
                listView1.Items[index].EnsureVisible();

                // Clear items in the status strip and add a success message
                statusStrip1.Items.Clear();
                statusStrip1.Items.Add("Data successfully found");
            }
            else
            {
                // Name not found, clear input controls

                // Clear input controls such as TextBox, ComboBox, and radio buttons
                ClearInputs();

                // Show a message box indicating that the data structure was not found
                MessageBox.Show("Data Structure not found.");
            }
        }

        /*
        6.11 Create a ListView event so a user can select a Data Structure Name from the list of Names and the
        associated information will be displayed in the related text boxes combo box and radio button.
        */
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if any item in the ListView is selected
            if (listView1.SelectedItems.Count > 0)
            {
                // Clear the items in a status strip (if present)
                statusStrip1.Items.Clear();

                // Get the index of the selected item in the ListView
                int index = listView1.SelectedItems[0].Index;

                // Retrieve the structure of the selected item from a data source (presumably an array or list called "Wiki")
                string structure = Wiki[index].GetStructure();

                // Set the text of a TextBox control to the name of the selected item
                textBoxDSN.Text = Wiki[index].GetName();

                // Set the text of a ComboBox control to the category of the selected item
                comboBoxCategory.Text = Wiki[index].GetCategory();

                // Check if the structure of the selected item is "Linear"
                if (structure == "Linear")
                {
                    // Set the selected radio button to the first option (presumably a radio button group)
                    SetSelectedRadioButton(0);
                }
                else
                {
                    // Set the selected radio button to the second option
                    SetSelectedRadioButton(1);
                }

                // Set the text of a TextBox control to the definition of the selected item
                textBoxDefinition.Text = Wiki[index].GetDefinition();
            }
        }

        // 6.12 Create a custom method that will clear and reset the TextBoxes, ComboBox and Radio button
        private void ClearInputs()
        {
            // Clear the selection of the "Non-Linear" radio button.
            radioButtonNonLin.Checked = false;
            // Clear the selection of the "Linear" radio button.
            radioButtonLinear.Checked = false;
            // Clear the text in the "Data Structure Name" text box.
            textBoxDSN.Clear();
            // Deselect any selected item in the "Category" combo box.
            comboBoxCategory.SelectedItem = null;
            // Clear the text in the "Definition" text box.
            textBoxDefinition.Clear();
            // Clear any items in the status strip.
            statusStrip1.Items.Clear();
        }

        // 6.13 Create a double click event on the Name TextBox to clear the TextBboxes, ComboBox and Radio button.
        private void textBoxDSN_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the left mouse button is clicked twice (double-click). If it has, clear the textboxes, combobox and radio buttons.
            if (e.Button == MouseButtons.Left & e.Clicks == 2)
            {
                ClearInputs();
            }
        }

        /*
        6.14 Create two buttons for the manual open and save option; this must use a dialog box to select a file or 
        rename a saved file. All Wiki data is stored/retrieved using a binary reader/writer file format.
        */
        // Event handler for the "Load" button click.
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to let the user choose a file to open.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the initial directory and filter for file selection.
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

                                // Create an Information object and add it to the Wiki list.
                                Information info = new Information(name, category, structure, definition);
                                Wiki.Add(info);
                                // Enable the "Save" button and clear input controls.
                                buttonSave.Enabled = true;
                                ClearInputs();
                            }

                            // Display the updated data.
                            DisplayData();
                            // Update the status strip to indicate that data was successfully loaded.
                            statusStrip1.Items.Clear();
                            statusStrip1.Items.Add("Data successfully loaded");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors or exceptions that occur during the loading process.
                        MessageBox.Show("Error opening the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Event handler for the "Save" button click.
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to let the user specify a file for saving.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the initial directory and filter for file selection.
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
                            
                            // Clear input controls and update the status strip to indicate that data was successfully saved.
                            ClearInputs();
                            statusStrip1.Items.Clear();
                            statusStrip1.Items.Add("Data successfully saved");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors or exceptions that occur during the saving process.
                        MessageBox.Show("Error saving the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 6.15 The Wiki application will save data when the form closes.
        public void SaveOnClose()
        {
            // Check if the ListView is empty (contains no items).
            if (listView1.Items.Count == 0)
            {
                // Show a message indicating that the form is closed, and no data will be saved.
                MessageBox.Show("Form closed. Nothing in ListView. No data will be saved.");
            }
            else
            {
                // Specify the file path where you want to save the data.
                string filePath = "closed_form.bin";

                try
                {
                    // Create a BinaryWriter to write data to the file.
                    using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                    {
                        // Write the count of data items to the file.
                        writer.Write(Wiki.Count);

                        // Iterate through each Information object in the Wiki list and write its properties to the file.
                        foreach (Information info in Wiki)
                        {
                            writer.Write(info.GetName());
                            writer.Write(info.GetCategory());
                            writer.Write(info.GetStructure());
                            writer.Write(info.GetDefinition());
                        }

                        // Show a message indicating that the form is closed, and the data in the ListView has been saved.
                        MessageBox.Show("Form closed. Data in ListView saved in Debug file.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors or exceptions that occur during the saving process.
                    MessageBox.Show("Error saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Event handler for the FormClosing event of the form.
        public void FormWikiDefinitions_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Call the SaveOnClose method when the form is closing to save data.
            SaveOnClose();
        }

    }
}
