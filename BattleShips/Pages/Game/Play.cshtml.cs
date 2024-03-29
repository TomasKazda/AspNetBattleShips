﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleShips.Models.ViewModel;
using BattleShips.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static BattleShips.Models.GameState;

namespace BattleShips
{
    public class PlayModel : PageModel
    {
        private readonly GameService _gs;

        public PlayModel(GameService gs)
        {
            _gs = gs;
        }

        public GameBoardData GameBoardData { get; set; }
        
        public Dictionary<string, GameStatsPlayerInfo> GameInfo { get; set; }

        public string PlayerOnTurnMessage { get; set; }

        public IActionResult OnGet(Guid? gameId)
        {
            if (gameId != default && _gs.ContinueToGame((Guid)gameId))
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.info, $"Úspěšně připojeno ke hře... ({gameId})");
            }

            if (!_gs.IsGameLoaded)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.warning, $"Nejprve je potřeba vytvořit / vybrat hru...");
                return RedirectToPage("ListGames");
            }

            var g = _gs.GetGame();
            if (g.GameState == GameCreating)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.info, $"Druhý hráč není pravděpodobně připojen...");
                return RedirectToPage("ListGames");
            }
            if (g.GameState == ShipDeploying)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.info, $"Stále probíhá umísťování lodí...");
                return RedirectToPage("Deploy");
            }
            if (g.GameState == End)
            {
                TempData.AddMessage("BattleMessages", TempDataExtension.MessageType.success, $"Hra ukončena - vyhrál {(g.GameStateP1 == Winner ? g.Player1.UserName : g.Player2.UserName)}");
                _gs.UnloadGame();
                return RedirectToPage("ListGames");
            }

            GameBoardData = new GameBoardData(g, _gs.GetUserId())
            {
                PageHandler = "fire",
                RouteDataId = true
            };

            GameInfo = _gs.GetGameInfo();

            if (g.PlayerOnTurn == _gs.GetUserId())
                PlayerOnTurnMessage = "Jsi na tahu";
            else PlayerOnTurnMessage = "Čeká se na soupeře";

            return Page();
        }

        public IActionResult OnGetFire(int? id)
        {
            if (id == null) return Page();
            _gs.ChargeBoat((int)id);


            return RedirectToPage();
        }
    }
}