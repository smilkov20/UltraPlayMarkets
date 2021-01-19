using System;
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

        private string URLString = "https://sports.ultraplay.net/sportsxml?clientKey=b4dde172-4e11-43e4-b290-abdeb0ffd711&sportId=2357&days=2";

        public void Read()
        {
            string xmlText;
            using (var webclient = new WebClient())
            {
                xmlText = webclient.DownloadString(URLString);
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);

            using (var db = new MarketsDbContext())
            {
                db.Odd.RemoveRange(db.Odd);
                db.Bet.RemoveRange(db.Bet);
                db.Match.RemoveRange(db.Match);
                db.Event.RemoveRange(db.Event);
                db.Sports.RemoveRange(db.Sports);
                db.SaveChanges();

                var nodes = xmlDocument.SelectNodes("//Sport");

                foreach (XmlNode node in nodes)
                {
                    Sport entity = new Sport();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);

                    db.Sports.Add(entity);
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

                    db.Event.Add(entity);
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event/Match");

                foreach (XmlNode node in nodes)
                {
                    Match entity = new Match();
                    var nameValue = node.Attributes.GetNamedItem("Name").Value;

                    var splitValues = nameValue.Split(" - ");
                    entity.Name = splitValues[0].TrimStart().TrimEnd();
                    entity.OpponentName = splitValues[1].TrimStart().TrimEnd();
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.StartDate = DateTime.Parse(node.Attributes.GetNamedItem("StartDate").Value);
                    MatchTypeEnum entityMatchType;
                    Enum.TryParse(node.Attributes.GetNamedItem("MatchType").Value, out entityMatchType);
                    entity.MatchType = entityMatchType;
                    entity.EventId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

                    db.Match.Add(entity);
                }

                nodes = xmlDocument.SelectNodes("//Sport/Event/Match/Bet");

                foreach (XmlNode node in nodes)
                {
                    Bet entity = new Bet();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.IsLive = bool.Parse(node.Attributes.GetNamedItem("IsLive").Value);
                    entity.MatchId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

                    db.Bet.Add(entity);
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
                        if (lastBetId != -1)
                        {
                            isGuestOdd = true;
                        }    
                        
                    }


                    lastBetId = entity.BetId;


                    db.Odd.Add(entity);
                }

                db.SaveChanges();

            }
        }
    }
}
