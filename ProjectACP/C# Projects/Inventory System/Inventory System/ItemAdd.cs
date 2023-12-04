using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_System
{
    public partial class frmItemAdd : Form
    {
        // Declare an event to notify the parent form about the new item
        public event EventHandler<ItemAddedEventArgs> ItemAdded;

        public frmItemAdd()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Assuming you have the necessary validation for txtName and txtStocks
            string itemName = txtName.Text;
            int stocks = int.Parse(txtStocks.Text);
            string supplier = cbbSupplier.Text;

            // Trigger the event and pass the data to the parent form
            ItemAdded?.Invoke(this, new ItemAddedEventArgs(itemName, stocks, supplier));

            this.Close();
        }

        private void cbbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the combobox selection change if needed
        }

        private void frmItemAdd_Load(object sender, EventArgs e)
        {
            // Perform any initialization logic if needed
        }
    }

    // Custom EventArgs class for passing item details
    public class ItemAddedEventArgs : EventArgs
    {
        public string ItemName { get; }
        public int Stocks { get; }
        public string Supplier { get; }

        public ItemAddedEventArgs(string itemName, int stocks, string supplier)
        {
            ItemName = itemName;
            Stocks = stocks;
            Supplier = supplier;
        }
    }
}
