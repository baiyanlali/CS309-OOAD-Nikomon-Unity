using System;
using System.Reflection;

namespace PokemonCore.Combat.Interface
{
    public interface IPropertyModify
    {
        public object this[string propertyName]
        {
            get;
            // {
            //     Type t = this.GetType();
            //     PropertyInfo pi = t.GetProperty(propertyName);
            //     return pi.GetValue(this, null);
            // }
            set;
            // {
            //     Type t = this.GetType();
            //     PropertyInfo pi = t.GetProperty(propertyName);
            //     pi.SetValue(this, value, null);
            // }
    }
    }
}