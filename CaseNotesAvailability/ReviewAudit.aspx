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
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditClinicAnswersBLL"  DataObjectTypeName="BusinessObjects.AuditClinicAnswersBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="Status" runat="server" SelectMethod="GetStatus" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetUnavailableReason" runat="server" SelectMethod="GetUnavailableReason" TypeName="BLL.ReviewAuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ImpReviewAudit" runat="server" SelectMethod="GetImprovementDetails" UpdateMethod="UpdateImprovementDetails" DataObjectTypeName="BusinessObjects.RequiresImprovementDetailsBO" InsertMethod="InsertImprovementDetails" TypeName="BLL.ReviewAuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ActPointReviewAudit" runat="server" SelectMethod="GetActPointAuditReview" TypeName="BLL.ReviewAuditBLL"></asp:ObjectDataSource>

    <div class="container-fluid p-4 mb-5">
        <div class="row">
            <div class="col-lg-12">
                <div class="pb-2">
                    <dx:ASPxButton ID="AuditDetails"
                        runat="server"
                        Text="Go back"
                        RenderMode="Button"
                        Width="200px"
                        AutoPostBack="false"
                        UseSubmitBehavior="false"
                        CausesValidation="false">
                        <ClientSideEvents Click="AuditDetails_Click" />
                        <Image>
                            <SpriteProperties CssClass="fas fa-chevron-left" />
                        </Image>
                    </dx:ASPxButton>
                </div>
                <div id="DefaultPageTitleContainer" class="DefaultPageTitleContainer d-flex align-items-center">
                    <div>
                        <dx:ASPxLabel ID="DefaultPageTitleLabel"
                            runat="server"
                            OnInit="DefaultPageTitleLabel_Init"
                            CssClass="PageHeader"
                            EncodeHtml="false"
                            Font-Bold="true"
                            Font-Size="20px">
                        </dx:ASPxLabel>

                        <br />
                        <asp:Panel ID="Panel1" runat="server">
                            <div>
                                <div>
                                    <dx:ASPxLabel ID="CasenoteLabel"
                                        ClientInstanceName="CasenoteLabel"
                                        runat="server"
                                        OnInit="CasenoteLabel_Init"
                                        ForeColor="GrayText"
                                        EncodeHtml="false">
                                    </dx:ASPxLabel>
                                </div>
                                <div>
                                    <dx:ASPxLabel ID="lblSpeciality"
                                        ClientInstanceName="lblSpeciality"
                                        runat="server"
                                        OnInit="lblSpeciality_Init"
                                        ForeColor="GrayText"
                                        EncodeHtml="false">
                                    </dx:ASPxLabel>
                                </div>
                                <div>
                                    <dx:ASPxLabel ID="lblSite"
                                        ClientInstanceName="lblSite"
                                        runat="server"
                                        OnInit="lblSite_Init"
                                        ForeColor="GrayText"
                                        EncodeHtml="false">
                                    </dx:ASPxLabel>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </div>
        </div>

        <div class="mt-3">
            <dx:ASPxGridView ID="ReviewAuditRecordsGridView"
                runat="server"
                AllowSorting="true"
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
                                <dx:GridViewColumnLayoutItem Caption="Total Appointments" ColumnName="Totalappointments" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="Number Of Appointments Allocated" ColumnName="NumberOfAppointmentsAllocated" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="CaseNotes Available StartCount" ColumnName="CaseNotesAvailableStartCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="TemporaryNotes Count" ColumnName="TemporaryNotesCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem Caption="Number of case notes Unavailable" ColumnName="TemporaryNotesCount" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
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
                                    RenderMode="Link"
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
                    <dx:GridViewDataTextColumn Caption="Total Appointments" FieldName="Totalappointments" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Number Of Appointments Allocated" FieldName="NumberOfAppointmentsAllocated" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="CaseNotes Available Start Count" FieldName="CaseNotesAvailableStartCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Temporary Notes Count" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Number of case notes Unavailable" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                </Columns>

                <Settings ShowFilterRow="true" />
                <SettingsBehavior AllowEllipsisInText="true" />
                <SettingsResizing ColumnResizeMode="NextColumn" />
                <Templates>
                    <EditForm>
                        <div class="p-4">

                            <div>
                                <dx:ASPxLabel ID="ClinicCodeLabel"
                                    ClientInstanceName="ClinicCodeLabel"
                                    runat="server"
                                    OnInit="ClinicCodeLabel_Init"
                                    Font-Size="20px"
                                    EncodeHtml="false"
                                    ForeColor="GrayText">
                                </dx:ASPxLabel>
                                <%--    <dx:ASPxLabel ID="LabelRead"
                                    ClientInstanceName="LabelRead"
                                    runat="server"
                                    Font-Size="20px"
                                    OnInit="LabelRead_Init"
                                    ClientVisible="false"
                                    EncodeHtml="false"
                                    ForeColor="GrayText">
                                </dx:ASPxLabel>
                                <dx:ASPxLabel ID="LblAuditClinicAnswersID"
                                    ClientInstanceName="LblAuditClinicAnswersID"
                                    runat="server"
                                    Font-Size="20px"
                                    OnInit="LblAuditClinicAnswersID_Init"
                                    ClientVisible="false"
                                    EncodeHtml="false"
                                    ForeColor="GrayText">
                                </dx:ASPxLabel>--%>
                            </div>

                            <div class="mt-2">
                                <dx:ASPxButton ID="AddImpDetails"
                                    runat="server"
                                    Text="Add Improvement Details"
                                    RenderMode="Button"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    CausesValidation="false"
                                    OnInit="AddImpDetails_Init">
                                </dx:ASPxButton>

                            </div>

                            <div class="mt-3">
                                <dx:ASPxGridView ID="ImprovementDetailsGridView"
                                    ClientInstanceName="ImprovementDetailsGridView"
                                    OnCustomCallback="ImprovementDetailsGridView_CustomCallback"
                                    runat="server"
                                    AllowSorting="true"
                                    KeyFieldName="RequiresImprovementDetailsID"
                                    ClientVisible="false"
                                    AutoGenerateColumns="False"
                                    SettingsDataSecurity-AllowReadUnexposedColumnsFromClientApi="True"
                                    SettingsDataSecurity-PreventLoadClientValuesForInvisibleColumns="True">
                                    <ClientSideEvents EndCallback="ImprovementDetailsGridView_EndCallback" />

                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>

                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="RequiresImprovementDetailsID" Visible="false"></dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="25" MaxWidth="100" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                                            <DataItemTemplate>
                                                <div>
                                                    <dx:ASPxButton ID="DeleteReviewButton"
                                                        OnInit="DeleteImpReviewButton_Init"
                                                        runat="server"
                                                        Text="Delete"
                                                        RenderMode="Link"
                                                        AutoPostBack="false"
                                                        UseSubmitBehavior="false"
                                                        CausesValidation="false">
                                                    </dx:ASPxButton>
                                                </div>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataComboBoxColumn Caption="Improvement Details" PropertiesComboBox-ClientInstanceName="ImprovementDetailID" FieldName="ImprovementDetailID" VisibleIndex="4" MinWidth="200" MaxWidth="500">
                                        </dx:GridViewDataComboBoxColumn>
                                        <dx:GridViewDataTextColumn FieldName="Comment" PropertiesTextEdit-ClientInstanceName="Comment" PropertiesTextEdit-ValidationSettings-EnableCustomValidation="true" PropertiesTextEdit-MaxLength="255" VisibleIndex="3" MinWidth="200" MaxWidth="300">
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <Settings ShowFilterRow="false" />
                                    <SettingsBehavior AllowEllipsisInText="true" AllowDragDrop="false" AllowSort="false" />
                                    <SettingsResizing ColumnResizeMode="NextColumn" />
                                </dx:ASPxGridView>
                                <dx:ASPxLabel ID="NoImprovementDetailReviewLabel" ClientInstanceName="NoImprovementDetailReviewLabel" runat="server" Text="There are no improvement detail reviews"></dx:ASPxLabel>
                            </div>
                            <div class="mt-2">
                                <dx:ASPxButton ID="AddActionPoint"
                                    runat="server"
                                    Text="Add Action Point"
                                    RenderMode="Button"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    CausesValidation="false"
                                    OnInit="AddActionPoint_Init">
                                </dx:ASPxButton>
                            </div>
                            <div class="mt-3">
                                <dx:ASPxGridView ID="ActionPointDetailsGridView"
                                    ClientInstanceName="ActionPointDetailsGridView"
                                    OnCustomCallback="ActionPointDetailsGridView_CustomCallback"
                                    runat="server"
                                    AllowSorting="true"
                                    KeyFieldName="RequiresImprovementDetailsID"
                                    ClientVisible="false"
                                    AutoGenerateColumns="False">
                                    <ClientSideEvents EndCallback="ActionPointDetailsGridView_EndCallback" />

                                    <SettingsAdaptivity AdaptivityMode="HideDataCells" HideDataCellsAtWindowInnerWidth="780" AllowOnlyOneAdaptiveDetailExpanded="true" AdaptiveDetailColumnCount="2"></SettingsAdaptivity>

                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="RequiresImprovementDetailsID" Visible="false"></dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="25" MaxWidth="100" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                                            <DataItemTemplate>
                                                <div>
                                                    <dx:ASPxButton ID="DeleteReviewButton"
                                                        OnInit="DeleteActionReviewButton_Init"
                                                        runat="server"
                                                        Text="Delete"
                                                        RenderMode="Link"
                                                        AutoPostBack="false"
                                                        UseSubmitBehavior="false"
                                                        CausesValidation="false">
                                                    </dx:ASPxButton>
                                                </div>
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>
                                        <%--<dx:GridViewDataComboBoxColumn Caption="Improvement Details" FieldName="ImprovementDetailID" VisibleIndex="4" MinWidth="200" MaxWidth="500">
                                        </dx:GridViewDataComboBoxColumn>--%>
                                        <dx:GridViewDataTextColumn FieldName="Comment" PropertiesTextEdit-ClientInstanceName="Comment" PropertiesTextEdit-ValidationSettings-EnableCustomValidation="true" PropertiesTextEdit-MaxLength="255" VisibleIndex="3" MinWidth="200" MaxWidth="300">
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <Settings ShowFilterRow="false" />
                                    <SettingsBehavior AllowEllipsisInText="true" AllowDragDrop="false" AllowSort="false" />
                                    <SettingsResizing ColumnResizeMode="NextColumn" />
                                </dx:ASPxGridView>
                                <dx:ASPxLabel ID="ASPxLabel1" ClientInstanceName="NoActionPointDetailReviewLabel" runat="server" Text="There are no action points to add"></dx:ASPxLabel>
                            </div>

                            <%--                            <div>
                                Complete audit review button - popup to confirm then take the two in memory ds and save back to ReviewAuditRecordsGridView. 
                                Button should probably also check for atleast one improvement and one action before allowing to submit
                            </div>--%>

                            <%--                            <div>
                                cancel button but with a warning that it will remove all existing comments on this clinic review
                            </div>--%>
                        </div>

                        </div>
                            <div class="mt-2">
                                <dx:ASPxButton ID="btnCompleteReview"
                                    runat="server"
                                    Text="Complete"
                                    RenderMode="Button"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    CausesValidation="false"
                                    OnInit="CompleteAuditReview_Init">
                                    <%--<ClientSideEvents Click="CompleteClient_Click" />--%>
                                </dx:ASPxButton>
                    </EditForm>
                </Templates>
            </dx:ASPxGridView>
        </div>
        <%--                                            <PropertiesComboBox ClientInstanceName="Specialities" DataSourceID="GetUnavailableReason" TextField="ReasonText" ValueField="ReasonUnavailableID">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField IsRequired="true" ErrorText="Unavailable reason is required" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>--%>
    </div>

    <dx:ASPxPopupControl ID="AddReviewPopup"
        ClientInstanceName="AddReviewPopup"
        runat="server"
        AutoUpdatePosition="true"
        AllowDragging="true"
        PopupVerticalAlign="WindowCenter"
        PopupHorizontalAlign="WindowCenter"
        CloseAction="CloseButton"
        ScrollBars="Auto"
        Modal="true"
        ModalBackgroundStyle-Opacity="015"
        ShowHeader="true"
        ShowFooter="true"
        HeaderStyle-CssClass="defaultBorderBottom">
        <ClientSideEvents CloseButtonClick="" />

        <SettingsAdaptivity Mode="Always" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" MinWidth="600" MinHeight="400" />

        <HeaderContentTemplate>
            <div class="ps-3">
                <dx:ASPxLabel ID="AddReviewPopupHeaderLabel"
                    ClientInstanceName="AddReviewPopupHeaderLabel"
                    EncodeHtml="false"
                    runat="server" Text="Require details"
                    Font-Size="16px"
                    Font-Bold="true">
                </dx:ASPxLabel>
            </div>
        </HeaderContentTemplate>

        <ContentCollection>
            <dx:PopupControlContentControl>
                <div id="AddReviewContainer" runat="server" class="p-3">
                    <dx:ASPxFormLayout ID="AddReviewFormLayout" ClientInstanceName="AddReviewFormLayout" runat="server" AlignItemCaptions="true" AlignItemCaptionsInAllGroups="true" Width="100%">
                        <Items>
                            <dx:LayoutGroup ColSpan="1" GroupBoxDecoration="Box" Caption="Details" Name="ImprovementDetails" ClientVisible="false">
                                <Items>
                                    <dx:LayoutItem ColSpan="1" Caption="Require improvement reason">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">

                                                <dx:ASPxComboBox ID="ImpReasonComboBox"
                                                    ClientInstanceName="ImpReasonComboBox"
                                                    runat="server"
                                                    Width="200px"
                                                    DataSourceID="GetUnavailableReason" TextField="ReasonText" ValueField="ReasonUnavailableID">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="SubmitReview">
                                                        <RequiredField IsRequired="true" ErrorText="This field is required" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>

                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                    <dx:LayoutItem ColSpan="1" Caption="Review comment">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">

                                                <dx:ASPxMemo ID="ReviewCommentMemo" ClientInstanceName="ReviewCommentMemo" runat="server" Height="100px" Width="100%" MaxLength="1000">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="SubmitReview">
                                                        <RequiredField IsRequired="true" ErrorText="This field is required" />
                                                    </ValidationSettings>
                                                </dx:ASPxMemo>

                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>


                            <dx:LayoutGroup ColSpan="1" GroupBoxDecoration="Box" Caption="Action point details" Name="ActionPointsReview" ClientVisible="false">
                                <Items>
                                    <%-- <dx:LayoutItem ColSpan="1" Caption="Unavailable Reason">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">

                                                <dx:ASPxComboBox ID="ActReasonComboBox"
                                                    ClientInstanceName="ActReasonComboBox"
                                                    runat="server"
                                                    Width="200px"
                                                    DataSourceID="GetUnavailableReason" TextField="ReasonText" ValueField="ReasonUnavailableID">

                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="SubmitReview">
                                                        <RequiredField IsRequired="true" ErrorText="This field is required" />
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>


                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>--%>
                                    <dx:LayoutItem ColSpan="1" Caption="details">
                                        <LayoutItemNestedControlCollection>
                                            <dx:LayoutItemNestedControlContainer runat="server">

                                                <dx:ASPxMemo ID="ActReviewCommentMemo" ClientInstanceName="ActReviewCommentMemo" runat="server" Height="100px" Width="100%" MaxLength="1000">
                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" ValidationGroup="SubmitReview">
                                                        <RequiredField IsRequired="true" ErrorText="This field is required" />
                                                    </ValidationSettings>
                                                </dx:ASPxMemo>

                                            </dx:LayoutItemNestedControlContainer>
                                        </LayoutItemNestedControlCollection>
                                    </dx:LayoutItem>
                                </Items>
                            </dx:LayoutGroup>


                        </Items>
                    </dx:ASPxFormLayout>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <FooterContentTemplate>
            <div class="d-flex align-items-center">
                <div class="ms-auto px-3 py-1">
                    <dx:ASPxButton ID="SubmitReviewButton"
                        runat="server"
                        Text="Submit"
                        RenderMode="Button"
                        AutoPostBack="false"
                        UseSubmitBehavior="false"
                        CausesValidation="true"
                        ValidationGroup="SubmitReview">
                        <ClientSideEvents Click="SubmitReviewButton_Click" />
                    </dx:ASPxButton>
                </div>
                <div>
                    <dx:ASPxButton ID="CloseReviewButton"
                        runat="server"
                        Text="Close"
                        RenderMode="Link"
                        AutoPostBack="false"
                        UseSubmitBehavior="false"
                        CausesValidation="false">
                        <ClientSideEvents Click="CloseReviewButton_click" />
                    </dx:ASPxButton>
                </div>
            </div>
        </FooterContentTemplate>
    </dx:ASPxPopupControl>
    <dx:ASPxCallback ID="CompleteCallback" ClientInstanceName="CompleteCallback" OnCallback="CompleteCallback_Callback" runat="server"></dx:ASPxCallback>
    <uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />
</asp:Content>
