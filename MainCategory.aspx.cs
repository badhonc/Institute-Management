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
    public partial class MainCategory : System.Web.UI.Page
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

                GetDropDownList10Data();
                GetDropDownList3Data();
                GetData();
                //DropDownList4.Enabled = false;

                DropDownList10.Items.Insert(0, new ListItem("Select Category", ""));
                DropDownList20.Items.Insert(0, new ListItem("Select Sub-Category", ""));
                DropDownList3.Items.Insert(0, new ListItem("Select Category", ""));
                DropDownList4.Items.Insert(0, new ListItem("Select Sub-Category", ""));
                //DropDownList10.Items[0].Selected = true;
                //DropDownList10.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList20.Items[0].Selected = true;
                //DropDownList20.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList3.Items[0].Selected = true;
                //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList4.Items[0].Selected = true;
                //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            }
            ///bool chk = Convert.ToBoolean(DropDownList4_SelectedIndexChanged(sender, e));
            //if (DropDownList4_SelectedIndexChanged(sender,e) == true)

        }

        public void GetDropDownList10Data()
        {

            string query = @"select id, concat(name,'-',code) as name from categories order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList10.DataTextField = "name";
            DropDownList10.DataValueField = "id";
            DropDownList10.DataSource = rdr;
            DropDownList10.DataBind();
            con.Close();
        }

        public void GetData()
        {

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from maincategories as t1
            join subcategories as t3 on t3.id = t1.subcategory_id
            join categories as t2 on t2.id = t3.category_id

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
            if (DropDownList10.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select category before";
                return;
            }

            if (DropDownList20.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select sub-category before";
                return;
            }


            string name = TextBox1.Text;
            int subcategory_id = Convert.ToInt32(DropDownList20.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"insert into maincategories (name,subcategory_id) 
                                values (@name,@subcategory_id)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            //cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = "";
                TextBox2.Text = "";
                fromdate.Value = null;
                todate.Value = null;
                DropDownList10.SelectedIndex = 0;
                DropDownList20.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedValue = "";

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
            if (DropDownList10.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select category before";
                return;
            }

            if (DropDownList20.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select subcategory before";
                return;
            }


            string name = TextBox1.Text;
            //int country_id = Convert.ToInt32(DropDownList10.SelectedValue);
            int subcategory_id = Convert.ToInt32(DropDownList20.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }

            string query = @"update maincategories set name = @name,subcategory_id = @subcategory_id where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            //cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = "";
                TextBox2.Text = "";
                fromdate.Value = null;
                todate.Value = null;
                DropDownList10.SelectedIndex = 0;
                DropDownList20.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedValue = "";

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
            t2.id as subcategory_id, t3.id as category_id

            from maincategories as t1
            join subcategories as t2 on t2.id = t1.subcategory_id
            join categories as t3 on t3.id = t2.category_id
            where t1.id = '" + _id + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(rdr);

            DropDownList20.Items.Clear();
            DropDownList20.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList4.Items.Clear();
            DropDownList4.Items.Insert(0, new ListItem("Select Item", ""));

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    // Condition
                    string ch_query = 
                    @"select t1.id as id
                    from products as t1 
                    join maincategories as t2 on t2.id = t1.maincategory_id
                    join subcategories as t3 on t3.id = t2.subcategory_id
                    join categories as t4 on t4.id = t3.category_id 
                    where (t3.category_id = '" + Convert.ToInt32(rdr["category_id"].ToString()) + "' AND t2.subcategory_id = '" + Convert.ToInt32(rdr["subcategory_id"].ToString()) + "')";
                    SqlConnection con1 = new SqlConnection(CS);
                    SqlCommand cmd1 = new SqlCommand(ch_query, con1);
                    con1.Open();
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.HasRows)
                    {
                        //DropDownList10.Enabled = false;
                        //DropDownList20.Enabled = false;
                        success_area.Visible = true;
                        error_area.Visible = false;
                    }
                    con1.Close(); 
                    // Condition
                    GetDropDownList20Data(Convert.ToInt32(rdr["category_id"].ToString()));

                    TextBox1.Text = rdr["name"].ToString();
                    DropDownList10.SelectedValue = rdr["category_id"].ToString();
                    DropDownList20.SelectedValue = rdr["subcategory_id"].ToString();
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
            string ch_query = @"select id from products where maincategory_id = '" + _id + "'";
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
           
            string query = @"delete from maincategories where id = @id";
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
            DropDownList3.SelectedValue = "";
            DropDownList4.SelectedValue = "";
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList10.SelectedValue == "")
            {
                DropDownList20.Items.Clear();
                DropDownList20.Items.Insert(0, new ListItem("Select sub-category", ""));
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select category before";
                return;
            }

            int category_id = Convert.ToInt32(DropDownList10.SelectedValue);

            DropDownList20.Items.Clear();
            DropDownList20.Items.Insert(0, new ListItem("Select Sub-Category", ""));
            //DropDownList10.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList20Data(category_id);
        }


        public void GetDropDownList20Data(int category_id)
        {

            string query = @"select id, concat(name,'-',code) as name from subcategories where category_id = '" + category_id + "' order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList20.DataTextField = "name";
            DropDownList20.DataValueField = "id";
            DropDownList20.DataSource = rdr;
            DropDownList20.DataBind();
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

            fromdate.Value = null;
            todate.Value = null;
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox2.Text;


            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from maincategories as t1
            join subcategories as t3 on t3.id = t1.subcategory_id
            join categories as t2 on t2.id = t3.category_id

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

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from maincategories as t1
            join subcategories as t3 on t3.id = t1.subcategory_id
            join categories as t2 on t2.id = t3.category_id
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

            string query = @"select id, concat(name,'-',code) as name from categories order by id asc";
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
            if (DropDownList3.SelectedValue == "")
            {
                DropDownList4.Items.Clear();
                DropDownList4.Items.Insert(0, new ListItem("Select sub-category", ""));

                GetData();
                return;
            }

            int category_id = Convert.ToInt32(DropDownList3.SelectedValue);

            TextBox2.Text = "";
            fromdate.Value = null;
            todate.Value = null;
            DropDownList4.Items.Clear();
            DropDownList4.Items.Insert(0, new ListItem("Select Sub-Category", ""));
            
            GetDropDownList4Data(category_id);

            //Response.Write("ID IS " + DropDownList3.SelectedValue);
            string search = DropDownList3.SelectedValue;

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from maincategories as t1
            join subcategories as t3 on t3.id = t1.subcategory_id
            join categories as t2 on t2.id = t3.category_id

            where t3.category_id = '" + Convert.ToInt32(search) + "'";


            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
            
        }

        public void GetDropDownList4Data(int category_id)
        {

            string query = @"select id, concat(name,'-',code) as name from subcategories where category_id = '" + category_id + "' order by id asc";
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
            if (DropDownList4.SelectedValue == "")
            {
                DropDownList3.SelectedIndex = 0;
                DropDownList4.Items.Clear();
                DropDownList4.Items.Insert(0, new ListItem("Select sub-category", ""));
                GetData();
                return;
            }
            TextBox2.Text = "";
            fromdate.Value = null;
            todate.Value = null;
            //Response.Write("ID IS " + DropDownList4.SelectedValue);
            string search = DropDownList4.SelectedValue;

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from maincategories as t1
            join subcategories as t3 on t3.id = t1.subcategory_id
            join categories as t2 on t2.id = t3.category_id
            where t1.subcategory_id = '" + Convert.ToInt32(search) + "'";

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