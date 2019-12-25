using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Services;
using AvitoHelper.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvitoHelper.Controllers
{
    public class BuyController : CustomController
    {
        public BuyController(DatabaseContext context) : base(context)
        {
        }
        [HttpGet("Pay")]
        public ActionResult Pay(string InvId, string uId, string outSumm, string type, string count, string SignatureValue)
        {
            if (new RoboKassaService().CheckHash(outSumm, InvId, type, count, uId, SignatureValue))
            {
                var user = _context.Users.Find(int.Parse(uId));
                _context.Attach(user);
                if (user != null)
                {
                    float _outSumm = float.Parse(outSumm);
                    var _count = int.Parse(count);

                    if (type.ToLower() == "subscribe")
                    {
                        var summ = _count * 300;
                        if (_count > 1)
                            summ -= (summ / 100) * 5;


                        if (summ >= _outSumm)
                        {
                            var days = _count * 30;
                            var nextDay = DateTimeOffset.Now.AddDays(days);
                            user.DayEndSubscribe = nextDay;
                        }
                        else
                        {
                            var days = _outSumm / 10;
                            var nextDay = DateTimeOffset.Now.AddDays(days);
                            user.DayEndSubscribe = nextDay;
                        }
                    }
                    else if (type.ToLower() == "push")
                    {
                        var summ = _count * 0.5;
                        if (_count > 100)
                            summ -= (summ / 100) * 5;

                        if (summ >= _outSumm)
                        {
                            user.Limit += _count;
                        }
                        else
                        {
                            var new_count = (int)(_outSumm * 2);
                            user.Limit += new_count;
                        }
                    }
                    _context.SaveChanges();
                }
                return new JsonResult(InvId + "OK");
            }
            return new JsonResult("BAD");
        }
        [HttpGet("Buy/GetLink")]
        public ActionResult GetLink(string type, int count)
        {
            decimal summ = 0;
            if (type.ToLower() == "subscribe")
            {
                summ = count * 300;
                if (count > 1)
                    summ -= (summ / 100) * 5;
            }
            else if (type.ToLower() == "push")
            {
                summ = (decimal)(count * 0.5);
                if (count > 100)
                    summ -= (summ / 100) * 5;
            }
            else
            {
                return new StatusCodeResult(403);
            }
            var payId = _context.Purchaces.AsEnumerable().LastOrDefault()?.PayId ?? 1;
            var newPurchase = new Purchace()
            {
                PayId = payId,
                outSumm = summ,
                type = type,
                Count = count,
                uId = Account.Id
            };
            var link = new RoboKassaService().GetLink(newPurchase);
            return new JsonResult(link);
        }
    }
}