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
                        <input type="text" class="form-control" id="projectName" aria-describedby="nameHelper" placeholder="Enter Project Name" />
                    </div>
                    <div class="form-group">
                        <label for="contactNo">Budget</label>
                        <input type="text" class="form-control" id="budget" aria-describedby="contactNoHelp" placeholder="Enter budget" />
                    </div>
                    <div class="form-group">
                        <label for="password">Customer</label>
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
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        });

        if (mode == "UPDATE") {

            $.ajax({
                url: "Customer.aspx/GetCustomer",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    customerId: customerId,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var data = result.d.data;
                    $('#name').val(data.name);
                    $('#email').val(data.emailId);
                    $('#contactNo').val(data.contactNo);
                    $('#password').val(data.password);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function saveProject() {
            var projectName = $('#projectName').val();
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
