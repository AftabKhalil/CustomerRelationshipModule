<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditTaskAssignment.aspx.cs" Inherits="CustomerRelationshipModule.EditTaskAssignment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <form>
                    <div class="form-group">
                        <label for="name">Assignment Type</label>
                        <input class="form-control" type="text" id="assignmentType" />
                    </div>
                    <div class="form-group">
                        <label for="employee">Employee</label>
                        <select class="form-control" id="employee">
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="review">Review</label>
                        <textarea class="form-control" id="review"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="rating">Rating</label>
                        <input class="form-control" type="number" min="0" max="5" id="rating" />
                    </div>
                    <span class="btn btn-primary" onclick="SaveTaskAssignment()">Save</span>
                </form>
            </div>
        </div>
    </div>
    <script>
        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        var taskAssignmentId = sessionStorage.getItem("taskAssignmentId");

        $(document).ready(function () {
            $.ajax({
                url: "Employees.aspx/GetEmployees",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    overpassAdminCheck: true
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var x = '';
                    $(result.d.data).each(function (index, item) {
                        if (item.position != "Admin")
                            x += '<option value="' + item.id + '">' + item.name + '</option>';
                    })
                    $('#employee').html(x);
                    getTaskAssignment();
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });

        });

        function getTaskAssignment() {
            $.ajax({
                url: "EditTaskAssignment.aspx/GetTaskAssignment",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    taskAssignmentId: taskAssignmentId,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var data = result.d.data;
                    $('#assignmentType').val(data.AssignmentType);
                    $('#assignmentType').attr('disabled', true);

                    $('#employee').val(data.EmployeeId);
                    $('#employee').attr('disabled', true);

                    $('#review').val(data.Review);
                    $('#review').attr('disabled', true);

                    $('#rating').val(data.Rating);
                    $('#rating').attr('disabled', true);

                    if (currentUserType == "Admin") {
                        $('#rating').attr('disabled', false);
                    }
                    if (currentUserType == "Customer") {
                        $('#review').attr('disabled', false);
                    }

                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function SaveTaskAssignment() {
            var review = $('#review').val();
            var rating = $('#rating').val();

            $.ajax({
                url: "EditTaskAssignment.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    taskAssignmentId: taskAssignmentId,
                    review: review,
                    rating: rating,
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
