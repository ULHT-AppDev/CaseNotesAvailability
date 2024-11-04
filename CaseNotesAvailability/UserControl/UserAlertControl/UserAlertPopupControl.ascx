<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAlertPopupControl.ascx.cs" Inherits="UserControls.UserAlertControl.UserAlertPopupControl" %>

<dx:ASPxPopupControl ID="UserToastNotification"
    ClientInstanceName="UserToastNotification"
    runat="server"
    ClientIDMode="Static"
    ShowHeader="false"
    CloseAnimationType="Fade"
    AllowDragging="false"
    AllowResize="false"
    Cursor="pointer"
    CloseAction="None"
    PopupHorizontalAlign="LeftSides"
    PopupVerticalAlign="Below"
    PopupHorizontalOffset="20"
    PopupVerticalOffset="20"
    PopupAnimationType="Slide"
    AutoUpdatePosition="false"
    CssClass="UserNotificationToastPopup"
    Width="400px">
    <ClientSideEvents Shown="OnAlertShown" />

    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <div class="d-flex align-items-center">
                <div class="ms-1">
                    <dx:ASPxLabel ID="UserToastNotificationIconLabel"
                        ClientInstanceName="UserToastNotificationIconLabel"
                        runat="server"
                        ForeColor="#ffffff"
                        EncodeHtml="false"
                        Font-Size="X-Large">
                    </dx:ASPxLabel>
                </div>
                <div class="ms-3">
                    <div>
                        <dx:ASPxLabel ID="UserToastPopupHeaderLabel"
                            ClientInstanceName="UserToastPopupHeaderLabel"
                            EncodeHtml="false"
                            runat="server"
                            Font-Size="16px"
                            Font-Bold="true"
                            ForeColor="#ffffff">
                        </dx:ASPxLabel>
                    </div>
                    <div>
                        <dx:ASPxLabel ID="UserToastPopupContentLabel"
                            ClientInstanceName="UserToastPopupContentLabel"
                            CssClass="SuccessPopupMessageLabelClass"
                            runat="server"
                            Font-Size="Small"
                            ForeColor="#fafafa"
                            EncodeHtml="false">
                        </dx:ASPxLabel>
                    </div>
                </div>
            </div>


        </dx:PopupControlContentControl>
    </ContentCollection>


</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="CustomAlertPopup"
    ClientInstanceName="CustomAlertPopup"
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
    <ClientSideEvents CloseButtonClick="CustomAlertPopupCloseButton_Click" />
    <SettingsAdaptivity Mode="Always" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />

    <HeaderContentTemplate>
        <div class="ps-3">
            <dx:ASPxLabel ID="CustomAlertPopupHeaderLabel"
                ClientInstanceName="CustomAlertPopupHeaderLabel"
                EncodeHtml="false"
                runat="server"
                Font-Size="16px"
                Font-Bold="true">
            </dx:ASPxLabel>
        </div>
    </HeaderContentTemplate>

    <ContentCollection>
        <dx:PopupControlContentControl>
            <div class="p-3">
                <dx:ASPxLabel ID="CustomAlertPopupMessage" ClientInstanceName="CustomAlertPopupMessage" runat="server"></dx:ASPxLabel>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <FooterContentTemplate>
        <div class="d-flex align-items-center">
            <div class="ms-auto px-2 py-1">
                <dx:ASPxButton ID="CustomAlertPopupCloseButton"
                    runat="server"
                    Text="Close"
                    RenderMode="Link"
                    AutoPostBack="false"
                    UseSubmitBehavior="false"
                    CausesValidation="false">
                    <ClientSideEvents Click="CustomAlertPopupCloseButton_Click" />
                </dx:ASPxButton>
            </div>
        </div>
    </FooterContentTemplate>

</dx:ASPxPopupControl>
