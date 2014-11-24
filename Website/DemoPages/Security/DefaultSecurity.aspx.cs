﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region
using eRestaurantSystem.BLL.Security;
using eRestaurantSystem.BLL;
using eRestaurantSystem.Entities;
using eRestaurantSystem.Entities.Security;
using eRestaurantSystem.POCOs;
using Microsoft.AspNet.Identity;
#endregion

public partial class DemoPages_Security_DefaultSecurity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindUserList();
            DataBindRoleList();
        }
    }
    private void DataBindRoleList()
    {
        // Populate the Roles Info
        RoleListView.DataSource = new RoleManager().Roles.ToList();
        RoleListView.DataBind();
    }
    private void DataBindUserList()
    {
        // Populate the Users Info
        UserListView.DataSource = new UserManager().Users.ToList();
        UserListView.DataBind();
    }

    protected void DataPagerUser_PreRender(object sender, EventArgs e)
    {
        //used for embedding paging on the ListView
        //call the method that binds your data to your ListView
        DataBindUserList();
    }
    protected void DataPagerRoles_PreRender(object sender, EventArgs e)
    {
        //call the method that binds your data to your ListView
        DataBindRoleList();
    }
    protected void UserListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "AddWaiters":
                new UserManager().AddDefaultUsers();
                DataBindUserList();
                break;
            default:
                break;
        }
    }
    protected void RoleListView_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "AddDefaultRoles":
                new RoleManager().AddDefaultRoles();
                DataBindRoleList();
                break;
            default:
                break;
        }
    }
}