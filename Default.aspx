<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GridViewEditDemo.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <title>VDWWD - GridView Edit & Update Demo</title>

    <!-- style sheets -->

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.7.0/css/bootstrap-datepicker.min.css" />

    <style>
        body {
            background-color: #4b4d66;
        }

        /* to make the tables slightly more compact */
        .btn {
            padding: 3px 10px;
        }

        .table th,
        .table td,
        table.dataTable thead th,
        table.dataTable tbody td {
            padding: 4px;
            vertical-align: middle;
        }

        .table th {
            background-color: #adadad;
            border-top: 0px;
            border-bottom: 1px solid #808080 !important;
        }

            .table th a {
                color: black;
                /* by making the header link a block you can click the entire cell */
                display: block;
            }
    </style>

</head>
<body>

    <!-- van der Waal Webdesign -->
    <!-- https://www.vanderwaal.eu -->

    <form id="form1" runat="server">

        <!-- When using ScriptManager with target framework 4.8 or higher, the validators on the page will NOT prevent a PostBack... -->

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <!-- Used an updatepanel because many people are having problems making something like datepickers, datatables, masonry, autocomplete jqery functions work after Partial PostBack -->

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="container">

                    <div class="row">
                        <div class="col p-3">

                            <!-- This GridView is editable -->

                            <asp:GridView ID="GridView1"
                                runat="server"
                                CssClass="table table-striped"
                                BackColor="White"
                                AutoGenerateColumns="false"
                                EmptyDataText="No books found."
                                EmptyDataRowStyle-CssClass="font-weight-bold"
                                DataKeyNames="ID"
                                AllowSorting="True"
                                ItemType="GridViewEditDemo.Classes.GridViewDemo.Book"
                                OnRowDataBound="GridView_RowDataBound"
                                OnRowEditing="GridView_RowEditing"
                                OnRowCancelingEdit="GridView_RowCancelingEdit"
                                OnRowUpdating="GridView_RowUpdating"
                                OnRowDeleting="GridView_RowDeleting"
                                OnSorting="GridView_Sorting">
                                <Columns>
                                    <asp:TemplateField HeaderText="ID">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="ID" CommandName="Sort" CommandArgument="ID"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <!-- You can use "Item" to access properties because the GridView is Strongly Typed (set the ItemType of the GridView) -->

                                            <%# Item.ID %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Title">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Title" CommandName="Sort" CommandArgument="Title"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <%# Item.Title %>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <asp:TextBox ID="TextBox1" runat="server" MaxLength="50" CssClass="form-control form-control-sm"></asp:TextBox>

                                            <!-- Make a ValidationGroup per editable GridView. Make sure the "Update" button also has it -->

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="EditGridView" Display="Dynamic"
                                                ControlToValidate="TextBox1" CssClass="text-danger font-weight-bold" ErrorMessage="Title is required.">
                                            </asp:RequiredFieldValidator>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Category">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton3" runat="server" Text="Category" CommandName="Sort" CommandArgument="Category"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <%# Item.Category.Name %>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" CssClass="form-control form-control-sm">
                                                <asp:ListItem Text="Select..." Value=""></asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="EditGridView" Display="Dynamic"
                                                ControlToValidate="DropDownList1" CssClass="text-danger font-weight-bold" ErrorMessage="Category is required.">
                                            </asp:RequiredFieldValidator>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="LinkButton4" runat="server" Text="Date" CommandName="Sort" CommandArgument="Date"></asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <%# Item.Date.ToLongDateString() %>
                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <asp:TextBox ID="TextBox2" runat="server" MaxLength="10" CssClass="form-control form-control-sm bootstrap-datepicker" placeholder="dd/mm/yyyy"></asp:TextBox>

                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="EditGridView" Display="Dynamic"
                                                ControlToValidate="TextBox2" CssClass="text-danger font-weight-bold" ErrorMessage="Date is required.">
                                            </asp:RequiredFieldValidator>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30">
                                        <ItemTemplate>

                                            <!-- do not change the CommandName -->

                                            <asp:LinkButton runat="server" CommandName="Edit" CssClass="btn btn-primary">Edit</asp:LinkButton>

                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <!-- do not change the CommandName -->

                                            <asp:LinkButton runat="server" CommandName="Update" CssClass="btn btn-success" ValidationGroup="EditGridView">Update</asp:LinkButton>

                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" HeaderStyle-Width="30">
                                        <ItemTemplate>

                                            <!-- do not change the CommandName -->

                                            <asp:LinkButton runat="server" CommandName="Delete" CssClass="btn btn-danger"
                                                OnClientClick='<%# "return confirm(\u0027Are you sure you want to delete the book \"" + Item.Title + "\"?\u0027)" %>'>
                                                        Delete
                                            </asp:LinkButton>

                                        </ItemTemplate>
                                        <EditItemTemplate>

                                            <!-- do not change the CommandName -->

                                            <asp:LinkButton runat="server" CommandName="Cancel" CssClass="btn btn-secondary">Cancel</asp:LinkButton>

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col p-3">

                            <!-- This GridView uses the datatables -->

                            <asp:GridView ID="GridView2"
                                runat="server"
                                CssClass="table table-striped w-50 datatable"
                                BackColor="White"
                                AutoGenerateColumns="true"
                                ItemType="GridViewEditDemo.Classes.GridViewDemo.BookCategory"
                                OnRowDataBound="GridView_RowDataBound"
                                EnableViewState="false">
                            </asp:GridView>

                        </div>
                    </div>

                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </form>

    <!-- script libraries -->

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.9/js/jquery.dataTables.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.7.0-RC3/js/bootstrap-datepicker.min.js"></script>

    <script type="text/javascript">
        var $datatable;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        //this code is executed after the UpdatePanel is finished loading. You will need to trigger all scripts again that have bindings to DOM elements within the UpdatePanel.
        prm.add_endRequest(function () {
            InitDataTable();
            InitDatePicker();
        });

        $(document).ready(function () {
            InitDataTable();
        });

        //create the datatables
        //datatables is not used for the editable gridview because when you go in edit mode the sortorder changes,
        //and the row the user clicked to edit may change position or dissapear entirely when using paging.
        function InitDataTable() {

            //if the datatable exists, destroy it first
            if ($datatable != null) {
                $datatable.destroy();
            }

            $datatable = $('.datatable').DataTable({
                'stateSave': true, //to make sure the current sorting survives across postbacks
                'searching': false,
                'paging': false,
                'info': false
            });
        }

        //create the datepickers
        function InitDatePicker() {
            $('.bootstrap-datepicker').datepicker({
                dateFormat: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            });
        }

    </script>
</body>
</html>
