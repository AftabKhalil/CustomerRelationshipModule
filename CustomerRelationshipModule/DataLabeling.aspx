<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DataLabeling.aspx.cs" Inherits="CustomerRelationshipModule.DataLabeling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="form">
            <div class="form-group">
                <label for="uploadFile">For Labeling : </label>
                <asp:Button ID="Button2" runat="server" CssClass="btn btn-success" Text="Download Data" OnClick="Button2_Click" />
            </div>
            <div class="form-group">
                <label for="uploadFile">After Labeling : </label>
                <asp:FileUpload ID="FileUpload1" runat="server" ToolTip="Select Only Excel File" />
                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Upload" OnClick="Button1_Click" />
                <br />
                <asp:Label ID="Label1" runat="server" Text="label"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
