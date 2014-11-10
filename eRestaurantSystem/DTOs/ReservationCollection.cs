using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.POCOs;
#endregion

namespace eRestaurantSystem.DTOs
{
    public class ReservationCollection
    {
        public int Hour { get; set;}
        //this will become a readonly property
        public TimeSpan SeatingTime
        {
            get
            {
                return new TimeSpan(Hour, 0, 0);
            }

        }
      
        public List<ReservationSummary> Reservations { get; set; }

        
    }
}
