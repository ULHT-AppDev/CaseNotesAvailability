<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReviewAudit.aspx.cs" Inherits="ReviewAudit._ReviewAudit" %>

<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CustomCSS/NotificationHelper.css" rel="stylesheet" />
    <script src="CustomJS/ReviewAudit.js"></script>
    <script src="UserControl/UserAlertControl/Scripts/UserAlert.js"></script>
    <link href="UserControl/UserAlertControl/CSS/UserAlert.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="ReviewAuditRecordsView" runat="server" SelectMethod="GetAuditClinicAnswers" OnSelecting="ReviewAuditRecordsView_Selecting" UpdateMethod="UpdateAuditRecords" DeleteMethod=""
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditClinicAnswersBLL" DataObjectTypeName="BusinessObjects.AuditClinicAnswersBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="Status" runat="server" SelectMethod="GetStatus" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ImpReviewAudit" runat="server" SelectMethod="GetImpAuditReview" TypeName="BLL.ReviewAuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ActPointReviewAudit" runat="server" SelectMethod="GetActPointAuditReview" TypeName="BLL.ReviewAuditBLL"></asp:ObjectDataSource>
    <div id="HeaderSubContainer" class="container">
        <div id="HeaderRow" class="row align-items-center">

            <div id="PageTitleContainer" class="col-md-6 pt-3 pb-3">
                <h2 id="PageTitle">
                    <asp:Literal ID="LoginPageTitle" Text="Audit" runat="server" />
                </h2>
                <%--  <div id="VersionNumber" class="pl-1" style="color: #e69500;">
            <asp:Literal ID="VersionNumberText" runat="server" />
        </div>
    </div>--%>
            </div>
        </div>
    </div>
    <div>
        <%--<dx:ASPxButton ID="NewRef"
            runat="server"
            Text="Create New Audit"
            RenderMode="Button"
            AutoPostBack="false"
            UseSubmitBehavior="false"
            CausesValidation="false"
            OnInit="NewRef_Init">
        </dx:ASPxButton>--%>

        <div class="container-fluid p-4 mb-5">
            <div class="row">
                <div class="col-lg-12">
                    <div id="DefaultPageTitleContainer" class="DefaultPageTitleContainer d-flex align-items-center">
                        <div>
                            <dx:ASPxLabel ID="DefaultPageTitleLabel"
                                runat="server"
                                OnInit="DefaultPageTitleLabel_Init"
                                CssClass="PageHeader"
                                EncodeHtml="false"
                                Font-Bold="true" Font-Size="16px">
                            </dx:ASPxLabel>

                            <br />
                            <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Width="400px">
                                <div>
                                    <div>
                                        <dx:ASPxLabel ID="CasenoteLabel"
                                            ClientInstanceName="CasenoteLabel"
                                            runat="server"
                                            OnInit="CasenoteLabel_Init"
                                            Font-Size="20px"
                                            ForeColor="GrayText">
                                        </dx:ASPxLabel>
                                    </div>
                                    <div>
                                        <dx:ASPxLabel ID="lblSpeciality"
                                            ClientInstanceName="lblSpeciality"
                                            runat="server"
                                            OnInit="lblSpeciality_Init"
                                            Font-Size="20px"
                                            ForeColor="GrayText">
                                        </dx:ASPxLabel>
                                    </div>
                                    <div>
                                        <dx:ASPxLabel ID="lblSite"
                                            ClientInstanceName="lblSite"
                                            runat="server"
                                            OnInit="lblSite_Init"
                                            Font-Size="20px"
                                            ForeColor="GrayText">
                                        </dx:ASPxLabel>
                                    </div>
                                    <%-- <div>
                            <dx:ASPxLabel ID="LockedToUserInfoLabel"
                                ClientInstanceName="LockedToUserInfoLabel"
                                runat="server"
                                Font-Size="10px"
                                ForeColor="Gray">
                            </dx:ASPxLabel>
                        </div>--%>
                                </div>
                            </asp:Panel>

                        </div>
                    </div>
                </div>
            </div>
            <div class="ml-auto text-black text-right">
            </div>
            <div>
                <dx:ASPxButton ID="AuditDetails"
                    runat="server"
                    Text="Edit audit details"
                    RenderMode="Button"
                    AutoPostBack="false"
                    UseSubmitBehavior="false"
                    CausesValidation="false"
                    Paddings-PaddingTop="10px"
                    Paddings-PaddingBottom="10px">
                    <ClientSideEvents Click="AuditDetails_Click" />
                    <Image>
                        <SpriteProperties CssClass="fas fa-chevron-left" />
                    </Image>
                </dx:ASPxButton>
            </div>
            <div class="ml-auto text-black text-right">
            </div>
            <dx:ASPxGridView ID="ReviewAuditRecordsGridView" runat="server" AllowSorting="true"
                ClientInstanceName="ReviewAuditRecordsGridView"
                OnRowUpdating="AuditRow_Updating"
                KeyFieldName="AuditClinicAnswersID"
                DataSourceID="ReviewAuditRecordsView"
                AutoGenerateColumns="False"
                OnInitNewRow="ReviewAuditRecordsGridView_InitNewRow"
                OnStartRowEditing="ReviewAuditRecordsGridView_StartRowEditing"
                OnCellEditorInitialize="ReviewAuditRecordsGridView_CellEditorInitialize"
                OnCustomCallback="ReviewAuditRecordsGridView_CustomCallback"
                Width="100%">
                <ClientSideEvents EndCallback="ReviewAuditRecordsGridView_EndCallBack" />
                <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>

                <SettingsEditing EditFormColumnCount="2"></SettingsEditing>

                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>

                <Styles>
                    <EditingErrorRow BackColor="Yellow" />
                </Styles>

                <EditFormLayoutProperties AlignItemCaptionsInAllGroups="false" AlignItemCaptions="true" LeftAndRightCaptionsWidth="125" Styles-LayoutGroup-CssClass="p-3">
                    <Styles>
                        <LayoutGroupBox>
                            <Caption Font-Bold="true" Font-Size="18px"></Caption>
                        </LayoutGroupBox>
                    </Styles>
                    <Items>
                        <dx:GridViewLayoutGroup Name="FieldGroup" Caption="Health Records View" ColCount="2" ColumnCount="2" ColSpan="1" ColumnSpan="1">
                            <Items>
                                <dx:GridViewColumnLayoutItem ClientVisible="false" ColumnName="AuditClinicAnswersID" Name="AuditClinicAnswersID" Caption="AuditClinic AnswersID" Width="100%" ColumnSpan="1"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem ColumnName="ClinicCode" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <%--<dx:GridViewColumnLayoutItem ColumnName="AuditID" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>--%>
                                <%--<dx:GridViewColumnLayoutItem ColumnName="ClinicCode" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>--%>
                                <dx:GridViewColumnLayoutItem Caption="Number Of Appointments Allocated" ColumnName="NumberOfAppointmentsAllocated" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="CaseNotes Available StartCount" ColumnName="CaseNotesAvailableStartCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="TemporaryNotes Count" ColumnName="TemporaryNotesCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:EditModeCommandLayoutItem ColSpan="2" CssClass="ps-3"></dx:EditModeCommandLayoutItem>
                            </Items>
                        </dx:GridViewLayoutGroup>

                    </Items>
                </EditFormLayoutProperties>
                <Columns>
                    <%--<dx:GridViewCommandColumn VisibleIndex="0" Width="100px" Caption="Action" ShowNewButtonInHeader="false" ShowEditButton="false" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>--%>
                    <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="25" MaxWidth="100" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                        <DataItemTemplate>
                            <div>
                                <dx:ASPxButton ID="AuditorView"
                                    runat="server"
                                    Text="Add Review"
                                    RenderMode="Button"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    CausesValidation="false"
                                    OnInit="AuditorView_Init">
                                </dx:ASPxButton>
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn Caption="AuditClinicAnswersID" Visible="false" ReadOnly="true" FieldName="AuditClinicAnswersID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                        <EditItemTemplate>
                            <dx:ASPxLabel ID="AuditClinicAnswersIDReadonlyLabel" runat="server" Text='<%# Eval("AuditClinicAnswersID") %>'></dx:ASPxLabel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="ClinicCode" ReadOnly="true" FieldName="ClinicCode" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                        <EditItemTemplate>
                            <dx:ASPxLabel ID="ClinicCodeReadonlyLabel" runat="server" Text='<%# Eval("ClinicCode") %>'></dx:ASPxLabel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <%-- <dx:GridViewDataTextColumn Caption="AuditID" ReadOnly="true" FieldName="AuditID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                        <EditItemTemplate>
                            <dx:ASPxLabel ID="AuditIDReadonlyLabel" runat="server" Text='<%# Eval("AuditID") %>'></dx:ASPxLabel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn Caption="Number Of Appointments Allocated" FieldName="NumberOfAppointmentsAllocated" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="CaseNotes Available Start Count" FieldName="CaseNotesAvailableStartCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Temporary Notes Count" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                </Columns>

                <Settings ShowFilterRow="true" />
                <SettingsBehavior AllowEllipsisInText="true" />
                <SettingsResizing ColumnResizeMode="NextColumn" />
                <Templates>
                    <EditForm>
                        <div>
                            <dx:ASPxButton ID="AuditorView1"
                                runat="server"
                                Text="Add Improvement Details"
                                RenderMode="Button"
                                AutoPostBack="false"
                                UseSubmitBehavior="false"
                                CausesValidation="false">
                            </dx:ASPxButton>

                        </div>
                        <div>
                            <dx:ASPxGridView ID="ReviewAuditClinicsGridView" runat="server" AllowSorting="true"
                                ClientInstanceName="ReviewAuditClinicsGridView"
                                OnRowUpdating="ReviewAuditClinicsGridView_Updating"
                                KeyFieldName="AuditClinicAnswersID"
                                DataSourceID="ReviewAuditRecordsView"
                                AutoGenerateColumns="False"
                                OnInitNewRow="ReviewAuditClinicsGridView_InitNewRow"
                                OnStartRowEditing="ReviewAuditClinicsGridView_StartRowEditing"
                                OnCellEditorInitialize="ReviewAuditClinicsGridView_CellEditorInitialize"
                                OnCustomCallback="ReviewAuditClinicsGridView_CustomCallback"
                                Width="100%">
                                <ClientSideEvents EndCallback="ReviewAuditClinicsGridView_EndCallBack" />
                                <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>

                                <SettingsEditing EditFormColumnCount="2"></SettingsEditing>

                                <SettingsPopup>
                                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                                </SettingsPopup>

                                <Styles>
                                    <EditingErrorRow BackColor="Yellow" />
                                </Styles>

                                <EditFormLayoutProperties AlignItemCaptionsInAllGroups="false" AlignItemCaptions="true" LeftAndRightCaptionsWidth="125" Styles-LayoutGroup-CssClass="p-3">
                                    <Styles>
                                        <LayoutGroupBox>
                                            <Caption Font-Bold="true" Font-Size="18px"></Caption>
                                        </LayoutGroupBox>
                                    </Styles>
                                    <Items>
                                        <dx:GridViewLayoutGroup Name="FieldGroup" Caption="Health Records View" ColCount="2" ColumnCount="2" ColSpan="1" ColumnSpan="1">
                                            <Items>
                                                <dx:GridViewColumnLayoutItem ClientVisible="false" ColumnName="AuditClinicAnswersID" Name="AuditClinicAnswersID" Caption="AuditClinic AnswersID" Width="100%" ColumnSpan="1"></dx:GridViewColumnLayoutItem>
                                                <dx:GridViewColumnLayoutItem ColumnName="ClinicCode" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                                <%--<dx:GridViewColumnLayoutItem ColumnName="AuditID" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>--%>
                                                <%--<dx:GridViewColumnLayoutItem ColumnName="ClinicCode" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>--%>
                                                <dx:GridViewColumnLayoutItem Caption="Number Of Appointments Allocated" ColumnName="NumberOfAppointmentsAllocated" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                                <dx:GridViewColumnLayoutItem Caption="CaseNotes Available StartCount" ColumnName="CaseNotesAvailableStartCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                                <dx:GridViewColumnLayoutItem Caption="TemporaryNotes Count" ColumnName="TemporaryNotesCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                                <dx:EditModeCommandLayoutItem ColSpan="2" CssClass="ps-3"></dx:EditModeCommandLayoutItem>
                                            </Items>
                                        </dx:GridViewLayoutGroup>

                                    </Items>
                                </EditFormLayoutProperties>
                                <Columns>
                                    <%--<dx:GridViewCommandColumn VisibleIndex="0" Width="100px" Caption="Action" ShowNewButtonInHeader="false" ShowEditButton="false" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>--%>
                                    <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="25" MaxWidth="100" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                                        <DataItemTemplate>
                                            <div>
                                                <dx:ASPxButton ID="AuditorView"
                                                    runat="server"
                                                    Text="Add Review"
                                                    RenderMode="Button"
                                                    AutoPostBack="false"
                                                    UseSubmitBehavior="false"
                                                    CausesValidation="false"
                                                    OnInit="AuditorView_Init">
                                                </dx:ASPxButton>
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn Caption="AuditClinicAnswersID" Visible="false" ReadOnly="true" FieldName="AuditClinicAnswersID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                                        <EditItemTemplate>
                                            <dx:ASPxLabel ID="AuditClinicAnswersIDReadonlyLabel" runat="server" Text='<%# Eval("AuditClinicAnswersID") %>'></dx:ASPxLabel>
                                        </EditItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ClinicCode" ReadOnly="true" FieldName="ClinicCode" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                                        <EditItemTemplate>
                                            <dx:ASPxLabel ID="ClinicCodeReadonlyLabel" runat="server" Text='<%# Eval("ClinicCode") %>'></dx:ASPxLabel>
                                        </EditItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <%-- <dx:GridViewDataTextColumn Caption="AuditID" ReadOnly="true" FieldName="AuditID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
             <EditItemTemplate>
                 <dx:ASPxLabel ID="AuditIDReadonlyLabel" runat="server" Text='<%# Eval("AuditID") %>'></dx:ASPxLabel>
             </EditItemTemplate>
         </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn Caption="Number Of Appointments Allocated" FieldName="NumberOfAppointmentsAllocated" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="CaseNotes Available Start Count" FieldName="CaseNotesAvailableStartCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Temporary Notes Count" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                                </Columns>

                                <Settings ShowFilterRow="true" />
                                <SettingsBehavior AllowEllipsisInText="true" />
                                <SettingsResizing ColumnResizeMode="NextColumn" />
                            </dx:ASPxGridView>

                        </div>
                    </EditForm>
                </Templates>
            </dx:ASPxGridView>


        </div>
        <uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />
    </div>
</asp:Content>
<%-- Starts here  --%>

