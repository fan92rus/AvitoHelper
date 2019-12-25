using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AvitoHelper.Services
{
    public abstract class BaseParser : IParsedService, IHostedService
    {
        private EmailSender _emailer { get; set; }
        private DatabaseContext _context { get; set; }
        protected string _baseUrl { get; set; }
        EmailHelper _EmailHelper { get; set; }

        IServiceScopeFactory _scopeFactory;
        public BaseParser(EmailSender emailer, EmailHelper EmailHelper, IServiceScopeFactory scopeFactory)
        {
            _EmailHelper = EmailHelper;
            _emailer = emailer;
            _scopeFactory = scopeFactory;
        }
        public abstract List<Product> Parse(Order order, int pages, bool isAnalize);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Task(() =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    _context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                    PaseAll(_context);
                }
            }).Start();
            return Task.CompletedTask;
        }
        public void PaseAll(DatabaseContext _context)
        {
            string EmailLogn = Environment.GetEnvironmentVariable("PRODUCT_EMAIL_LOGIN");
            string EmailPass = Environment.GetEnvironmentVariable("PRODUCT_EMAIL_PASSWORD");
            while (true)
            {
                var users = _context.Users.ToList();
                foreach (var user in users)
                {
                    var tasks = user?.Orders?.Take(user.Limit);
                    if (tasks != null)
                        foreach (var task in tasks)
                        {
                            List<Product> products = task?.products?.ToList();

                            var is_Analize = user.DayEndSubscribe >= DateTimeOffset.Now;

                            var newProducts = Parse(task, user.Deep, is_Analize);

                            _context.Attach(task);
                            task.products = newProducts;
                            var unicProducts = newProducts.Where(p => !products.Any(pr => pr.Link == p.Link) && CheckWords(p, task.NeedWords, false) && CheckWords(p, task.ignoreWords, true));
                            if (!(task.IgnoreFirstIteration && products.Count == 0 || unicProducts.Count() == 0))
                            {
                                var links = unicProducts.Select(p => p.Name + "   " + _baseUrl + p.Link);
                                string mail = _EmailHelper.GetProductEmail(products);
                                Task.Run(() =>
                                {
                                    _context.Attach(user);
                                    this._emailer.Execute(EmailLogn, EmailPass, user.email, "Найдены новые продукты ", mail);
                                    user.Limit--;
                                    _context.SaveChanges();
                                });
                            }
                            task.products = newProducts;
                            _context.SaveChanges();
                        }
                }
                Thread.Sleep(10000 * 30);
            }
        }
        public bool CheckWords(Product p, IEnumerable<Word> words, bool Ignore)
        {
            if (words == null || words.Count() == 0 || words.All(w => Regex.IsMatch(p.Text, w.Text)))
                return Ignore ? false : true;
            return Ignore ? false : false;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
