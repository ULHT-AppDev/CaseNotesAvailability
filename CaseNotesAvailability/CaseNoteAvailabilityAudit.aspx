<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CaseNoteAvailabilityAudit.aspx.cs" Inherits="CaseNotesAvailability._CaseNoteAvailabilityAudit" %>

<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CustomCSS/NotificationHelper.css" rel="stylesheet" />
    <script src="CustomJS/CaseNoteAvailabilityAudit.js"></script>
    <script src="UserControl/UserAlertControl/Scripts/UserAlert.js"></script>
    <link href="UserControl/UserAlertControl/CSS/UserAlert.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="CaseNoteAvailabilityAuditRecordsView" runat="server" SelectMethod="GetAuditClinicAnswers" OnSelecting="CaseNoteAvailabilityAuditRecordsView_Selecting" UpdateMethod="UpdateAuditRecords" DeleteMethod=""
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditClinicAnswersBLL" DataObjectTypeName="BusinessObjects.AuditClinicAnswersBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="Status" runat="server" SelectMethod="GetStatus" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <div>


        <div class="container-fluid p-4 mb-5">
            <div class="row">
                <div class="col-lg-12">
                    <div id="DefaultPageTitleContainer1" class="DefaultPageTitleContainer d-flex align-items-center">

                        <div id="DefaultPageTitleContainer" class="DefaultPageTitleContainer d-flex align-items-center">
                            <div>
                                <dx:ASPxLabel ID="ASPxLabel1"
                                    runat="server"
                                    OnInit="DefaultPageTitleLabel_Init"
                                    CssClass="PageHeader"
                                    EncodeHtml="false"
                                    Font-Bold="true"
                                    Font-Size="20px">
                                </dx:ASPxLabel>
                                <br />
                                <asp:Panel ID="Panel2" runat="server">
                                    <div>
                                        <div>
                                            <dx:ASPxLabel ID="ASPxLabel2"
                                                ClientInstanceName="CasenoteLabel"
                                                runat="server"
                                                OnInit="CasenoteLabel_Init"
                                                ForeColor="GrayText"
                                                EncodeHtml="false">
                                            </dx:ASPxLabel>
                                        </div>
                                        <div>
                                            <dx:ASPxLabel ID="ASPxLabel3"
                                                ClientInstanceName="lblSpeciality"
                                                runat="server"
                                                OnInit="lblSpeciality_Init"
                                                ForeColor="GrayText"
                                                EncodeHtml="false">
                                            </dx:ASPxLabel>
                                        </div>
                                        <div>
                                            <dx:ASPxLabel ID="ASPxLabel4"
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
                   <br />
                </div>
            </div>
            <div class="ml-auto text-black text-right">
            </div>
            <div>
                <dx:ASPxButton ID="AuditDetails"
                    runat="server"
                    Text="Go back to Audit page"
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
                    <br />
            <dx:ASPxGridView ID="CaseNoteAvailabilityAuditRecordsGridView" runat="server" AllowSorting="true"
                ClientInstanceName="CaseNoteAvailabilityAuditRecordsGridView"
                OnRowUpdating="AuditRow_Updating"
                KeyFieldName="AuditClinicAnswersID"
                DataSourceID="CaseNoteAvailabilityAuditRecordsView"
                AutoGenerateColumns="False"
                OnInitNewRow="CaseNoteAvailabilityAuditRecordsGridView_InitNewRow"
                OnStartRowEditing="CaseNoteAvailabilityAuditRecordsGridView_StartRowEditing"
                OnCellEditorInitialize="CaseNoteAvailabilityAuditRecordsGridView_CellEditorInitialize"
                OnCustomCallback="CaseNoteAvailabilityAuditRecordsGridView_CustomCallback"
                Width="100%">
                <ClientSideEvents EndCallback="CaseNoteAvailabilityAuditRecordsGridView_EndCallBack" />
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
                                <dx:GridViewColumnLayoutItem ColumnName="AuditID" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
                                <%--<dx:GridViewColumnLayoutItem ColumnName="ClinicCode" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>--%>
                                <dx:GridViewColumnLayoutItem Caption="Total Appointments" ColumnName="Totalappointments" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
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
                                    Text="Complete Audit"
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
                    <dx:GridViewDataTextColumn Caption="Total Appointments" FieldName="Totalappointments" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Number Of Appointments Allocated" FieldName="NumberOfAppointmentsAllocated" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="CaseNotes Available Start Count" FieldName="CaseNotesAvailableStartCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Temporary Notes Count" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>
                    <%--<dx:GridViewDataTextColumn Caption="Temporary Notes Count" FieldName="TemporaryNotesCount" PropertiesTextEdit-MaxLength="50" VisibleIndex="6" MinWidth="200" MaxWidth="200"></dx:GridViewDataTextColumn>--%>
                    <%-- <dx:GridViewDataButtonEditColumn VisibleIndex="1">
                    <DataItemTemplate>
                        <dx:ASPxButton ID="AuditorView"
                            runat="server"
                            Text="Choose"
                            RenderMode="Button"
                            AutoPostBack="false"
                            UseSubmitBehavior="false"
                            CausesValidation="false">
                        </dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataButtonEditColumn>--%>
                </Columns>

                <Settings ShowFilterRow="true" />
                <SettingsBehavior AllowEllipsisInText="true" />
                <SettingsResizing ColumnResizeMode="NextColumn" />

            </dx:ASPxGridView>


        </div>
        <uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />
    </div>
</asp:Content>
<%-- Starts here  --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="p-4">

        <%--  <div id="DefaultPageTitleContainer1" class="DefaultPageTitleContainer">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Please fill out the clinic audit details" CssClass="PageHeader"></dx:ASPxLabel>
        </div>--%>

        <dx:ASPxPopupControl ID="CaseNoteAvailabilityUnAvailabilityPopup"
            ClientInstanceName="CaseNoteAvailabilityUnAvailabilityPopup"
            runat="server"
            PopupVerticalAlign="WindowCenter"
            PopupHorizontalAlign="WindowCenter"
            CloseAction="CloseButton"
            AutoUpdatePosition="true"
            AllowDragging="true"
            Modal="true"
            ModalBackgroundStyle-Opacity="025"
            ShowHeader="true"
            ShowFooter="false" HeaderStyle-Border-BorderWidth="0" HeaderStyle-Paddings-PaddingBottom="0" ContentStyle-Paddings-PaddingTop="0">
            <SettingsAdaptivity MinWidth="95%" MinHeight="90%" Mode="Always" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
            <ClientSideEvents Closing="popup_Closing" />
            <ContentStyle>
                <Paddings PaddingTop="0px"></Paddings>
            </ContentStyle>

            <HeaderStyle>
                <Paddings PaddingBottom="0px"></Paddings>
                <Border BorderWidth="0px"></Border>
            </HeaderStyle>

            <ModalBackgroundStyle Opacity="25"></ModalBackgroundStyle>

            <HeaderContentTemplate>
            </HeaderContentTemplate>

            <ContentCollection>
                <dx:PopupControlContentControl>

                    <dx:ASPxCallbackPanel ID="CaseNoteAvailabilityUnAvailabilityCallbackPanel"
                        ClientInstanceName="CaseNoteAvailabilityUnAvailabilityCallbackPanel"
                        runat="server"
                        Width="100%"
                        HideContentOnCallback="true"
                        OnCallback="CaseNoteAvailabilityUnAvailabilityCallbackPanel_Callback">
                        <ClientSideEvents EndCallback="CaseNoteAvailabilityUnAvailabilityCallbackPanel_EndCallback" />
                        <PanelCollection>
                            <dx:PanelContent>
                                <div class="px-4 pb-3 defaultBorderBottom">
                                    <div>
                                        <dx:ASPxLabel ID="CaseNoteAvailabilityUnAvailabilityHeaderLabel" Text="Please fill out the clinic audit details" runat="server" Font-Bold="true" Font-Size="16px"></dx:ASPxLabel>
                                    </div>
                                    <div>
                                        <dx:ASPxLabel ID="ClinicCode" runat="server" ForeColor="Gray" EncodeHtml="false"></dx:ASPxLabel>
                                    </div>
                                    <div>
                                        <%--  <dx:ASPxLabel ID="ASPxLabel2"
                                            runat="server"
                                            CssClass="PageHeader"
                                            EncodeHtml="false">
                                        </dx:ASPxLabel>--%>
                                        <%--<dx:ASPxLabel ID="ASPxLabel2" runat="server" ForeColor="Gray" EncodeHtml="false"></dx:ASPxLabel>--%>
                                        <%--<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>--%>
                                        <br />
                                        <br />
                                        <dx:ASPxFormLayout ID="ReferralTopSectionFormLayout"
                                            runat="server"
                                            LeftAndRightCaptionsWidth="380"
                                            AlignItemCaptionsInAllGroups="True"
                                            AlignItemCaptions="true"
                                            Width="100%">
                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="800"></SettingsAdaptivity>
                                            <Items>
                                                <dx:LayoutItem Caption="Audit Clinic AnswerId :" ClientVisible="false" Name="ClinicCode" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxLabel ID="lblAuditClinicAnswerId" runat="server" ClientInstanceName="lblAuditClinicAnswerId"></dx:ASPxLabel>--%>
                                                            <dx:ASPxTextBox ID="txtAuditClinicAnswerId" ClientInstanceName="txtAuditClinicAnswerId" ValidationSettings-ValidationGroup="CaseNoteVal" runat="server" Width="170px"></dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="AuditId :" ClientVisible="false" Name="AuditId" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxLabel ID="lblAuditClinicAnswerId" runat="server" ClientInstanceName="lblAuditClinicAnswerId"></dx:ASPxLabel>--%>
                                                            <dx:ASPxTextBox ID="txtAuditId" ClientInstanceName="txtAuditId" ValidationSettings-ValidationGroup="CaseNoteVal" runat="server" Width="170px"></dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="Clinic Code :" Name="ClinicCode" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <%--<dx:ASPxLabel ID="lblClinicCode1" runat="server" ClientInstanceName="lblClinicCode1"></dx:ASPxLabel>--%>
                                                            <dx:ASPxTextBox ID="txtClinicCode" ClientEnabled="false" ValidationSettings-ValidationGroup="CaseNoteVal" ClientInstanceName="txtClinicCode" runat="server" Width="200px"></dx:ASPxTextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Total appointments  :" Name="Totalappointments" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="txtTotalAppointments" ClientInstanceName="txtTotalAppointments" runat="server" DecimalPlaces="2"
                                                                NumberType="Float" Width="200px" AllowMouseWheel="false" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                <%--<ClientSideEvents ValueChanged="UnavailableCaseNoteCount_SelectedIndexChanged" />--%>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="CaseNoteVal" Display="Dynamic" EnableCustomValidation="true"
                                                                    ErrorTextPosition="Right" SetFocusOnError="true">
                                                                    <RequiredField ErrorText="This field is required" IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Actual Appointments :" Name="NumberofAppointments" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="txtNumAppointments" ClientInstanceName="txtActualAppointments" runat="server" DecimalPlaces="2"
                                                                NumberType="Float" Width="200px" AllowMouseWheel="false" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                <%--<ClientSideEvents ValueChanged="UnavailableCaseNoteCount_SelectedIndexChanged" />--%>
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="CaseNoteVal" Display="Dynamic" EnableCustomValidation="true"
                                                                    ErrorTextPosition="Right" SetFocusOnError="true">
                                                                    <RequiredField ErrorText="This field is required" IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="How many case notes were available at the start of clinic? :" Name="CaseNoteAvailabileStartCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="txtStartCount" ClientInstanceName="txtStartCount" runat="server" DecimalPlaces="2"
                                                                NumberType="Float" Width="200px" AllowMouseWheel="false" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                <ClientSideEvents Validation="StartCount_Validation" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="CaseNoteVal" Display="Dynamic" EnableCustomValidation="true"
                                                                    ErrorTextPosition="Right" SetFocusOnError="true">
                                                                    <RequiredField ErrorText="This field is required" IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>
                                                <dx:LayoutItem Caption="Of the notes provided how many were Temporary Notes? :" Name="TempNotesCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="txtTempNotesCount" ClientInstanceName="txtTempNotesCount" runat="server"
                                                                NumberType="Integer" Width="200px" AllowMouseWheel="false" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                <ClientSideEvents Validation="TempCount_Validation" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="CaseNoteVal" Display="Dynamic" EnableCustomValidation="true"
                                                                    ErrorTextPosition="Right" SetFocusOnError="true">
                                                                    <RequiredField ErrorText="This field is required" IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </dx:LayoutItemNestedControlContainer>

                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <%-- <dx:LayoutItem Caption="Unavailable case note count :" Name="UnavailableCaseNotesCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:TextBox ID="txtCaseNoteCount" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>
                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>--%>

                                                <dx:LayoutItem Name="UnavailableCaseNoteCount" Caption="Number of case notes Unavailable :" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="txtUnavailableCaseNoteCount" ClientInstanceName="txtUnavailableCaseNoteCount" runat="server"
                                                                NumberType="Integer" Width="200px" AllowMouseWheel="false" MinValue="0" MaxValue="10" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>
                                                                <ClientSideEvents Validation="UnavailableCaseNoteCount_SelectedIndexChanged" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="CaseNoteVal" Display="Dynamic" EnableCustomValidation="true"
                                                                    ErrorTextPosition="Right" SetFocusOnError="true">
                                                                    <RequiredField ErrorText="This field is required" IsRequired="true" />
                                                                </ValidationSettings>
                                                            </dx:ASPxSpinEdit>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                            </Items>
                                        </dx:ASPxFormLayout>
                                    </div>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <dx:ASPxCallbackPanel ID="CreateFormDynamically_CallbackPanel" ClientVisible="false" ClientInstanceName="CreateFormDynamically_CallbackPanel" OnCallback="CreateFormDynamically_CallbackPanel_Callback" runat="server">
                        <PanelCollection>
                            <dx:PanelContent>
                                <div id="DynamicFormContainer" runat="server">
                                    <dx:ASPxFormLayout ID="UnavailabilityFormLayout" ClientInstanceName="UnavailabilityFormLayout" runat="server" ColCount="1">
                                    </dx:ASPxFormLayout>
                                </div>

                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <div class="p-4">
                        <div class="d-flex align-items-center">
                            <div>
                                <dx:ASPxButton ID="CompleteButton"
                                    ClientInstanceName="CompleteButton"
                                    runat="server"
                                    Text="Complete"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    ValidationGroup="CaseNoteVal"
                                    CausesValidation="true"
                                    OnInit="CompleteButton_Init">
                                </dx:ASPxButton>
                            </div>
                            <%-- <div id="NoReferralsContainer" runat="server" class="mt-3" visible="false">
<dx:ASPxLabel ID="NoReferralsLabel" runat="server"></dx:ASPxLabel>
</div>--%>
                            <%--<dx:GridViewDataCheckColumn FieldName="DocumentsToUpload" VisibleIndex="1" Visible="false"></dx:GridViewDataCheckColumn>--%>
                        </div>

                    </div>
                    <dx:ASPxCallback ID="CompleteCallback" ClientInstanceName="CompleteCallback" OnCallback="CompleteCallback_Callback" runat="server"></dx:ASPxCallback>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
</asp:Content>
