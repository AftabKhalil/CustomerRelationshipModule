<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CustomerRelationshipModule.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 Live_feed_grid_view_logs">
                <h1>Welcome : <span id="userName" class="text-success"></span></h1>
            </div>
        </div>
    </div>
    <script>
        var currentUserId = sessionStorage.getItem("currentUserId");
        var currentUserType = sessionStorage.getItem("currentUserType");
        $(document).ready(function () {
            $.ajax({
                url: "Home.aspx/GetName",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    currentUserId: currentUserId,
                    currentUserType: currentUserType,
                },
                success: function (result) {
                    console.log(result);
                    if (result.d.isSuccess == false) {
                        alert("Please SignIn first");
                        window.location = '/SignIn.aspx';
                    }
                    $('#userName').html(result.d.data);
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                    console.log(xhr);
                    return;
                },
            });
        });
    </script>
</asp:Content>
