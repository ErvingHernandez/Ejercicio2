using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;


namespace Ejercicio2
{

    public partial class Form1 : Form
    {

        //public List<Employee> emp = new List<Employee>();

        internal Employee Emp { get; set; }
        public int counter = 0;
        string path = @"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\Employees.json"; //Direccion del archivo en donde se guardara o utilizara
        string path2 = @"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\prueba.bin"; //direccion del archivo binario
        StreamWriter log = File.AppendText(@"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\Log.txt"); //direccion para guardar el log

        public Form1()
        {
            InitializeComponent();
        }

        //Boton Create new
        private void button1_Click(object sender, EventArgs e)
        {
            NewEmployee newData = new NewEmployee();
            if (newData.ShowDialog() == DialogResult.OK)
            {
                Emp = newData.Emp;
                Emp.Id = counter += 1;

                //Guardar datos de forma manual
                //dataGridView1.Rows.Add(new object[]
                //{
                //    Emp.Id.ToString(),
                //    Emp.Name,
                //    Emp.LastName,
                //    Emp.Gender,
                //    Emp.Email
                //});

                //Agregar un elemento a al lista
                employeeBindingSource.Add(Emp); // se liga los datos recopilados en Emp para el enlace de employeeBindingSource que comparte con el datagridview1
                
                string data = JsonConvert.SerializeObject(employeeBindingSource.List, Formatting.Indented); //se crea una variable local data, en donde se serializa "guarda" los datos de la lista 
                //archivo Json
                //File.WriteAllText(path, data); //Escribe todo el texto recopilado en la variable data, en la direccion declara Path

                //Escritura en archivo binario 
                using (BinaryWriter writer = new BinaryWriter(File.Create(path2)))
                {
                    System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();

                    Byte[] cadena;
                    cadena = codificador.GetBytes(data);

                    byte[] buffer = cadena;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i] == 0x0d || buffer[i] == 0x0a)
                            continue;
                        else
                            buffer[i] ^= 0xff;
                    }
                    writer.Write(buffer);
                }

                //Escritura en log
                log.WriteLine("El empleado: " + Emp.Name.ToString() + "\tha sido registrado con exito,\t" + DateTime.Now.ToLongTimeString());
                log.Flush();
                MessageBox.Show("Data have been saved", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data no saved", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //edit user
        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.Rows.Count > 0)
            {
                Emp = new Employee();
                EditForm editEmployee = new EditForm();

                Emp.Id = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                Emp.Name = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                Emp.LastName = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                Emp.Gender = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                Emp.Email = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                editEmployee.Emp = Emp;

                if (editEmployee.ShowDialog() == DialogResult.OK)
                {
                    Emp = editEmployee.Emp;
                    //Emp = editEmployee.Emp;
                    //dataGridView1.SelectedRows[0].Cells[0].Value = Emp.Id.ToString();
                    //dataGridView1.SelectedRows[0].Cells[1].Value = Emp.Name;
                    //dataGridView1.SelectedRows[0].Cells[2].Value = Emp.LastName;
                    //dataGridView1.SelectedRows[0].Cells[3].Value = Emp.Gender;
                    //dataGridView1.SelectedRows[0].Cells[4].Value = Emp.Email;
                    //employeeBindingSource.DataSource = null;
                    employeeBindingSource[dataGridView1.SelectedRows[0].Index] = Emp; //se sobreescribe los datos recopilados en Emp, en la posicion de la celda que se selecciono
                                                                                      //se obtiene la posicion de la celda seleccionada
                    string data = JsonConvert.SerializeObject(employeeBindingSource.List, Formatting.Indented); //se crea una variable local data, en donde se serializa "guarda" los datos de la lista 
                    
                    ////Archivo Json
                    //File.WriteAllText(path, data);

                    //Archivo binario 
                    using (BinaryWriter writer = new BinaryWriter(File.Create(path2)))
                    {
                        System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();

                        Byte[] cadena;
                        cadena = codificador.GetBytes(data);

                        byte[] buffer = cadena;
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            if (buffer[i] == 0x0d || buffer[i] == 0x0a)
                                continue;
                            else
                                buffer[i] ^= 0xff;
                        }
                        writer.Write(buffer);
                    }

                    //Escritura en log
                    log.WriteLine("Los datos del empleado:\t" + Emp.Name.ToString() + "\than sido modificados con exito,\t" + DateTime.Now.ToLongTimeString());
                    log.Flush();

                    MessageBox.Show("Data have been changed successfully", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Any changed have been registered", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
                MessageBox.Show("You need to add an employee" + "\nFor more information consult: Help", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Borrar linea seleccionada 
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string deletedEmployee;
                deletedEmployee = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                employeeBindingSource.RemoveAt(dataGridView1.SelectedRows[0].Index); // se elimina los valores de la celda seleccionada, de la lista "employeeBindingSource"
                string data = JsonConvert.SerializeObject(employeeBindingSource.List, Formatting.Indented);

                //Escritura en archivo Json
                //File.WriteAllText(path, data);

                //Escritura archivoBinario
                using (BinaryWriter writer = new BinaryWriter(File.Create(path2)))
                {
                    System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();

                    Byte[] cadena;
                    cadena = codificador.GetBytes(data);

                    byte[] buffer = cadena;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i] == 0x0d || buffer[i] == 0x0a)
                            continue;
                        else
                            buffer[i] ^= 0xff;
                    }
                    writer.Write(buffer);
                }

                //Escritura en log
                log.WriteLine("Los datos del empleado:\t" + deletedEmployee + "\than sido borrados,\t" + DateTime.Now.ToLongTimeString());
                log.Flush();
            }
            else
            {
                MessageBox.Show("There's no data found!\nYou new to add an employee" + "\nFor more information consult: Help", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //Informacion al usuario 
        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Create new: Crea nueva coleccion de datos de un empleado" + "\n\nEdit user: Edita los datos de un empleado previamente registrado" + "\n\nClear Data: Permite eliminar la informacion de un empleado", "Instrucciones", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //buscador
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                dataGridView1.CurrentCell = null; //declarar que ninguna celda del data grid view ha sido seleccionada para que no genere la excepcion al momento de ocultar una celda que no entre en la busqueda :v

                //string valor = Convert.ToString(row.Cells[1].Value);
                if (row.Cells[1].Value.ToString().StartsWith(textBox1.Text))
                {
                    row.Visible = true;
                    row.DefaultCellStyle.BackColor = Color.Aqua;
                }

                else
                {
                    row.Visible = false;
                    row.DefaultCellStyle.BackColor = Color.White;
                }

                if (textBox1.Text == "")
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)  //evento de carga de variables
        {

            //Archivo JSon
            //if (!File.Exists(@"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\Employees.json")) //si no existe en archivo en la direccion...
            //    File.Create(@"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\Employees.json").Close(); //se crea un archivo en la direccion, y se maneja un administrador del archivo 
            //else
            //{
            //    string data = File.ReadAllText(path); //si ya existe el archivo, se lee todo el texto del archivo que se encuentra en la ubicacion declarada en path
            //    employeeBindingSource.DataSource = JsonConvert.DeserializeObject<List<Employee>>(data); // Se deserializa los datos: los archivos se leen y se guardan en el vinculo "employeeBindingSource" que esta ligado al datagridview1

            //    foreach (var sub in employeeBindingSource)
            //    {
            //        counter++;
            //    }
            //}

            //Escritura en Log
            log.WriteLine("\nAplicacion iniciada:\t" + DateTime.Now.ToLongTimeString() + ",\t" + DateTime.Now.ToLongDateString());
            log.Flush();

            //Leer archivo binario 
            if (!File.Exists(@"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\prueba.bin"))
                File.Create(@"C:\Users\ZT-SW-008\Documents\Visual Studio 2015\Projects\Ejercicio2\prueba.bin");
            else
            {
                Byte[] red = File.ReadAllBytes(path2);
                System.Text.ASCIIEncoding conver = new System.Text.ASCIIEncoding();
                byte[] buffer = red;
                for (int i = 0; i < buffer.Length; i++)
                {
                    if (buffer[i] == 0x0d || buffer[i] == 0x0a)
                        continue;
                    else
                        buffer[i] ^= 0xff;
                }
                string data2 = Encoding.ASCII.GetString(buffer);
                employeeBindingSource.DataSource = JsonConvert.DeserializeObject<List<Employee>>(data2);
                foreach (var sub in employeeBindingSource)
                {
                    counter++;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //guardar datos en archivos json
            string data = JsonConvert.SerializeObject(employeeBindingSource.List, Formatting.Indented);
            File.WriteAllText(path, data);
            //guardar datos en log sobre evento de cierrre
            log.WriteLine("Aplicacion cerrada:\t" + DateTime.Now.ToLongTimeString() + ",\t" + DateTime.Now.ToLongDateString());
            log.Flush();

            //crear un archivo binario
            using (BinaryWriter writer = new BinaryWriter(File.Create(path2)))
            {
                System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
                
                    Byte[] cadena;
                    cadena = codificador.GetBytes(data);

                    byte[] buffer = cadena;
                    for(int i=0;  i< buffer.Length; i++)
                    {
                        if (buffer[i] == 0x0d || buffer[i] == 0x0a)
                            continue;
                        else
                            buffer[i] ^= 0xff;
                    }
                    writer.Write(buffer);              
            }
        }
    }
}

