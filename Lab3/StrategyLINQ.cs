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
                from todo in XMLData.Element("todolist").Elements("todo")
                select new Todo()
                {
                    Title = todo.Element("title").Value,
                    Progress = todo.Element("progress").Value,
                    Implementer = todo.Element("implementer").Value,
                    Description = todo.Element("description").Value.Trim(),
                    DaysLeft = todo.Element("daysLeft").Value,
                });
        }

        public MatchInfo Parse(Filter filter)
        {
            Todo[] match = (
                from todo in Todos
                where filter.CheckTodo(todo)
                select todo).ToArray();

            return new MatchInfo()
            {
                TodosList = match,
                ProgressStages = (from todo in match
                            select todo.Progress).Distinct().ToArray(),
                Implementers = (from todo in match
                          select todo.Implementer)
                    .Distinct().ToArray(),
            };
        }
    }
}
