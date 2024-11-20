using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnetedArchDBStudent
{
    public partial class Form1 : Form
    {
        SqlConnection conn;
        SqlCommandBuilder scd;
        SqlDataAdapter da;
        DataSet ds;
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["defaultCon"].ConnectionString);
        }

        private DataSet getStudent()
        {
            da = new SqlDataAdapter("select * from studentinfo", conn);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            scd = new SqlCommandBuilder(da);
            ds= new DataSet();
            da.Fill(ds, "student");
            return ds;
        }
        public void clearStudentForm()
        {
            txtId.Clear();
            txtName.Clear();
            txtPercentage.Clear();
            comboBoxDepartment.SelectedIndex=(-1);
            
           
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ds = getStudent();
                DataRow row = ds.Tables["student"].NewRow();
                row["name"] = txtName.Text;
                row["department"] = comboBoxDepartment.SelectedItem;
                row["percentage"] = txtPercentage.Text;

                ds.Tables["student"].Rows.Add(row);

                int result = da.Update(ds.Tables["student"]);
                if (result >= 1)
                {
                    MessageBox.Show("Student Added Successfully");
                    clearStudentForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ds = getStudent();
                DataRow row = ds.Tables["student"].Rows.Find(txtId.Text);
                if (row != null)
                {
                    txtName.Text = row["name"].ToString();
                    comboBoxDepartment.SelectedItem = row["department"].ToString();
                    txtPercentage.Text = row["percentage"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found");
                    clearStudentForm();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearStudentForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ds=getStudent();
            DataRow row = ds.Tables["student"].Rows.Find(txtId.Text);
            if (row != null)
            {
                row.Delete();
                int result = da.Update(ds.Tables["student"]);
                if(result >=1)
                {
                    MessageBox.Show("Student Delete Successfully");
                    clearStudentForm();
                }
                else
                {
                    MessageBox.Show("Student not found !");
                }
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            ds = getStudent();
            dataGridView1.DataSource = ds.Tables["student"];
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ds = getStudent();
            DataRow row = ds.Tables["student"].Rows.Find(txtId.Text);
            if (row != null)
            {
                row["name"]=txtName.Text;
                row["department"] = comboBoxDepartment.SelectedItem;
                row["percentage"] = txtPercentage.Text;

                int result = da.Update(ds.Tables["student"]);
                if( result >=1)
                {
                    MessageBox.Show("Record Updated Successfully");
                }
            }
            else
            {
                MessageBox.Show("Record not found");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
