<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddTask.aspx.cs" Inherits="CustomerRelationshipModule.AddTask" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <form>
                    <div class="form-group">
                        <label for="name">Task Name</label>
                        <input type="text" class="form-control" id="taskName" aria-describedby="nameHelper" placeholder="Enter Task Name" />
                    </div>
                    <div class="form-group">
                        <label for="project">Project</label>
                        <select class="form-control" id="project">
                        </select>
                    </div>
                    <span class="btn btn-primary" onclick="saveTask()">Submit</span>
                </form>
            </div>
        </div>
    </div>
    <script>
        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        var taskId = params.taskId;
        var mode = (taskId == null || taskId == "") ? "CREATE" : "UPDATE";

        $(document).ready(function () {
            debugger;
            $.ajax({
                url: "Projects.aspx/GetProjects",
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
                        x += '<option value="' + item.ID + '">' + item.Name + '</option>';
                    })
                    $('#project').html(x);
                    if (mode == "UPDATE") getTask();
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        });

        function getTask() {
            $.ajax({
                url: "AddTask.aspx/GetTask",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    taskId: taskId,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    var data = result.d.data;
                    $('#taskName').val(data.Name);
                    $('#project').val(data.ProjectId);
                    $('#project').attr('disabled', true);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function saveTask() {
            var taskName = $('#taskName').val();
            var project = $('#project').val();

            $.ajax({
                url: "AddTask.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    taskName: taskName,
                    project: project,
                    taskId: taskId,
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
