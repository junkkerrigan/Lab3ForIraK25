using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab3
{
    public class StrategySAX : ParsingStrategy
    {
        string FileName;
        XmlReader Reader;

        public StrategySAX(string file)
        {
            FileName = file;
        }

        public MatchInfo Parse(Filter filter)
        {
            GetXML(FileName);
            Todo Todo = null;
            int i = 0;
            List<Todo> match = new List<Todo>();
            List<string> progressStages = new List<string>();
            List<string> implementers = new List<string>();

            while (Reader.Read())
            {
                switch (Reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (Reader.Name)
                        {
                            case "todo":
                                Todo = new Todo();
                                break;
                            case "title":
                                Reader.Read();
                                Todo.Title = Reader.Value;
                                break;
                            case "daysLeft":
                                Reader.Read();
                                Todo.DaysLeft = Reader.Value;
                                break;
                            case "description":
                                Reader.Read();
                                Todo.Description = Reader.Value.Trim();
                                break;
                            case "progress":
                                Reader.Read();
                                Todo.Progress= Reader.Value;
                                break;
                            case "implementer":
                                Reader.Read();
                                Todo.Implementer = Reader.Value;
                                break;
                            default:
                                break;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if (Reader.Name == "todo")
                        {
                            if (filter.CheckTodo(Todo))
                            {
                                progressStages.Add(Todo.Progress);
                                implementers.Add(Todo.Implementer);
                                i++;
                                match.Add(Todo);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return new MatchInfo()
            {
                TodosList = match.ToArray(),
                ProgressStages = progressStages.Distinct().ToArray(),
                Implementers = implementers.Distinct().ToArray(),
            };
        }

        public void GetXML(string file)
        {
            Reader = XmlReader.Create(file);
        }
    }
}
