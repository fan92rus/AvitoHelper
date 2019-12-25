using AvitoHelper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Builders
{
    public class OrderModelBuilder : BaseBuilder<OrderModel, Order>
    {
        public override BaseBuilder<OrderModel, Order> Build()
        {
            _target.id = _source.id;
            _target.IgnoreFirstIteration = _source.IgnoreFirstIteration;
            _target.ignoreWords = _source.ignoreWords.Select(w => w.Text);
            _target.Link = _source.Link;
            _target.NeedWords = _source.NeedWords.Select(w => w.Text);
            _target.Name = _source.Name;
            return base.Build();
        }
    }
    public class OrderBuilder : BaseBuilder<Order, OrderModel>
    {
        public override BaseBuilder<Order, OrderModel> Build()
        {
            _target.id = _source.id;
            _target.IgnoreFirstIteration = _source.IgnoreFirstIteration;
            _target.ignoreWords = _source.ignoreWords.Select(w => new Word() { Text = w }).ToList();
            _target.NeedWords = _source.NeedWords.Select(w => new Word() { Text = w }).ToList();
            _target.Link = _source.Link;
            _target.Name = _source.Name;
            return base.Build();
        }
    }
    public class OrderModel
    {
        public int id { get; set; }
        public bool IgnoreFirstIteration { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<string> ignoreWords { get; set; }
        public IEnumerable<string> NeedWords { get; set; }
    }
}
