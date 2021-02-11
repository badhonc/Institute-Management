using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Institute_Management
{
    public partial class Department : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["CSDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            success_area.Visible = false;
            error_area.Visible = false;

            if (!IsPostBack)
            {
                Button1.Visible = true;
                Button2.Visible = false;

                GetData();
            }

        }

        public void GetData()
        {

            string query = @"select top 200 row_number() over (order by id) as row_number, id, name, code, 
                            convert(varchar(11),created_at,106) as created_at from departments order by id desc";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = TextBox1.Text;

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"insert into departments (name) 
                                values (@name)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;

                success_area.Visible = true;
                error_area.Visible = false;
                success_msg.Text = "Data inserted successfully";
                GetData();

            }
            else
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Data did not inserted successfully";
            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string name = TextBox1.Text;

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"update departments set name = @name where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;

                ViewState["uid"] = null;
                Button1.Visible = true;
                Button2.Visible = false;

                success_area.Visible = true;
                error_area.Visible = false;
                success_msg.Text = "Data updated successfully";
                GetData();

            }
            else
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Data was not updated successfully";
            }

        }





        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            int _id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            string query = @"select id, name from departments where id = '" + _id + "'";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    TextBox1.Text = rdr["name"].ToString();
                }

            }
            con.Close();
            ViewState["uid"] = _id;

            Button1.Visible = false;
            Button2.Visible = true;
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            int _id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //Response.Write("ID IS " + _id);

            // Condition
            string ch_query = @"select id from students where department_id = '" + _id + "'";
            SqlConnection con1 = new SqlConnection(CS);
            SqlCommand cmd1 = new SqlCommand(ch_query, con1);
            con1.Open();
            SqlDataReader rdr1 = cmd1.ExecuteReader();
            if (rdr1.HasRows)
            {
               error_area.Visible = true;
               success_area.Visible = false;
               error_msg.Text = "This data can not be deleted becase it is related with other data";
               return;
            }
            con1.Close();
            // Condition


            string query = @"delete from departments where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@id", _id);
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {

                success_area.Visible = true;
                error_area.Visible = false;
                success_msg.Text = "Data deleted successfully";
                GetData();

            }
            else
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Data did not delete successfully";
            }
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            fromdate.Value = null;
            todate.Value = null;
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox2.Text;

            string query = @"select top 1000 row_number() over (order by id) as row_number, id, name, code,
                            convert(varchar(11),created_at,106) as created_at from departments 
                            where name LIKE '%' + @search + '%' OR code like '%' + @search + '%' order by id desc";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string fromdateVal = fromdate.Value.ToString();
            string todateVal = todate.Value.ToString();

            if (fromdateVal == "" || todateVal == "")
            {
                fromdate.Value = null;
                todate.Value = null;

                GetData();
                return;
            }




            //Response.Write("fromdateVal IS " + fromdateVal + " To Date Is " + todateVal);

            string query = @"select top 1000 row_number() over (order by id) as row_number, id, name, code,
                           convert(varchar(11),created_at,106) as created_at from departments 
                           where convert(varchar(11),created_at,121) between '" + fromdateVal + "' and '" + todateVal + "'  order by id desc";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
        }
    }
}