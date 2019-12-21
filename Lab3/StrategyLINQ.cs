using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab3
{
    public class StrategyLINQ : ParsingStrategy
    {
        List<Todo> Todos;

        public StrategyLINQ(string file)
        {
            GetXML(file);
        }

        public void GetXML(string file)
        {
            XDocument XMLData = XDocument.Load(file);
            Todos = new List<Todo>(
                from book in XMLData.Element("todolist").Elements("todo")
                select new Todo()
                {
                    Title = book.Element("title").Value,
                    Progress = book.Element("progress").Value,
                    Implementer = book.Element("implementer").Value,
                    Description = book.Element("description").Value.Trim(),
                    DaysLeft = book.Element("daysLeft").Value,
                });
        }

        public MatchInfo Parse(Filter filter)
        {
            Todo[] match = (
                from book in Todos
                where filter.CheckTodo(book)
                select book).ToArray();

            return new MatchInfo()
            {
                TodosList = match,
                ProgressStages = (from book in match
                            select book.Progress).Distinct().ToArray(),
                Implementers = (from book in match
                          select book.Implementer)
                    .Distinct().ToArray(),
            };
        }
    }
}
