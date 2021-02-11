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
    public partial class Purchase : System.Web.UI.Page
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
                GetDropDownList4Data();
                GetData();
                //DropDownList5.Enabled = false;
                //DropDownList6.Enabled = false;

                DropDownList1.Items.Insert(0, new ListItem("Select Category", ""));
                DropDownList2.Items.Insert(0, new ListItem("Select Sub-Category", ""));
                DropDownList3.Items.Insert(0, new ListItem("Select Main-Category", ""));
                DropDownList4.Items.Insert(0, new ListItem("Select Category", ""));
                DropDownList5.Items.Insert(0, new ListItem("Select Sub-Category", ""));
                DropDownList6.Items.Insert(0, new ListItem("Select Main-Category", ""));
                //DropDownList1.Items[0].Selected = true;
                //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList2.Items[0].Selected = true;
                //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList3.Items[0].Selected = true;
                //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
                //DropDownList4.Items[0].Selected = true;
                //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            }

        }


        public void GetDropDownList1Data()
        {

            string query = @"select id, concat(name,'-',code) as name from categories order by id asc";
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
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
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
            if (DropDownList1.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select category before";
                return;
            }

            if (DropDownList2.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select sub-category before";
                return;
            }
            if (DropDownList3.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select main-category before";
                return;
            }

            string name = TextBox1.Text;
            int maincategory_id = Convert.ToInt32(DropDownList3.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }

            string query = @"insert into products (name,maincategory_id) 
                                values (@name,@maincategory_id)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            //cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@maincategory_id", maincategory_id);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = "";
                TextBox2.Text = "";
                fromdate.Value = null;
                todate.Value = null;
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedValue = "";
                DropDownList5.SelectedValue = "";
                DropDownList6.SelectedValue = "";
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
                error_msg.Text = "Please select category before";
                return;
            }

            if (DropDownList2.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select sub-category before";
                return;
            }
            if (DropDownList3.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select main-category before";
                return;
            }


            string name = TextBox1.Text;
            int maincategory_id = Convert.ToInt32(DropDownList3.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }

            string query = @"update products set name = @name,maincategory_id = @maincategory_id where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@maincategory_id", maincategory_id);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;
                fromdate.Value = null;
                todate.Value = null;
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedValue = "";
                DropDownList5.SelectedValue = "";
                DropDownList6.SelectedValue = "";

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
            t2.id as maincategory_id, t3.id as subcategory_id, t4.id as category_id
            from products as t1
            join maincategories as t2 on t2.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t2.subcategory_id
            join categories as t4 on t4.id = t3.category_id
            where t1.id = '" + _id + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(rdr);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
            //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
            //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
            DropDownList4.SelectedValue = "";
            DropDownList5.SelectedValue = "";
            DropDownList6.SelectedValue = "";

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    GetDropDownList2Data(Convert.ToInt32(rdr["category_id"].ToString()));
                    GetDropDownList3Data(Convert.ToInt32(rdr["subcategory_id"].ToString()));

                    TextBox1.Text = rdr["name"].ToString();
                    DropDownList1.SelectedValue = rdr["category_id"].ToString();
                    DropDownList2.SelectedValue = rdr["subcategory_id"].ToString();
                    DropDownList3.SelectedValue = rdr["maincategory_id"].ToString();
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

            string query = @"delete from products where id = @id";
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
            DropDownList4.SelectedValue = "";
            DropDownList5.SelectedValue = "";
            DropDownList6.SelectedValue = "";
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList1.SelectedValue == "")
            {
                DropDownList2.Items.Clear();
                DropDownList2.Items.Insert(0, new ListItem("Select sub-category", ""));
                DropDownList3.Items.Clear();
                DropDownList3.Items.Insert(0, new ListItem("Select main-category", ""));
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select category before";
                return;
            }
            //Label2.Text = DateTime.Now.ToLongTimeString();
            int category_id = Convert.ToInt32(DropDownList1.SelectedValue);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "select2", "select2()", true);
            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select sub-category", ""));
            //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList2Data(category_id);
            ////DropDownList3.Items.Clear();
            //DropDownList3.Items.Insert(0, new ListItem("Select main-category", ""));
            DropDownList3.SelectedIndex = 0;
            //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList2.SelectedValue == "")
            {
                DropDownList3.Items.Clear();
                DropDownList3.Items.Insert(0, new ListItem("Select main-category", ""));
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select sub-category before";
                return;
            }
            int subcategory_id = Convert.ToInt32(DropDownList2.SelectedValue);

            DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select main-category", ""));
            DropDownList3.SelectedIndex = 0;
            //DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList3Data(subcategory_id);

            //DropDownList1.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList2.Items[0].Attributes["disabled"] = "disabled";
        }


        public void GetDropDownList2Data(int category_id)
        {

            string query = @"select id, concat(name,'-',code) as name from subcategories where category_id = '" + category_id + "' order by id asc";
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
        public void GetDropDownList3Data(int subcategory_id)
        {

            string query = @"select id, concat(name,'-',code) as name from maincategories where subcategory_id = '" + subcategory_id + "' order by id asc";
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
        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

            DropDownList4.SelectedIndex = 0;
            DropDownList5.SelectedIndex = 0;
            DropDownList6.SelectedIndex = 0;
            //DropDownList5.Enabled = false;
            //DropDownList6.Enabled = false;
            //DropDownList4.Items[0].Selected = true;
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList5.Items[0].Selected = true;
            //DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList6.Items[0].Selected = true;
            //DropDownList6.Items[0].Attributes["disabled"] = "disabled";

            fromdate.Value = null;
            todate.Value = null;
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox2.Text;


            string query = @"
			select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
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
            DropDownList4.SelectedIndex = 0;
            DropDownList5.SelectedIndex = 0;
            DropDownList6.SelectedIndex = 0;


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
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
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

        public void GetDropDownList4Data()
        {

            string query = @"select id, concat(name,'-',code) as name from categories order by id asc";
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
                DropDownList5.Items.Clear();
                DropDownList5.Items.Insert(0, new ListItem("Select sub-category", ""));
                DropDownList6.Items.Clear();
                DropDownList6.Items.Insert(0, new ListItem("Select main-category", ""));
                GetData();
                return;
            }

            int category_id = Convert.ToInt32(DropDownList4.SelectedValue);

            TextBox2.Text = "";
            fromdate.Value = null;
            todate.Value = null;
            //DropDownList5.Enabled = true;
            //DropDownList6.Enabled = false;
            DropDownList5.Items.Clear();
            DropDownList5.Items.Insert(0, new ListItem("Select sub-category", ""));
            DropDownList6.Items.Clear();
            DropDownList6.Items.Insert(0, new ListItem("Select main-category", ""));
            //DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList5Data(category_id);



            ///Response.Write("ID IS " + DropDownList4.SelectedValue);
            string search = DropDownList4.SelectedValue;



            string query = @"
			select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
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

        public void GetDropDownList5Data(int category_id)
        {

            string query = @"select id, concat(name,'-',code) as name from subcategories where category_id = '" + category_id + "' order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList5.DataTextField = "name";
            DropDownList5.DataValueField = "id";
            DropDownList5.DataSource = rdr;
            DropDownList5.DataBind();
            con.Close();
        }
        public void GetDropDownList6Data(int subcategory_id)
        {

            string query = @"select id, concat(name,'-',code) as name from maincategories where subcategory_id = '" + subcategory_id + "' order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList6.DataTextField = "name";
            DropDownList6.DataValueField = "id";
            DropDownList6.DataSource = rdr;
            DropDownList6.DataBind();
            con.Close();
        }
        protected void DropDownList5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList5.SelectedValue == "")
            {
                DropDownList4.SelectedIndex = 0;
                DropDownList5.Items.Clear();
                DropDownList5.Items.Insert(0, new ListItem("Select sub-category", ""));

                DropDownList6.Items.Clear();
                DropDownList6.Items.Insert(0, new ListItem("Select main-category", ""));
                GetData();
                return;
            }
            //Response.Write("ID IS " + DropDownList5.SelectedValue);
            string search = DropDownList5.SelectedValue;
            int subcategory_id = Convert.ToInt32(DropDownList5.SelectedValue);
            TextBox2.Text = "";
            fromdate.Value = null;
            todate.Value = null;
            //DropDownList6.Enabled = true;
            DropDownList6.Items.Clear();
            DropDownList6.Items.Insert(0, new ListItem("Select main-category", ""));
            //DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";

            GetDropDownList6Data(subcategory_id);

            string query = @"
			select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
            join categories as t2 on t2.id = t3.category_id
            where t4.subcategory_id = '" + Convert.ToInt32(search) + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@search", search);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            GridView1.DataSource = rdr;
            GridView1.DataBind();
            con.Close();
        }

        protected void DropDownList6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownList6.SelectedValue == "")
            {
                DropDownList5.Items.Clear();
                DropDownList5.Items.Insert(0, new ListItem("Select sub-category", ""));
                DropDownList6.Items.Clear();
                DropDownList6.Items.Insert(0, new ListItem("Select main-category", ""));
                DropDownList4.SelectedIndex = 0;
                DropDownList5.SelectedIndex = 0;
                GetData();
                return;
            }
            TextBox2.Text = "";
            //DropDownList6.Enabled = false;
            fromdate.Value = null;
            todate.Value = null;
            //DropDownList4.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList6.Items[0].Attributes["disabled"] = "disabled";

            //Response.Write("ID IS " + DropDownList6.SelectedValue);

            string search = DropDownList6.SelectedValue;

            string query = @"
			select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            concat(t2.name,'-',t2.code) as category,
            concat(t3.name,'-',t3.code) as subcategory,
            concat(t4.name,'-',t4.code) as maincategory,
            convert(varchar(11),t1.created_at,106) as created_at 
            from products as t1
            join maincategories as t4 on t4.id = t1.maincategory_id
            join subcategories as t3 on t3.id = t4.subcategory_id
            join categories as t2 on t2.id = t3.category_id

            where t1.maincategory_id = '" + Convert.ToInt32(search) + "'";


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