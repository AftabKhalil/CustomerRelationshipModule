<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Employee.aspx.cs" Inherits="CustomerRelationshipModule.Employee" %>

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
                        <label for="expirence">expirence</label>
                        <input type="number" min="0" class="form-control" id="expirence" aria-describedby="expirenceHelp" placeholder="Enter expirence" />
                    </div>
                    <div class="form-group">
                        <label for="position">position</label>
                        <select class="form-control" id="position">
                            <option value="1">Admin</option>
                            <option value="2">Developer</option>
                            <option value="3">Projectmanager</option>
                            <option value="4">SQA</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="salary">salary</label>
                        <input type="number" min="0" class="form-control" id="salary" aria-describedby="salaryHelp" placeholder="Enter salary" />
                    </div>
                    <div class="form-group">
                        <label for="password">Password</label>
                        <input type="password" class="form-control" id="password" placeholder="Password" />
                    </div>
                    <span class="btn btn-primary" onclick="saveEmployee()">Submit</span>
                </form>
            </div>
        </div>
    </div>
    <script>

        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        var employeeId = params.employeeId;
        var mode = (employeeId == null || employeeId == "") ? "CREATE" : "UPDATE";

        if (mode == "UPDATE") {
            $.ajax({
                url: "Employee.aspx/GetEmployee",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    employeeId: employeeId,
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
                    $('#expirence').val(data.expirence);
                    $('#position').val(data.position);
                    $('#salary').val(data.salary);
                    $('#password').val(data.password);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function saveEmployee() {
            var name = $('#name').val();
            var email = $('#email').val();
            var contactNo = $('#contactNo').val();
            var expirence = $('#expirence').val();
            var position = $('#position').val();
            var salary = $('#salary').val();
            var password = $('#password').val();

            $.ajax({
                url: "Employee.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    name: name,
                    email: email,
                    contactNo: contactNo,
                    expirence: expirence,
                    position: position,
                    salary: salary,
                    password: password,
                    mode: mode,
                    employeeId: employeeId,
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
