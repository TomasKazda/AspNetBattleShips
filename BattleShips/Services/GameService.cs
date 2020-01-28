using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Session.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShips.Services
{
    public class GameService
    {
        readonly HttpContext _httpContext;
        readonly UserManager<IdentityUser> _userManager;

        public GameService(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {

            _httpContext = httpContextAccessor.HttpContext;
            _userManager = userManager;
            GameId = LoadOrCreate();
        }

        public Guid GameId { get; set; }

        private Guid LoadOrCreate()
        {
            //if (typeof(T).IsClass && result == null) result = (T)Activator.CreateInstance(typeof(T));
            return _httpContext.Session.Get<Guid>("GameKey");
        }
        public void Save(Guid data)
        {
            _httpContext.Session.Set("GameKey", data);
        }

        public string GetUserId()
        {
            return _httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "";
        }

        //public string GetUserId()
        //{

        //    return _userManager.GetUserId(_httpContext.User);
        //}
    }
}
