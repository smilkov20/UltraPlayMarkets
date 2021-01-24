using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

using UltraPlayMarkets.Data;
using UltraPlayMarkets.Models;
using UltraPlayMarkets.Utilities.Enums;

namespace UltraPlayMarkets.Utilities
{
    public class XMLReader
    {

        private List<Sport> sports = new List<Sport>();
        private List<Event> events = new List<Event>();
        private List<Match> matches = new List<Match>();
        private List<Bet> bets = new List<Bet>();
        private List<Odd> odds = new List<Odd>();


        public void Read()
        {
            string xmlText;
            using (var webclient = new WebClient())
            {
                xmlText = webclient.DownloadString(GlobalConstants.URLString);
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);

            using (var db = new MarketsDbContext())
            {
                db.Match.RemoveRange(db.Match.Where(x => x.StartDate < DateTime.Now));

                var nodes = xmlDocument.SelectNodes("//Sport");

                foreach (XmlNode node in nodes)
                {
                    Sport entity = new Sport();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);

                    //db.Sports.Add(entity);

                    sports.Add(entity);
                    if (!db.Sports.Any(x => x == entity))
                    {
                        db.Sports.Add(entity);
                    }
                    else
                    {
                        db.Sports.Update(entity);
                    }
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event");

                foreach (XmlNode node in nodes)
                {
                    Event entity = new Event();

                    var nameValue = node.Attributes.GetNamedItem("Name").Value;

                    var splitValues = nameValue.Split(',');
                    entity.Name = splitValues[0].TrimStart().TrimEnd();
                    entity.Tournament = splitValues[1].TrimStart().TrimEnd();
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.IsLive = bool.Parse(node.Attributes.GetNamedItem("IsLive").Value);
                    entity.CategoryId = int.Parse(node.Attributes.GetNamedItem("CategoryID").Value);
                    entity.SportId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

                    //db.Event.Add(entity);

                    events.Add(entity);
                    if (!db.Event.Any(x => x == entity))
                    {
                        db.Event.Add(entity);
                    }
                    else
                    {
                        db.Event.Update(entity);
                    }
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event/Match");

                foreach (XmlNode node in nodes)
                {
                    Match entity = new Match();

                    MatchTypeEnum entityMatchType;
                    Enum.TryParse(node.Attributes.GetNamedItem("MatchType").Value, out entityMatchType);                  
                    entity.MatchType = entityMatchType;

                    var nameValue = node.Attributes.GetNamedItem("Name").Value;


                    var splitValues = nameValue.Split(" - ");
                    entity.Name = splitValues[0].TrimStart().TrimEnd();

                    // Skip OpponentName if Name attribute contains only one name
                    if (splitValues.Length > 1)
                    {
                        entity.OpponentName = splitValues[1].TrimStart().TrimEnd();

                    }
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.StartDate = DateTime.Parse(node.Attributes.GetNamedItem("StartDate").Value);
                    
                    entity.EventId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

                    //db.Match.Add(entity);

                    matches.Add(entity);
                    if (!db.Match.Any(x => x == entity))
                    {
                        db.Match.Add(entity);
                    }
                    else
                    {
                        db.Match.Update(entity);
                    }
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event/Match/Bet");

                foreach (XmlNode node in nodes)
                {
                    Bet entity = new Bet();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.IsLive = bool.Parse(node.Attributes.GetNamedItem("IsLive").Value);
                    entity.MatchId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

                    //db.Bet.Add(entity);

                    bets.Add(entity);
                    if (!db.Bet.Any(x => x == entity))
                    {
                        db.Bet.Add(entity);
                    }
                    else
                    {
                        db.Bet.Update(entity);
                    }
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event/Match/Bet/Odd");

                bool isGuestOdd = false;
                int lastBetId = -1;

                foreach (XmlNode node in nodes)
                {
                    Odd entity = new Odd();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.Value = double.Parse(node.Attributes.GetNamedItem("Value").Value);
                    entity.SpecialBetValue = node.Attributes.GetNamedItem("SpecialBetValue")?.Value;
                    entity.BetId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);
                    entity.IsGuest = false;

                    if (lastBetId == entity.BetId)
                    {
                        switch (entity.Name)
                        {
                            case "1":
                                isGuestOdd = false;
                                break;
                            case "2":
                                isGuestOdd = true;
                                break;
                            default:
                                isGuestOdd = !isGuestOdd;
                                break;
                        }
                        entity.IsGuest = isGuestOdd;
                    }
                    else
                    {
                        isGuestOdd = false;
                    }


                    lastBetId = entity.BetId;


                    //db.Odd.Add(entity);

                    odds.Add(entity);

                    if (!db.Odd.Any(x => x == entity))
                    {
                        db.Odd.Add(entity);
                    }
                    else
                    {
                        db.Odd.Update(entity);
                    }
                }

                db.SaveChanges();
            }
        }
    }
}
