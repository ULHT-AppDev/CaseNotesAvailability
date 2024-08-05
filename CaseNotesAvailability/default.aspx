<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CaseNotesAvailability._default" %>

<%--<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CustomCSS/NotificationHelper.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="HealthRecordsView" runat="server" SelectMethod="GetAudit" UpdateMethod="UpdateAuditRecords"
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditBLL" DataObjectTypeName="BusinessObjects.AuditBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <div>
        <dx:ASPxGridView ID="HealthRecordsGridView" runat="server" AllowSorting="true"
            ClientInstanceName="HealthRecordsGridView"
            OnRowUpdating="AuditRow_Updating"
            KeyFieldName="AuditID"
            DataSourceID="HealthRecordsView"
            AutoGenerateColumns="False"
            OnInitNewRow="HealthRecordsGridView_InitNewRow"
            OnStartRowEditing="HealthRecordsGridView_StartRowEditing"
             OnCellEditorInitialize="HealthRecordsGridView_CellEditorInitialize"
            Width="100%">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>

            <SettingsEditing EditFormColumnCount="2"></SettingsEditing>

            <SettingsPopup>
                <FilterControl AutoUpdatePosition="False"></FilterControl>
            </SettingsPopup>

            <Styles>
                <EditingErrorRow BackColor="Yellow" />
            </Styles>

            <EditFormLayoutProperties AlignItemCaptionsInAllGroups="true" AlignItemCaptions="true" LeftAndRightCaptionsWidth="125" Styles-LayoutGroup-CssClass="p-3">
                <Styles>
                    <LayoutGroupBox>
                        <Caption Font-Bold="true" Font-Size="18px"></Caption>
                    </LayoutGroupBox>
                </Styles>
                <Items>
                    <dx:GridViewLayoutGroup Name="FieldGroup" Caption="Health Records View" ColCount="2" ColumnCount="2" ColSpan="1" ColumnSpan="1">
                        <Items>
                            <dx:GridViewColumnLayoutItem ColumnName="AuditID" Name="AuditID" Caption="Audit ID" Width="100%" ColSpan="2"></dx:GridViewColumnLayoutItem>

                            <dx:GridViewColumnLayoutItem ColumnName="Date" Caption="Audit Start Date" ColSpan="2" Width="400px"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="Audit Due By Date" ColumnName="DueByDate" ColSpan="2" Width="400px"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="Site" ColumnName="SiteID" ColSpan="2" Width="400px"></dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem Caption="Speciality" ColumnName="SpecialtyID" ColSpan="2" Width="400px"></dx:GridViewColumnLayoutItem>

                            <dx:GridViewColumnLayoutItem Caption="" Width="100%">
                                <Template>
                                    <dx:ASPxLabel ID="ClinicCodesHelpLabel" runat="server" OnInit="ClinicCodesHelpLabel_Init" EncodeHtml="false"></dx:ASPxLabel>
                                </Template>
                            </dx:GridViewColumnLayoutItem>
                            <dx:GridViewColumnLayoutItem ColumnName="ClinicCodes" Caption="Clinic Codes" ColSpan="2" Width="100%">
                            </dx:GridViewColumnLayoutItem>

                            <dx:EditModeCommandLayoutItem ColSpan="2" CssClass="ps-3"></dx:EditModeCommandLayoutItem>
                        </Items>

                    </dx:GridViewLayoutGroup>
                </Items>
            </EditFormLayoutProperties>

            <Columns>
                <dx:GridViewCommandColumn VisibleIndex="0" Width="100px" Caption="Edit" ShowNewButtonInHeader="true" ShowEditButton="true" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>

                <dx:GridViewDataTextColumn Caption="AuditID" ReadOnly="true" FieldName="AuditID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                    <EditItemTemplate>
                        <dx:ASPxLabel ID="AuditIDReadonlyLabel" runat="server" Text='<%# Eval("AuditID") %>'></dx:ASPxLabel>
                    </EditItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataDateColumn Caption="Audit Start Date" FieldName="Date" VisibleIndex="2" MinWidth="200" MaxWidth="200">
                    <PropertiesDateEdit ClientInstanceName="AuditStartDate" DisplayFormatString="dd-MMM-yyyy">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="A date is required" />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataDateColumn Caption="DueByDate" FieldName="DueByDate" VisibleIndex="3" MinWidth="200" MaxWidth="200">
                    <PropertiesDateEdit ClientInstanceName="DueByDate" DisplayFormatString="dd-MMM-yyyy">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="A date is required" />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataComboBoxColumn Caption="Specialities" FieldName="SpecialtyID" VisibleIndex="4" MinWidth="200" MaxWidth="500" >
                    <PropertiesComboBox ClientInstanceName="Specialities" DataSourceID="GetSpeciality" TextField="SpecilatiesName" ValueField="SpecilatiesID">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="Specialty is required" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataComboBoxColumn Caption="Sites" SettingsHeaderFilter-DateRangeCalendarSettings-ShowClearButton="true"
                    PropertiesComboBox-ClearButton-DisplayMode="OnHover" FieldName="SiteID" VisibleIndex="5" MinWidth="200" MaxWidth="400">
                    <PropertiesComboBox DataSourceID="GetSites" TextField="SiteName" ValueField="SiteID">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="Site is required" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dx:GridViewDataComboBoxColumn>

                <dx:GridViewDataTokenBoxColumn FieldName="ClinicCodes" VisibleIndex="6">
                    <PropertiesTokenBox AllowCustomTokens="true" ValueSeparator="," MaxLength="100">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField IsRequired="true" ErrorText="At least one clinic code is required & blank text is not allowed" />
                        </ValidationSettings>
                    </PropertiesTokenBox>
                </dx:GridViewDataTokenBoxColumn>

            </Columns>
            <Settings ShowFilterRow="true" />
            <SettingsBehavior AllowEllipsisInText="true" />
            <SettingsResizing ColumnResizeMode="NextColumn" />
        </dx:ASPxGridView>

    </div>
    <%--<uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />--%>
</asp:Content>
