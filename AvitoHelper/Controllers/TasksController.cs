using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.Builders;
using AvitoHelper.DataBase;
using AvitoHelper.Filters;
using AvitoHelper.Services;
using AvitoHelper.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvitoHelper.Controllers
{
    [AccessFilter]
    public class TasksController : CustomController
    {
        public TasksController(DatabaseContext context) : base(context)
        {
        }
        [HttpPost("Tasks/Add")]
        public ActionResult AddTask([FromBody]OrderModel orderModel, OrderBuilder builder, OrderModelBuilder modelBuilder)
        {
            var order = builder.Init(new Order(), orderModel).Build();

            if (Account.Limit > Account.Orders.Count)
            {
                _context.Attach(Account);
                Account.Orders.Add(order);
                Account.Limit--;
                _context.SaveChanges();
                return new OkObjectResult((OrderModel)modelBuilder.Init(new OrderModel(), order).Build());
            }
            return new StatusCodeResult(403);
        }
        [HttpPost("Tasks/Edit")]
        public ActionResult Edit([FromBody]OrderModel orderModel, OrderBuilder builder)
        {
            if (Account != null)
            {
                var order = Account.Orders.FirstOrDefault(o => o.id == orderModel.id);
                if (order != null)
                {
                    _context.Attach(order);
                    order = builder.Init(order, orderModel);
                    _context.SaveChanges();
                    return new OkResult();
                }
                return new StatusCodeResult(404);
            }
            return new StatusCodeResult(403);
        }
        [HttpPost("Tasks/GetAll")]
        public ActionResult GetAll(OrderModelBuilder builder) => new JsonResult(Account.Orders.Select(o => (OrderModel)builder.Init(new OrderModel(), o).Build()));

    }
}