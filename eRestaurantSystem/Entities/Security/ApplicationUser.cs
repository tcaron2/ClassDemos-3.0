﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace eRestaurantSystem.Entities.Security
{
    public class ApplicationUser : IdentityUser
    {
        //property to point to the user entity ID
        public int? WaiterID { get; set; }
    }
}