<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TaskAssignmnet.aspx.cs" Inherits="CustomerRelationshipModule.TaskAssignmnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .dt-buttons {
            display: contents !important;
        }

        .add-assignment {
            padding: 5px 15px 5px 15px !important;
            margin-right: 10px !important;
        }
    </style>
    <div class="container">
        <h3>Task assignments for task id <span id="taskId"></span></h3>
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <table id="TaskAssignment" class="table table-striped table-bordered table-hover Live_feed_grid_view_logs" style="width: 100%">
                    <thead>
                        <tr>
                            <th class="text-center" style="width: 10%;">Assignment Type</th>
                            <th class="text-center" style="width: 10%;">Employee Id</th>
                            <th class="text-center" style="width: 10%;">Employee Name</th>
                            <th class="text-center" style="width: 40%;">Review</th>
                            <th class="text-center" style="width: 10%;">Rating</th>
                            <th class="text-center" style="width: 20%;">Actions</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <div class="row" id="addAssignmentDiv">
            <h3>Add new assignment</h3>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <form class="form-inline">
                    <div class="form-group" style="padding-right: 20px;">
                        <label for="name">Assignment Type :&nbsp;</label>
                        <select class="form-control" id="assignmentType">
                            <option value="1">Development</option>
                            <option value="2">SQA</option>
                            <option value="3">ProjectManagement</option>
                        </select>
                    </div>
                    <div class="form-group" style="padding-right: 20px;">
                        <label for="employee">Employee :&nbsp;</label>
                        <select class="form-control" id="employee">
                        </select>
                    </div>
                    <div class="form-group">
                        <span class="btn btn-primary" onclick="addAssignment()">Submit</span>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <script>
        var currentUserId, currentUserType;
        var data, index = -1;
        var table;
        var taskId = sessionStorage.getItem("taskId");

        $('#taskId').text(taskId);

        $(document).ready(function () {
            currentUserId = sessionStorage.getItem("currentUserId");
            currentUserType = sessionStorage.getItem("currentUserType");

            if (currentUserType != "Admin")
                $("#addAssignmentDiv").fadeOut();

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
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });

            table = $('#TaskAssignment').DataTable({
                "columnDefs": [
                    { "className": "text-center custom-middle-align", "targets": "_all" },
                ],

                //CHANGE TEXT OF show xx entries TO -> Latest: xx (https://stackoverflow.com/questions/27788778/rename-show-xx-entries-drop-down-in-datatables)
                "oLanguage": {
                    "sLengthMenu": "Latest: _MENU_",
                    "sProcessing": "<div class='overlay custom-loader-background'><i class='fa fa-spinner fa-spin custom-loader-color'></i></div>"
                },

                "processing": true,
                "serverSide": false,
                "ordering": true,
                "info": true,
                "pagging": true,

                //ALIGN PAGE SIZE TO LEFT, AND SEARCH BOX TO RIGHT OF PAGE
                //https://datatables.net/examples/basic_init/dom.html
                "dom": '<"row"<"col-lg-6 col-md-6 col-sm-6 col-xs-6 text-left"lr><"col-lg-6 col-md-6 col-sm-6 col-xs-6 text-right"Bf>>tip',

                ajax: {
                    url: "TaskAssignmnet.aspx/GetTaskAssignment",
                    contentType: "application/json",
                    type: "GET",
                    dataType: "JSON",
                    data: {
                        "currentUserId": currentUserId,
                        "currentUserType": currentUserType,
                        "taskId": taskId,
                    },
                    error: function (xhr, status, error) {
                        $('#TaskAssignment_processing').hide();
                        alert(xhr.responseText);
                        return;
                    },
                    dataSrc: function (json) {
                        var jsonData = json.d;
                        if (jsonData.isSuccess == false) {
                            alert(jsonData.error);
                            window.location = '/SignIn.aspx';
                            return;
                        }
                        json.draw = jsonData.draw;
                        json.recordsTotal = jsonData.recordsTotal;
                        json.recordsFiltered = jsonData.recordsFiltered;
                        json.data = jsonData.data;

                        //This variable will be used on edit and delete button
                        index = -1;
                        data = jsonData.data;
                        return jsonData.data;
                    },
                },

                columns: [
                    { data: 'AssignmentType', name: 'AssignmentType' },
                    { data: 'EmployeeId', name: 'EmployeeId' },
                    { data: 'EmployeeName', name: 'EmployeeName' },
                    { data: 'Review', name: 'Review' },
                    { data: 'Rating', name: 'Rating' },
                    {
                        data: 'Id', name: 'Id', render: function (data) {
                            index++;
                            var actions = '---';
                            if (currentUserType == 'Admin')
                                actions = '<span class="btn btn-success" onclick="editTaskAssignmnet(' + index + ')">Edit</span>&nbsp;&nbsp;<span class="btn btn-danger" onclick="deleteTaskAssignmnet(' + index + ')">Delete</span>';
                            else
                                actions = '<span class="btn btn-success" onclick="editTaskAssignmnet(' + index + ')">Edit</span>';
                            return actions;
                        }
                    }
                ]
            });
        });

        function addAssignment() {
            var employeeId = $('#employee').val();
            var assignmentType = $('#assignmentType').val();

            $.ajax({
                url: "TaskAssignmnet.aspx/Save",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    taskId: taskId,
                    employeeId: employeeId,
                    assignmentType: assignmentType,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    table.ajax.reload();
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        }

        function editTaskAssignmnet(i) {
            var d = data[i];
            sessionStorage.setItem('taskAssignmentId', d.Id);
            window.location = '/EditTaskAssignment.aspx';
        }

    </script>
</asp:Content>
