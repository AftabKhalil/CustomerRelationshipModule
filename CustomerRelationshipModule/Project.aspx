<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Project.aspx.cs" Inherits="CustomerRelationshipModule.Project" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <form>
                    <div class="form-group">
                        <label for="name">Project Name</label>
                        <input type="text" class="form-control" id="name" aria-describedby="nameHelper" placeholder="Enter Project Name" />
                    </div>
                    <div class="form-group">
                        <label for="budget">Budget</label>
                        <input type="text" class="form-control" id="budget" aria-describedby="contactNoHelp" placeholder="Enter budget" />
                    </div>
                    <div class="form-group">
                        <label for="customer">Customer</label>
                        <select class="form-control" id="customer">
                        </select>
                    </div>
                    <span class="btn btn-primary" onclick="saveProject()">Submit</span>
                </form>
            </div>
        </div>
    </div>
    <script>

        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        var projectId = params.projectId;
        var mode = (projectId == null || projectId == "") ? "CREATE" : "UPDATE";


        $(document).ready(function () {
            $.ajax({
                
                url: "Customers.aspx/GetCustomers",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var x = '';
                    $(result.d.data).each(function (index, item) {
                        x += '<option value="' + item.id + '">' + item.name + '</option>';
                    })
                    $('#customer').html(x);
                    if (mode == "UPDATE") getCustomer();
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        });

        function getCustomer(){
            $.ajax({
                url: "Project.aspx/GetProject",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    projectId: projectId,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var data = result.d.data;
                    debugger;
                    $('#name').val(data.Name);
                    $('#budget').val(data.Budget);
                    $('#customer').val(data.CustomerId);
                    $('#customer').attr('disabled', true);
                   
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function saveProject() {
            var projectName = $('#name').val();
            var budget = $('#budget').val();
            var customer = $('#customer').val();

            $.ajax({
                url: "Project.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    projectName: projectName,
                    budget: budget,
                    customer: customer,
                    projectId: projectId,
                    mode: mode,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    alert(result.d.data);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }
    </script>
</asp:Content>
