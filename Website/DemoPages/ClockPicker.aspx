<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ClockPicker.aspx.cs" Inherits="DemoPages_ClockPicker" %>

<%@ Register src="../UserControls/MessageUserControl.ascx" tagname="MessageUserControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="well">
    <div class="pull-right col-md-5">
        <h4>
         <small> Last Billed Date/time:</small>
         <asp:Repeater ID="BillDateRepeater" runat="server" 
         DataSourceID="ODSRepeater" ItemType="System.DateTime">
            <ItemTemplate>
                <b class="label label-primary">
                    <%# Item.ToShortDateString() %>
                </b>&ndash;
                <b class="label label-info">
                    <%# Item.ToShortTimeString() %>
                </b>
            </ItemTemplate>
         </asp:Repeater>
            <asp:ObjectDataSource ID="ODSRepeater" runat="server" 
            OldValuesParameterFormatString="original_{0}" 
            SelectMethod="GetLastBillDateTime" 
            TypeName="eRestaurantSystem.BLL.eRestaurantController"></asp:ObjectDataSource>
        </h4>
    </div>
    <h4>Mock Date/Time</h4>
    <asp:LinkButton ID="MockDateTime" runat="server"
            CssClass="btn btn-primary">
            Post-back new date/time</asp:LinkButton>
    &nbsp;&nbsp;
    <asp:LinkButton ID="MockLastBillingDateTime" runat="server"
            CssClas="btn btn-defualt" OnClick="MockLastBillingDateTime_Click">
            set to Last-Bill date/time:</asp:LinkButton>
    &nbsp;&nbsp;
    <asp:TextBox ID="SearchDate" runat="server"
            TextMode="Date" Text="2014-10-27"></asp:TextBox>
    <asp:TextBox ID="SearchTime" runat="server"
            TextMode="Time" Text="15:00"
            CssClass="clockpicker"> </asp:TextBox>
    <%--add clock picker--%>
    <script src="../Scripts/clockpicker.js"></script>
    <script type="text/javascript">
        $('.clockpicker').clockpicker({ donetext: 'Accept' });
    </script>
    <link itemprop="url" href="../Content/standalone.css" rel="stylesheet" />
    <link itemprop="url" href="../Content/clockpicker.css" rel="stylesheet" />
  
</div >
<div class="pull-left col-md-12">  
    <uc1:MessageUserControl ID="MessageUserControl" runat="server" />
</div>
    <div class="pull-right col-md-5">
    <details open> 
        <asp:Panel ID="ReservationSeatingPanel" runat="server"><%--Visible='<%# ShowReservationSeating() %>'--%>
            <asp:DropDownList ID="WaiterDropDownList" runat="server" CssClass="seating"
                AppendDataBoundItems="true" DataSourceID="WaitersDataSource"
                DataTextField="FullName" DataValueField="WaiterId">
                <asp:listitem value="0">[select a waiter]</asp:listitem>
            </asp:DropDownList>
            <asp:ListBox ID="ReservationTableListBox" runat="server" CssClass="seating"                             
                DataSourceID="AvailableSeatingObjectDataSource" SelectionMode="Multiple" Rows="14"
                DataTextField="Table" DataValueField="Table">
            </asp:ListBox>
        </asp:Panel>
        <Strong>Today's Reservations</Strong>
        <asp:Repeater ID="ReservationsRepeater" runat="server"
            ItemType="eRestaurantSystem.DTOs.ReservationCollection" 
            DataSourceID="ReservationsByTimeDataSource">
          
           <ItemTemplate>
                <div>
                    <h4><%# Item.SeatingTime %></h4>
                    <asp:ListView ID="ReservationSummaryListView" runat="server" 
                            ItemType="eRestaurantSystem.POCOs.ReservationSummary" 
                        DataSource='<%# Item.Reservations %>'
                        OnItemCommand="ReservationSummaryListView_OnItemCommand">
                        <LayoutTemplate>
                            <div class="seating">
                                <span runat="server" id="itemPlaceholder" />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div>
                                <%# Item.Name %> &mdash;
                                <%# Item.NumberinParty %> &mdash;
                                <%# Item.Status %> &mdash;
                                PH:&nbsp;<%# Item.Contact %>&mdash;
                                <asp:LinkButton ID="InsertButton" runat="server" 
                                    CommandName="Seat" 
                                    CommandArgument='<%# Item.ID %>'>Reservation Seating
                                    <span class="glyphicon glyphicon-plus"></span></asp:LinkButton>
                               
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </details>
 </div> 
  

          <%--For the Waiter DropDown--%>
            <asp:ObjectDataSource runat="server" ID="WaitersDataSource" 
                OldValuesParameterFormatString="original_{0}" SelectMethod="ListWaiters" 
                TypeName="eRestaurantSystem.BLL.eRestaurantController"></asp:ObjectDataSource>
    
          <%--For the Available Tables DropDown (seating reservation)--%>
            <asp:ObjectDataSource runat="server" ID="AvailableSeatingObjectDataSource"
                OldValuesParameterFormatString="original_{0}" SelectMethod="AvailableSeatingByDateTime" 
                TypeName="eRestaurantSystem.BLL.eRestaurantController">
                <selectparameters>
                    <asp:controlparameter ControlID="SearchDate" PropertyName="Text" name="date"
                        type="DateTime"></asp:controlparameter>
                    <asp:controlparameter ControlID="SearchTime" PropertyName="Text" dbtype="Time" 
                        name="time"></asp:controlparameter>
                </selectparameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ReservationsByTimeDataSource" 
                runat="server" OldValuesParameterFormatString="original_{0}" 
                SelectMethod="ReservationsByTime" 
                TypeName="eRestaurantSystem.BLL.eRestaurantController">
                <SelectParameters>
                    <asp:ControlParameter ControlID="SearchDate" 
                        Name="date" PropertyName="Text" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource>

<div class="col-md-7">
        <details open>
            <summary>Tables</summary>
            <p class="well">This GridView uses a &lt;asp:TemplateField …&gt; for the table number and 
                the controls to handle walk-in seating. Additionally, the walk-in seating form 
                is in a panel that only shows if the seat is <em>not</em> taken. Handling of the 
                action to <b>seat customers</b> is done in the code-behind, on the GridView's 
                <code>OnSelectedIndexChanging</code> event.</p>
            <style type="text/css">
                .inline-div {
                    display: inline;
                }
            </style>
            <asp:GridView ID="SeatingGridView" runat="server" AutoGenerateColumns="False"
                 CssClass="table table-hover table-striped table-condensed"
               
                DataSourceID="SeatingObjectDataSource" 
                ItemType="eRestaurantSystem.POCOs.SeatingSummary" 
                
                OnSelectedIndexChanging="SeatingGridView_SelectedIndexChanging">
                <Columns>
                    <asp:CheckBoxField DataField="Taken" HeaderText="Taken" 
                        SortExpression="Taken" ItemStyle-HorizontalAlign="Center"></asp:CheckBoxField>
                    <asp:TemplateField HeaderText="Table" SortExpression="Table" 
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="TableNumber" runat="server" Text='<%# Item.Table %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Seating" SortExpression="seating" 
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="Seating" runat="server" Text='<%# Item.Seating %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reservation Notes / Seat Walk-In">
                        <ItemTemplate>
                            <asp:Panel ID="WalkInSeatingPanel" runat="server" 
                                CssClass="input-group input-group-sm"
                                    Visible='<%# !Item.Taken %>'>
                                <asp:TextBox ID="NumberInParty" runat="server" CssClass="form-control col-md-1"
                                        TextMode="Number" placeholder="# people"></asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:DropDownList ID="WaiterList" runat="server"
                                            CssClass="selectpicker"
                                            AppendDataBoundItems="true" DataSourceID="ODSWaiterList"
                                            DataTextField="FullName" DataValueField="WaiterId">
                                        <asp:ListItem Value="0">[select a waiter]</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <span class="input-group-addon" style="width:5px;padding:0;border:0;background-color:white;"></span>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Seat Customers"
                                    CssClass="input-group-btn" CommandName="Select" CausesValidation="False" />
                            </asp:Panel>
                            <asp:Panel ID="OccupiedTablePanel" runat="server"
                                    Visible='<%# Item.Taken  %>'>
                                <%# Item.Waiter %>
                                <asp:Label ID="ReservationNameLabel" runat="server" 
                                        Text='<%# "&mdash;" + Item.ReservationName %>'
                                        Visible='<%# !string.IsNullOrEmpty(Item.ReservationName) %>' />
                                <asp:Panel ID="BillInfo" runat="server" CssClass="inline-div"
                                        Visible='<%# Item.BillTotal.HasValue && Item.BillTotal.Value > 0 %>'>
                                    <asp:Label ID="Label1" runat="server" Text='<%# string.Format(" &ndash; {0:C}", Item.BillTotal) %>' />
                                </asp:Panel>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </details>
    </div>
    <asp:ObjectDataSource runat="server" ID="SeatingObjectDataSource" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SeatingByDateTime" TypeName="eRestaurantSystem.BLL.eRestaurantController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SearchDate" PropertyName="Text" Name="date" Type="DateTime"></asp:ControlParameter>
            <asp:ControlParameter ControlID="SearchTime" PropertyName="Text" DbType="Time" Name="time"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSWaiterList" runat="server"
         OldValuesParameterFormatString="original_{0}" 
        SelectMethod="ListWaiters" 
        TypeName="eRestaurantSystem.BLL.eRestaurantController"></asp:ObjectDataSource>
   

</asp:Content>

