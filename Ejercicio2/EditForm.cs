using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejercicio2
{
    public partial class EditForm : Form
    {
        internal Employee Emp { get; set; }

        public EditForm()
        {
            InitializeComponent();
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            txtID.Text = Emp.Id.ToString();
            txtName.Text = Emp.Name;
            txtLastName.Text = Emp.LastName;
            txtEmail.Text = Emp.Email;
        
            if (Emp.Gender == "Male")
                comboBox1.SelectedIndex  = 0;
            else
                comboBox1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Emp = new Employee();
            Emp.Id = int.Parse(txtID.Text);
            Emp.Name = txtName.Text.ToString();
            Emp.LastName = txtLastName.Text.ToString();
            if (comboBox1.SelectedIndex == 1)
                Emp.Gender = "Female";
            else
                Emp.Gender = "Male";
            Emp.Email = txtEmail.Text;
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
