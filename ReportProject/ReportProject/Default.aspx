<%@ Page Title="OneZero" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ReportProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .button {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
    </style>
    <div class="jumbotron">
        <div class="column">

            <table>
                <tr>
                    <td>
                        <h3>Ekstre Dosyası</h3>
                        <asp:FileUpload ID="FileUploadId" runat="server" ForeColor="Black" AllowMultiple="false" Font-Size="Small" Font-Bold="true" ToolTip="Belge Yüklemek İçin lütfen butona basınız!" />
                        <br />
                        <asp:Button ID="btnUpload" runat="server" class="button" Font-Size="Small" Text="Ekstre Dosyası Yükle" OnClick="btnUpload_Click" />
                    </td>
                    <td>
                        <h3>Formül Dosyası</h3>
                        <asp:FileUpload ID="FileUpload1" runat="server" ForeColor="Black" AllowMultiple="false" Font-Size="Small" Font-Bold="true" ToolTip="Belge Yüklemek İçin lütfen butona basınız!" />
                        <br />
                        <asp:Button ID="btnUploadFormul" runat="server" class="button" Font-Size="Small" Text="Formül Dosyası Yükle" OnClick="btnUploadFormul_Click" />

                    </td>
                </tr>
            </table>


        </div>

    </div>

    <div class="jumbotron">
        <asp:Button runat="server" ID="btnReport" class="btn btn-primary btn-lg" Text="Rapor İndir" OnClick="btnReport_Click" CausesValidation="false"></asp:Button>
    </div>
    <%--   <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>--%>
</asp:Content>
