using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Inventory_System
{
    public partial class frmInventory : Form
    {
        // Assuming dataGridView1 is bound to a DataTable during initialization
        private DataTable dataTable;

        public frmInventory()
        {
            InitializeComponent();
            InitializeDataTable();
            LoadData(); // Load data when the form is initially created
        }

        private void InitializeDataTable()
        {
            // Create DataTable and add columns
            dataTable = new DataTable();
            dataTable.Columns.Add("Product_ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Stocks", typeof(int));
            dataTable.Columns.Add("Supplier", typeof(string));

            // Set the DataTable as the DataSource for the DataGridView
            dataGridView1.DataSource = dataTable;

            // Set AutoGenerateColumns to false to manually define columns
            dataGridView1.AutoGenerateColumns = false;
         
        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmItemAdd frmItemAdd = new frmItemAdd();

            // Subscribe to the event in frmItemAdd
            frmItemAdd.ItemAdded += FrmItemAdd_ItemAdded;

            frmItemAdd.ShowDialog();
        }

        // Event handler for the ItemAdded event in frmItemAdd
        private void FrmItemAdd_ItemAdded(object sender, ItemAddedEventArgs e)
        {
            // Update the DataGridView
            DataRow newRow = dataTable.NewRow();
            newRow["Name"] = e.ItemName;
            newRow["Stocks"] = e.Stocks;
            newRow["Supplier"] = e.Supplier;
            dataTable.Rows.Add(newRow);

            // Update the database
            InsertItemToDatabase(e.ItemName, e.Stocks, e.Supplier);

            // Refresh the DataTable to get the updated Product_ID
            LoadData();
        }


        // Method to insert data into the database (implement as needed)
        private void InsertItemToDatabase(string itemName, int stocks, string supplier)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password='';database=inventory"))
                {
                    connection.Open();

                    string query = "INSERT INTO products (Name, Stocks, Supplier) VALUES (@itemName, @stocks, @supplier)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@itemName", itemName);
                    command.Parameters.AddWithValue("@stocks", stocks);
                    command.Parameters.AddWithValue("@supplier", supplier);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password='';database=inventory"))
                {
                    connection.Open();

                    string query = "SELECT * FROM products";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        // Clear existing data in the DataTable
                        dataTable.Clear();

                        // Fill the DataTable with data from the database
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            // Any additional logic you want to perform when the form loads
        }
    }
}
