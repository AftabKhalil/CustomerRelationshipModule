<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="CustomerRelationshipModule.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <table id="Customers" class="table table-striped table-bordered table-hover Live_feed_grid_view_logs" style="width: 100%">
                    <thead>
                        <tr>
                            <th class="text-center" style="width: 10%;">Name</th>
                            <th class="text-center" style="width: 20%;">Contact No</th>
                            <th class="text-center" style="width: 10%;">Email ID</th>
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

            table = $('#Customers').DataTable({
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
                    url: "Customers.aspx/GetCustomers",
                    contentType: "application/json",
                    type: "GET",
                    dataType: "JSON",
                    data: {
                        "currentUserId": currentUserId,
                    },
                    error: function (xhr, status, error) {
                        $('#Customers_processing').hide();
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
                        

                        index = -1;
                        data = jsonData.data;
                        return jsonData.data;
                    },
                },

                columns: [
                    { data: 'name', name: 'name' },
                    { data: 'contactNo', name: 'Contact No' },
                    { data: 'emailId', name: 'Email ID' },
                   // { data: 'Project', name: 'Project' },
                    //{ data: 'password', name: 'password' }
                    {
                        data: 'id', name: 'id', render: function (data) {
                            index++;
                            return '<span class="btn btn-success" onclick="editCustomer(' + index + ')">Edit</span>&nbsp;&nbsp;<span class="btn btn-danger" onclick="deleteCustomer(' + index + ')">Delete</span>';
                        }
                    }
                   
                ]
            });
        });

        
        function editCustomer(i) {
            debugger;
            var d = data[i];
            window.location = '/Customer.aspx?customerId=' + d.systemId;
        }

        function deleteCustomer(i) {
            debugger;
            var d = data[i];

            $.ajax({
                url: "Customers.aspx/DeleteCustomer",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                    customerId: d.systemId
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