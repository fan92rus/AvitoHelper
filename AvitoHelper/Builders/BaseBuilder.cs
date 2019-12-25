using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Builders
{
    public class BaseBuilder<T, S>
    {
        protected T _target { get; set; }
        protected S _source { get; set; }
        public BaseBuilder<T, S> Init(T target, S source)
        {
            _target = target;
            _source = source;
            return this;
        }
        public virtual BaseBuilder<T, S> Build()
        {
            return this;
        }

        public static implicit operator T(BaseBuilder<T, S> builder)
        {
            return builder._target;
        }
    }
}
