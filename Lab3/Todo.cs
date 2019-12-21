using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Todo
    {
        public string Prettify()
        {
            return $"Title: {Title}\n" + $"Progress: {Progress}\n" + 
                $"Description: {Description}\n" + $"Days left: {DaysLeft}\n" + 
                $"Implementer: {Implementer}\n";
        }
        public string Title { get; set; } = "";
        public string DaysLeft { get; set; } = "";
        public string Description { get; set; } = "";
        public string Progress { get; set; } = "";
        public string Implementer { get; set; } = "";
    }
}
