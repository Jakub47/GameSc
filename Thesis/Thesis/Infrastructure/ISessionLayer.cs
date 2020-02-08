using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thesis.Infrastructure
{
    public interface ISessionLayer
    {
        T Get<T>(string key);
        void Set<T>(string name, T value);
        void Abandon();
        T TryGet<T>(string key);
    }
}