<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CaseNotesAvailability._default" %>

<%--<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="HealthRecordsView" runat="server" SelectMethod="GetAudit" UpdateMethod="UpdateAuditRecords" OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditBLL" DataObjectTypeName="BusinessObjects.AuditBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <div>
        <dx:ASPxGridView ID="HealthRecordsGridView" runat="server" AllowSorting="true"
            ClientInstanceName="HealthRecordsGridView" KeyFieldName="AuditID" DataSourceID="HealthRecordsView"
            AutoGenerateColumns="False" Width="100%">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>
            <SettingsEditing EditFormColumnCount="2"></SettingsEditing>
            <SettingsPopup>
                <FilterControl AutoUpdatePosition="False"></FilterControl>
            </SettingsPopup>
            <Styles>
                <EditingErrorRow BackColor="Yellow" />
            </Styles>

            <EditFormLayoutProperties>
                <Items>
                    <dx:GridViewLayoutGroup Name="FieldGroup" Caption="Health Records View" ColCount="2" ColumnCount="2" ColSpan="1" ColumnSpan="1">
                        <Items>
                            <dx:GridViewColumnLayoutItem ColumnName="AuditID" Width="500" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem ColumnName="Date" Width="300" Height="30" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="Specilaties Name" Width="300" ColumnName="Specialities" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="SiteID" ColumnName="SiteID" Width="500" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem ColumnName="ClinicCodes" Width="400" Height="30" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="CreatedByUserID"  Visible ="false" ColumnName="CreatedByUserID" Width="500" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="CompletedByUserID" Visible ="false" ColumnName="CompletedByUserID" Width="500" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="DueByDate" ColumnName="AuditDate" Width="300" ColSpan="1"></dx:GridViewColumnLayoutItem>
                            <dx:EditModeCommandLayoutItem ColSpan="1"></dx:EditModeCommandLayoutItem>
                        </Items>

                    </dx:GridViewLayoutGroup>
                </Items>
            </EditFormLayoutProperties>

            <%--   <SettingsDataSecurity AllowInsert="false" />
        <EditFormLayoutProperties>
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
        </EditFormLayoutProperties>
        <SettingsPopup>
            <EditForm Width="600">
                <SettingsAdaptivity Mode="OnWindowInnerWidth" SwitchAtWindowInnerWidth="768" />
            </EditForm>
        </SettingsPopup>--%>

            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" Width="100px" Caption="Edit" ShowNewButtonInHeader="true" ShowEditButton="true" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn Caption="AuditID" ReadOnly="true" FieldName="AuditID" VisibleIndex="1" MinWidth="50" MaxWidth="100"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="Date" PropertiesDateEdit-ClientInstanceName="AuditDate" FieldName="Date" VisibleIndex="2" MinWidth="50" MaxWidth="100" PropertiesDateEdit-ValidationSettings-Display="Dynamic" PropertiesDateEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesDateEdit-ValidationSettings-RequiredField-ErrorText="Please enter a date"></dx:GridViewDataDateColumn>
                <dx:GridViewDataComboBoxColumn Caption="Specialities" PropertiesComboBox-ClientInstanceName="Specialities" FieldName="SpecialtyID" VisibleIndex="3" MinWidth="200" MaxWidth="500" PropertiesComboBox-ValidationSettings-Display="Dynamic" PropertiesComboBox-ValidationSettings-RequiredField-IsRequired="true" PropertiesComboBox-ValidationSettings-RequiredField-ErrorText="Please enter Application Type">
                    <PropertiesComboBox DataSourceID="GetSpeciality" TextField="SpecilatiesName" ValueField="SpecilatiesID"></PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataComboBoxColumn Caption="Sites" SettingsHeaderFilter-DateRangeCalendarSettings-ShowClearButton="true" PropertiesComboBox-ClearButton-DisplayMode="OnHover" FieldName="SiteID" VisibleIndex="4" MinWidth="200" MaxWidth="400">
                    <PropertiesComboBox DataSourceID="GetSites" TextField="SiteName" ValueField="SiteID"></PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataDateColumn Caption="Date" PropertiesDateEdit-ClientInstanceName="AuditDate" FieldName="Date" VisibleIndex="5" MinWidth="200" MaxWidth="200" PropertiesDateEdit-ValidationSettings-Display="Dynamic" PropertiesDateEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesDateEdit-ValidationSettings-RequiredField-ErrorText="Please enter a date"></dx:GridViewDataDateColumn>
                <dx:GridViewDataMemoColumn FieldName="ClinicCodes" VisibleIndex="6" PropertiesMemoEdit-MaxLength="255" MinWidth="200" MaxWidth="500" PropertiesMemoEdit-ValidationSettings-Display="Dynamic" PropertiesMemoEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesMemoEdit-ValidationSettings-RequiredField-ErrorText="Please enter Clinic codes"></dx:GridViewDataMemoColumn>
                <dx:GridViewDataTextColumn Caption="CreatedByUserID" Visible ="false" ReadOnly="true" FieldName="AuditID" VisibleIndex="7" MinWidth="50" MaxWidth="100"></dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn Caption="DueByDate" PropertiesDateEdit-ClientInstanceName="DueByDate" FieldName="Date" VisibleIndex="8" MinWidth="50" MaxWidth="100" PropertiesDateEdit-ValidationSettings-Display="Dynamic" PropertiesDateEdit-ValidationSettings-RequiredField-IsRequired="true" PropertiesDateEdit-ValidationSettings-RequiredField-ErrorText="Please enter a date"></dx:GridViewDataDateColumn>
            </Columns>
            <Settings ShowFilterRow="true" />
            <SettingsBehavior AllowEllipsisInText="true" />
            <SettingsResizing ColumnResizeMode="NextColumn" />
        </dx:ASPxGridView>

    </div>
    <%--<uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />--%>
</asp:Content>
