using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace GridViewEditDemo
{
    public partial class Default : System.Web.UI.Page
    {
        public List<GridViewDemo.Book> MyBookList;
        public SortDirection SortDirection;
        public string SortColumn;

        protected void Page_Load(object sender, EventArgs e)
        {
            //get the gridview sort direction and column from the session
            if (Session["dir_" + GridView1.ID] != null)
            {
                SortDirection = (SortDirection)Session["dir_" + GridView1.ID];
            }
            if (Session["col_" + GridView1.ID] != null)
            {
                SortColumn = (string)Session["col_" + GridView1.ID];
            }

            //check if there is a list of books for this user
            if (Session["booklist"] != null)
            {
                MyBookList = (List<GridViewDemo.Book>)Session["booklist"];
            }

            //if there is no list or it is empty create one
            if (MyBookList == null || !MyBookList.Any())
            {
                MyBookList = GridViewDemo.GetBooks();
            }

            //sort the list of books
            MyBookList = GridViewDemo.SortBooks(MyBookList, SortColumn, SortDirection);

            //put the list in a session (normally MyBookList would just be a database table, not a list in a session)
            Session["booklist"] = MyBookList;

            //create the editable gridview with the postback check
            if (!IsPostBack)
            {
                BuildGridView(GridView1);
            }

            //create the gridview with the categories every time the page is loaded
            //this is needed to ensure the RowDataBound event is triggered so <thead> is added to the gridview
            //without it datatables.net will not work
            //because it is loaded every time you can set the EnableViewState to False
            BuildGridView(GridView2);
        }


        private void BuildGridView(GridView gridview)
        {
            //the books gridview
            if (gridview.ID == "GridView1")
            {
                GridView1.DataSource = MyBookList;
                GridView1.DataBind();
            }

            //the categories gridview
            if (gridview.ID == "GridView2")
            {
                GridView2.DataSource = GridViewDemo.GetBookCategories();
                GridView2.DataBind();
            }
        }


        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //check if the row is the header row (needed for datatables.net)
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //add the thead and tbody section programatically
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                //find the controls in the edit template
                var tb1 = (TextBox)e.Row.FindControl("TextBox1");
                var tb2 = (TextBox)e.Row.FindControl("TextBox2");
                var dl1 = (DropDownList)e.Row.FindControl("DropDownList1");

                //bind the categories to the dropdowlist
                dl1.DataSource = GridViewDemo.GetBookCategories();
                dl1.DataTextField = "Name";
                dl1.DataValueField = "ID";
                dl1.DataBind();

                //cast the row's dataitem back to the class Book
                var book = (GridViewDemo.Book)e.Row.DataItem;

                //set the values of the controle
                tb1.Text = book.Title;
                tb2.Text = book.Date.ToShortDateString();
                dl1.SelectedValue = book.Category.ID.ToString();
            }
        }


        protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            var gridview = (GridView)sender;

            gridview.EditIndex = e.NewEditIndex;
            BuildGridView(gridview);
        }


        protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            var gridview = (GridView)sender;

            gridview.EditIndex = -1;
            BuildGridView(gridview);
        }


        protected void GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var gridview = (GridView)sender;

            //get the id of the book from the row with DataKeyNames
            int id = Convert.ToInt32(gridview.DataKeys[e.RowIndex].Values[0]);

            //find the controls in the edit template
            var tb1 = (TextBox)gridview.Rows[e.RowIndex].FindControl("TextBox1");
            var tb2 = (TextBox)gridview.Rows[e.RowIndex].FindControl("TextBox2");
            var dl1 = (DropDownList)gridview.Rows[e.RowIndex].FindControl("DropDownList1");

            //get the book from the list
            var book = MyBookList.Where(x => x.ID == id).FirstOrDefault();

            //try to parse the date field
            DateTime date = DateTime.TryParse(tb2.Text.Trim(), out date) ? date : DateTime.Now;

            //set the values from the controls to the book
            book.Title = tb1.Text.Trim();
            book.Category = GridViewDemo.GetBookCategories().Where(x => x.ID == Convert.ToInt32(dl1.SelectedValue)).FirstOrDefault();
            book.Date = date;

            gridview.EditIndex = -1;
            BuildGridView(gridview);
        }


        protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gridview = (GridView)sender;

            //get the id of the book from the row with DataKeyNames
            int id = Convert.ToInt32(gridview.DataKeys[e.RowIndex].Values[0]);

            //remove the book from the list and save
            MyBookList.RemoveAll(x => x.ID == id);

            gridview.EditIndex = -1;
            BuildGridView(gridview);
        }


        protected void GridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            var gridview = (GridView)sender;

            //reverse the sort direction
            SortDirection = SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;

            //if the column is not the one sorted previously then always start ascending
            if (Session["col_" + GridView1.ID] != null && Session["col_" + GridView1.ID].ToString() != e.SortExpression)
            {
                SortDirection = SortDirection.Ascending;
            }

            //put the solumn and direction in the session
            Session["dir_" + GridView1.ID] = SortDirection;
            Session["col_" + GridView1.ID] = e.SortExpression;

            //re-sort the list of books
            MyBookList = GridViewDemo.SortBooks(MyBookList, e.SortExpression, SortDirection);

            BuildGridView(gridview);

            //add sort direction arrows to the clicked header cell
            for (int i = 1; i < gridview.HeaderRow.Cells.Count - 1; i++)
            {
                var linkbutton = (LinkButton)gridview.HeaderRow.FindControl("LinkButton" + i);

                if (linkbutton.Text == e.SortExpression && SortDirection == SortDirection.Descending)
                {
                    linkbutton.Text += "&nbsp; &uarr;";
                }
                else if (linkbutton.Text == e.SortExpression)
                {
                    linkbutton.Text += "&nbsp; &darr;";
                }
            }
        }
    }
}