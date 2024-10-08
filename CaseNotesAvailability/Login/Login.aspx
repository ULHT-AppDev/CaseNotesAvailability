﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login.Login" %>

<%@ Register Assembly="DevExpress.Web.v23.2, Version=23.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=0" />

    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <script src="<%=Page.ResolveUrl(String.Format("{0}?{1}", "~/Scripts/jquery-3.6.0.min.js", VersionNumber))%>"></script>
        <script src="<%=Page.ResolveUrl(String.Format("{0}?{1}", "~/Scripts/jquery-ui-1.13.0.min.js", VersionNumber))%>"></script>
        <script src="<%=Page.ResolveUrl(String.Format("{0}?{1}","~/Scripts/bootstrap.min.js", VersionNumber))%>"></script>
        <link href="<%=Page.ResolveUrl(String.Format("{0}?{1}","~/Content/bootstrap.min.css", VersionNumber))%>" rel="stylesheet" />

        <script src="<%=Page.ResolveUrl(String.Format("{0}?{1}","~/Scripts/fontawesome/all.min.js", VersionNumber))%>"></script>
        <link href="<%=Page.ResolveUrl(String.Format("{0}?{1}","~/Content/fontawesome.min.css", VersionNumber))%>" rel="stylesheet" />

        <script src="<%=Page.ResolveUrl(String.Format("{0}?{1}", "Login.js", VersionNumber))%>"></script>
        <link href="<%=Page.ResolveUrl(String.Format("{0}?{1}","Login.css", VersionNumber))%>" rel="stylesheet" />
    </asp:PlaceHolder>

    <title></title>

</head>
<body>

    <form id="form1" runat="server">

        <dx:ASPxGlobalEvents ID="globalEvents" runat="server">
            <ClientSideEvents ControlsInitialized="OnPageControlsInit" />
        </dx:ASPxGlobalEvents>

        <div id="HeaderContainer" class="container-fluid">
            <div id="HeaderSubContainer" class="container">
                <div id="HeaderRow" class="row align-items-center">

                    <div id="PageTitleContainer" class="col-md-6 pt-3 pb-3">
                        <h1 id="PageTitle">
                            <asp:Literal ID="LoginPageTitle" runat="server" />
                        </h1>
                        <div id="VersionNumber" class="ps-1" style="color: #e69500;">
                            <asp:Literal ID="VersionNumberText" runat="server" />
                        </div>
                    </div>

                    <div id="CompanyLogoContainer" class="col-md-6">
                        <div class="w-100 text-end">
                            <img id="CompanyLogo" src="ULH_Logo.png" title="ULHT Logo" />
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <dx:ASPxCallbackPanel ID="FullLoginCallbackPanel"
            ClientInstanceName="FullLoginCallbackPanel"
            CssClass="mt-5 pb-5"
            runat="server"
            Width="100%"
            OnCallback="FullLoginCallbackPanel_Callback">
            <ClientSideEvents EndCallback="FullLoginCallbackPanelEndCallback" />
            <SettingsLoadingPanel Enabled="false" />
            <PanelCollection>
                <dx:PanelContent>

                    <dx:ASPxHiddenField ID="ShowNoticeHiddenField" ClientInstanceName="ShowNoticeHiddenField" runat="server"></dx:ASPxHiddenField>

                    <!-- Maintenance Modal -->
                    <div class="modal fade" id="myModal" data-backdrop="static" data-keyboard="false">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <div class="container-fluid">
                                        <div id="modalHeader" class="row align-items-center">
                                            <dx:ASPxLabel ID="ModalHeaderLabel" runat="server" CssClass="modal-title font-XL ps-3" EncodeHtml="false"></dx:ASPxLabel>
                                        </div>
                                    </div>
                                    <button type="button" class="close" data-bs-dismiss="modal">&times;</button>
                                </div>
                                <div class="modal-body">
                                    <dx:ASPxLabel ID="ModalContenetLabel" runat="server" EncodeHtml="false"></dx:ASPxLabel>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="LoginContainer" class="container">
                        <div class="row">
                            <div class="col-lg-4"></div>

                            <div id="LoginDiv" class="col-lg-4">

                                <div id="WelcomeHeaderText">
                                    <dx:ASPxLabel ID="LoginInformation"
                                        Font-Size="Large"
                                        runat="server"
                                        Theme="Material"
                                        Text="Login with your <strong>Network Account</strong>"
                                        EncodeHtml="false"
                                        class="pe-2">
                                    </dx:ASPxLabel>
                                </div>

                                <dx:ASPxComboBox ID="DomainComboBox"
                                    ClientInstanceName="DomainComboBox"
                                    runat="server"
                                    ValueType="System.String"
                                    SelectedIndex="0"
                                    Width="100%"
                                    Caption="Domain">
                                    <Items>
                                        <dx:ListEditItem Text="ULH" Value="1" />
                                        <dx:ListEditItem Text="LSS" Value="2" />
                                    </Items>
                                    <CaptionSettings Position="Top" />
                                    <CaptionStyle Font-Size="Medium"></CaptionStyle>
                                    <CaptionCellStyle>
                                        <Paddings PaddingTop="20px" PaddingBottom="5px" />
                                    </CaptionCellStyle>
                                    <Paddings Padding="10px" />
                                    <ItemStyle HoverStyle-BackColor="#e6e6e6" HoverStyle-ForeColor="#484848" />
                                    <FocusedStyle CssClass="LoginEditorFocusStyle focusStylePriority"></FocusedStyle>
                                </dx:ASPxComboBox>

                                <dx:ASPxTextBox ID="FullUsernameTextBox"
                                    ClientIDMode="Static"
                                    ClientInstanceName="FullUsernameTextBox"
                                    runat="server"
                                    Width="100%"
                                    NullText="Username"
                                    Caption="Username">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic">
                                        <RequiredField ErrorText="Username is required" IsRequired="true" />
                                        <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Username must not contain illegal characters" />
                                    </ValidationSettings>
                                    <CaptionSettings Position="Top" />
                                    <CaptionStyle Font-Size="Medium"></CaptionStyle>
                                    <CaptionCellStyle>
                                        <Paddings PaddingTop="20px" PaddingBottom="5px" />
                                    </CaptionCellStyle>
                                    <Paddings Padding="10px" />
                                    <FocusedStyle CssClass="LoginEditorFocusStyle focusStylePriority"></FocusedStyle>
                                </dx:ASPxTextBox>

                                <dx:ASPxTextBox ID="FullPasswordTextBox"
                                    ClientInstanceName="FullPasswordTextBox"
                                    runat="server"
                                    Width="100%"
                                    Password="true"
                                    NullText="Password"
                                    Caption="Password">
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic">
                                        <RequiredField ErrorText="Password is required" IsRequired="true" />
                                        <RegularExpression ValidationExpression="^[^<>]+$" ErrorText="Password contains illegal characters" />
                                    </ValidationSettings>
                                    <CaptionSettings Position="Top" />
                                    <CaptionStyle Font-Size="Medium"></CaptionStyle>
                                    <CaptionCellStyle>
                                        <Paddings PaddingTop="20px" PaddingBottom="5px" />
                                    </CaptionCellStyle>
                                    <Paddings Padding="10px" />
                                    <FocusedStyle CssClass="LoginEditorFocusStyle focusStylePriority"></FocusedStyle>
                                </dx:ASPxTextBox>

                                <div id="DoNotSavePasswordWarningContainer" class="mt-3">
                                    <dx:ASPxLabel ID="DoNotSavePassword"
                                        runat="server"
                                        Text="<i class='fa fa-exclamation-triangle' style='color:orange;'></i> Do not save password if prompted."
                                        EncodeHtml="false"
                                        Theme="Material"
                                        Font-Size="14px">
                                    </dx:ASPxLabel>
                                </div>

                                <div class="mt-4 mb-3">

                                    <div id="ScheduledMaintenanceDiv" class="pb-3" runat="server">
                                        <asp:Label ID="ScheduledMaintenanceLabel" runat="server"></asp:Label>
                                    </div>

                                    <dx:ASPxButton ID="LoginButton"
                                        ClientInstanceName="LoginButton"
                                        runat="server"
                                        Width="100%"
                                        AutoPostBack="false"
                                        ImagePosition="right"
                                        ImageSpacing="10">
                                        <Image>
                                            <SpriteProperties CssClass="fas fa-chevron-right" />
                                        </Image>
                                        <ClientSideEvents Click="OnLoginClick" />
                                    </dx:ASPxButton>
                                </div>

                                <dx:ASPxLoadingPanel ID="LoginLoadingPanel"
                                    ClientInstanceName="LoginLoadingPanel"
                                    ClientIDMode="Static"
                                    Text="Logging in..."
                                    runat="server"
                                    VerticalAlign="Bottom">
                                </dx:ASPxLoadingPanel>

                                <div class="col-md-12 p-1">
                                    <dx:ASPxLabel ID="ErrorLoginLabel"
                                        ClientInstanceName="FullLoginErrorLabel"
                                        runat="server"
                                        Theme="Material"
                                        ForeColor="Red"
                                        EncodeHtml="false"
                                        Font-Size="14px">
                                    </dx:ASPxLabel>
                                </div>

                                <div class="col-lg-4"></div>

                            </div>

                            <div id="RoleContent" class="col-lg-4 hideContent">

                                <dx:ASPxButton ID="GoBackButton"
                                    ClientInstanceName="RoleSelectGoBackButton"
                                    runat="server"
                                    Text="Go Back"
                                    AutoPostBack="false"
                                    CausesValidation="false"
                                    UseSubmitBehavior="false"
                                    RenderMode="Link"
                                    ImagePosition="Left"
                                    ImageSpacing="10">
                                    <Image>
                                        <SpriteProperties CssClass="fas fa-chevron-left" />
                                    </Image>
                                    <ClientSideEvents Click="OnGoBackClick" />
                                </dx:ASPxButton>

                                <div class="mt-4">
                                    <span class='fa fa-exclamation-triangle' style='color: orange;'></span>
                                    <dx:ASPxLabel ID="popupHeaderLabel"
                                        runat="server"
                                        Text="Multiple <strong>Roles</strong> Found"
                                        Theme="Material"
                                        Font-Size="Large"
                                        CssClass="ps-2"
                                        Font-Bold="true" EncodeHtml="false">
                                    </dx:ASPxLabel>
                                </div>

                                <div class="w-100 mt-3">
                                    <dx:ASPxLabel ID="MultipleRolesInfo"
                                        runat="server"
                                        EncodeHtml="false"
                                        Text="<i>Multiple roles have been found for you in this system.</i> <br /><br />Select below which role you would like to enter the system with.<br/> <span style='color:orange'>Note:</span> You will not be able to change role until you log out of the system and log back in.">
                                    </dx:ASPxLabel>
                                </div>
                                <div id="RoleChoiceContainer" class="mt-3">
                                    <dx:ASPxComboBox ID="RoleChoiceCombobox"
                                        ClientInstanceName="RoleChoiceCombobox"
                                        runat="server"
                                        Width="100%" TextField="Value" ValueField="Key"
                                        Caption="Choose a role:">
                                        <ValidationSettings ValidationGroup="RoleSelect" Display="Static" ErrorTextPosition="Bottom">
                                            <RequiredField IsRequired="true" ErrorText="A role is required to continue" />
                                        </ValidationSettings>
                                        <CaptionSettings Position="Top" />
                                        <CaptionStyle Font-Size="Medium"></CaptionStyle>
                                        <CaptionCellStyle>
                                            <Paddings PaddingTop="20px" PaddingBottom="5px" />
                                        </CaptionCellStyle>
                                        <Paddings Padding="10px" />
                                        <FocusedStyle CssClass="LoginEditorFocusStyle focusStylePriority"></FocusedStyle>
                                        <ItemStyle HoverStyle-BackColor="#e6e6e6" HoverStyle-ForeColor="#484848" />
                                    </dx:ASPxComboBox>
                                </div>

                                <div class="mt-4 mb-3 w-100">
                                    <dx:ASPxButton ID="ConfirmRoleButton"
                                        ClientInstanceName="ConfirmRoleButton"
                                        runat="server"
                                        Text="Confirm Role"
                                        AutoPostBack="false"
                                        CausesValidation="true"
                                        ValidationGroup="RoleSelect"
                                        UseSubmitBehavior="false"
                                        Width="100%"
                                        ImagePosition="Right"
                                        ImageSpacing="10">
                                        <Image>
                                            <SpriteProperties CssClass="fas fa-check" />
                                        </Image>
                                        <ClientSideEvents Click="ConfirmRoleClick" />
                                    </dx:ASPxButton>
                                </div>

                                <dx:ASPxLoadingPanel ID="RolesLoginLoadingPanel"
                                    ClientInstanceName="RolesLoginLoadingPanel"
                                    ClientIDMode="Static"
                                    Text="Logging in..."
                                    runat="server"
                                    VerticalAlign="Bottom">
                                </dx:ASPxLoadingPanel>

                            </div>

                            <div class="col-lg-4"></div>

                        </div>
                    </div>

                    <dx:ASPxHiddenField ID="RoleChoiceHiddenField" ClientInstanceName="RoleChoiceHiddenField" runat="server"></dx:ASPxHiddenField>

                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxCallbackPanel>

        <dx:ASPxGlobalEvents ID="ASPxGlobalEvents1" runat="server">
            <ClientSideEvents ControlsInitialized="OnControlsInitialised" />
        </dx:ASPxGlobalEvents>

        <dx:ASPxPopupControl ID="BetterExperiencePopup"
            ClientInstanceName="BetterExperiencePopup"
            runat="server" Modal="true"
            ModalBackgroundStyle-Opacity="015"
            PopupVerticalAlign="WindowCenter"
            PopupHorizontalAlign="WindowCenter"
            AutoUpdatePosition="true"
            AllowDragging="true"
            CloseAction="CloseButton"
            ShowFooter="true"
            Width="500px">

            <HeaderContentTemplate>
                <div class="ps-3">
                    <dx:ASPxLabel ID="BEPopupHeaderLabel" runat="server" Text="Consider using a modern browser" Font-Bold="true" Font-Size="Large"></dx:ASPxLabel>
                </div>
            </HeaderContentTemplate>

            <ContentCollection>
                <dx:PopupControlContentControl>

                    <div class="px-3 pb-3">
                        <dx:ASPxLabel ID="BEContentLabel"
                            runat="server"
                            EncodeHtml="false"
                            Text="For a better experience whilst using this application, use a modern browser such as...">
                        </dx:ASPxLabel>

                        <ul class="p-3 mb-0">
                            <li>Microsoft Edge</li>
                            <li>Google Chrome</li>
                            <li>Mozilla Firefox</li>
                        </ul>
                        <%--                            <li><a href="https://www.microsoft.com/en-us/edge">Microsoft Edge</a></li>
                            <li><a href="https://www.google.com/chrome/">Google Chrome</a></li>
                            <li><a href="https://www.mozilla.org/en-GB/">Mozilla Firefox</a></li>--%>

                        <div>
                            <dx:ASPxLabel ID="RatherThanLabel" ClientInstanceName="RatherThanLabel" runat="server" Text="Rather than using Internet Explorer."></dx:ASPxLabel>
                        </div>

                    </div>

                </dx:PopupControlContentControl>
            </ContentCollection>

            <FooterTemplate>
                <div class="p-3 text-center">
                    <dx:ASPxButton ID="HidePopupButton" runat="server" Text="Okay" Width="200px" AutoPostBack="false" CausesValidation="false" UseSubmitBehavior="false">
                        <ClientSideEvents Click="function(s, e){ BetterExperiencePopup.Hide(); }" />
                    </dx:ASPxButton>
                </div>
            </FooterTemplate>
        </dx:ASPxPopupControl>

    </form>
</body>
</html>
