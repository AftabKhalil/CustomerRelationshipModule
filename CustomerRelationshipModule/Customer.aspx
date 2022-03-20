<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customer.aspx.cs" Inherits="CustomerRelationshipModule.Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <form>
                    <div class="form-group">
                        <label for="name">Name</label>
                        <input type="text" class="form-control" id="name" aria-describedby="nameHelper" placeholder="Enter Name" />
                    </div>
                    <div class="form-group">
                        <label for="email">Email address</label>
                        <input type="email" class="form-control" id="email" aria-describedby="emailHelp" placeholder="Enter email" />
                    </div>
                    <div class="form-group">
                        <label for="contactNo">Contact No</label>
                        <input type="text" class="form-control" id="contactNo" aria-describedby="contactNoHelp" placeholder="Enter contactNo" />
                    </div>
                    <div class="form-group">
                        <label for="password">Password</label>
                        <input type="password" class="form-control" id="password" placeholder="Password" />
                    </div>
                    <span class="btn btn-primary" onclick="savecustomer()">Submit</span>
                </form>
            </div>
        </div>
    </div>
    <script>

        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        var customerId = params.customerId;
        var mode = (customerId == null || customerId == "") ? "CREATE" : "UPDATE";

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

        function savecustomer() {
            var name = $('#name').val();
            var email = $('#email').val();
            var contactNo = $('#contactNo').val();
            var password = $('#password').val();

            $.ajax({
                url: "Customer.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    name: name,
                    email: email,
                    contactNo: contactNo,
                    password: password,
                    mode: mode,
                    customerId: customerId,
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
