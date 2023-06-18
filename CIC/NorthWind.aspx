<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NorthWind.aspx.cs" Inherits="CIC.NorthWind" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        table 
        {
        border-collapse: collapse;
        width: 100%;
        }

    table, th, td 
    {
        border: 1px solid black;
    }
    </style>
    <table>
    <asp:Repeater ID="OrderRepeater" runat="server">
        <HeaderTemplate>
                <tr>
                    <th>OrderID</th>
                    <th>CustomerID</th>
                    <th>EmployeeID</th>
                    <th>OrderDate</th>
                    <th>RequiredDate</th>
                    <th>ShippedDate</th>
                    <th>ShipVia</th>
                    <th>Freight</th>
                    <th>ShipName</th>
                    <th>ShipAddress</th>
                    <th>ShipCity</th>
                    <th>ShipRegion</th>
                    <th>ShipPostalCode</th>
                    <th>ShipCountry</th>
                    <th>狀態</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# Eval("OrderID") %></td>
                <td><%# Eval("CustomerID") %></td>
                <td><%# Eval("EmployeeID") %></td>
                <td><%# Eval("OrderDate", "{0:yyyy-MM-dd}") %></td>
                <td><%# Eval("RequiredDate", "{0:yyyy-MM-dd}") %></td>
                <td><%# Eval("ShippedDate", "{0:yyyy-MM-dd}") %></td>
                <td><%# Eval("ShipVia") %></td>
                <td><%# Eval("Freight","{0:F2}") %></td>
                <td><%# Eval("ShipName") %></td>
                <td><%# Eval("ShipAddress") %></td>
                <td><%# Eval("ShipCity") %></td>
                <td><%# Eval("ShipRegion") %></td>
                <td><%# Eval("ShipPostalCode") %></td>
                <td><%# Eval("ShipCountry") %></td>
                <td><%# Eval("ReadDate") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
</asp:Content>