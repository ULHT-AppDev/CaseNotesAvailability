<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="CaseNotesAvailability._default" %>

<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CustomCSS/NotificationHelper.css" rel="stylesheet" />
    <script src="CustomJS/Audit.js">  </script>
    <script src="UserControl/UserAlertControl/Scripts/UserAlert.js"></script>
    <link href="UserControl/UserAlertControl/CSS/UserAlert.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="HealthRecordsView" runat="server" SelectMethod="GetAudit" UpdateMethod="UpdateAuditRecords" DeleteMethod=""
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditBLL" DataObjectTypeName="BusinessObjects.AuditBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="Status" runat="server" SelectMethod="GetStatus" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <div class="py-3 mx-3 defaultBorderBottom d-flex align-items-center">
        <div>
            <div>
                <dx:ASPxLabel ID="PageHeaderLabel" ClientInstanceName="pageHeaderLabel" CssClass="PageHeaderLabel" OnInit="PageHeaderLabel_Init" runat="server"></dx:ASPxLabel>
            </div>

            <div class="mt-2">
                <dx:ASPxButton ID="NewRef"
                    runat="server"
                    Text="Create New Audit"
                    RenderMode="Button"
                    AutoPostBack="false"
                    UseSubmitBehavior="false"
                    CausesValidation="false"
                    OnInit="NewRef_Init"
                    Paddings-PaddingTop="10px"
                    Paddings-PaddingBottom="10px">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
    <div class="ml-auto text-black text-right">
    </div>
    <dx:ASPxGridView ID="HealthRecordsGridView" runat="server" AllowSorting="true"
        ClientInstanceName="HealthRecordsGridView"
        OnRowUpdating="AuditRow_Updating"
        KeyFieldName="AuditID"
        DataSourceID="HealthRecordsView"
        AutoGenerateColumns="False"
        OnInitNewRow="HealthRecordsGridView_InitNewRow"
        OnStartRowEditing="HealthRecordsGridView_StartRowEditing"
        OnCellEditorInitialize="HealthRecordsGridView_CellEditorInitialize"
        OnRowUpdated="HealthRecordsGridView_RowUpdated"
        OnRowInserted="HealthRecordsGridView_RowInserted"
        OnCustomButtonInitialize="RoleControlGridView_CustomButtonInitialize"
        OnCustomCallback="HealthRecordsGridView_CustomCallback"
        Width="100%">
        <ClientSideEvents EndCallback="HealthRecordsGridView_EndCallBack" />
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
            <dx:GridViewCommandColumn Visible="false" VisibleIndex="0" Width="100px" Caption="Edit" ShowNewButtonInHeader="true" ShowEditButton="true" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>

            <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="75" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                <DataItemTemplate>
                    <div>
                        <dx:ASPxButton ID="AuditorView"
                            runat="server"
                            Text="Choose"
                            RenderMode="Link"
                            AutoPostBack="false"
                            UseSubmitBehavior="false"
                            CausesValidation="false"
                            OnInit="AuditorView_Init">
                        </dx:ASPxButton>
                        <t></t>
                        <dx:ASPxButton ID="EditUserButton"
                            runat="server"
                            Text="Choose"
                            RenderMode="Link"
                            AutoPostBack="false"
                            UseSubmitBehavior="false"
                            CausesValidation="false"
                            OnInit="EditUserButton_Init">
                        </dx:ASPxButton>
                        <t></t>

                        <dx:ASPxButton ID="DeleteUserButton"
                            runat="server"
                            Text="Choose"
                            RenderMode="Link"
                            AutoPostBack="false"
                            UseSubmitBehavior="false"
                            CausesValidation="false"
                            OnInit="DeleteUserButton_Init">
                        </dx:ASPxButton>
                    </div>
                </DataItemTemplate>
            </dx:GridViewDataColumn>

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

            <dx:GridViewDataDateColumn Caption="Due By Date" FieldName="DueByDate" VisibleIndex="3" MinWidth="200" MaxWidth="200">
                <PropertiesDateEdit ClientInstanceName="DueByDate" DisplayFormatString="dd-MMM-yyyy">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="A date is required" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataComboBoxColumn Caption="Specialities" FieldName="SpecialtyID" VisibleIndex="4" MinWidth="200" MaxWidth="500">
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

            <dx:GridViewDataComboBoxColumn Caption="Staus" SettingsHeaderFilter-DateRangeCalendarSettings-ShowClearButton="true"
                PropertiesComboBox-ClearButton-DisplayMode="OnHover" FieldName="StatusID" VisibleIndex="7" MinWidth="200" MaxWidth="400">
                <PropertiesComboBox DataSourceID="Status" TextField="StatusName" ValueField="StatusID">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="Status is required" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dx:GridViewDataComboBoxColumn>
        </Columns>
        <Settings ShowFilterRow="true" />
        <SettingsBehavior AllowEllipsisInText="true" />
        <SettingsResizing ColumnResizeMode="NextColumn" />
    </dx:ASPxGridView>

    <dx:ASPxPopupControl ID="DeleteCaseNotePopup"
        ClientInstanceName="DeleteCaseNotePopup"
        runat="server"
        PopupVerticalAlign="WindowCenter"
        PopupHorizontalAlign="WindowCenter"
        CloseAction="CloseButton"
        AutoUpdatePosition="true"
        AllowDragging="true"
        Modal="true"
        ShowCloseButton="false"
        ModalBackgroundStyle-Opacity="015"
        ShowHeader="true"
        ShowFooter="true"
        HeaderStyle-CssClass="defaultBorderBottom">
        <SettingsAdaptivity MaxWidth="700" MaxHeight="400" Mode="Always" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
        <ClientSideEvents CloseButtonClick="CancelDeleteButton_Click" />
        <HeaderContentTemplate>
            <div class="ml-3">
                <dx:ASPxLabel ID="ConfirmationDeleteHeaderLabel"
                    runat="server"
                    Text="Delete Case Note?"
                    Font-Size="20px"
                    Font-Bold="true"
                    ForeColor="#e74343">
                </dx:ASPxLabel>
            </div>
        </HeaderContentTemplate>

        <ContentCollection>
            <dx:PopupControlContentControl>


                <dx:ASPxPanel ID="DeleteCaseNotePanel"
                    ClientInstanceName="DeleteCaseNotePanel"
                    runat="server">
                    <PanelCollection>
                        <dx:PanelContent>

                            <div id="DeleteConfirmationForm" class="p-3">
                                <div>
                                    <dx:ASPxLabel ID="DeleteCaseNoteConfirmationLabel"
                                        ClientInstanceName="DeleteCaseNoteConfirmationLabel"
                                        runat="server"
                                        EncodeHtml="false" Font-Bold="true">
                                    </dx:ASPxLabel>
                                </div>
                                <%--   <div class="mt-4">
                                    <dx:ASPxComboBox ID="DeleteCaseNoteReasonComboBox"
                                        runat="server"
                                        Caption="Reason for deletion"
                                        Width="200px"
                                        CaptionSettings-Position="Top"
                                        TextField="StatusChangedReason"
                                        ValueField="StatusChangedReasonID">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="DeleteCaseNote" Display="Dynamic">
                                            <RequiredField IsRequired="true" ErrorText="A reason is required" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="mt-1">
                                    <dx:ASPxMemo ID="DeleteCaseNoteCommentsMemo" runat="server" Height="75px" Width="100%" MaxLength="500" Caption="Other comments" CaptionSettings-Position="Top">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="DeleteCaseNote" Display="Dynamic">
                                            <RequiredField IsRequired="false" />
                                        </ValidationSettings>
                                    </dx:ASPxMemo>
                                </div>--%>
                            </div>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>

                <%--  <dx:ASPxPanel ID="ConfirmDeleteCaseNotePanel"
                    ClientInstanceName="ConfirmDeleteCaseNotePanel"
                    runat="server"
                    ClientVisible="false">
                    <PanelCollection>
                        <dx:PanelContent>

                            <div id="ConfirmDeleteConfirmationForm" class="p-3">

                                <dx:ASPxCheckBox runat="server" ID="ConfirmDeleteCheckBox" ClientInstanceName="ConfirmDeleteCheckBox" Text="I confirm I want to delete this CaseNote" TextAlign="Left">
                                    <ClientSideEvents Validation="ConfirmDeleteCheckBox_Validation" />
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ValidationGroup="ConfirmDeleteCaseNote" Display="Dynamic" EnableCustomValidation="true">
                                        <RequiredField IsRequired="true" />
                                    </ValidationSettings>
                                </dx:ASPxCheckBox>
                            </div>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>--%>
            </dx:PopupControlContentControl>
        </ContentCollection>

        <FooterContentTemplate>
            <div class="d-flex align-items-center p-2">
                <div class="ml-auto pr-3">
                    <dx:ASPxButton ID="CancelDeleteButton"
                        ClientInstanceName="CancelDeleteButton"
                        runat="server"
                        Text="Cancel"
                        Width="200px"
                        RenderMode ="Secondary"
                        AutoPostBack="false"
                        CausesValidation="true"
                        ValidationGroup="CancelCaseNote"
                        UseSubmitBehavior="false">
                        <ClientSideEvents Click="CancelDeleteButton_Click" />
                    </dx:ASPxButton>
                </div>
                <div>&nbsp;</div> 
                <div class="pl-3">
                    <dx:ASPxButton ID="DeleteCaseNoteButton"
                        ClientInstanceName="DeleteCaseNoteButton"
                        runat="server"
                        Text="Delete"
                        Width="200px"
                        RenderMode ="Danger"
                        AutoPostBack="false"
                        CausesValidation="true"
                        ValidationGroup="DeleteCaseNote"
                        UseSubmitBehavior="false">
                        <ClientSideEvents Click="DeleteThisCaseNoteButton_Click" />
                    </dx:ASPxButton>

                </div>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>

    <uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />
</asp:Content>
