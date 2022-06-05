using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms_DatabaseManager
{
    public partial class Form1 : Form
    {
        string error_message = "";
        string query_database = "";
        string query_table = "";
        string query0 = "";
        int num_columns = 0; // number of columns, calculated by ShowColumns()
        // int num_rows = 0; // number of rows, calculate by ShowValues()
        int num_databases = 0;
        int num_tables = 0;
        int num_table_entries = 0;
        int last_row_index = 0; // index of last row, calculated by ShowValues()
        string cell_value0 = ""; // calculate by CellBeginEdit()
        int cell_rowindex0 = 0; // calculated by CellBeginEdit()
        int cell_columnindex0 = 0; // calculated by CellBeginEdit()
        string column_name0 = ""; // calculated by CellBeginEdit()
        string pr_cell_value0 = ""; // calculated by CellBeginEdit()
        int newrow = 0; // calculated by CellBeginEdit()
        string selected_database = "";
        string selected_table = "";
        List<string> Fields = new List<string>(); // list of column names
        bool showColumns = true;
        string primary_value = "";
        int PK_row_index = -1;
        string PK_field_name = "";
        bool edit_mode = false;
        bool edit_activated = false;
        bool insert_activated = false;
        bool deleteRow_activated = false;
        string new_table_name = "";
        bool create_table = false;
        bool create_database = false;


        // List<string> New_row_values = new List<string>();
        public Form1()
        {
            InitializeComponent();
            dataGridView_showDatabases.AllowUserToResizeRows = false;
            dataGridView_showDatabases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_showTables.AllowUserToResizeRows = false;
            dataGridView_showTables.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_values.AllowUserToResizeRows = false;
            button_createDatabase.Visible = false;
            button_deleteDatabase.Visible = false;
            button_createTable.Visible = false;
            button_deleteTable.Visible = false;
            button_edit.Visible = false;
            button_insertRow.Visible = false;
            button_deleteRow.Visible = false;
            button_showValues.Visible = false;
            button_showColumns.Visible = false;
        }

        private void ShowDatabases()
        {
            dataGridView_showDatabases.Columns.Clear(); // this function runs upon database deletion or creation, so Clear is needed
            string connectionString = $"datasource=localhost;port=3306;username=root;password=;"; // there is no selected database yet
            query_database = "SHOW DATABASES;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query_database, databaseConnection);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                dataGridView_showDatabases.Columns.Add(new DataGridViewColumn() { HeaderText = "Databases:", CellTemplate = new DataGridViewTextBoxCell() });
                dataGridView_showDatabases.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.00F, FontStyle.Bold);
                if (reader.HasRows)
                {
                    int row_index = 0;
                    int row_selected_database = 0;
                    // ERROR MESSAGE if we query more columns than exist!
                    while (reader.Read())
                    {
                        string[] row = { reader.GetString(0) };
                        if (create_table || create_database) // when creating new database or table, it selects the selected row and not the first row!
                        {
                            if (selected_database == reader.GetString(0)) row_selected_database = row_index; // the selected_database is here the database selected upon table creation!
                        }
                        // richTextBox1.Text += row[0];
                        // richTextBox1.Text += "\n";

                        dataGridView_showDatabases.Rows.Insert(row_index, reader.GetString(0)); // should the row_index remain 0, it would always insert the rows at the fist position, resulting in a reverse order!
                        row_index++;
                    }
                    // dataGridView_showDatabases.Sort(this.dataGridView_showDatabases.Columns[0], ListSortDirection.Ascending);
                    dataGridView_showDatabases.CurrentCell = dataGridView_showDatabases.Rows[row_selected_database].Cells[0];
                    selected_database = dataGridView_showDatabases.CurrentCell.Value.ToString();
                    dataGridView_showDatabases.AllowUserToAddRows = false; // hide last empty row
                    dataGridView_showDatabases.RowHeadersVisible = false; // hide first control column
                    // when initialising Form1, it automatically fills the size of the ClientSize: DataGridViewAutoSizeColumnsMode.Fill 
                    /* 
                    if (dataGridView_showDatabases.Columns[0].Width < dataGridView_showDatabases.ClientSize.Width - 3)
                    {
                        dataGridView_showDatabases.Columns[0].Width = dataGridView_showDatabases.ClientSize.Width - 3;
                    }
                    */

                    // num_columns = dataGridView_showDatabases.ColumnCount;
                    num_databases = dataGridView_showDatabases.RowCount; // adatbázisok száma
                    if (button_connect.Enabled) // this runs only once upon start, when the Connect button is still active
                    {
                        button_createDatabase.Visible = true;
                        button_deleteDatabase.Visible = true;
                        button_createTable.Visible = true;
                        button_deleteTable.Visible = true;
                        button_edit.Visible = true;
                        button_insertRow.Visible = true;
                        button_deleteRow.Visible = true;
                        button_showValues.Visible = true;
                        button_showColumns.Visible = true;
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                error_message = ex.ToString();
                if (error_message.Contains("Unable to connect to any of the specified MySQL hosts."))
                {
                    MessageBox.Show("Unable to connect to the database! \nPlease make sure that the database server is available.");
                }
                label_message.Text = error_message;
            }
            if (error_message == "") // if there is no error message (connection to database present)
            {
                ShowTables();
                if (selected_table != "") ShowColumns(); // executed if there is a tabel in the selected database 
                query0 = ""; // show this value only once
            }

        }

        private void ShowTables()
        {
            dataGridView_showTables.Columns.Clear(); // not necessary if there is no reconnect
            string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
            query_table = $"SHOW TABLES FROM `{selected_database}`;";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query_table, databaseConnection);
            MySqlDataReader reader;
            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                dataGridView_showTables.Visible = true;
                dataGridView_showTables.Columns.Add(new DataGridViewColumn() { HeaderText = "Tables:", CellTemplate = new DataGridViewTextBoxCell() });
                dataGridView_showTables.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.00F, FontStyle.Bold);
                selected_table = ""; // remains empty if there is no table in the database
                if (reader.HasRows)
                {
                    int row_index = 0;
                    int row_selected_table = 0;
                    while (reader.Read())
                    {
                        dataGridView_showTables.Rows.Insert(row_index, reader.GetString(0));
                        if (create_table) // when creating new table, its row is selected instead of the first one!
                        {
                            if (new_table_name == reader.GetString(0)) row_selected_table = row_index;
                        }
                        row_index++;
                    }
                    // dataGridView_showTables.Sort(this.dataGridView_showTables.Columns[0], ListSortDirection.Ascending);

                    dataGridView_showTables.CurrentCell = dataGridView_showTables.Rows[row_selected_table].Cells[0];
                    selected_table = dataGridView_showTables.CurrentCell.Value.ToString();
                    dataGridView_showTables.AllowUserToAddRows = false; // hide last empty row
                    dataGridView_showTables.RowHeadersVisible = false; // hide first control column
                    num_tables = dataGridView_showTables.RowCount; // number of tables
                                                                   //  label_message.Text = query_database + "\n" + query_table;
                                                                   // decreasing column by the width of VScrollbar szélességével, if VScrollBar is present (to avoid HSCrollbar from appearing at the bottom)
                    /*
                    foreach (var Control in dataGridView_showTables.Controls)
                    {
                        if (Control.GetType() == typeof(VScrollBar))
                        {
                            if (((VScrollBar)Control).Visible == false)
                            {
                                dataGridView_showTables.Columns[0].Width -= 17;
                            }
                        }
                    }
                    */
                    /*
                    if (dataGridView_showTables.RowCount < 12)
                    {
                        dataGridView_showTables.Columns[0].Width = dataGridView_showTables.ClientSize.Width-3;
                    }
                    */
                    // when initialising Form1, it automatically fills the size of the ClientSize: DataGridViewAutoSizeColumnsMode.Fill 
                    /*
                    if (dataGridView_showTables.Columns[0].Width < dataGridView_showTables.ClientSize.Width - 3)
                    {
                        dataGridView_showTables.Columns[0].Width = dataGridView_showTables.ClientSize.Width - 3;
                    }
                    */
                }
                else
                {
                    // Console.WriteLine("No rows found.");
                    // MessageBox.Show("There is no table in the database!");
                    dataGridView_values.Columns.Clear(); // the Columns Gridview has to be deleted because of the empty table
                    label_message.Text = "There is no table in the selected database!";
                    button_showValues.Enabled = false; // be not active if there is no table in the database
                }
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                label_message.Text = ex.ToString();
            }

        }

        private void dataGridView_showTablesAndColumns_CellClick(object sender, DataGridViewCellEventArgs e)
        { // selecting database
            if (selected_database != dataGridView_showDatabases.CurrentCell.Value.ToString())
            {
                selected_database = dataGridView_showDatabases.CurrentCell.Value.ToString();
                ShowTables();
                if (selected_table != "") ShowColumns(); // executed only if there is a table in the database
            }
        }

        private void dataGridView_showColumns_CellClick(object sender, DataGridViewCellEventArgs e)
        {   // select table IF there is one, because when eventually clicking on the "Tables:" header the program looks for the name of the tabel in row 1, which does not exist in such case and returns null value (and thus an error message)
            if (label_message.Text != "There is no table in the database!")
            { 
                if (dataGridView_showTables.CurrentCell.Value.ToString() != selected_table) // if selects a table other than the selected
                {
                    selected_table = dataGridView_showTables.CurrentCell.Value.ToString();
                    ShowColumns();
                    query0 = ""; // show this value only once
                }
            }
        }

        private void ShowColumns()
        {
            edit_mode = false;
            edit_activated = false;
            insert_activated = false;
            deleteRow_activated = false;
            button_deleteRow.ForeColor = Color.Black;
            dataGridView_values.Columns.Clear();
            Fields.Clear();
            string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
            string query = $"SHOW COLUMNS FROM `{selected_database}`.`{selected_table}`";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            MySqlDataReader reader;
            if (num_tables > 0) // if there is at least one table in the database
            {
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    int num_field_values = reader.FieldCount; // number of field values base on number of columns, which is 6
                                                              // naming columns

                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Field", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Type", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Null", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Key", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Default", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Extra", CellTemplate = new DataGridViewTextBoxCell() });
                    dataGridView_values.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.00F, FontStyle.Bold);
                    dataGridView_values.DefaultCellStyle.ForeColor = Color.Navy;
                    if (reader.HasRows)
                    {
                        string[] row = new string[num_field_values]; // creating row array based on the number of the table's columns
                        int row_index = 0;
                        PK_row_index = -1;
                        while (reader.Read())
                        {
                            // ERROR MESSAGE if we query more columns than exist!
                            for (int i = 0; i < num_field_values; i++)
                            {
                                row[i] = !reader.IsDBNull(i) ? reader.GetString(i) : ""; // filling up row array with the individual elements of the actual row
                                // row[i] = reader.GetString(i) != null ? reader.GetString(i) : "";
                                if (i == 0) Fields.Add(row[0]); // filling up Fields list with the name of the fields
                                if (i == 3 && row[3] == "PRI")
                                {
                                    PK_row_index = row_index; // determining number of primary key field, as it is not sure that the PK value is in column 1!
                                    PK_field_name = row[0]; // retermining name of PK field
                                }
                                
                            }
                            dataGridView_values.Rows.Insert(row_index, row);
                            row_index++;
                            // dataGridView_values.Rows.Insert(0, reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));

                            // string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5) };
                            // richTextBox1.Text += row[0] + " " + row[1] + " " + row[2] + " " + row[3] + " " + row[4] + " " + row[5];
                            // richTextBox1.Text += "\n";
                        }
                        // dataGridView_values.Sort(this.dataGridView_values.Columns[0], ListSortDirection.Ascending);

                        last_row_index = dataGridView_values.Rows.Count - 2; // -2 due to the last empty row added !
                        // filling up label fields with the values of the database's last row
                        // label_message.Text = dataGridView1.Rows.Count.ToString();
                        // textBox_rendszam.Text = dataGridView_values.Rows[last_row_index].Cells[0].Value.ToString();
                        // textBox_markanev.Text = dataGridView_values.Rows[
                        // last_row_index].Cells[1].Value.ToString();
                        // textBox_szin.Text = dataGridView_values.Rows[last_row_index].Cells[2].Value.ToString();
                        // textBox_vegsebesseg.Text = dataGridView_values.Rows[last_row_index].Cells[3].Value.ToString();
                        // dataGridView_values.CurrentCell = dataGridView_values.Rows[num_tables - 2].Cells[0];
                        // dataGridView_values.BeginEdit(false);
                        dataGridView_values.ReadOnly = true;
                        dataGridView_values.AllowUserToAddRows = false; // hide last empty row
                        dataGridView_values.RowHeadersVisible = false; // hide first empty control column 
                        dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.White; // remove blue selection highlight
                        dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.Navy;
                        // dataGridView_values.DefaultCellStyle.ForeColor = Color.Empty;
                        // dataGridView_values.DefaultCellStyle.BackColor = Color.Empty;
                        num_columns = dataGridView_values.RowCount; // number of database columns based on ROWS!
                        label_message.Text = query0 + query_database + "\n" + query_table + "\n" + query;
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                    }
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    label_message.Text = ex.ToString();
                }
                // runs even if there is no row in the table (but empty columns with header)
                // dataGridView_values.Visible = true;
                showColumns = true;
                button_edit.Enabled = false;
                button_insertRow.Enabled = false;
                button_deleteRow.Enabled = false;
                button_showValues.Enabled = true;
                button_showColumns.Enabled = false;
                create_table = false; // if table creation does not run and user clicks elsewhere, then it defaults to fale (so that a table can be created again, and that the Edit cell and Insert row do work again; IS THIS HERE LOCATED CORRECTLY ITT??
                create_database = false;
                // setting ClientSize, if larger than the total width of the columns
                dataGridView_values.AutoResizeColumns();
                int total_width = 5;
                for (int i = 0; i < dataGridView_values.ColumnCount; i++)
                {
                    total_width += dataGridView_values.Columns[i].Width + 2; // 2 pixels due to the 2 borders?
                }
                if (total_width < 731)
                {
                    dataGridView_values.ClientSize = new Size(total_width, 285);
                }
                else
                {
                    dataGridView_values.ClientSize = new Size(731, 285);
                }
            }
            else
            {
                // MessageBox.Show("There is no table in the database!");
            }
        }

        private void button_showColumns_Click(object sender, EventArgs e)
        {
            ShowColumns();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            dataGridView_showDatabases.ReadOnly = true;
            dataGridView_showTables.ReadOnly = true;
            ShowDatabases();
            if (error_message == "") // if the connection succeeded
            {
                button_connect.Text = "Connected";
                button_connect.Enabled = false;
                MessageBox.Show("Please select first the desired database and table by clicking on their corresponding row.", "Message:");
            }
            else
            {
                error_message = "";
            }
        }
        private void ShowValues()
        {
            // richTextBox1.Clear();
            dataGridView_values.Columns.Clear();
            button_deleteRow.ForeColor = Color.Black;
            edit_activated = false;
            insert_activated = false; // each time only one row can be inserted
            dataGridView_values.AllowUserToAddRows = false; // hide last empty row: the new empty row becomes visible only upon clicking the Insert Row button, after that it must be clicked again to insertion!
            deleteRow_activated = false;
            newrow = 0;
            if (!edit_mode)
            { // ha nem volt szerkesztés, akkor olvasási nézet
                dataGridView_values.ReadOnly = true;
                dataGridView_values.RowHeadersVisible = false; // hide first empty control column
            }
            // string connectionString = "datasource=localhost;port=3306;username=root;password=;database=gyar_dolgozoi;";
            // string query = "SELECT * FROM auto";
            // string connectionString = "datasource=localhost;port=3306;username=root;password=;database=autok;";
            // string query = "SELECT * FROM autok";
            string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};Convert Zero Datetime=True;"; // this last entry is required for eventual zero-value dates!
            string query = $"SELECT * FROM `{selected_table}`";
            if (!edit_mode) label_message.Text = query_database + "\n" + query_table + "\n" + query;

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            MySqlDataReader reader;
            if (num_columns > 0) // if there is a column in the table
            {
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    // naming columns
                    foreach (var field in Fields)
                    {
                        dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = field, CellTemplate = new DataGridViewTextBoxCell() });
                    }
                    // dataGridView_values.Columns[4].DefaultCellStyle.Format = "yyyy.MM.dd"; // does not work!
                    if (reader.HasRows)
                    {
                        string[] row = new string[num_columns];
                        int row_index = 0;
                        while (reader.Read())
                        {
                            // string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3) };
                            //richTextBox1.Text += row[0] + " " + row[1] + " " + row[2] + " " + row[3];
                            // richTextBox1.Text += "\n";
                            // creating row array based on the number of columns of the table!
                            string value = "";
                            for (int i = 0; i < num_columns; i++)
                            {
                                // string test = reader.GetString(i);
                                value = reader.GetValue(i).ToString(); // GetString(i) generates error when the Date is "0000-00-00" !
                                row[i] = !reader.IsDBNull(i) ? value : "";
                                if (value.Contains(" 00:00:00")) row[i] = value.Replace(" 00:00:00", "");  // remove long date format
                                if (value == "01.01.01 00:00:00") row[i] = "00-00-00"; // in case of date "0000-00-00" the .NET display is wrong

                            }
                            dataGridView_values.Rows.Insert(row_index, row);
                            // dataGridView_values.Rows.Insert(0, reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            row_index++;
                        }

                        //dataGridView_values.Sort(this.dataGridView_values.Columns[0], ListSortDirection.Ascending);

                        // TODO: ezt dinamikussá tenni??
                        // filling up label fields with the values of the table's last row
                        last_row_index = dataGridView_values.Rows.Count - 2; // -2 due to the last empty row added!
                        // num_columns = dataGridView_values.ColumnCount;
                        num_table_entries = dataGridView_values.RowCount;
                        /*
                        textBox_rendszam.Text = dataGridView_values.Rows[last_row_index].Cells[0].Value.ToString();
                        textBox_markanev.Text = dataGridView_values.Rows[last_row_index].Cells[1].Value.ToString();
                        textBox_szin.Text = dataGridView_values.Rows[last_row_index].Cells[2].Value.ToString();
                        textBox_vegsebesseg.Text = dataGridView_values.Rows[last_row_index].Cells[3].Value.ToString();
                        */

                        if (primary_value == "") // TODO: if the primary value may eventually be empty, then it is NOT good!
                        {
                            dataGridView_values.CurrentCell = dataGridView_values.Rows[0].Cells[0]; // is this needed?
                        }
                        else // if the primary key was edited, thent the cell right from it must be selected
                        {
                            int i = 0;
                            while (i < dataGridView_values.RowCount - 1)
                            {
                                if (dataGridView_values.Rows[i].Cells[0].Value != null && dataGridView_values.Rows[i].Cells[0].Value.ToString().ToLower() == primary_value.ToString().ToLower())
                                {
                                    break;
                                }
                                i++;
                            }
                            dataGridView_values.CurrentCell = dataGridView_values.Rows[i].Cells[0]; // is this needed?
                        }
                        // dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.Blue; // restore blue selection highlight
                        // dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.White;
                        button_edit.Enabled = true;
                        button_insertRow.Enabled = true;
                        button_deleteRow.Enabled = true;
                    }
                    else
                    {
                        Console.WriteLine("No rows found.");
                        button_insertRow.Enabled = true;
                    }
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // richTextBox1.Text = "Errod: There is a big trouble.";
                    label_message.Text = ex.ToString();
                }
                // runs even if there is no row in the table (but only empty columns with header)
                // dataGridView_values.Visible = true;
                showColumns = false;
                button_showValues.Enabled = false;
                button_showColumns.Enabled = true;

                // setting ClientSize if greater than the total width of columns
                dataGridView_values.AutoResizeColumns();
                int total_width = dataGridView_values.RowHeadersWidth + 3;
                for (int i = 0; i < dataGridView_values.ColumnCount; i++)
                {
                    total_width += dataGridView_values.Columns[i].Width + 2; // 2 pixels due to the border?
                }
                if (total_width < 731)
                {
                    dataGridView_values.ClientSize = new Size(total_width, 285);
                }
                else
                {
                    dataGridView_values.ClientSize = new Size(731, 285);
                }

            }
            else
            {
                MessageBox.Show("There is no column in the table");
            }
        }

        private void button_showValues_Click(object sender, EventArgs e)
        {
            if (selected_table != "")
            {
                ShowValues();
            }
            else
            {
                MessageBox.Show("Please select first the database and the table!");
            }
        }
        // CAN THIS BE DELETED?
        /*
        private void button_insert_Click(object sender, EventArgs e)
        {
            int newrow = 0;
            if (!CheckPrimaryKeyValue(textBox_rendszam.Text, newrow)) // ha már van primary key value, akkor leáll, és a lenti lekérdezést sem hajtja végre)
            {
                string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                // TODO: változókra átalakítani!
                // string query = "INSERT INTO `autok` (`rendszam`, `markanev`, `szin`, `vegsebesseg`) VALUES ('REN666', 'Fiat', 'fekete', '160');";
                string query_insert = "INSERT INTO `autok` (`rendszam`, `markanev`, `szin`, `vegsebesseg`) VALUES ('";
                query_insert += textBox_rendszam.Text;
                query_insert += "', '";
                query_insert += textBox_markanev.Text;
                query_insert += "', '";
                query_insert += textBox_szin.Text;
                query_insert += "', '";
                query_insert += textBox_vegsebesseg.Text;
                query_insert += "');";
                label_message.Text = query_insert;

                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query_insert, databaseConnection);
                // MySqlDataReader reader;
                try
                {
                    databaseConnection.Open();
                    commandDatabase.Prepare();
                    commandDatabase.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // richTextBox1.Text = "Hiba: Nagy baj van.";
                    label_message.Text = ex.ToString();
                }
                ShowValues();
            }
        }
        */
        private void button_edit_Click(object sender, EventArgs e)
        {
            if (!edit_activated)
            { // IF THE Edit button was not clicked yet (it makes the gdw editable, selects the first cell...)
                Dgw_values_activateEdit();
            }
            else
            {
                MessageBox.Show("Select the cell and enter the desired value. The new value will be saved automatically (after pressign Enter or selecting another cell).");
                dataGridView_values.Focus();
            }
        }

        private void Dgw_values_activateEdit()
        {
            // makes the gdw editable, selects the first cell...
            edit_mode = true;
            dataGridView_values.ReadOnly = false;
            dataGridView_values.AllowUserToAddRows = false; // disallow last empty row
            dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.Blue; // restore blue selection highlight
            dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView_values.Focus(); // this gdw becomes focused, so its selected cell will become editable immediately on pressing any key
            dataGridView_values.CurrentCell = dataGridView_values.Rows[dataGridView_values.FirstDisplayedScrollingRowIndex].Cells[0]; // selects the first VISIBLE cell
            edit_activated = true;
            insert_activated = false;
            deleteRow_activated = false;
            button_deleteRow.ForeColor = Color.Black;
        }


        private void button_insertRow_Click(object sender, EventArgs e)
        {
            if (!insert_activated)
            { // if the Insert button was not clicked yet, it selects the last empty row
                edit_mode = true;
                dataGridView_values.ReadOnly = false;
                dataGridView_values.AllowUserToAddRows = true; // allow last empty row
                // dataGridView_values.RowHeadersVisible = true; // unhide first empty control column
                // dataGridView_values.BeginEdit(true);
                dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.Blue; // restore blue selection highlight
                dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView_values.Focus(); // this way the selected and focused cell will become editable immediately on pressing any key
                dataGridView_values.CurrentCell = dataGridView_values.Rows[num_table_entries].Cells[PK_row_index]; // selects last empty row, and the PK field's column within it
                insert_activated = true;

                edit_activated = false;
                deleteRow_activated = false;
                button_deleteRow.ForeColor = Color.Black;
            }
            else
            {
                MessageBox.Show("The values of the new row must be filled in in (into the blue cell just being selected), starting with the value of the primary key. The entered values will be saved automatically (after pressing Enter or selecting another cell).");
                dataGridView_values.Focus();
            }
        }


        private void button_deleteRow_Click(object sender, EventArgs e)
        {

            if (!deleteRow_activated) // clicking button for the first time
            {
                edit_mode = true;
                deleteRow_activated = true;
                button_deleteRow.ForeColor = Color.Red;
                dataGridView_values.AllowUserToAddRows = false; // hide last empty row, because if insert occured before, then there was an empty last row
                // dataGridView_values.RowHeadersVisible = true; // unhide first empty control column
                // dataGridView_values.CurrentCell = dataGridView_values.Rows[0].Cells[0]; // utolsó üres sort kiválaszt

                dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.Blue; // restore blue selection highlight
                dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView_values.ClearSelection();
                dataGridView_values.CurrentCell = dataGridView_values.Rows[dataGridView_values.FirstDisplayedScrollingRowIndex].Cells[0]; // due to button_edit I selected the current cell as one cell to the right, because if it stays the same here too, then it will not be blue in case of clicking the edit_button ??
                dataGridView_values.Rows[dataGridView_values.FirstDisplayedScrollingRowIndex].Selected = true; // selecting the full first VISIBLE row
                // dataGridView_values.BeginEdit(true);

                edit_activated = false;
                insert_activated = false;
            }
            else
            {
                string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                // label_message.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString(); // selected row (for deletion)
                // label_message.Text = dataGridView1.SelectedCells[0].Value.ToString(); // selected cell
                // determine selected dataGridView row
                int selectedRows_count = dataGridView_values.SelectedRows.Count;
                if (selectedRows_count > 0)
                {
                    // törléshez megerősítés
                    string question = selectedRows_count == 1 ? "Are you sure to delete the selected row?" : "Are you sure to delete the selected rows?";
                    var result_MessageBox = MessageBox.Show(question, "Question:",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                    if (result_MessageBox == DialogResult.Yes)
                    {
                        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                        label_message.Text = ""; // the multiline text writing does not do deletion, thus it should be deleted beforehand
                        // PK_field_name = dataGridView_values.Columns[PK_row_index].HeaderText; // this is the name of primary key
                        for (int i = 0; i < dataGridView_values.SelectedRows.Count; i++)
                        {
                            string field_value = dataGridView_values.SelectedRows[i].Cells[PK_row_index].Value.ToString(); // this is the value of the actual PK field
                            // string query_delete = "DELETE FROM `autok` WHERE `autok`.`rendszam` = \'REN777\";
                            string query_delete = $"DELETE FROM `{selected_table}` WHERE `{selected_table}`.`{PK_field_name}` = '{field_value}';";
                            label_message.Text += query_delete + "\n";

                            MySqlCommand commandDatabase = new MySqlCommand(query_delete, databaseConnection);
                            try
                            {
                                databaseConnection.Open();
                                commandDatabase.Prepare();
                                commandDatabase.ExecuteNonQuery();
                                databaseConnection.Close();
                            }
                            catch (Exception ex)
                            {
                                // richTextBox1.Text = "Error: There is a big trouble.";
                                label_message.Text = ex.ToString();
                            }
                            dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.White; // remove blue selection highlight
                            dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.Navy;


                        }
                        ShowValues();
                    }
                }
                else // clicking button for the second time
                {
                    MessageBox.Show("The entire row(s) must be selected! \n (Clicke once on any field of the row(s) to be selected. To select multiple rows, keep the Ctlr key pressed down when clicking.) ", "Delete");
                }
            }
        }

        private void dataGridView_values_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!create_table) 
            {
                // uses "" in case of null
                cell_value0 = dataGridView_values.SelectedCells[0].Value != null ? dataGridView_values.SelectedCells[0].Value.ToString() : ""; // value of the cell selected for editing
                cell_rowindex0 = dataGridView_values.SelectedCells[0].RowIndex;
                cell_columnindex0 = dataGridView_values.SelectedCells[0].ColumnIndex;
                column_name0 = dataGridView_values.Columns[cell_columnindex0].HeaderText;
                // used "" in case of null
                // string value0_above = dataGridView_values.Rows[Math.Max(cell_rowindex0 - 1, 0)].Cells[0].Value != null ? dataGridView_values.Rows[Math.Max(cell_rowindex0 - 1, 0)].Cells[0].Value.ToString() : "";
                // string value0 = dataGridView_values.Rows[cell_rowindex0].Cells[0].Value != null? dataGridView_values.Rows[cell_rowindex0].Cells[0].Value.ToString() : "null";
                // deleteRow_activated estén nincs alsó üres sor! Ezt módosítani kell, mert hibaüzenetet ad az utolsó sor kiválasztása esetén, HA előtte rákattintottam az utolsó sorban még a Show nézetben!
                // string value0_below = dataGridView_values.Rows[Math.Min(cell_rowindex0 + 1, dataGridView_values.RowCount-1)].Cells[0].Value != null? dataGridView_values.Rows[cell_rowindex0+1].Cells[0].Value.ToString() : "null";

                // pr_cell_value0 = dataGridView1.SelectedCells[0].Value != null ? dataGridView1.Rows[cell_rowindex0].Cells[0].Value.ToString() : "";
                pr_cell_value0 = dataGridView_values.Rows[cell_rowindex0].Cells[PK_row_index].Value != null ? dataGridView_values.Rows[cell_rowindex0].Cells[PK_row_index].Value.ToString() : "";
            }

        }

        private void dataGridView_values_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!create_table)
            {
                primary_value = "";
                // uses "" in case of null (to avoid error message)
                string cell_value1 = dataGridView_values.Rows[cell_rowindex0].Cells[cell_columnindex0].Value != null ? dataGridView_values.Rows[cell_rowindex0].Cells[cell_columnindex0].Value.ToString() : "";


                if (cell_value0 != cell_value1) // executed only if the cell value is modified (not executed either in case of null, because both values will be "")
                {
                    if (insert_activated && cell_rowindex0 == num_table_entries) newrow++; // due to editing the last row, in case of new empty row the row positions is shifted down!
                    bool samePrimaryKeyValue = false;
                    if (cell_columnindex0 == PK_row_index) samePrimaryKeyValue = CheckPrimaryKeyValue(cell_value1, newrow); // executed if the value of the PK field is edited and checks that there are no identical PK values
                    if (!samePrimaryKeyValue) // executed if the primary key is modified and they are not the same  (or not a primary key is modified)
                    {
                        if (insert_activated && cell_rowindex0 == num_table_entries && cell_columnindex0 != PK_row_index) // the previous num_table_entries value must be used because due to the edition of the last row it swithes to the new row and the number of empty rows is increased once again by the empty last row!
                        {
                            MessageBox.Show($"In order to insert a new row,  please enter the value of the primary key column '{PK_field_name}'!");
                            dataGridView_values.Rows[cell_rowindex0].Visible = false; // hiding row, because it generated a new one below it and it appears as the last row already; it is not necessary to delete the value entered into the hidden row
                            num_table_entries++; // the number rows increased, AND so did the index number of the last row, therefore this value must be updated
                            // dataGridView_values.CurrentCell = dataGridView_values.Rows[0].Cells[0];
                            dataGridView_values.CurrentCell = dataGridView_values.Rows[num_table_entries].Cells[PK_row_index];
                            // dataGridView1.BeginEdit(true);
                            // dataGridView1.Rows[cell_rowindex0].Dispose();
                            // dataGridView1.Rows[cell_rowindex0].Cells.Clear();
                            return;
                        }

                        // string pr_column_name = dataGridView_values.Columns[PK_row_index].HeaderText;
                        string query_update = "";
                        string query_type = "update";
                        // I calculate with the value of CellBeginEdit, because when clicking outside of the cell it would calculate with other (wrong) values!
                        if ( !(insert_activated && cell_rowindex0 == num_table_entries))  // if Insert is FALSE, AND it is the last row
                        {
                            string quote_or_NULL = cell_value1 != "NULL" ? "'" : ""; // for NULL value entry
                            query_update = $"UPDATE `{selected_table}` SET `{column_name0}` = {quote_or_NULL}{cell_value1}{quote_or_NULL} WHERE `{selected_table}`.`{PK_field_name}` = '{pr_cell_value0}';";
                            
                            if (cell_columnindex0 == 0)
                            {
                                query_type = "update_primary";
                                primary_value = cell_value1; // for searching after sorting
                            }
                        }
                        else // In case of Insert and last row already INSERT INTO os needed for the value of the 1st primary column's value (if there is already value of 1st primary column, then it will subsequently appear not in the last row, but its fields can be edited as an existing column:)
                        {
                            query_type = "insert";
                            // num_table_entries++; // the number of rows increased due to editing the last row, therefore this value must be updated
                            // cell_rowindex0++; // if another row is inserted, this must be updated // SURE?
                            // int newrow = 1; // due to editing in cell, a new row is added which must be disregarded
                            // contains_primary_key = CheckPrimaryKeyValue(cell_value1, newrow); // if there is a primary key, it stops
                           
                            query_update = $"INSERT INTO `{selected_table}` (`{PK_field_name}`) VALUES ('{cell_value1}');";
                            if (cell_columnindex0 == PK_row_index)
                            {
                                query_type = "insert_primary";
                                primary_value = cell_value1; // for searching after sorting
                            }
                        }
                        label_message.Text = query_update;
                        // dataGridView1.CurrentCell = dataGridView1.Rows[cell_rowindex0].Cells[cell_columnindex0];

                        string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                        MySqlCommand commandDatabase = new MySqlCommand(query_update, databaseConnection);
                        try
                        {
                            databaseConnection.Open();
                            commandDatabase.Prepare();
                            commandDatabase.ExecuteNonQuery();
                            databaseConnection.Close();
                        }
                        catch (Exception ex)
                        {
                            // richTextBox1.Text = "Error: There is a big trouble.";
                            label_message.Text = ex.ToString();
                        }

                        // Connect();
                        // waits for ending the cell edit mode; otherwise, cancelling it causes an error message
                        if (query_type == "update_primary" || query_type == "insert_primary")
                        {
                            BeginInvoke(new MethodInvoker(ShowValues)); // this time the order of the rows can change, therefore it updates the gridview
                        }
                    }
                }
            }
        }

        private bool CheckPrimaryKeyValue(string cell_value1, int newrow)
        {
            // stops if the primary value is already present
            /*
            int contains = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == cell_value1)
                {
                    contains++;
                    // break;
                }
            }
            if (contains > 1) // it is featured as itself!
            {
                MessageBox.Show("The entered value is already available in the table!");
                dataGridView1.Rows[cell_rowindex0].Visible = false; // hiding the row, because it generated a new one below it, and it appears already as the new row; it is not necessary to delete the values entered into the hidded row
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                return;
            }
            */

            // stops if the primary key value is already present
            bool contains = false;
            // for (int i = 0; i < dataGridView_values.RowCount - newrow; i++) // newrow: in case of editing in cell, it does not test the new row before the last one, because it is already inclued in it itself!
            for (int i = 0; i <= cell_rowindex0 - newrow; i++) //  in case of editing in cell, it does not test the new row before the last one, because it is already inclued in it itself!
            {
                // if (dataGridView_values.Rows[i].Cells[0].Value != null && (i != cell_rowindex0 ? dataGridView_values.Rows[i].Cells[PK_row_index].Value.ToString().ToLower() : cell_value0.ToLower()) == cell_value1.ToLower()) // in the row being modified it compares the original value to the new value (otherwise it would always find the new value which is not even present in the database)
                if (dataGridView_values.Rows[i].Cells[PK_row_index].Value != null && i != cell_rowindex0 && dataGridView_values.Rows[i].Cells[PK_row_index].Value.ToString().ToLower() == cell_value1.ToLower()) // if any PK value is identical with the new value (it does not check the row being modified, because it was for sure not identical with any PK value
                {
                    contains = true;
                    break;
                }
            }
            if (contains)
            {
                MessageBox.Show($"A(z) '{Fields[cell_columnindex0]}' mező megadott '{cell_value1}'PRIMARY KEY value is already available in the database.\nPlease enter a value other than this! ");
                if (newrow >= 1) // if the last (empty) row is edited
                {
                    dataGridView_values.Rows[cell_rowindex0].Visible = false; //  hiding row, because a new one was generated below it, and it appears already as the last row; it is not necessary to delete the values entered into the hidden row
                    cell_rowindex0++; // due to the (hiding) of the new empty row created due to editing
                    num_table_entries++; // due to the (hiding) of the new empty row created due to editing
                }
                else
                {
                    dataGridView_values.Rows[cell_rowindex0].Cells[PK_row_index].Value = cell_value0; // restoring original value, because the new value cannot be applied
                }
                dataGridView_values.CurrentCell = dataGridView_values.Rows[cell_rowindex0].Cells[cell_columnindex0]; // it moves down by 1 anyway
            }
            return contains;
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            // label_message.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString(); // selected row (for deletion)
            //label_message.Text = dataGridView1.Columns[0].HeaderText;
            // label_message.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            // label_message.Text = dataGridView1.SelectedCells[0].Value.ToString(); // selected cell
            // label_message.Text = dataGridView1.SelectedCells[0].RowIndex.ToString();

            string query = "SHOW DATABASES;";
            string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            try
            {
                databaseConnection.Open();
                commandDatabase.Prepare();
                commandDatabase.ExecuteNonQuery();
                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // richTextBox1.Text = "Error: There is a big trouble.";
                label_message.Text = ex.ToString();
            }

            ShowValues();
            // richTextBox1.Text = "x";
        }


        private void dataGridView_values_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // selecting row in cas eof deleteRow_activated (the header e.RowIndex is -1, therefore nothing is done when clicking on it!)
            if (deleteRow_activated && e.RowIndex > -1 && dataGridView_values.Rows[e.RowIndex].Selected == false)
            {
                dataGridView_values.Rows[e.RowIndex].Selected = true;
            }
        }

        private void button_deleteDatabase_Click(object sender, EventArgs e)
        {
            selected_database = dataGridView_showDatabases.CurrentCell.Value.ToString();
            // törléshez megerősítés
            string question = $"Are you sure to delete the '{selected_database}' database?";
            var result_MessageBox = MessageBox.Show(question, "Question:",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result_MessageBox == DialogResult.Yes)
            {
                string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                string query = $"DROP DATABASE `{selected_database}`;";
                query0 = query + "\n";

                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                try
                {
                    databaseConnection.Open();
                    commandDatabase.Prepare();
                    commandDatabase.ExecuteNonQuery();
                    databaseConnection.Close();
                }
                catch (Exception ex)
                {
                    // richTextBox1.Text = "Error: There is a big trouble.";
                    label_message.Text = ex.ToString();
                }
                ShowDatabases();
                query0 = "";
            }
        }

        private void button_createDatabase_Click(object sender, EventArgs e)
        {
            string new_database_name = Interaction.InputBox(Prompt: "Enter the name of the new database:", Title: "Message", DefaultResponse: "");
            if (new_database_name != "" && !new_database_name.Contains(" "))
            {
                string connectionString = $"datasource=localhost;port=3306;username=root;password=;";
                // check if database exists:
                string query = $"SHOW DATABASES WHERE `database` = '{new_database_name}';";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                MySqlDataReader reader;
                try
                {
                    databaseConnection.Open();
                    reader = commandDatabase.ExecuteReader();
                    if (!reader.HasRows) // if there is no result, no such database exits and we create it
                    {
                        databaseConnection.Close();
                        query = $"CREATE DATABASE `{new_database_name}` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;";
                        query0 = query + "\n";

                        commandDatabase = new MySqlCommand(query, databaseConnection);
                        try
                        {
                            databaseConnection.Open();
                            commandDatabase.Prepare();
                            commandDatabase.ExecuteNonQuery();
                            databaseConnection.Close();
                            MessageBox.Show(text: $"The new '{new_database_name}' databse was created.", caption: "Message");
                        }
                        catch (Exception ex)
                        {
                            // richTextBox1.Text = "Error: There is a big trouble.";
                            label_message.Text = ex.ToString();
                        }
                        selected_database = new_database_name; // to select the new database in the ShowDatabases() function
                        create_database = true;
                        ShowDatabases();
                        query0 = "";
                    }
                    else
                    {
                        databaseConnection.Close();
                        MessageBox.Show(text: "The database already exists.");
                    }
                }
                catch (Exception ex)
                {
                    // richTextBox1.Text = "Error: There is a big trouble.";
                    label_message.Text = ex.ToString();
                }
            }
            else
            {
                MessageBox.Show("You entered an invalid name. Please try it again!");
            }
        }

        private void button_createTable_Click(object sender, EventArgs e)
        {
            if (!create_table) // clicking the button for the 1st time
            {
                new_table_name = Interaction.InputBox(Prompt: "Enter the name of the new table:", Title: "Message", DefaultResponse: "");
                if (new_table_name != "" && !new_table_name.Contains(" ") )
                {
                    string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                    // check if table exists:
                    string query = $"SHOW TABLES LIKE '{new_table_name}';";
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    MySqlDataReader reader;
                    try
                    {
                        databaseConnection.Open();
                        reader = commandDatabase.ExecuteReader();
                        if (!reader.HasRows) // if there is no result, no such table exits and we create it
                        {
                            databaseConnection.Close();
                            // creating Gridview to hold the data of the new table
                            dataGridView_values.Columns.Clear();
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Field name", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Type", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Length", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Null", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Key", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.Columns.Add(new DataGridViewColumn() { HeaderText = "Extra", CellTemplate = new DataGridViewTextBoxCell() });
                            dataGridView_values.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.00F, FontStyle.Bold);
                            dataGridView_values.DefaultCellStyle.ForeColor = Color.Navy;
                            // adding empty rows                       
                            for (int row_index = 0; row_index < 50; row_index++)
                            {
                                dataGridView_values.Rows.Insert(row_index);
                            }

                            // dataGridView_values.Rows.Insert(0, reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));

                            // string[] row = { reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5) };
                            // richTextBox1.Text += row[0] + " " + row[1] + " " + row[2] + " " + row[3] + " " + row[4] + " " + row[5];
                            // richTextBox1.Text += "\n";


                            dataGridView_values.ReadOnly = true;
                            dataGridView_values.AllowUserToAddRows = false; // hide last empty row
                            dataGridView_values.RowHeadersVisible = false; // hide first empty control column 
                            dataGridView_values.DefaultCellStyle.SelectionBackColor = Color.White; // remove blue selection highlight
                            dataGridView_values.DefaultCellStyle.SelectionForeColor = Color.Navy;
                            // dataGridView_values.DefaultCellStyle.ForeColor = Color.Empty;
                            // dataGridView_values.DefaultCellStyle.BackColor = Color.Empty;
                            label_message.Text = "Ener the values of the new table, then click the Create table button again!";

                            // dataGridView_values.Visible = true;
                            showColumns = true;
                            button_edit.Enabled = false;
                            button_insertRow.Enabled = false;
                            button_deleteRow.Enabled = false;
                            button_showValues.Enabled = false; // at this time it is not active, because there is no value in the table yet
                            button_showColumns.Enabled = false;
                            // setting ClientSize, it it is larger than the total width of the columns
                            // dataGridView_values.AutoResizeColumns(); // this sets the columns to the least width, BUT here it is not needed due to the 6 columns this sets the columns to the least width, BUT here it is not needed due to the 6 columns
                            int total_width = 8;
                            for (int i = 0; i < dataGridView_values.ColumnCount; i++)
                            {
                                total_width += dataGridView_values.Columns[i].Width + 2; // 2 pixels due to the 2 borders?
                            }
                            if (total_width < 731)
                            {
                                dataGridView_values.ClientSize = new Size(total_width, 285);
                            }
                            else
                            {
                                dataGridView_values.ClientSize = new Size(731, 285);
                            }

                            // writing PRIMARY KEY into the Key column of the first row
                            dataGridView_values.Rows[0].Cells[4].Value = "PRIMARY KEY";
                            Dgw_values_activateEdit();
                            create_table = true;
                        }
                        else
                        {
                            MessageBox.Show($"The table '{new_table_name}' already exists!");
                        }
                        // executed even if such table already exists)
                    }
                    catch (Exception ex)
                    {
                        label_message.Text = ex.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid name. Please try it again!");
                }
            }
            else // clicing Create table button for the second time (create_table = true)
            {
                // putting together the create table command
                string query = $"CREATE TABLE `{new_table_name}` (";
                // iterating through the gdw rows
                int i = 0;
                // string table_row = "";
                bool query_OK = true;
                while (dataGridView_values.Rows[i].Cells[0].Value != null)  // if there is no value i the 1. cell of the row (field name), then no (further) field is given for the table
                {
                    if (i > 0) query += ", "; // in case of new row it closed down the values originating from the preceding row

                    // Field name
                    string field_name = dataGridView_values.Rows[i].Cells[0].Value.ToString(); // the while cycle already checked whether it is not null value
                    if (!field_name.Contains(" ")) // of the Field name is valid
                    {
                        query += $"`{field_name}` ";
                        // Type
                        string type = dataGridView_values.Rows[i].Cells[1].Value != null ? dataGridView_values.Rows[i].Cells[1].Value.ToString().ToLower() : "";
                        if (type == "int" || type == "tinyint" || type == "float" || type == "varchar" || type == "datetime")
                        {
                            if (type == "int")
                            {
                                query += "int";
                            }
                            else if (type == "tinyint")
                            {
                                query += "tinyint";
                            }
                           else if (type == "float")
                            {
                                query += "float";
                            }
                            else if (type == "varchar")
                            {
                                query += "varchar";
                            }
                            else if (type == "datetime")
                            {
                                query += "datetime";
                            }
                            // Length
                            string length = dataGridView_values.Rows[i].Cells[2].Value != null ? dataGridView_values.Rows[i].Cells[2].Value.ToString() : "";
                            if (length != "")
                            {
                                int out_int = 0;
                                bool isInteger = int.TryParse(length, out out_int); // 0 (false) if not integer
                                if (isInteger)
                                {
                                    query += $"({length})";
                                }
                                else
                                {
                                    MessageBox.Show($"In row {i + 1} the value '{length}' of Length of the '{field_name}' field is invalid, please enter a valid integer (or leave the cell empty)!");
                                    query_OK = false;
                                    break;
                                }

                            }
                            else if (type == "varchar")
                            {
                                MessageBox.Show($"In row {i + 1} the value '{length}' of Length of the field '{field_name}' is invalid. In case of 'VARCHAR' the value of Length must be entered!");
                                query_OK = false;
                                break;
                            }
                            // Null
                            string null_value = dataGridView_values.Rows[i].Cells[3].Value != null ? dataGridView_values.Rows[i].Cells[3].Value.ToString().ToLower() : "";
                            if (null_value == "" || null_value == "no" || null_value == "yes")
                            {
                                if (null_value == "no")
                                {
                                    query += " NOT NULL"; // this is when No appears below the Null value
                                }
                                else
                                {
                                    // query += " DEFAULT NULL"; // this is when YES appears below the Null value (this is the default, as far as I know, this is why it is not needed)
                                }
                                // Key
                                string key = dataGridView_values.Rows[i].Cells[4].Value != null ? dataGridView_values.Rows[i].Cells[4].Value.ToString().ToLower() : "";
                                if (key == "" || key == "primary key")
                                {
                                    if (key == "primary key")
                                    {
                                        query += " PRIMARY KEY";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"In row {i + 1} the value '{key}' of Key of the field '{field_name}' is invalid. Valid valus are 'PRIMARY KEY', or leaving the cell empty.");
                                    query_OK = false;
                                    break;
                                }
                                // Extra
                                string extra = dataGridView_values.Rows[i].Cells[5].Value != null ? dataGridView_values.Rows[i].Cells[5].Value.ToString().ToLower() : "";
                                if (extra == "" || extra == "auto_increment")
                                {
                                    if (extra == "auto_increment")
                                    {
                                        query += " AUTO_INCREMENT";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"In row {i + 1}. the value '{extra}' of Extra of the field '{field_name}' is invalid. Valid values are 'AUTO_INCREMENT', or leaving the cell empty.");
                                    query_OK = false;
                                    break;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"In row {i + 1} the value '{null_value}' of Null of the field '{field_name}' is invalid. Valid values are 'No', 'Yes', or leaving the cell empty (which corresponds to the value 'No').");
                                query_OK = false;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show($"in row {i + 1} the value Type of the field '{field_name}' is invalid. Only the valules INT, TINYINT, FLOAT, VARCHAR, DATETIME can be speficied. Please enter a valid type!");
                            query_OK = false;
                            break;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"In row {i+1} the field name '{field_name}' is invalid, please enter a valid name!");
                        query_OK = false;
                        break;
                    }
                    i++;
                }
                if (dataGridView_values.Rows[0].Cells[0].Value == null) // if no field wad entered
                {
                    MessageBox.Show($"In row {i + 1} the field name is invalid, please enter a valid name!");
                    query_OK = false;
                }

                if (query_OK) // if there is a valid query to create the table based on the entered data
                {
                    query += ");"; // the query is completed
                    string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                    MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                    MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
                    try
                    {
                        databaseConnection.Open();
                        commandDatabase.Prepare();
                        commandDatabase.ExecuteNonQuery();
                        databaseConnection.Close();
                        MessageBox.Show(text: $"The new table '{new_table_name}' was cerated.", caption: "Message");
                        query0 = query + "\n";
                    }
                    catch (Exception ex)
                    {
                        // richTextBox1.Text = "Error: There is a big trouble.";
                        label_message.Text = ex.ToString();
                    }
                    ShowDatabases(); // is this not neded below the try?
                }
            }
        }

        private void button_deleteTable_Click(object sender, EventArgs e)
        {
            // confirm deletion
            string question = $"Are you sure to delete the table '{selected_table}' of the database '{selected_database}'?";
            var result_MessageBox = MessageBox.Show(question, "Question:",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result_MessageBox == DialogResult.Yes)
            {
                string connectionString = $"datasource=localhost;port=3306;username=root;password=;database={selected_database};";
                MySqlConnection databaseConnection = new MySqlConnection(connectionString);
                string query_delete = $"DROP TABLE `{selected_database}`.`{selected_table}`;";
                query0 = query_delete + "\n";

                MySqlCommand commandDatabase = new MySqlCommand(query_delete, databaseConnection);
                try
                {
                    databaseConnection.Open();
                    commandDatabase.Prepare();
                    commandDatabase.ExecuteNonQuery();
                    databaseConnection.Close();
                    MessageBox.Show(text: $"The table was deleted.", caption: "Message");
                }
                catch (Exception ex)
                {
                    // richTextBox1.Text = "Error: There is a big trouble.";
                    label_message.Text = ex.ToString();
                }

                ShowDatabases(); // is this not neded below the try?
            }

        }




    }
}
