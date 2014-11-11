using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.Entities;
using eRestaurantSystem.DAL;
using System.ComponentModel;
using eRestaurantSystem.POCOs;
using eRestaurantSystem.DTOs;
using System.Data.Entity;
#endregion

namespace eRestaurantSystem.BLL
{
    [DataObject]
    public class eRestaurantController
    {
    #region SpecialEvents
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SpecialEvent> SpecialEvent_List()
        {
            //interfacing with our Context class
            using(eRestaurantContext context = new eRestaurantContext())
            {
                // using Context DbSet to get entity data
                //return context.SpecialEvents.ToList();

                //get a list of instances for entity using LINQ
                var results = from item in context.SpecialEvents
                              select item;
                return results.ToList();

            }
        }
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public SpecialEvent SpecialEventByEventCode(string eventcode)
        {
            using(eRestaurantContext context = new eRestaurantContext())
            {
                return context.SpecialEvents.Find(eventcode);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public void SpecialEvents_Add(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                SpecialEvent added = null;
                added = context.SpecialEvents.Add(item);
                // commits the add to the database
                // evaluates the annotations (validations) on your entity
                // [Required],[StringLength],[Range],...
                context.SaveChanges();  
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public void SpecialEvents_Update(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                context.Entry<SpecialEvent>(context.SpecialEvents.Attach(item)).State =System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public void SpecialEvents_Delete(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                SpecialEvent existing = context.SpecialEvents.Find(item.EventCode);
                context.SpecialEvents.Remove(existing);
                context.SaveChanges();
            }
        }
    #endregion

    #region Reservations
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Reservation> Reservation_List()
        {
            //interfacing with our Context class
            using (eRestaurantContext context = new eRestaurantContext())
            {
                return context.Reservations.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<Reservation> ReservationbyEvent(string eventcode)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                return context.Reservations.Where(anItem => anItem.Eventcode == eventcode).ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<DTOs.ReservationCollection> ReservationsByTime(DateTime date)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from data in context.Reservations
                              where data.ReservationDate.Year == date.Year
                                 && data.ReservationDate.Month == date.Month
                                 && data.ReservationDate.Day == date.Day
                                 && data.ReservationStatus == "B"
                              select new POCOs.ReservationSummary
                              {
                                  ID = data.ReservationID,
                                  Name = data.CustomerName,
                                  Date = data.ReservationDate,
                                  Status = data.ReservationStatus,
                                  Event = data.SpecialEvent.Description,
                                  NumberinParty = data.NumberinParty,
                                  Contact = data.ContactPhone
                              };
                //example of using group
                //when you are developing your queries in LinqPad
                //    you are working with Linq to SQL
                //When you are using the queries in the controller
                //    you are working with Linq to Entities
                //TimeOfDay is OK for Linq to SQL
                //we will use the DateTime Hour property in our controller
                //Hour is an integer
                var finalResults = from item in results
                                   orderby item.NumberinParty
                                   group item by item.Date.Hour into itemGroup
                                   select new DTOs.ReservationCollection
                                   {
                                       Hour = itemGroup.Key,
                                       Reservations = itemGroup.ToList()
                                   };
                return finalResults.ToList();
            }
        }
    #endregion

    #region Waiter
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Waiter> Waiter_List()
        {
            //interfacing with our Context class
            using (eRestaurantContext context = new eRestaurantContext())
            {
                // using Context DbSet to get entity data
                //return context.SpecialEvents.ToList();

                //get a list of instances for entity using LINQ
                var results = from item in context.Waiters
                              orderby item.LastName, item.FirstName
                              select item;
                return results.ToList();

            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterOnDuty> ListWaiters()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from item in context.Waiters
                              orderby item.LastName, item.FirstName
                              select new POCOs.WaiterOnDuty
                              {
                                  WaiterId = item.WaiterID,
                                  FullName = item.FirstName + " " + item.LastName
                              };
                return results.ToList();

            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Waiter GetWaiter(int waiterid)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                return context.Waiters.Find(waiterid);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void Waiter_Add(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                Waiter added = null;
                added = context.Waiters.Add(item);
                // commits the add to the database
                // evaluates the annotations (validations) on your entity
                // [Required],[StringLength],[Range],...
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Waiter_Update(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                context.Entry<Waiter>(context.Waiters.Attach(item)).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Waiter_Delete(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                Waiter existing = context.Waiters.Find(item.WaiterID);
                context.Waiters.Remove(existing);
                context.SaveChanges();
            }
        }

    
    #endregion
    
    #region Reports
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<POCOs.CategoryMenuItems> GetReportCategoryMenuItems()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from data in context.Items
                              select new POCOs.CategoryMenuItems
                              {
                                  CategoryDescription = data.MenuCategory.Description,
                                  ItemDescription = data.Description,
                                  Price = data.CurrentPrice,
                                  Calories = data.Calories,
                                  Comment = data.Comment
                              };
                return results.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<POCOs.TotalItemSalesByMenuCategory> TotalItemSalesByMenuCategory()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from data in context.Items
                              orderby data.MenuCategory.Description,
                                      data.Description
                              select new POCOs.TotalItemSalesByMenuCategory
                              {
                                  CatDescription = data.MenuCategory.Description,
                                  ItemDescription = data.Description,
                                  Quantity = data.BillItems.Sum(x => x.Quantity),
                                  Price = data.BillItems.Sum(x => x.SalePrice * x.Quantity),
                                  Cost = data.BillItems.Sum(x => x.UnitCost * x.Quantity)
                              };
                return results.ToList();
            }
        }
    #endregion

    #region Seating
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<POCOs.SeatingSummary> SeatingByDateTime(DateTime date, TimeSpan time)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var step1 = from data in context.Tables
                            select new
                            {
                                Table = data.TableNumber,
                                Seating = data.Capacity,
                                // This sub-query gets the bills for walk-in customers
                                WalkIns = from walkIn in data.Bills
                                          where
                                                 walkIn.BillDate.Year == date.Year
                                              && walkIn.BillDate.Month == date.Month
                                              && walkIn.BillDate.Day == date.Day
                                             
                                              // The following won't work in EF to Entities - it will return this exception:
                                              //  "The specified type member 'TimeOfDay' is not supported..."
                                               //&& walkIn.BillDate.TimeOfDay <= time //TimeOfDay not supported
                                    && DbFunctions.CreateTime(walkIn.BillDate.Hour, walkIn.BillDate.Minute, walkIn.BillDate.Second) <= time
                                              && (!walkIn.OrderPaid.HasValue || walkIn.OrderPaid.Value >= time)
                                          //                          && (!walkIn.PaidStatus || walkIn.OrderPaid >= time)
                                          select walkIn,
                                // This sub-query gets the bills for reservations
                                Reservations = from booking in data.Reservations
                                               from reservationParty in booking.Bills
                                               where
                                                      reservationParty.BillDate.Year == date.Year
                                                   && reservationParty.BillDate.Month == date.Month
                                                   && reservationParty.BillDate.Day == date.Day
                                                   
                                                   // The following won't work in EF to Entities - it will return this exception:
                                                   //  "The specified type member 'TimeOfDay' is not supported..."
                                                   //&& reservationParty.BillDate.TimeOfDay <= time
                                    && DbFunctions.CreateTime(reservationParty.BillDate.Hour, reservationParty.BillDate.Minute, reservationParty.BillDate.Second) <= time
                                                   && (!reservationParty.OrderPaid.HasValue || reservationParty.OrderPaid.Value >= time)
                                               //                          && (!reservationParty.PaidStatus || reservationParty.OrderPaid >= time)
                                               select reservationParty
                            };
               
                // Step 2 - Union the walk-in bills and the reservation bills while extracting the relevant bill info
                // .ToList() helps resolve the "Types in Union or Concat are constructed incompatibly" error
                var step2 = from data in step1.ToList() // .ToList() forces the first result set to be in memory
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                CommonBilling = from info in data.WalkIns.Union(data.Reservations)
                                                select new // info
                                                {
                                                    BillID = info.BillID,
                                                    BillTotal = info.BillItems.Sum(bi => bi.Quantity * bi.SalePrice),
                                                    Waiter = info.Waiter.FirstName,
                                                    Reservation = info.Reservation
                                                }
                            };
                
                // Step 3 - Get just the first CommonBilling item
                //         (presumes no overlaps can occur - i.e., two groups at the same table at the same time)
                var step3 = from data in step2.ToList()
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.CommonBilling.Count() > 0,
                                // .FirstOrDefault() is effectively "flattening" my collection of 1 item into a 
                                // single object whose properties I can get in step 4 using the dot (.) operator
                                CommonBilling = data.CommonBilling.FirstOrDefault()
                            };
              
                // Step 4 - Build our intended seating summary info
                var step4 = from data in step3
                            select new SeatingSummary() // the DTO/POCO class to use in my BLL
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.Taken,
                                // use a ternary expression to conditionally get the bill id (if it exists)
                                BillID = data.Taken ?               // if(data.Taken)
                                         data.CommonBilling.BillID  // value to use if true
                                       : (int?)null,               // value to use if false
                                BillTotal = data.Taken ?
                                            data.CommonBilling.BillTotal : (decimal?)null,
                                Waiter = data.Taken ? data.CommonBilling.Waiter : (string)null,
                                ReservationName = data.Taken ?
                                                  (data.CommonBilling.Reservation != null ?
                                                   data.CommonBilling.Reservation.CustomerName : (string)null)
                                                : (string)null
                            };
                return step4.ToList();
            }

        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<SeatingSummary> AvailableSeatingByDateTime(DateTime date, TimeSpan time)
        {
            var result = from seats in SeatingByDateTime(date, time)
                         where !seats.Taken
                         select seats;
            return result.ToList();
        }
        /// <summary>
        /// Seats a customer that is a walk-in
        /// </summary>
        /// <param name="when">A mock value of the date/time (Temporary - see remarks)</param>
        /// <param name="tableNumber">Table number to be seated</param>
        /// <param name="customerCount">Number of customers being seated</param>
        /// <param name="waiterId">Id of waiter that is serving</param>
        public void SeatCustomer(DateTime when, byte tableNumber, int customerCount, int waiterId)
        {
            var availableSeats = AvailableSeatingByDateTime(when.Date, when.TimeOfDay);
            using (var context = new eRestaurantContext())
            {
                List<string> errors = new List<string>();
                // Rule checking:
                // - Table must be available - typically a direct check on the table, but proxied based on the mocked time here
                // - Table must be big enough for the # of customers
                if (!availableSeats.Exists(x => x.Table == tableNumber))
                {
                    errors.Add("Table is currently not available");
                }
                else if (!availableSeats.Exists(x => x.Table == tableNumber && x.Seating >= customerCount))
                { 
                    errors.Add("Insufficient seating capacity for number of customers.");
                }
                if (errors.Count > 0)
                { 
                    throw new BusinessRuleException("Unable to seat customer", errors);
                }
                Bill seatedCustomer = new Bill()
                {
                    BillDate = when,
                    NumberInParty = customerCount,
                    WaiterID = waiterId,
                    TableID = context.Tables.Single(x => x.TableNumber == tableNumber).TableID
                };
                context.Bills.Add(seatedCustomer);
                context.SaveChanges();
            }
        }

        public void SeatCustomer(DateTime when, int reservationId, List<byte> tables, int waiterId)
        {
            var availableSeats = AvailableSeatingByDateTime(when.Date, when.TimeOfDay);
            using (var context = new eRestaurantContext())
            {
                List<string> errors = new List<string>();
                // Rule checking:
                // - Reservation must be in Booked status
                // - Table must be available - typically a direct check on the table, but proxied based on the mocked time here
                // - Table must be big enough for the # of customers
                var reservation = context.Reservations.Find(reservationId);
                if (reservation == null)
                {
                    errors.Add("The specified reservation does not exist");
                }
                else if (reservation.ReservationStatus != Reservation.Booked)
                { 
                    errors.Add("The reservation's status is not valid for seating. Only booked reservations can be seated.");
                }
                var capacity = 0;
                foreach (var tableNumber in tables)
                {
                    if (!availableSeats.Exists(x => x.Table == tableNumber))
                    {
                        errors.Add("Table " + tableNumber + " is currently not available");
                    }
                    else
                    { 
                        capacity += availableSeats.Single(x => x.Table == tableNumber).Seating;
                    }
                }
                if (capacity < reservation.NumberinParty)
                { 
                    errors.Add("Insufficient seating capacity for number of customers. Alternate tables must be used.");
                }
                if (errors.Count > 0)
                { 
                    throw new BusinessRuleException("Unable to seat customer", errors);
                }
                // 1) Create a blank bill with assigned waiter
                Bill seatedCustomer = new Bill()
                {
                    BillDate = when,
                    NumberInParty = reservation.NumberinParty,
                    WaiterID = waiterId,
                    ReservationID = reservation.ReservationID
                };
                context.Bills.Add(seatedCustomer);
                // 2) Add the tables for the reservation and change the reservation's status to arrived
                foreach (var tableNumber in tables)
                { 
                    reservation.Tables.Add(context.Tables.Single(x => x.TableNumber == tableNumber));
                }
                reservation.ReservationStatus = Reservation.Arrived;
                var updatable = context.Entry(context.Reservations.Attach(reservation));
                updatable.Property(x => x.ReservationStatus).IsModified = true;
                //updatable.Reference(x=>x.Tables).
                // 3) Save changes
                context.SaveChanges();
            }
            //string message = String.Format("Not yet implemented. Need to seat reservation {0} for waiter {1} at tables {2}", reservationId, waiterId, string.Join(", ", tables));
            //throw new NotImplementedException(message);
        }
    #endregion

    #region Linq Queries
    [DataObjectMethod(DataObjectMethodType.Select,false)]
    public List<DTOs.CategoryMenuItems> GetCategoryMenuItems()
    {
        using(eRestaurantContext context = new eRestaurantContext())
        {
            var results = from cat in context.MenuCategories
                            orderby cat.Description
                            select new DTOs.CategoryMenuItems()
                            {
                                Description = cat.Description,
                                MenuItems = from item in cat.Items
                                            where item.Active
                                            select new MenuItem()
                                            {
                                                Description = item.Description,
                                                Price = item.CurrentPrice,
                                                Calories = item.Calories,
                                                Comment = item.Comment
                                            }
                            };

            return results.ToList(); // this was .Dump() in Linqpad
        }
    }

#endregion

    
    #region UX Clockpicker
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public DateTime GetLastBillDateTime()
    {
        using (var context = new eRestaurantContext())
        {
            var results = context.Bills.Max(x => x.BillDate);
            return results;
        }
    }
    #endregion
    }
}
