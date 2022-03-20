<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Employees.aspx.cs" Inherits="CustomerRelationshipModule.Employees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <table id="Employees" class="table table-striped table-bordered table-hover Live_feed_grid_view_logs" style="width: 100%">
                    <thead>
                        <tr>
                            <th class="text-center" style="width: 10%;">Name</th>
                            <th class="text-center" style="width: 20%;">Position</th>
                            <th class="text-center" style="width: 10%;">Salary</th>
                            <th class="text-center" style="width: 10%;">Experience</th>
                            <th class="text-center" style="width: 10%;">Contact No</th>
                            <th class="text-center" style="width: 10%;">Eamil Id</th>
                            <th class="text-center" style="width: 20%">Action</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>

    <script>
        var currentUserId, currentUserType;
        var data, index = -1;
        var table;
        $(document).ready(function () {
            currentUserId = sessionStorage.getItem("currentUserId");
            currentUserType = sessionStorage.getItem("currentUserType");

            table = $('#Employees').DataTable({
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
                "dom": 'lrftip',

                ajax: {
                    url: "Employees.aspx/GetEmployees",
                    contentType: "application/json",
                    type: "GET",
                    dataType: "JSON",
                    data: {
                        "currentUserId": currentUserId,
                    },
                    error: function (xhr, status, error) {
                        $('#Employees_processing').hide();
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
                    { data: 'name', name: 'name' },
                    { data: 'position', name: 'position' },
                    { data: 'salary', name: 'salary' },
                    { data: 'expirence', name: 'expirence' },
                    { data: 'contactNo', name: 'contactNo' },
                    { data: 'emailId', name: 'emailId' },
                    {
                        data: 'id', name: 'id', render: function (data) {
                            index++;
                            return '<span class="btn btn-success" onclick="editEmployee(' + index + ')">Edit</span>&nbsp;&nbsp;<span class="btn btn-danger" onclick="deleteEmployee(' + index + ')">Delete</span>';
                        }
                    }
                ]
            });
        });

        function editEmployee(i) {
            var d = data[i];
            window.location = '/Employee.aspx?employeeId=' + d.systemId;
        }

        function deleteEmployee(i) {
            var d = data[i];

            $.ajax({
                url: "Employees.aspx/DeleteEmployee",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    employeeId: d.systemId
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert(result.d.error);
                        return;
                    }
                    alert(result.d.data);
                    table.ajax.reload();
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
