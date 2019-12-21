using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab3
{
    public class StrategyDOM : ParsingStrategy
    {
        XmlDocument xmlDoc = new XmlDocument();

        public StrategyDOM(string file)
        {
            GetXML(file);
        }

        public MatchInfo Parse(Filter filter)
        {
            var TodoNodes = xmlDoc.SelectNodes("//todo");
            List<Todo> match = new List<Todo>();
            List<string> mealtimes = new List<string>();
            List<string> implementers = new List<string>();
            foreach (XmlNode node in TodoNodes)
            {
                Todo Todo = new Todo()
                {
                    Title = node.SelectSingleNode("title").InnerText,
                    Progress = node.SelectSingleNode("progress").InnerText,
                    Implementer = node.SelectSingleNode("implementer").InnerText,
                    Description = node.SelectSingleNode("description").InnerText.Trim(),
                    DaysLeft = node.SelectSingleNode("daysLeft").InnerText,
                };
                if (filter.CheckTodo(Todo))
                {
                    match.Add(Todo);
                    mealtimes.Add(Todo.Progress);
                    implementers.Add(Todo.Implementer);
                }
            }

            return new MatchInfo()
            {
                TodosList = match.ToArray(),
                ProgressStages = mealtimes.Distinct().ToArray(),
                Implementers = implementers.Distinct().ToArray(),
            };
        }

        public void GetXML(string file)
        {
            xmlDoc.Load(file);
        }
    }
}
