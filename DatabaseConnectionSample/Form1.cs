using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DatabaseConnectionSample
{
    public partial class Form1 : Form
    {
        // My Connection String
        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\\..\\..\\INventoryDatabase.accdb";

        OleDbConnection con; // Code that handles the connection of the database

        public Form1()
        {
            InitializeComponent();
        }


        // After the Form loads this will execute the connection of the database.
        // If the connection is error the application will exit
        private void Form1_Load(object sender, EventArgs e)
        {

            // Try the code, if the code is error then it will run the catch block
            try
            {
                // Setup the connection
                con = new OleDbConnection(connectionString);

                // try to connect to the database
                // by opening the connection
                con.Open();
                //Show a message that the program has connected to the database
                MessageBox.Show("Connected to the Database");

                // close the connection
                con.Close();

                // Load the data from the Database into our listview
                // See the void LoadListView() to see how to implement.
                LoadListView();
                LoadListView();

            }
            catch (Exception ex)
            {
                // If any error has occured in the try block statement 
                // e.g Wrong Connection string
                // it will run the catch codeblock


                // Show this message if an error occured in thy try block code
                MessageBox.Show("Cannot connect to the database");

                // Show the Error
                MessageBox.Show(ex.ToString());

                // The app will close if there is an error to the connection of the database
                Application.Exit();
            }
        }


        // A custom function/method to be able to load 
        // data into my ListView.
        // I can call this block of code anywhere within 
        // this class only.
        void LoadListView()
        {
            // Open our connection first
            con.Open();


            // Setup SQL command
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con; // Set the command connection to our connection we created.
            cmd.CommandText = "SELECT * FROM tblItems"; //SQL command that load all data in the tblItems.

            OleDbDataReader reader = cmd.ExecuteReader(); // Execute the SQL command and the returned data will be thrown to the reader object

            // Clear the list so the data loaded wont repeat
            listItem.Items.Clear();


            while (reader.Read()) // Read every rows that returned in the SQL command
            {

                // ListViewItem represents a row in the  ListView
                ListViewItem lvi = new ListViewItem(reader.GetValue(0).ToString()); // Set the first column of our list to the ID of the tblItems. 
                lvi.SubItems.Add(reader.GetValue(1).ToString()); //Set the 2nd column of our list to the NAME of the tblItems. 
                lvi.SubItems.Add(reader.GetValue(2).ToString()); //Set the 3rd column of our list to the PRICE of the tblItems. 
                lvi.SubItems.Add(reader.GetValue(3).ToString());//Set the 4th column of our list to the QTY of the tblItems. 
                lvi.SubItems.Add(reader.GetValue(4).ToString());//Set the 5th column of our list to the MANUFACTURER of the tblItems. 


                listItem.Items.Add(lvi); // Add a row to the listView; 

            }

            // Close the connection of the database
            con.Close();

        }

        // A custom function that adds item to the database
        void AddItem()
        {
            // Open the connection 
            con.Open();

            //Setup SQL command
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO tblItems " +
                              " (itemName, itemPrice, itemQty, itemManufacturer)  VALUES " +
                              " (@name,    @price,    @qty,    @manufacturer)";
            /*
             * The @name will be replaced in thee cmd.Parameters.... by the textName.Text
             */
            cmd.Parameters.Add("@name", OleDbType.VarChar, 100).Value = textName.Text; // Replace the @name to the text of textName
            cmd.Parameters.Add("@price", OleDbType.Double, 100).Value = textPrice.Text; // Replace the @price to the text of textName
            cmd.Parameters.Add("@qty", OleDbType.Integer, 100).Value = textQty.Text; // Replace the @qty to the text of textQty
            cmd.Parameters.Add("@manufacturer", OleDbType.VarChar, 100).Value = textManufacturer.Text; // Replace the @qty to the text of textManufacturer

            /*
             * Note the paramters for this is
             * ("@string", OleDbType.DATATYPE, LIMIT)
             * 
             * If the column of your database is Text then Use OleDbType.VarChar
             *  If the column of your database is a floating point then Use OleDbType.Double
             *  If the column of your database is a Number then Use OleDbType.Integer
             */


            // Execute the command
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            // CLose the connection
            con.Close();

            // Show a message that the item has been added
            MessageBox.Show("Item added!", "",MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        // Check if the textbox is correctly typed
        // returns true if there is no error in the textboxes
        bool Validate()
        {
            // https://stackoverflow.com/questions/9823836/checking-if-a-variable-is-of-data-type-double

            double doubleResult;
            int intResult;

            bool isDouble = Double.TryParse(textPrice.Text, out doubleResult); // returns true if the textPrice is a double
            bool isInteger = Int32.TryParse(textQty.Text, out intResult);// returnse true if the textQty is an integer

            bool ret = true;

            if (!isDouble) // if the textPrice is not a double then show a message box
            {
                ret = false;
                MessageBox.Show("Invalid Price", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (!isInteger) // if the textQty is not an integer then show a messagebox
            {
                ret = false;
                MessageBox.Show("Invalid Qty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return ret; // if there are no errors then this function will return true




        }


        // Update Database implementation
        void UpdateItem()
        {

            // Open the connection 
            con.Open();

            //Setup SQL command
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE tblItems " +
                              " SET itemName=@name, itemPrice=@price, itemQty=@qty, itemManufacturer=@manufacturer " +
                              " WHERE id=@id";
            /*
             * The @name will be replaced in thee cmd.Parameters.... by the textName.Text
             */
            cmd.Parameters.Add("@name", OleDbType.VarChar, 100).Value = textName.Text; // Replace the @name to the text of textName
            cmd.Parameters.Add("@price", OleDbType.Double, 100).Value = textPrice.Text; // Replace the @price to the text of textName
            cmd.Parameters.Add("@qty", OleDbType.Integer, 100).Value = textQty.Text; // Replace the @qty to the text of textQty
            cmd.Parameters.Add("@manufacturer", OleDbType.VarChar, 100).Value = textManufacturer.Text; // Replace the @qty to the text of textManufacturer
            cmd.Parameters.Add("@id", OleDbType.Integer, 100).Value = textId.Text; // Replace the @id to the text of id

            /*
             * Note the paramters for this is
             * ("@string", OleDbType.DATATYPE, LIMIT)
             * 
             * If the column of your database is Text then Use OleDbType.VarChar
             *  If the column of your database is a floating point then Use OleDbType.Double
             *  If the column of your database is a Number then Use OleDbType.Integer
             */


            // Execute the command
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            // CLose the connection
            con.Close();

            // Show a message that the item has been added
            MessageBox.Show("Item Updated!");
        }

        // Delete Database implementation
        void DeleteItem()
        {

            // Open the connection 
            con.Open();

            //Setup SQL command
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            cmd.CommandText = "DELETE FROM tblItems WHERE id=@id";

            /*
             * The @name will be replaced in thee cmd.Parameters.... by the textName.Text
             */
            cmd.Parameters.Add("@id", OleDbType.Integer, 100).Value = textId.Text; // Replace the @id to the text of id

            /*
             * Note the paramters for this is
             * ("@string", OleDbType.DATATYPE, LIMIT)
             * 
             * If the column of your database is Text then Use OleDbType.VarChar
             *  If the column of your database is a floating point then Use OleDbType.Double
             *  If the column of your database is a Number then Use OleDbType.Integer
             */


            // Execute the command
            cmd.Prepare();
            cmd.ExecuteNonQuery();

            MessageBox.Show("Item Deleted!","", MessageBoxButtons.OK,MessageBoxIcon.Information);
            // CLose the connection
            con.Close();

            // Show a message that the item has been added
        }


        // A custom function that clears all the textbox
        // 
        void ClearTextBoxes()
        {
            textId.Text = "";
            textName.Text = "";
            textPrice.Text = "";
            textQty.Text = "";
            textManufacturer.Text = "";
        }

        // Add button; Execute the AddItem function
        private void button1_Click(object sender, EventArgs e)
        {
            // Executes the void AddItem();
            // see void AddItem() to see the how to code

            // If validate() returns a true(No error in the textboxes)
            // Then Add the item to the Database
            if (Validate())
            {
                AddItem();
                
                // Clear the textboxes after the item has been added
                ClearTextBoxes();

                // Refreshes the ListView
                LoadListView();
            }


        }


        // If the user selects an item in the ListView then set the selected item to the textboxes
        private void listItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If there are no selected Items
            if (listItem.SelectedItems.Count == 0)
            {
                ClearTextBoxes();
                button1.Enabled = true; // Enable the save button
                button2.Enabled = false; // Disable the Edit Button
                button3.Enabled = false; // Disable the Delete Button
                return; // Stops the function
            }
            else
            {
                button1.Enabled = false; // Disable the save button
                button2.Enabled = true; // Enable the Edit Button
                button3.Enabled = true; // Enable the Delete Button
            }

            // Set the Selected item to the textbox
            textId.Text = listItem.SelectedItems[0].SubItems[0].Text ; // [0] is the id in our listview
            textName.Text = listItem.SelectedItems[0].SubItems[1].Text; // [1] is the name in our listview
            textPrice.Text = listItem.SelectedItems[0].SubItems[2].Text; // [2] is the price in our listview
            textQty.Text = listItem.SelectedItems[0].SubItems[3].Text; // [3] is the qty in our listview
            textManufacturer.Text = listItem.SelectedItems[0].SubItems[4].Text; // [4] is the qty in our listview


        }


        // Update the Item
        private void button2_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                UpdateItem();
                ClearTextBoxes();
                // Refreshes the listview
                LoadListView();

                button1.Enabled = true; // Enable the save button
                button2.Enabled = false; // Disable the Edit Button
                button3.Enabled = false; // Disable the Delete Button
            }
        }

        // Delete item
        private void button3_Click(object sender, EventArgs e)
        {

            // http://stackoverflow.com/questions/3036829/ddg#3036880
            DialogResult result = MessageBox.Show("Delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ClearTextBoxes();
                LoadListView();

                button1.Enabled = true; // Enable the save button
                button2.Enabled = false; // Disable the Edit Button
                button3.Enabled = false; // Disable the Delete Button
            }

        }
    }
}
