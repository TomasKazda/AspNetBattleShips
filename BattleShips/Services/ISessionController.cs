using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Services
{
    public interface ISessionController<T>
    {
        T LoadOrCreate(string key);
        void Save(string key, T data);
    }
}
