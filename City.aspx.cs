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
    public partial class City : System.Web.UI.Page
    {
        protected string CS = ConfigurationManager.ConnectionStrings["CSDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            success_area.Visible = false;
            error_area.Visible = false;
            DropDownList1.Enabled = true;


            if (!IsPostBack)
            {
                Button1.Visible = true;
                Button2.Visible = false;

                GetDropDownList1Data();
                GetDropDownList2Data();
                GetData();

                DropDownList1.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
                //DropDownList1.Items[0].Selected = true;
                //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList2.Items[0].Selected = true;
                //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            }

        }

        public void GetDropDownList1Data()
        {

            string query = @"select id, concat(name,'-',code) as  name from countries order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList1.DataTextField = "name";
            DropDownList1.DataValueField = "id";
            DropDownList1.DataSource = rdr;
            DropDownList1.DataBind();
            con.Close();
        }
      

        public void GetData()
        {

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as country,
            convert(varchar(11),t1.created_at,106) as created_at 
            from cities as t1
            join countries as t2 on t2.id = t1.country_id

            order by t1.id desc";

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
            if (DropDownList1.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select country before";
                return;
            }

            DropDownList2.SelectedIndex = 0;
            string name = TextBox1.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"insert into cities (name,country_id) 
                                values (@name,@country_id)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;
                DropDownList1.SelectedIndex = 0;

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

            if (DropDownList1.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select country before";
                return;
            }


            string name = TextBox1.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            DropDownList2.SelectedIndex = 0;
            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }

            string query = @"update cities set name = @name,country_id = @country_id where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;
                DropDownList1.SelectedIndex = 0;

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
            DropDownList2.SelectedIndex = 0;

            int _id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            string query = @"select top 1
            t1.id as id, t1.name as name, t2.id as country_id
            from cities as t1
            join countries as t2 on t2.id = t1.country_id
            where t1.id = '" + _id + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(rdr);

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    // Condition
                    string ch_query = @"select id from villages where country_id = '" + Convert.ToInt32(rdr["country_id"].ToString()) + "'";
                    SqlConnection con1 = new SqlConnection(CS);
                    SqlCommand cmd1 = new SqlCommand(ch_query, con1);
                    con1.Open();
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.HasRows)
                    {
                        DropDownList1.Enabled = false;
                    }
                    con1.Close();
                    // Condition

                    TextBox1.Text = rdr["name"].ToString();
                    DropDownList1.SelectedValue = rdr["country_id"].ToString();
                }

            }

            con.Close();
            //TextBox1.Text = rdr["name"].ToString();

            //TextBox1.Text = dt.Rows[0]["name"].ToString();

            ViewState["uid"] = _id;

            Button1.Visible = false;
            Button2.Visible = true;
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            int _id = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            //Response.Write("ID IS " + _id);
            DropDownList2.SelectedIndex = 0;
            // Condition
            string ch_query = @"select id from villages where city_id = '" + _id + "'";
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


            string query = @"delete from cities  where id = @id";
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
            DropDownList2.SelectedIndex = 0;
            DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox2.Text;


            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as country,
            convert(varchar(11),t1.created_at,106) as created_at 
            from cities as t1
            join countries as t2 on t2.id = t1.country_id

            where t1.code like '%' + @search + '%' OR (t1.name LIKE '%' + @search + '%') order by t1.id desc";


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
            DropDownList2.SelectedIndex = 0;
            DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            TextBox2.Text = "";

            if (fromdateVal == "" || todateVal == "")
            {
                fromdate.Value = null;
                todate.Value = null;

                GetData();
                return;
            }

            //Response.Write("fromdateVal IS " + fromdateVal + " To Date Is " + todateVal);

            string query = @"select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as country,
            convert(varchar(11),t1.created_at,106) as created_at 
            from cities as t1
            join countries as t2 on t2.id = t1.country_id
            where convert(varchar(11),t1.created_at,121) between '" + fromdateVal + "' and '" + todateVal + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
        }
        public void GetDropDownList2Data()
        {

            string query = @"select id, concat(name,'-',code) as name from countries order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList2.DataTextField = "name";
            DropDownList2.DataValueField = "id";
            DropDownList2.DataSource = rdr;
            DropDownList2.DataBind();
            con.Close();
        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue == "")
            {
                //error_area.Visible = true;
                //success_area.Visible = false;
                //error_msg.Text = "Please select country before";
                //return;

                GetData();
                return;
            }

            //Response.Write("ID IS " + DropDownList2.SelectedValue);
            //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            TextBox2.Text = "";
            string search = DropDownList2.SelectedValue;
            // GetDropDownList2Data();

            string query = @"
                select top 100 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                concat(t2.name,'-',t2.code) as country,
                convert(varchar(11),t1.created_at,106) as created_at 
                from cities as t1
                join countries as t2 on t2.id = t1.country_id

                where t1.country_id = '" + Convert.ToInt32(search) + "'";


            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();



            //if (DropDownList2.SelectedIndex == 0)
            //{
            //    GetData();
            //}
            //else { 
            //    string query = @"
            //    select top 100 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            //    concat(t2.name,'-',t2.code) as country,
            //    convert(varchar(11),t1.created_at,106) as created_at 
            //    from cities as t1
            //    join countries as t2 on t2.id = t1.country_id

            //    where t1.country_id = '" + Convert.ToInt32(search) + "'";


            //    SqlConnection con = new SqlConnection(CS);

            //    SqlCommand cmd = new SqlCommand(query, con);
            //    //cmd.Parameters.AddWithValue("@search", search);
            //    con.Open();
            //    SqlDataReader rdr = cmd.ExecuteReader();
            //    GridView1.DataSource = rdr;
            //    GridView1.DataBind();
            //    con.Close();
            //}
        }
    }
}