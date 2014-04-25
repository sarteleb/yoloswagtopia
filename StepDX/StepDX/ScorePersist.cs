﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using System.IO;

namespace StepDX
{
    class ScorePersist
    {
        private XmlReader persistenceFilestream;

        private XDocument scoreboard;

        public ScorePersist()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            persistenceFilestream = XmlReader.Create(assembly.GetManifestResourceStream("StepDX.highscores.xml"));
            scoreboard = XDocument.Load(persistenceFilestream);
        }

        public void Load()
        {
            try
            {
                scoreboard = XDocument.Load(persistenceFilestream);
            }
            catch
            {
                scoreboard = null;
            }
            scoreboard = scoreboard ?? new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Mario App Scoreboard Persistent Storage"),
                new XElement("scoreboard"));
        }

        public void Save()
        {
            //scoreboard.Save(persistenceFilestream);
        }

        public void AddScore(string name, int score)
        {
            scoreboard.Root.Add(new XElement("score",
                new XAttribute("name", name),
                new XAttribute("value", score)));
        }

        public HighScores GetHighScores()
        {
            HighScores scores = new HighScores();
            var orderedList = scoreboard.Root.Elements("score").OrderByDescending(n => int.Parse(n.Attribute("value").Value));
            foreach (var d in orderedList.Take(10))
            {
                scores.AddEntry(d.Attribute("name").Value, int.Parse(d.Attribute("value").Value));
            }
            return scores;
        }
    }
}