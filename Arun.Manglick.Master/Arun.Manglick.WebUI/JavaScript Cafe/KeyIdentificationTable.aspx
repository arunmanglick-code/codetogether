<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="KeyIdentificationTable.aspx.cs" Inherits="KeyIdentificationTable" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeaderContent" runat="Server">

    <script src="../JS/KeyIdentificationTable.js" type="text/javascript"></script>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyContent" runat="Server">
    <!-- Table 1 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td colspan="2" class="title" style="height: 29px">
                <asp:Label ID="lblHeader" runat="server" Text="Key Identification Table"></asp:Label>
            </td>
        </tr>
        <!-- Row 3 -->
        <tr>
            <td colspan="2">
                <asp:Label ID="lblError" runat="server" CssClass="validation-error" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <!-- Table 2 -->
    <table width="100%" border="0" cellpadding="1" cellspacing="0">
        <!-- Row 1 -->
        <tr>
            <td>
                <div id="divform">
                    <br />
                    <br />
                    <!-- Features Div -->
                    <div class="DivClassFeature">
                        <b>Varoius Features Used.</b>
                        <ol>
                            <li>Key Identification Table</li>
                            <li>Testing - Press any key either on the Textbox or document</li>
                            <li><a href='http://www.w3.org/2002/09/tests/keys-cancel2.html'>Follow Link </a></li>
                        </ol>
                    </div>
                    <br />
                    <br />
                    <!-- Actual Feature Div -->
                    <div>
                        <p>
                            Enter some text with uppercase and lowercase letters:<br/><br/>
                            <input type='text' id='entry' size='60' 
                                onkeydown='showDown(event)' 
                                onkeypress='showPress(event)'
                                onkeyup='showUp(event)'/>
                         </p>
                            
                        <table border="2" cellpadding="5" cellspacing="5">
                            <caption>
                                Keyboard Event Properties</caption>
                            <tr>
                                <th>
                                    Data
                                </th>
                                <th>
                                    keydown
                                </th>
                                <th>
                                    keypress
                                </th>
                                <th>
                                    keyup
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    keyCode
                                </td>
                                <td id="downKey">
                                    &mdash;
                                </td>
                                <td id="pressKey">
                                    &mdash;
                                </td>
                                <td id="upKey">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    charCode
                                </td>
                                <td id="downChar">
                                    &mdash;
                                </td>
                                <td id="pressChar">
                                    &mdash;
                                </td>
                                <td id="upChar">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Target
                                </td>
                                <td id="keyTarget" colspan="3">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Character
                                </td>
                                <td id="character" colspan="3">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Shift
                                </td>
                                <td id="downShift">
                                    &mdash;
                                </td>
                                <td id="pressShift">
                                    &mdash;
                                </td>
                                <td id="upShift">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ctrl
                                </td>
                                <td id="downCtrl">
                                    &mdash;
                                </td>
                                <td id="pressCtrl">
                                    &mdash;
                                </td>
                                <td id="upCtrl">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Alt
                                </td>
                                <td id="downAlt">
                                    &mdash;
                                </td>
                                <td id="pressAlt">
                                    &mdash;
                                </td>
                                <td id="upAlt">
                                    &mdash;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Meta
                                </td>
                                <td id="downMeta">
                                    &mdash;
                                </td>
                                <td id="pressMeta">
                                    &mdash;
                                </td>
                                <td id="upMeta">
                                    &mdash;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
