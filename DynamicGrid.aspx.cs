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
    public partial class DynamicGrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            //throw new NotImplementedException();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.AddRange(new DataColumn[3] {new DataColumn("sno",typeof(int)),
                                                new DataColumn("Name",typeof(string)),
                                                new DataColumn("Phone",typeof(string)) });
            dr = dt.NewRow();
            dr["sno"] = 1;
            dr["name"] = string.Empty;
            dr["phone"] = string.Empty;
            dt.Rows.Add(dr);
            //dt.Rows.Add(1, string.Empty, string.Empty);
            ViewState["datatable"] = dt;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void addnew_Click(object sender, EventArgs e)
        {
            if (ViewState["datatable"] != null)
            {
                DataTable dt = (DataTable)ViewState["datatable"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["sno"] = dt.Rows.Count + 1;
                    dt.Rows.Add(dr);
                    ViewState["datatable"] = dt;

                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                        TextBox t1 = (TextBox)GridView1.Rows[i].Cells[1].FindControl("name");
                        TextBox t2 = (TextBox)GridView1.Rows[i].Cells[2].FindControl("phone");
                        dt.Rows[i]["Name"] = t1.Text;
                        dt.Rows[i]["Phone"] = t2.Text;
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            else
            {
                Response.Write("Viewstate is null");
            }
            setdata();
        }

        private void setdata()
        {
            if (ViewState["datatatble"] != null)
            {
                DataTable dt = (DataTable)ViewState["datatable"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox t11 = (TextBox)GridView1.Rows[i].Cells[1].FindControl("name");
                        TextBox t12 = (TextBox)GridView1.Rows[i].Cells[2].FindControl("phone");

                        if (i < dt.Rows.Count - 1)
                        {
                            t11.Text = dt.Rows[i]["Name"].ToString();
                            t12.Text = dt.Rows[i]["Phone"].ToString();

                        }
                    }
                }

            }
        }
    }
}