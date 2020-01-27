using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Session.Helpers;
using Microsoft.AspNetCore.Http;

namespace BattleShips.Services
{
    public class SessionController<T> : ISessionController<T>
    {
        readonly ISession _session;
        public SessionController(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public T LoadOrCreate(string key)
        {
            T result = _session.Get<T>(key);
            if (typeof(T).IsClass && result == null) result = (T)Activator.CreateInstance(typeof(T));
            return result;
        }

        public void Save(string key, T data)
        {
            _session.Set(key, data);
        }
    }
}
