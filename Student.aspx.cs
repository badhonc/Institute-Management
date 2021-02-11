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
    public partial class Student : System.Web.UI.Page
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
                GetDropDownList5Data();
                GetDropDownList8Data();
                GetData();
                DropDownList6.Enabled = false;
                DropDownList7.Enabled = false;


                DropDownList1.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList4.Items.Insert(0, new ListItem("Select Item", ""));
                DropDownList5.Items.Insert(0, new ListItem("Select Country", ""));
                DropDownList6.Items.Insert(0, new ListItem("Select City", ""));
                DropDownList7.Items.Insert(0, new ListItem("Select Village", ""));
                DropDownList8.Items.Insert(0, new ListItem("Select Department", ""));
                DropDownList1.Items[0].Selected = true;
                DropDownList1.Items[0].Attributes["disabled"] = "disabled";
                DropDownList2.Items[0].Selected = true;
                DropDownList2.Items[0].Attributes["disabled"] = "disabled";
                DropDownList3.Items[0].Selected = true;
                DropDownList3.Items[0].Attributes["disabled"] = "disabled";
                DropDownList4.Items[0].Selected = true;
                DropDownList4.Items[0].Attributes["disabled"] = "disabled";
                DropDownList5.Items[0].Selected = true;
                DropDownList5.Items[0].Attributes["disabled"] = "disabled";
                DropDownList6.Items[0].Selected = true;
                DropDownList6.Items[0].Attributes["disabled"] = "disabled";
                DropDownList7.Items[0].Selected = true;
                DropDownList7.Items[0].Attributes["disabled"] = "disabled";
                DropDownList8.Items[0].Selected = true;
                DropDownList8.Items[0].Attributes["disabled"] = "disabled";
                RadioButtonList1.ClearSelection();
            }
        }

        public void GetDropDownList1Data()
        {
            string query = @"select id, concat(name,'-',code) as name from countries order by id asc";
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

        public void GetDropDownList4Data()
        {

            string query = @"select id, concat(name,'-',code) as name from departments order by id asc";
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

        public void GetData()
        {

            string query = @"
            select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code, t1.gender as gender,
            t1.email as email, t1.father as father, t1.mother as mother,
            concat(t2.name,'-',t2.code) as country,
            concat(t3.name,'-',t3.code) as city,
            concat(t4.name,'-',t4.code) as village,
            concat(t5.name,'-',t5.code) as department,
            convert(varchar(11),t1.created_at,106) as created_at 
            from students as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
            join villages as t4 on t4.id = t1.village_id
            join departments as t5 on t5.id = t1.department_id
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
            if (DropDownList3.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select village before";
                return;
            }
            if (DropDownList4.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select department before";
                return;
            }


            string name = TextBox1.Text;
            string email = TextBox2.Text;
            string father = TextBox3.Text;
            string mother = TextBox4.Text;
            string gender = RadioButtonList1.SelectedItem.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            int city_id = Convert.ToInt32(DropDownList2.SelectedValue);
            int village_id = Convert.ToInt32(DropDownList3.SelectedValue);
            int department_id = Convert.ToInt32(DropDownList4.SelectedValue);

            if (name == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name before";
                return;
            }



            string query = @"insert into students (name,country_id,city_id,village_id,department_id,email,father,mother,gender) 
                                values (@name,@country_id,@city_id,@village_id,@department_id,@email,@father,@mother,@gender)";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@city_id", city_id);
            cmd.Parameters.AddWithValue("@village_id", village_id);
            cmd.Parameters.AddWithValue("@department_id", department_id);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@father", father);
            cmd.Parameters.AddWithValue("@mother", mother);
            cmd.Parameters.AddWithValue("@gender", gender);

            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;
                TextBox2.Text = null;
                TextBox3.Text = null;
                TextBox4.Text = null;
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedIndex = 0;
                RadioButtonList1.ClearSelection();
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
            if (DropDownList3.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select village before";
                return;
            }
            if (DropDownList4.SelectedValue == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select department before";
                return;
            }


            string name = TextBox1.Text;
            string email = TextBox2.Text;
            string father = TextBox3.Text;
            string mother = TextBox4.Text;
            string gender = RadioButtonList1.SelectedItem.Text;
            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            int city_id = Convert.ToInt32(DropDownList2.SelectedValue);
            int village_id = Convert.ToInt32(DropDownList3.SelectedValue);
            int department_id = Convert.ToInt32(DropDownList4.SelectedValue);

            if (name == "" || email == "")
            {
                error_area.Visible = true;
                success_area.Visible = false;
                error_msg.Text = "Please select name or email before";
                return;
            }



            string query = @"update students set name = @name,country_id = @country_id,city_id = @city_id,village_id = @village_id,
            department_id = @department_id,email = @email,father = @father,mother = @mother, gender = @gender where id = @id";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@country_id", country_id);
            cmd.Parameters.AddWithValue("@city_id", city_id);
            cmd.Parameters.AddWithValue("@village_id", village_id);
            cmd.Parameters.AddWithValue("@department_id", department_id);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@father", father);
            cmd.Parameters.AddWithValue("@mother", mother);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ViewState["uid"]));
            int rowCount = cmd.ExecuteNonQuery();
            con.Close();
            if (rowCount > 0)
            {
                TextBox1.Text = null;
                TextBox2.Text = null;
                TextBox3.Text = null;
                TextBox4.Text = null;
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
                DropDownList4.SelectedIndex = 0;
                RadioButtonList1.ClearSelection();

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
            t1.id as id, t1.name as name, t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
            t2.id as country_id, t3.id as city_id, t4.id as village_id, t5.id as department_id
            from students as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
            join villages as t4 on t4.id = t1.village_id
            join departments as t5 on t5.id = t1.department_id            
            where t1.id = '" + _id + "'";

            SqlConnection con = new SqlConnection(CS);

            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            //DataTable dt = new DataTable();
            //dt.Load(rdr);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
            

            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    GetDropDownList2Data(Convert.ToInt32(rdr["country_id"].ToString()));
                    GetDropDownList3Data(Convert.ToInt32(rdr["city_id"].ToString()));

                    //string did = rdr["department_id"].ToString();


                    TextBox1.Text = rdr["name"].ToString();
                    TextBox2.Text = rdr["email"].ToString();
                    TextBox3.Text = rdr["father"].ToString();
                    TextBox4.Text = rdr["mother"].ToString();
                    DropDownList1.SelectedValue = rdr["country_id"].ToString();
                    DropDownList2.SelectedValue = rdr["city_id"].ToString();
                    DropDownList3.SelectedValue = rdr["village_id"].ToString();
                    DropDownList4.SelectedValue = rdr["department_id"].ToString();
                    RadioButtonList1.SelectedValue = rdr["gender"].ToString();
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
            Response.Write("ID IS " + _id);

            string query = @"delete from students where id = @id";
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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

            int country_id = Convert.ToInt32(DropDownList1.SelectedValue);

            DropDownList2.Items.Clear();
            DropDownList2.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            //DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList3.SelectedIndex = 0;
            DropDownList3.Items[0].Attributes["disabled"] = "disabled";
            DropDownList1.Items[0].Attributes["disabled"] = "disabled";


            GetDropDownList2Data(country_id);

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int country_id = Convert.ToInt32(DropDownList1.SelectedValue);
            int city_id = Convert.ToInt32(DropDownList2.SelectedValue);

            DropDownList3.Items.Clear();
            DropDownList3.Items.Insert(0, new ListItem("Select Item", ""));
            DropDownList3.SelectedIndex = 0;
            DropDownList1.Items[0].Attributes["disabled"] = "disabled";
            DropDownList2.Items[0].Attributes["disabled"] = "disabled";
            DropDownList3.Items[0].Attributes["disabled"] = "disabled";

            //GetDropDownList2Data(country_id);
            GetDropDownList3Data(city_id);

        }


        public void GetDropDownList2Data(int country_id)
        {

            string query = @"select id, concat(name,'-',code) as name from cities where country_id = '" + country_id + "' order by id asc";
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
        public void GetDropDownList3Data(int city_id)
        {

            string query = @"select id, concat(name,'-',code) as name from villages where city_id = '" + city_id + "' order by id asc";
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
        protected void TextBox5_TextChanged(object sender, EventArgs e)
        {
            DropDownList5.SelectedIndex = 0;
            DropDownList6.SelectedIndex = 0;
            DropDownList8.SelectedIndex = 0;
            DropDownList6.Enabled = false;
            DropDownList7.Enabled = false;
            fromdate.Value = null;
            todate.Value = null;
            DropDownList5.Items[0].Selected = true;
            DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            DropDownList6.Items[0].Selected = true;
            DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            DropDownList7.Items[0].Selected = true;
            DropDownList7.Items[0].Attributes["disabled"] = "disabled";
            DropDownList8.Items[0].Selected = true;
            DropDownList8.Items[0].Attributes["disabled"] = "disabled";
            //Response.Write("ID IS " + TextBox2.Text);
            string search = TextBox5.Text;

            string query = @"select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
            concat(t2.name,'-',t2.code) as country,
            concat(t3.name,'-',t3.code) as city,
            concat(t4.name,'-',t4.code) as village,
            concat(t5.name,'-',t5.code) as department,
            convert(varchar(11),t1.created_at,106) as created_at 
            from students as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
            join villages as t4 on t4.id = t1.village_id
            join departments as t5 on t5.id = t1.department_id
            where t1.name LIKE '%' + @search + '%' OR t1.code like '%' + @search + '%'";

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
            TextBox5.Text = "";
            DropDownList5.SelectedIndex = 0;
            DropDownList6.SelectedIndex = 0;
            DropDownList7.SelectedIndex = 0;
            DropDownList8.SelectedIndex = 0;


            if (fromdateVal == "" || todateVal == "")
            {
                fromdate.Value = null;
                todate.Value = null;

                GetData();
                return;
            }

            //Response.Write("fromdateVal IS " + fromdateVal + " To Date Is " + todateVal);

            string query = @"select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
            t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
            concat(t2.name,'-',t2.code) as country,
            concat(t3.name,'-',t3.code) as city,
            concat(t4.name,'-',t4.code) as village,
            concat(t5.name,'-',t5.code) as department,
            convert(varchar(11),t1.created_at,106) as created_at 
            from students as t1
            join countries as t2 on t2.id = t1.country_id
            join cities as t3 on t3.id = t1.city_id
            join villages as t4 on t4.id = t1.village_id
            join departments as t5 on t5.id = t1.department_id
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
        public void GetDropDownList5Data()
        {

            string query = @"select id, concat(name,'-',code) as name from countries order by id asc";
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
        protected void DropDownList5_SelectedIndexChanged(object sender, EventArgs e)
        {
            int country_id = Convert.ToInt32(DropDownList5.SelectedValue);

            TextBox5.Text = "";
            DropDownList6.Enabled = true;
            DropDownList7.Enabled = false;
            DropDownList6.Items.Clear();
            DropDownList6.Items.Insert(0, new ListItem("Select City", ""));
            DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            DropDownList7.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList6Data(country_id);
            DropDownList7.SelectedIndex = 0;
            DropDownList8.SelectedIndex = 0;

            Response.Write("ID IS " + DropDownList5.SelectedValue);
            
            string search = DropDownList5.SelectedValue;
           
            if (DropDownList5.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                concat(t4.name,'-',t4.code) as village,
                concat(t5.name,'-',t5.code) as department,
                convert(varchar(11),t1.created_at,106) as created_at 
                from students as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id
                join villages as t4 on t4.id = t1.village_id
                join departments as t5 on t5.id = t1.department_id

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

        public void GetDropDownList6Data(int country_id)
        {

            string query = @"select id, concat(name,'-',code) as name from cities where country_id = '" + country_id + "' order by id asc";
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
        public void GetDropDownList7Data(int city_id)
        {

            string query = @"select id, concat(name,'-',code) as name from villages where city_id = '" + city_id + "' order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList7.DataTextField = "name";
            DropDownList7.DataValueField = "id";
            DropDownList7.DataSource = rdr;
            DropDownList7.DataBind();
            con.Close();
        }
        protected void DropDownList6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Write("ID IS " + DropDownList6.SelectedValue);
            string search = DropDownList6.SelectedValue;
            int city_id = Convert.ToInt32(DropDownList6.SelectedValue);

            TextBox5.Text = "";
            DropDownList7.Enabled = true;
            DropDownList7.Items.Clear();
            DropDownList7.Items.Insert(0, new ListItem("Select Village", ""));
            DropDownList7.Items[0].Attributes["disabled"] = "disabled";
            DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            GetDropDownList7Data(city_id);
            DropDownList8.SelectedIndex = 0;
            if (DropDownList6.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                concat(t4.name,'-',t4.code) as village,
                concat(t5.name,'-',t5.code) as department,
                convert(varchar(11),t1.created_at,106) as created_at 
                from students as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id
                join villages as t4 on t4.id = t1.village_id
                join departments as t5 on t5.id = t1.department_id
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
        protected void DropDownList7_SelectedIndexChanged(object sender, EventArgs e)
        {

            TextBox5.Text = "";
            //DropDownList6.Enabled = false;
            fromdate.Value = null;
            todate.Value = null;
            DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            DropDownList6.Items[0].Attributes["disabled"] = "disabled";
            DropDownList7.Items[0].Attributes["disabled"] = "disabled";

            Response.Write("ID IS " + DropDownList7.SelectedValue);

            string search = DropDownList7.SelectedValue;

            if (DropDownList7.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                concat(t4.name,'-',t4.code) as village,
                concat(t5.name,'-',t5.code) as department,
                convert(varchar(11),t1.created_at,106) as created_at 
                from students as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id
                join villages as t4 on t4.id = t1.village_id
                join departments as t5 on t5.id = t1.department_id

                where t1.village_id = '" + Convert.ToInt32(search) + "'";


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

        public void GetDropDownList8Data()
        {

            string query = @"select id, concat(name,'-',code) as name from departments order by id asc";
            SqlConnection con = new SqlConnection(CS);
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList8.DataTextField = "name";
            DropDownList8.DataValueField = "id";
            DropDownList8.DataSource = rdr;
            DropDownList8.DataBind();
            con.Close();
        }
        protected void DropDownList8_SelectedIndexChanged(object sender, EventArgs e)
        {

            TextBox5.Text = "";
            DropDownList5.SelectedIndex = 0;
            DropDownList6.SelectedIndex = 0;
            DropDownList7.SelectedIndex = 0;
            DropDownList6.Enabled = false;
            DropDownList7.Enabled = false;
            DropDownList5.Items[0].Attributes["disabled"] = "disabled";
            DropDownList8.Items[0].Attributes["disabled"] = "disabled";

            Response.Write("ID IS " + DropDownList8.SelectedValue);

            string search = DropDownList8.SelectedValue;

            if (DropDownList8.SelectedIndex == 0)
            {
                GetData();
            }
            else
            {
                string query = @"
                select top 1000 row_number() over (order by t1.id) as row_number, t1.id as id, t1.name as name, t1.code as code,
                t1.email as email, t1.father as father, t1.mother as mother, t1.gender as gender,
                concat(t2.name,'-',t2.code) as country,
                concat(t3.name,'-',t3.code) as city,
                concat(t4.name,'-',t4.code) as village,
                concat(t5.name,'-',t5.code) as department,
                convert(varchar(11),t1.created_at,106) as created_at 
                from students as t1
                join countries as t2 on t2.id = t1.country_id
                join cities as t3 on t3.id = t1.city_id
                join villages as t4 on t4.id = t1.village_id
                join departments as t5 on t5.id = t1.department_id

                where t1.department_id = '" + Convert.ToInt32(search) + "'";


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