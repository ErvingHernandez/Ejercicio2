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
    public partial class NewEmployee : Form
    {
        public NewEmployee()
        {
            InitializeComponent();
        }

        public Employee Emp { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            Emp = new Employee();
            Emp.Name = textBox1.Text.ToString();
            Emp.LastName = textBox2.Text.ToString();
            if (comboBox1.SelectedIndex == 1)
                Emp.Gender = "Female";
            else
                Emp.Gender = "Male";
            Emp.Email = textBox3.Text.ToString();
           DialogResult = DialogResult.OK;
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            
            DialogResult = DialogResult.Cancel;
        }
    }
}
