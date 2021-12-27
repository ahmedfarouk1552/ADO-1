using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace TaskAdoD01
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        
        void fillgrade()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["inscon"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select i.*, d.dept_id from instructor i ,department d where i.dept_id=d.dept_id", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            List<instructor> ins = new List<instructor>();
            while (dr.Read())
            {
                instructor i = new instructor();
                i.id = (int)dr["ins_id"];
                i.name = dr["ins_name"].ToString();
                i.degree = dr["ins_degree"].ToString();
                i.salary = (decimal)dr["salary"];
                i.dept = dr["dept_id"].ToString();
                ins.Add(i);
            }
            dgv_instructor.DataSource = ins;
            dr.Close();
            SqlCommand cmddept = new SqlCommand("select dept_id,dept_name from department", con);
            SqlDataReader dr2 = cmddept.ExecuteReader();
            List<department> depts = new List<department>();
            while (dr2.Read())
            {
                department d = new department();
                d.id = (int)dr2["dept_id"];
                d.name = dr2["dept_name"].ToString();
                depts.Add(d);
            }
            cb_dept.DataSource = depts;
            cb_dept.DisplayMember = "name";
            cb_dept.ValueMember = "id";
            //dr2.Close();
            con.Close();
        }
        public Form1()
        {
            InitializeComponent();
            fillgrade();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("insert into instructor(ins_id,ins_name,ins_degree,salary,dept_id)values(@id,@name,@degree,@salary,@dept)", con);
            cmd.Parameters.AddWithValue("@id", txt_id.Text);
            cmd.Parameters.AddWithValue("@name", txt_name.Text);
            cmd.Parameters.AddWithValue("@degree", txt_degree.Text);
            cmd.Parameters.AddWithValue("@salary", txt_salary.Text);
            cmd.Parameters.AddWithValue("@dept", cb_dept.SelectedValue);

            con.Open();
            int roweffect = cmd.ExecuteNonQuery();
            con.Close();
            if (roweffect > 0)
            {
                txt_id.Text = txt_name.Text = txt_degree.Text = txt_salary.Text = " ";
                lbl_result.Text = "Successfully Added";
                fillgrade();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            int id = (int)dgv_instructor.SelectedRows[0].Cells["id"].Value;
            SqlCommand cmd = new SqlCommand("delete from instructor where ins_id=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            int roweffect = cmd.ExecuteNonQuery();
            con.Close();
            if (roweffect > 0)
            {
                fillgrade();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            int id = (int)dgv_instructor.SelectedRows[0].Cells["id"].Value;


            SqlCommand cmd = new SqlCommand("update instructor set ins_name=@name,ins_degree=@degree,Salary=@salary,dept_id=@dept where ins_id=@id", con);
            cmd.Parameters.AddWithValue("@name", txt_name.Text);
            cmd.Parameters.AddWithValue("@degree", txt_degree.Text);
            cmd.Parameters.AddWithValue("@salary", txt_salary.Text);
            cmd.Parameters.AddWithValue("@dept", cb_dept.SelectedValue);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            int roweffect = cmd.ExecuteNonQuery();
            con.Close();
            if (roweffect > 0)
            {
                lbl_result.Text = "Updated";
                fillgrade();
                
            }
        }

        private void dgv_instructor_SelectionChanged(object sender, EventArgs e)
        {
            #region don't use 
            //int id = (int)dgv_instructor.Rows[0].Cells["id"].Value;


            //SqlCommand cmd = new SqlCommand("select i.ins_name,i.ins_degree,salary,d.dept_id from instructor i , department d where i.dept_id=d.dept_id", con);

            //con.Open();
            //SqlDataReader dr = cmd.ExecuteReader();

            //if (dr.Read())
            //{

            //    txt_id.Enabled = false;
            //    txt_id.Text = dgv_instructor.CurrentRow.Cells["id"].Value.ToString();
            //    txt_name.Text = dgv_instructor.CurrentRow.Cells["name"].Value.ToString();
            //    txt_degree.Text = dgv_instructor.CurrentRow.Cells["degree"].Value.ToString();
            //    txt_salary.Text = dgv_instructor.CurrentRow.Cells["Salary"].Value.ToString();
            //    cb_dept.SelectedValue = int.Parse(dgv_instructor.CurrentRow.Cells["Dept"].Value.ToString());
            //}
            //con.Close();
            #endregion


            // txt_id.Enabled = false; 
            txt_id.Text = dgv_instructor.CurrentRow.Cells["id"].Value.ToString();
            txt_name.Text = dgv_instructor.CurrentRow.Cells["name"].Value.ToString();
            txt_degree.Text = dgv_instructor.CurrentRow.Cells["degree"].Value.ToString();
            txt_salary.Text = dgv_instructor.CurrentRow.Cells["Salary"].Value.ToString();
            cb_dept.SelectedValue = int.Parse(dgv_instructor.CurrentRow.Cells["dept"].Value.ToString());






        }
    }
}

