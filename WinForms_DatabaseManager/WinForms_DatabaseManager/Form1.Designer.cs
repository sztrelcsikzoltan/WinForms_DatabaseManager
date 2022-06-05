
namespace WinForms_DatabaseManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_showValues = new System.Windows.Forms.Button();
            this.dataGridView_values = new System.Windows.Forms.DataGridView();
            this.button_test = new System.Windows.Forms.Button();
            this.label_message = new System.Windows.Forms.Label();
            this.button_deleteRow = new System.Windows.Forms.Button();
            this.dataGridView_showDatabases = new System.Windows.Forms.DataGridView();
            this.dataGridView_showTables = new System.Windows.Forms.DataGridView();
            this.button_showColumns = new System.Windows.Forms.Button();
            this.button_connect = new System.Windows.Forms.Button();
            this.button_insertRow = new System.Windows.Forms.Button();
            this.button_edit = new System.Windows.Forms.Button();
            this.button_deleteDatabase = new System.Windows.Forms.Button();
            this.button_createDatabase = new System.Windows.Forms.Button();
            this.button_createTable = new System.Windows.Forms.Button();
            this.button_deleteTable = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_values)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_showDatabases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_showTables)).BeginInit();
            this.SuspendLayout();
            // 
            // button_showValues
            // 
            this.button_showValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_showValues.Location = new System.Drawing.Point(278, 84);
            this.button_showValues.Margin = new System.Windows.Forms.Padding(2);
            this.button_showValues.Name = "button_showValues";
            this.button_showValues.Size = new System.Drawing.Size(108, 31);
            this.button_showValues.TabIndex = 0;
            this.button_showValues.Text = "Show values";
            this.button_showValues.UseVisualStyleBackColor = true;
            this.button_showValues.Click += new System.EventHandler(this.button_showValues_Click);
            // 
            // dataGridView_values
            // 
            this.dataGridView_values.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_values.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_values.Location = new System.Drawing.Point(278, 119);
            this.dataGridView_values.Name = "dataGridView_values";
            this.dataGridView_values.Size = new System.Drawing.Size(731, 285);
            this.dataGridView_values.TabIndex = 11;
            this.dataGridView_values.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_values_CellBeginEdit);
            this.dataGridView_values.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_values_CellClick);
            this.dataGridView_values.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_values_CellEndEdit);
            // 
            // button_test
            // 
            this.button_test.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_test.Location = new System.Drawing.Point(821, 13);
            this.button_test.Margin = new System.Windows.Forms.Padding(2);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(92, 31);
            this.button_test.TabIndex = 12;
            this.button_test.Text = "Test";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Visible = false;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // label_message
            // 
            this.label_message.AutoSize = true;
            this.label_message.Location = new System.Drawing.Point(25, 416);
            this.label_message.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(50, 13);
            this.label_message.TabIndex = 13;
            this.label_message.Text = "Message";
            // 
            // button_deleteRow
            // 
            this.button_deleteRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteRow.Location = new System.Drawing.Point(402, 50);
            this.button_deleteRow.Margin = new System.Windows.Forms.Padding(2);
            this.button_deleteRow.Name = "button_deleteRow";
            this.button_deleteRow.Size = new System.Drawing.Size(102, 31);
            this.button_deleteRow.TabIndex = 14;
            this.button_deleteRow.Text = "Delete row";
            this.button_deleteRow.UseVisualStyleBackColor = true;
            this.button_deleteRow.Click += new System.EventHandler(this.button_deleteRow_Click);
            // 
            // dataGridView_showDatabases
            // 
            this.dataGridView_showDatabases.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_showDatabases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_showDatabases.Location = new System.Drawing.Point(28, 119);
            this.dataGridView_showDatabases.Name = "dataGridView_showDatabases";
            this.dataGridView_showDatabases.Size = new System.Drawing.Size(120, 285);
            this.dataGridView_showDatabases.TabIndex = 15;
            this.dataGridView_showDatabases.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_showTablesAndColumns_CellClick);
            // 
            // dataGridView_showTables
            // 
            this.dataGridView_showTables.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_showTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_showTables.Location = new System.Drawing.Point(145, 119);
            this.dataGridView_showTables.Name = "dataGridView_showTables";
            this.dataGridView_showTables.Size = new System.Drawing.Size(120, 285);
            this.dataGridView_showTables.TabIndex = 16;
            this.dataGridView_showTables.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_showColumns_CellClick);
            // 
            // button_showColumns
            // 
            this.button_showColumns.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_showColumns.Location = new System.Drawing.Point(402, 84);
            this.button_showColumns.Margin = new System.Windows.Forms.Padding(2);
            this.button_showColumns.Name = "button_showColumns";
            this.button_showColumns.Size = new System.Drawing.Size(102, 31);
            this.button_showColumns.TabIndex = 17;
            this.button_showColumns.Text = "Show columns";
            this.button_showColumns.UseVisualStyleBackColor = true;
            this.button_showColumns.Click += new System.EventHandler(this.button_showColumns_Click);
            // 
            // button_connect
            // 
            this.button_connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_connect.Location = new System.Drawing.Point(28, 15);
            this.button_connect.Margin = new System.Windows.Forms.Padding(2);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(118, 31);
            this.button_connect.TabIndex = 18;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // button_insertRow
            // 
            this.button_insertRow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_insertRow.Location = new System.Drawing.Point(278, 50);
            this.button_insertRow.Margin = new System.Windows.Forms.Padding(2);
            this.button_insertRow.Name = "button_insertRow";
            this.button_insertRow.Size = new System.Drawing.Size(108, 31);
            this.button_insertRow.TabIndex = 19;
            this.button_insertRow.Text = "Insert row";
            this.button_insertRow.UseVisualStyleBackColor = true;
            this.button_insertRow.Click += new System.EventHandler(this.button_insertRow_Click);
            // 
            // button_edit
            // 
            this.button_edit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_edit.Location = new System.Drawing.Point(278, 15);
            this.button_edit.Margin = new System.Windows.Forms.Padding(2);
            this.button_edit.Name = "button_edit";
            this.button_edit.Size = new System.Drawing.Size(108, 31);
            this.button_edit.TabIndex = 20;
            this.button_edit.Text = "Edit cell";
            this.button_edit.UseVisualStyleBackColor = true;
            this.button_edit.Click += new System.EventHandler(this.button_edit_Click);
            // 
            // button_deleteDatabase
            // 
            this.button_deleteDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteDatabase.Location = new System.Drawing.Point(28, 50);
            this.button_deleteDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.button_deleteDatabase.Name = "button_deleteDatabase";
            this.button_deleteDatabase.Size = new System.Drawing.Size(118, 31);
            this.button_deleteDatabase.TabIndex = 21;
            this.button_deleteDatabase.Text = "Delete database";
            this.button_deleteDatabase.UseVisualStyleBackColor = true;
            this.button_deleteDatabase.Click += new System.EventHandler(this.button_deleteDatabase_Click);
            // 
            // button_createDatabase
            // 
            this.button_createDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_createDatabase.Location = new System.Drawing.Point(28, 84);
            this.button_createDatabase.Margin = new System.Windows.Forms.Padding(2);
            this.button_createDatabase.Name = "button_createDatabase";
            this.button_createDatabase.Size = new System.Drawing.Size(118, 31);
            this.button_createDatabase.TabIndex = 22;
            this.button_createDatabase.Text = "Create database";
            this.button_createDatabase.UseVisualStyleBackColor = true;
            this.button_createDatabase.Click += new System.EventHandler(this.button_createDatabase_Click);
            // 
            // button_createTable
            // 
            this.button_createTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_createTable.Location = new System.Drawing.Point(148, 84);
            this.button_createTable.Margin = new System.Windows.Forms.Padding(2);
            this.button_createTable.Name = "button_createTable";
            this.button_createTable.Size = new System.Drawing.Size(118, 31);
            this.button_createTable.TabIndex = 23;
            this.button_createTable.Text = "Create table";
            this.button_createTable.UseVisualStyleBackColor = true;
            this.button_createTable.Click += new System.EventHandler(this.button_createTable_Click);
            // 
            // button_deleteTable
            // 
            this.button_deleteTable.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_deleteTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_deleteTable.Location = new System.Drawing.Point(150, 49);
            this.button_deleteTable.Margin = new System.Windows.Forms.Padding(2);
            this.button_deleteTable.Name = "button_deleteTable";
            this.button_deleteTable.Size = new System.Drawing.Size(118, 31);
            this.button_deleteTable.TabIndex = 24;
            this.button_deleteTable.Text = "Delete table";
            this.button_deleteTable.UseVisualStyleBackColor = true;
            this.button_deleteTable.Click += new System.EventHandler(this.button_deleteTable_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 471);
            this.Controls.Add(this.button_deleteTable);
            this.Controls.Add(this.button_createTable);
            this.Controls.Add(this.button_createDatabase);
            this.Controls.Add(this.button_deleteDatabase);
            this.Controls.Add(this.button_edit);
            this.Controls.Add(this.button_insertRow);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.button_showColumns);
            this.Controls.Add(this.dataGridView_showTables);
            this.Controls.Add(this.dataGridView_showDatabases);
            this.Controls.Add(this.button_deleteRow);
            this.Controls.Add(this.label_message);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.dataGridView_values);
            this.Controls.Add(this.button_showValues);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Database Manager";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_values)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_showDatabases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_showTables)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_showValues;
        private System.Windows.Forms.DataGridView dataGridView_values;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.Label label_message;
        private System.Windows.Forms.Button button_deleteRow;
        private System.Windows.Forms.DataGridView dataGridView_showDatabases;
        private System.Windows.Forms.DataGridView dataGridView_showTables;
        private System.Windows.Forms.Button button_showColumns;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Button button_insertRow;
        private System.Windows.Forms.Button button_edit;
        private System.Windows.Forms.Button button_deleteDatabase;
        private System.Windows.Forms.Button button_createDatabase;
        private System.Windows.Forms.Button button_createTable;
        private System.Windows.Forms.Button button_deleteTable;
    }
}

