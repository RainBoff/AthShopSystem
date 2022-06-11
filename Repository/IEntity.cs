using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp_mvc_std_v2
{
    public interface IEntity<Type>
    {
        public Type Id { get; set; }
    }
    public interface IEntityWithName<Type> : IEntity<Type>
    {
        public Type Id { get; set; }
        public string Name { get; set; }
    }
}


