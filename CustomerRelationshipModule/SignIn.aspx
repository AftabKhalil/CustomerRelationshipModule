<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="CustomerRelationshipModule.SignIn" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="form">
            <div class="form-group">
                <label for="email">Email address</label>
                <input type="email" class="form-control" id="email" aria-describedby="emailHelp" placeholder="Enter email" />
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input type="password" class="form-control" id="password" placeholder="Password" />
            </div>
            <div class="form-group form-check">
                <input type="checkbox" class="form-check-input" id="customer" />
                <label class="form-check-label" for="exampleCheck1">I am a Customer</label>
            </div>
            <span class="btn btn-primary" onclick="getSystemId()">Submit</span>
        </div>
    </div>
    <script>

        var userSystemId;

        function getSystemId() {
            var emailId = $('#email').val();
            var password = $('#password').val();
            var isCustomer = $('#customer').prop("checked");

            $.ajax({
                url: "SignIn.aspx/GetSystemId",
                contentType: "application/json",
                type: "GET",
                dataType: "JSON",
                data: {
                    "emailId": emailId,
                    "password": password,
                    "isCustomer": isCustomer
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseJSON.Message);
                },
                success: function (response) {
                    console.log(response);
                    if (response.d.isSuccess == false) {
                        alert("Not able to SignIn");
                        sessionStorage.removeItem('currentUserId');
                        sessionStorage.removeItem('currentUserType');
                        location.reload();
                        return;
                    }
                    else {
                        currentUserId = response.d.data.currentUserId;
                        currentUserType = response.d.data.currentUserType
                        showHome(currentUserId, currentUserType);
                    }
                }
            });

            function showHome(currentUserId, currentUserType) {
                sessionStorage.setItem('currentUserId', currentUserId);
                sessionStorage.setItem('currentUserType', currentUserType);

                window.location = '/Home.aspx';
            }
        }
    </script>
</asp:Content>
