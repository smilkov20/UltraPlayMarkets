using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using UltraPlayMarkets.Data;
using UltraPlayMarkets.Models;
using UltraPlayMarkets.Utilities.Enums;

namespace UltraPlayMarkets.Utilities
{
    public class XMLReader
    {

        public string URLString = "https://sports.ultraplay.net/sportsxml?clientKey=b4dde172-4e11-43e4-b290-abdeb0ffd711&sportId=2357&days=2";

        public void Read()
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(URLString);


            string text;
            using (var webclient = new WebClient())
            {
                text = webclient.DownloadString(URLString);
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(text);

            using (var db = new MarketsDbContext())
            {
                var nodes = xmlDocument.SelectNodes("//Sport");
            
                foreach (XmlNode node in nodes)
                {
                    Sport entity = new Sport();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);

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

                foreach (XmlNode node in nodes)
                {
                    Odd entity = new Odd();

                    entity.Name = node.Attributes.GetNamedItem("Name").Value;
                    entity.Id = int.Parse(node.Attributes.GetNamedItem("ID").Value);
                    entity.Value = double.Parse(node.Attributes.GetNamedItem("Value").Value);
                    entity.SpecialBetValue = node.Attributes.GetNamedItem("SpecialBetValue")?.Value;
                    entity.BetId = int.Parse(node.ParentNode.Attributes.GetNamedItem("ID").Value);

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
