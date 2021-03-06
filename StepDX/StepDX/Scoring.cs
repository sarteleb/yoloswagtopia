﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using System.Windows.Forms;

namespace StepDX
{
    class Scoring
    {
        private string persistenceFilename;

        private XDocument scoreboard;

        public Scoring(string fileName)
        {
            persistenceFilename = fileName;
        }

        public void Load()
        {
            try
            {
                scoreboard = XDocument.Load(persistenceFilename);
            }
            catch
            {
                scoreboard = null;
            }
            scoreboard = scoreboard ?? new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("App Scoreboard"),
                new XElement("scoreboard"));
        }

        public void Save()
        {
            scoreboard.Save(persistenceFilename);
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

        public static string DialogBox(int score)
        {
            Form prompt = new Form();
            prompt.Width = 400;
            prompt.Height = 180;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("You win!");
            builder.Append("Score: ");
            builder.Append(score);
            builder.Append("\n");
            builder.AppendLine("Your name? : ");
            prompt.Text = "Victory!";
            Label textLabel = new Label() { Left = 50, Top = 20, Height = 60, Text = builder.ToString() };
            TextBox textBox = new TextBox() { Left = 50, Top = 80, Width = 200 };
            Button confirmation = new Button() { Text = "Ok", Left = 280, Width = 80, Top = 80 };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}
