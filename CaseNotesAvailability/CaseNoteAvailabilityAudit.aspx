﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CaseNoteAvailabilityAudit.aspx.cs" Inherits="CaseNotesAvailability._CaseNoteAvailabilityAudit" %>

<%--<%@ Register Src="~/UserControl/UserAlertControl/UserAlertPopupControl.ascx" TagPrefix="uc1" TagName="UserAlertPopupControl" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CustomCSS/NotificationHelper.css" rel="stylesheet" />
    <script src="CustomJS/CaseNoteAvailabilityAudit.js"></script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ObjectDataSource ID="CaseNoteAvailabilityAuditRecordsView" runat="server" SelectMethod="GetAuditClinicAnswers" OnSelecting="CaseNoteAvailabilityAuditRecordsView_Selecting" UpdateMethod="UpdateAuditRecords" DeleteMethod=""
        OnUpdating="Audit_Updating" OnInserting="Audit_Inserting" TypeName="BLL.AuditClinicAnswersBLL" DataObjectTypeName="BusinessObjects.AuditClinicAnswersBO" InsertMethod="InsertAudit"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSpeciality" runat="server" SelectMethod="GetSpeciality" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GetSites" runat="server" SelectMethod="GetSites" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="Status" runat="server" SelectMethod="GetStatus" TypeName="BLL.AuditBLL"></asp:ObjectDataSource>
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
                                <br />
                                <div class="ml-auto text-right">
                                    <div>
                                        <dx:ASPxLabel ID="CasenoteLabel"
                                            ClientInstanceName="CasenoteLabel"
                                            runat="server"
                                            OnInit="CasenoteLabel_Init"
                                            Font-Size="20px"
                                            ForeColor="#316bff">
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

            <dx:ASPxGridView ID="CaseNoteAvailabilityAuditRecordsGridView" runat="server" AllowSorting="true"
                ClientInstanceName="CaseNoteAvailabilityAuditRecordsGridView"
                OnRowUpdating="AuditRow_Updating"
                KeyFieldName="AuditClinicAnswersID"
                DataSourceID="CaseNoteAvailabilityAuditRecordsView"
                AutoGenerateColumns="False"
                OnInitNewRow="CaseNoteAvailabilityAuditRecordsGridView_InitNewRow"
                OnStartRowEditing="CaseNoteAvailabilityAuditRecordsGridView_StartRowEditing"
                OnCellEditorInitialize="CaseNoteAvailabilityAuditRecordsGridView_CellEditorInitialize"
                Width="100%">
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
                                <dx:GridViewColumnLayoutItem ColumnName="AuditClinicAnswersID" Name="AuditClinicAnswersID" Caption="AuditClinic AnswersID" Width="100%" ColumnSpan="1"></dx:GridViewColumnLayoutItem>
                                <dx:GridViewColumnLayoutItem ColumnName="AuditID" ColumnSpan="1" Width="400px"></dx:GridViewColumnLayoutItem>
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
                    <dx:GridViewCommandColumn VisibleIndex="0" Width="100px" Caption="Action" ShowNewButtonInHeader="false" ShowEditButton="false" ShowClearFilterButton="true" ShowApplyFilterButton="true"></dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn Name="Action" VisibleIndex="0" MinWidth="75" AdaptivePriority="0" CellStyle-HorizontalAlign="Center" Caption="Action">
                        <DataItemTemplate>
                            <div>
                                <dx:ASPxButton ID="AuditorView"
                                    runat="server"
                                    Text="Choose"
                                    RenderMode="Button"
                                    AutoPostBack="false"
                                    UseSubmitBehavior="false"
                                    CausesValidation="false"
                                    OnInit="AuditorView_Init">
                                </dx:ASPxButton>
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn Caption="AuditClinicAnswersID" ReadOnly="true" FieldName="AuditClinicAnswersID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                        <EditItemTemplate>
                            <dx:ASPxLabel ID="AuditClinicAnswersIDReadonlyLabel" runat="server" Text='<%# Eval("AuditClinicAnswersID") %>'></dx:ASPxLabel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="AuditID" ReadOnly="true" FieldName="AuditID" VisibleIndex="1" MinWidth="50" MaxWidth="100">
                        <EditItemTemplate>
                            <dx:ASPxLabel ID="AuditIDReadonlyLabel" runat="server" Text='<%# Eval("AuditID") %>'></dx:ASPxLabel>
                        </EditItemTemplate>
                    </dx:GridViewDataTextColumn>
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
        <%--<uc1:UserAlertPopupControl runat="server" ID="UserAlertPopupControl" />--%>
</asp:Content>
<%-- Starts here  --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div class="p-4">

        <div id="DefaultPageTitleContainer1" class="DefaultPageTitleContainer">
            <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Please fill out the clinic audit details" CssClass="PageHeader"></dx:ASPxLabel>
        </div>

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
                                            LeftAndRightCaptionsWidth="300"
                                            AlignItemCaptionsInAllGroups="True"
                                            AlignItemCaptions="true"
                                            Width="100%">
                                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="676"></SettingsAdaptivity>
                                            <Items>
                                                <dx:LayoutItem Caption="Clinic Code :" Name="ClinicCode" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:Label ID="lblClinicCode1" runat="server" CssClass="mylabel2"></asp:Label>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="Number of Appointments :" Name="NumberofAppointments" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:TextBox ID="txtNumAppointments" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="Case notes available Start Count :" Name="CaseNoteAvailabileStartCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:TextBox ID="txtStartCount" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="Temporary notes count :" Name="TempNotesCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:TextBox ID="txtTempNotesCount" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Caption="Unavailable case note count :" Name="UnavailableCaseNotesCount" Visible="true" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <asp:TextBox ID="txtCaseNoteCount" runat="server" Width="200px" MaxLength="50"></asp:TextBox>
                                                        </dx:LayoutItemNestedControlContainer>
                                                    </LayoutItemNestedControlCollection>

                                                    <CaptionStyle CssClass="Form-Caption-Style"></CaptionStyle>
                                                </dx:LayoutItem>

                                                <dx:LayoutItem Name="UnavailableCaseNoteCount" Caption="Unavailable case note count :" ColumnSpan="1" Width="100%" CaptionStyle-CssClass="Form-Caption-Style">
                                                    <LayoutItemNestedControlCollection>
                                                        <dx:LayoutItemNestedControlContainer runat="server">
                                                            <dx:ASPxSpinEdit ID="UnavailableCaseNoteCount" ClientInstanceName="UnavailableCaseNoteCount" runat="server" DecimalPlaces="2"
                                                                NumberType="Float" Width="200px" AllowMouseWheel="false" SpinButtons-ShowIncrementButtons="false">
                                                                <SpinButtons ShowIncrementButtons="False"></SpinButtons>

                                                                <ClientSideEvents ValueChanged="UnavailableCaseNoteCount_SelectedIndexChanged" />
                                                                <ValidationSettings ErrorDisplayMode="ImageWithText" ValidationGroup="FurtherDetails" Display="Dynamic" EnableCustomValidation="true"
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

                                <div class="p-4">
                                    <div class="d-flex align-items-center">
                                        <div>
                                            <dx:ASPxButton ID="CompleteButton"
                                                ClientInstanceName="CompleteButton"
                                                runat="server"
                                                Text="Complete"
                                                AutoPostBack="false"
                                                UseSubmitBehavior="false">
                                                <ClientSideEvents Click="Complete_Click" />
                                            </dx:ASPxButton>
                                        </div>
                                        <%-- <div id="NoReferralsContainer" runat="server" class="mt-3" visible="false">
                                            <dx:ASPxLabel ID="NoReferralsLabel" runat="server"></dx:ASPxLabel>
                                        </div>--%>
                                        <%--<dx:GridViewDataCheckColumn FieldName="DocumentsToUpload" VisibleIndex="1" Visible="false"></dx:GridViewDataCheckColumn>--%>
                                    </div>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                    <dx:ASPxCallbackPanel ID="CreateFormDynamically_CallbackPanel" ClientVisible="false" ClientInstanceName="CreateFormDynamically_CallbackPanel" OnCallback="CreateFormDynamically_CallbackPanel_Callback" runat="server">
                        <PanelCollection>
                            <dx:PanelContent>
                                <div id="DynamicFormContainer" runat="server"></div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>

</asp:Content>