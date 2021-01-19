﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


using UltraPlayMarkets.Data;
using UltraPlayMarkets.Models;
using UltraPlayMarkets.Utilities;

namespace UltraPlayMarkets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (var db = new MarketsDbContext())
            {
                DateTime dateFilter = DateTime.Now.AddHours(24);

                var allMatches = db.Match.Where(x => x.StartDate <= dateFilter && x.StartDate >= DateTime.Now).ToList();

                foreach (var match in allMatches)
                {
                    List<Bet> bets = db.Bet.Where(b => b.MatchId == match.Id).ToList();

                    foreach (var bet in bets)
                    {
                        bet.Odds = db.Odd.Where(o => o.BetId == bet.Id).ToList();
                    }

                    match.Bets = bets;

                }

                

                var entities = (from GetPreviewMatches in db.GetPreviewMatches
                                select new GetPreviewMatches
                                {
                                    MainRow = GetPreviewMatches.MainRow,
                                    AllMarkets = GetPreviewMatches.AllMarkets,
                                    HeaderRank = GetPreviewMatches.HeaderRank,
                                    DetailsRank = GetPreviewMatches.DetailsRank,
                                    IsLive = GetPreviewMatches.IsLive,
                                    EventId = GetPreviewMatches.EventId,
                                    EventName = GetPreviewMatches.EventName,
                                    MatchId = GetPreviewMatches.MatchId,
                                    PlayerOne = GetPreviewMatches.PlayerOne,
                                    PlayerTwo = GetPreviewMatches.PlayerTwo,
                                    StartDate = GetPreviewMatches.StartDate,
                                    BetId = GetPreviewMatches.BetId,
                                    BetName = GetPreviewMatches.BetName,
                                    HomecomingName = GetPreviewMatches.HomecomingName,
                                    HomecomingValue = GetPreviewMatches.HomecomingValue,
                                    HomecomingSBV = GetPreviewMatches.HomecomingSBV,
                                    GuestName = GetPreviewMatches.GuestName,
                                    GuestValue = GetPreviewMatches.GuestValue,
                                    GuestSBV = GetPreviewMatches.GuestSBV
                                }).ToList();


                return this.View(entities);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
