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
    public partial class Village : System.Web.UI.Page
    {
        protected string CS = ConfigurationManager.ConnectionStrings["CSDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            success_area.Visible = false;
            error_area.Visible = false;




            if (!IsPostBack)
            {
                Button1.Visible = true;
                Button2.Visible = false;

                GetDropDownList1Data();
                GetDropDownList3Data();
                GetData();


                DropDownList1.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList3.Items.Insert(0, new ListItem("Select Country", ""));
                DropDownList4.Items.Insert(0, new ListItem("Select City", ""));
                //DropDownList1.Items[0].Selected = true;
                //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList2.Items[0].Selected = true;
                //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList3.Items[0].Selected = true;
                //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList4.Items[0].Selected = true;
                //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            }
            ///bool chk = Convert.ToBoolean(DropDownList4_SelectedIndexChanged(sender, e));
            //if (DropDownList4_SelectedIndexChanged(sender,e) == true)
            
           // int country_id = Convert.ToInt32(DropDownList3.SelectedValue);
            //GetDropDownList4Data(country_id);

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
            concat(t3.name,'-',t3.code) as city,
            convert(varchar(11),t1.created_at,106) as created_at 
            from villages as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id

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

            if (DropDownList2.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select city before";
                return;
            }


            string name = TextBox1.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            int city_id = Convert.ToInt32(DropDownList2.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"insert into villages (name,country_id,city_id) 
                                values (@name,@country_id,@city_id)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@city_id", city_id);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = "";
                fromdate.Value = null;
                todate.Value = null;
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;

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

            if (DropDownList2.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select city before";
                return;
            }


            string name = TextBox1.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            int city_id = Convert.ToInt32(DropDownList2.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"update villages set name = @name,country_id = @country_id,city_id = @city_id where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@city_id", city_id);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = "";
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;

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
                error_msg.Text = "Data did not updated successfully";
            }

        }





        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            int _id = Convert.ToInt32(((LinkButton)sender).CommandArgument);

            string query = @"select top 1
            t1.id as id, t1.name as name, 
            t2.id as country_id, t3.id as city_id

            from villages as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
            where t1.id = '" + _id + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(rdr);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    // Condition
                    string ch_query = @"select id from students where (country_id = '" + Convert.ToInt32(rdr["country_id"].ToString()) + "' AND city_id = '" + Convert.ToInt32(rdr["city_id"].ToString()) + "')";
                    SqlConnection con1 = new SqlConnection(CS);
                    SqlCommand cmd1 = new SqlCommand(ch_query, con1);
                    con1.Open();
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.HasRows)
                    {
                        DropDownList1.Enabled = false;
                        DropDownList2.Enabled = false;
                    }
                    con1.Close();
                    // Condition
                    GetDropDownList2Data(Convert.ToInt32(rdr["country_id"].ToString()));

                    TextBox1.Text = rdr["name"].ToString();
                    DropDownList1.SelectedValue = rdr["country_id"].ToString();
                    DropDownList2.SelectedValue = rdr["city_id"].ToString();
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
            // Condition
            string ch_query = @"select id from students where village_id = '" + _id + "'";
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

            string query = @"delete from villages  where id = @id";
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
                error_msg.Text = "Data was not deleted successfully";
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select City", ""));
            //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList2.SelectedIndex = 0;
            //DropDownList2.Items[0].Attributes["disabled"] = "disabled";


            GetDropDownList2Data(country_id);

        }


        public void GetDropDownList2Data(int country_id)
        {

            string query = @"select id, concat(name,'-',code) as  name from cities where country_id = '" + country_id + "' order by id asc";
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
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            //DropDownList3.Items.Clear();
            //DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));

            //DropDownList4.Items.Clear();
            //DropDownList4.Items.Insert(0, new ListItem("Select Item", ""));

            DropDownList3.SelectedIndex = 0;
            DropDownList4.SelectedIndex = 0;
            //DropDownList4.Enabled = false;

            fromdate.Value = null;
            todate.Value = null;
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox2.Text;


            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as country,
            concat(t3.name,'-',t3.code) as city,
            convert(varchar(11),t1.created_at,106) as created_at 
            from villages as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id

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
            TextBox2.Text = "";
            DropDownList3.SelectedIndex = 0;
            DropDownList4.SelectedIndex = 0;


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
            concat(t3.name,'-',t3.code) as city,
            convert(varchar(11),t1.created_at,106) as created_at 
            from villages as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
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

        public void GetDropDownList3Data()
        {

            string query = @"select id, concat(name,'-',code) as name from countries order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList3.DataTextField = "name";
            DropDownList3.DataValueField = "id";
            DropDownList3.DataSource = rdr;
            DropDownList3.DataBind();
            con.Close();
        }
        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int country_id = Convert.ToInt32(DropDownList3.SelectedValue);
            
            TextBox2.Text = "";
            DropDownList4.Enabled = true;
            //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            DropDownList4.Items.Clear();
            DropDownList4.Items.Insert(0, new ListItem("Select City", ""));
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList4Data(country_id);

            //aResponse.Write("ID IS " + DropDownList3.SelectedValue);
            string search = DropDownList3.SelectedValue;
            // GetDropDownList2Data();
            if (DropDownList3.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                convert(varchar(11),t1.created_at,106) as created_at 
                from villages as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id

                where t1.country_id = '" + Convert.ToInt32(search) + "'";


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

        public void GetDropDownList4Data(int country_id)
        {

            string query = @"select id, concat(name,'-',code) as  name from cities where country_id = '" + country_id + "' order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList4.DataTextField = "name";
            DropDownList4.DataValueField = "id";
            DropDownList4.DataSource = rdr;
            DropDownList4.DataBind();
            con.Close();
        }
        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Write("ID IS " + DropDownList4.SelectedValue);
            string search = DropDownList4.SelectedValue;
            //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            // GetDropDownList2Data();
            if (DropDownList4.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                convert(varchar(11),t1.created_at,106) as created_at 
                from villages as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id

                where t1.city_id = '" + Convert.ToInt32(search) + "'";


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
}