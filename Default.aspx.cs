using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace GridViewEditDemo
{
    public partial class Default : System.Web.UI.Page
    {
        public List<Classes.Books.Book> MyBookList;

        protected void Page_Load(object sender, EventArgs e)
        {
            //load your books list from your own source like a database
            MyBookList = Classes.Data.GetMyBooks();

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
                GridView2.DataSource = Classes.Books.GetBookCategories();
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
                dl1.DataSource = Classes.Books.GetBookCategories();
                dl1.DataTextField = "Name";
                dl1.DataValueField = "ID";
                dl1.DataBind();

                //cast the row's dataitem back to the class Book
                var book = (Classes.Books.Book)e.Row.DataItem;

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
            book.Category = Classes.Books.GetBookCategories().Where(x => x.ID == Convert.ToInt32(dl1.SelectedValue)).FirstOrDefault();
            book.Date = date;

            //save the list
            Classes.Data.SaveMyBooks(MyBookList);

            //OR save the individual book
            book.Save();

            gridview.EditIndex = -1;
            BuildGridView(gridview);
        }


        protected void GridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var gridview = (GridView)sender;

            //get the id of the book from the row with DataKeyNames
            int id = Convert.ToInt32(gridview.DataKeys[e.RowIndex].Values[0]);

            //remove the book from the list en then save the list
            MyBookList.RemoveAll(x => x.ID == id);
            Classes.Data.SaveMyBooks(MyBookList);

            //OR save the individual book
            var book = MyBookList.Where(x => x.ID == id).FirstOrDefault();
            book.Delete();

            gridview.EditIndex = -1;
            BuildGridView(gridview);
        }
    }
}