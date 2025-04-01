<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="RelatorioAtivosPowerBI.aspx.cs" Inherits="Relatorios_RelatorioAtivosRealizados"
    Title="Call HSPM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
   <iframe title="hspmCallPowerBIEstatico" width="600" height="373.5" src="https://app.powerbi.com/view?r=eyJrIjoiMjE5Mzc1ZDktNmYyMy00NDc5LWJkMjMtMjQyZWNmZDE2YzRiIiwidCI6ImYzOThkZjljLWZkMGMtNDgyOS1hMDAzLWM3NzBhMWM0YTA2MyJ9&embedImagePlaceholder=true" frameborder="0" allowFullScreen="true"></iframe>
</asp:Content>