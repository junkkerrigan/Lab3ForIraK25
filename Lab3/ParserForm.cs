using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace Lab3
{
    public partial class ParserForm : Form
    {
        ComboBox FilterForProgress, FilterForImplementer;
        RichTextBox FilterForDescription, FilterForTitle, FilterForDaysLeftFrom, 
            FilterForDaysLeftTo, MatchData;
        Label Title, Description, Progress, Implementer, DaysLeft, Separator;
        Button Search, RefreshData, Clear, ToHTML;
        RadioButton LINQ, DOM, SAX;
        
        ParsingStrategy Parser;
        Filter CurrentFilters = new Filter(); 

        string PathToXSLTRules = "../../template.xsl";
        string PathToHTML = "../../todotable.html";
        string PathToXML = "../../todos.xml";

        public ParserForm()
        {
            SizeChanged += ParserFormSizeChanged;
            FormClosing += ParserFormClosed;
            FillLayout();
            Text = "XMLParser";
            Parser = new StrategySAX(PathToXML);
            FillMatchData();
        }

        void FillLayout()
        {
            MatchData = new RichTextBox
            {
                Location = new Point(50, 50),
                Size = new Size((ClientSize.Width - 200) / 2, ClientSize.Height - 100),
                Font = new Font("Times New Roman", 14),
            };

            MountFilters();
            MountLabels();
            MountRadiobuttons();
            MountButtons();
            Controls.Add(MatchData);
        }
        void MountFilters()
        {
            FilterForTitle = new RichTextBox
            {
                Location = new Point(MatchData.Right + 240, MatchData.Top + 30),
                Width = (ClientSize.Width - 200) / 2 - 140,
                Height = 35,
                Font = new Font("Verdana", 16),
                Name = "Title",
            };
            FilterForTitle.TextChanged += FilterChanged;

            FilterForProgress = new ComboBox
            {
                Location = new Point(FilterForTitle.Left, FilterForTitle.Bottom + 20),
                Width = FilterForTitle.Width,
                Font = new Font("Verdana", 16),
                Name = "Progress",
            };
            FilterForProgress.TextChanged += FilterChanged;

            FilterForDescription = new RichTextBox
            {
                Location = new Point(FilterForTitle.Left, FilterForProgress.Top + 55),
                Width = FilterForTitle.Width,
                Height = 35,
                Font = new Font("Verdana", 14),
                Name = "Description",
            };
            FilterForDescription.TextChanged += FilterChanged;

            FilterForImplementer = new ComboBox
            {
                Location = new Point(FilterForTitle.Left, FilterForDescription.Bottom + 20),
                Width = FilterForTitle.Width,
                Font = new Font("Verdana", 16),
                Name = "Implementer",
            };
            FilterForImplementer.TextChanged += FilterChanged;

            FilterForDaysLeftFrom = new RichTextBox
            {
                Location = new Point(FilterForTitle.Left, FilterForImplementer.Top + 55),
                Width = (FilterForTitle.Width - 20) / 2,
                Height = 35,
                Font = new Font("Verdana", 14),
                Name = "DaysLeftFrom",
            };
            FilterForDaysLeftFrom.TextChanged += FilterChanged;

            FilterForDaysLeftTo = new RichTextBox
            {
                Location = new Point(FilterForDaysLeftFrom.Right + 40, FilterForDaysLeftFrom.Top),
                Width = FilterForDaysLeftFrom.Width,
                Height = 35,
                Font = new Font("Verdana", 14),
                Name  = "DaysLeftTo",
            };
            FilterForDaysLeftTo.TextChanged += FilterChanged;

            Controls.Add(FilterForTitle);
            Controls.Add(FilterForProgress);
            Controls.Add(FilterForDescription);
            Controls.Add(FilterForImplementer);
            Controls.Add(FilterForDaysLeftFrom);
            Controls.Add(FilterForDaysLeftTo);
        }
        void MountLabels()
        {
            Title = new Label()
            {
                Text = "Title:",
                Location = new Point(MatchData.Right + 100, FilterForTitle.Top + 8),
                Width = 140,
                Font = new Font("Verdana", 12),
            };
            Progress = new Label()
            {
                Text = "Progress:",
                Location = new Point(Title.Left, FilterForProgress.Top + 8),
                Width = 140,
                Font = new Font("Verdana", 12),
            };
            Description = new Label()
            {
                Text = "Description:",
                Location = new Point(Title.Left, FilterForDescription.Top + 8),
                Width = 140,
                Font = new Font("Verdana", 12),
            };
            Implementer = new Label()
            {
                Text = "Implementer:",
                Location = new Point(Title.Left, FilterForImplementer.Top + 8),
                Width = 140,
                Font = new Font("Verdana", 12),
            };
            DaysLeft = new Label()
            {
                Text = "Days left:",
                Location = new Point(Title.Left, FilterForDaysLeftFrom.Top + 8),
                Width = 140,
                Font = new Font("Verdana", 12),
            };
            Separator = new Label()
            {
                Text = "-",
                Width = 20,
                Location = new Point(FilterForDaysLeftFrom.Right + 10, FilterForDaysLeftFrom.Top + 2),
                Font = new Font("Verdana", 16),
            };

            Controls.Add(Title);
            Controls.Add(Description);
            Controls.Add(Progress);
            Controls.Add(Implementer);
            Controls.Add(DaysLeft);
            Controls.Add(Separator);
        }
        void MountRadiobuttons()
        {
            SAX = new RadioButton()
            {
                Text = "SAX",
                Name = "SAX",
                Font = new Font("Verdana", 14),
                Height = 30,
                Width = 100,
                Checked = true,
                Location = new Point(FilterForTitle.Left + 
                    (MatchData.Width - 340) / 2, FilterForDaysLeftFrom.Bottom + 20),
            };
            SAX.CheckedChanged += StrategyChanged;

            DOM = new RadioButton()
            {
                Text = "DOM",
                Name = "DOM",
                Height = 30,
                Width = 100,
                Font = new Font("Verdana", 14),
                Location = new Point(SAX.Right + 10, SAX.Top),
            };
            DOM.CheckedChanged += StrategyChanged;
            
            LINQ = new RadioButton()
            {
                Text = "LINQ",
                Name = "LINQ",
                Height = 30,
                Width = 100,
                Font = new Font("Verdana", 14),
                Location = new Point(DOM.Right + 10, SAX.Top),
            };
            LINQ.CheckedChanged += StrategyChanged;

            Controls.Add(SAX);
            Controls.Add(DOM);
            Controls.Add(LINQ);
        }
        void MountButtons()
        {
            Clear = new Button()
            {
                Text = "Clear",
                Location = new Point(Title.Left + 
                    (MatchData.Width - 500) / 2, SAX.Bottom + 20),
                Font = new Font("Verdana", 14),
                Width = 150,
                Height = 40,
            };
            Clear.Click += (s, e) => ClearFilters();

            Search = new Button()
            {
                Text = "Search",
                Location = new Point(Clear.Right + 25, Clear.Top),
                Font = new Font("Verdana", 14),
                Width = 150,
                Height = 40,
            };
            Search.Click += (s, e) => FillMatchData();
            
            RefreshData = new Button()
            {
                Text = "Load",
                Location = new Point(Search.Right + 20, Search.Top),
                Font = new Font("Verdana", 14),
                Width = 150,
                Height = 40,
            };
            RefreshData.Click += (s, e) => LoadXML();

            ToHTML = new Button()
            {
                Text = "Transform",
                Location = new Point(MatchData.Right - 150, MatchData.Top + 10),
                Font = new Font("Verdana", 10),
                Size = new Size(120, 30),
            };
            ToHTML.Click += (s, e) => TransformToHTML();
            
            Controls.Add(Clear);
            Controls.Add(Search);
            Controls.Add(RefreshData);
            Controls.Add(ToHTML);
        }
        
        void FilterChanged(object sender, EventArgs e)
        {
            string newValue, propTitle;
            int oldValue;
            if (sender is RichTextBox)
            {
                RichTextBox f = sender as RichTextBox;
                newValue = f.Text;
                propTitle = f.Name;
                if (!(f.Name == "DaysLeftFrom" || f.Name == "DaysLeftTo"))
                {
                    CurrentFilters.UpdateFilter(propTitle, newValue);
                }
                else
                {
                    oldValue = (int)CurrentFilters.GetType().GetProperty(propTitle).GetValue(CurrentFilters);
                    try
                    {
                        CurrentFilters.UpdateFilter(propTitle, newValue);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid days left filter value", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (oldValue == int.MaxValue || oldValue == int.MinValue)
                            f.Text = "";
                        else f.Text = oldValue.ToString();
                    }
                }
            }
            else
            {
                ComboBox f = sender as ComboBox;
                newValue = f.Text;
                propTitle = f.Name;
                if (!(f.Name == "DaysLeftFrom" || f.Name == "DaysLeftTo"))
                {
                    CurrentFilters.UpdateFilter(propTitle, newValue);
                }
                else
                {
                    oldValue = (int)CurrentFilters.GetType().GetProperty(propTitle).GetValue(CurrentFilters);
                    try
                    {
                        CurrentFilters.UpdateFilter(propTitle, newValue);
                    }
                    catch
                    {
                        MessageBox.Show("Invalid days left filter value", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (oldValue == int.MinValue || oldValue == int.MaxValue)
                            f.Text = "";
                        else f.Text = oldValue.ToString();
                    }
                }
            }
        }
        void ParserFormSizeChanged(object sender, EventArgs e)
        {
            MatchData.Size = new Size((ClientSize.Width - 200) / 2, ClientSize.Height - 100);

            FilterForTitle.Left = MatchData.Right + 240;
            FilterForTitle.Width = (ClientSize.Width - 200) / 2 - 140;

            FilterForProgress.Left = FilterForTitle.Left;
            FilterForProgress.Width = FilterForTitle.Width;

            FilterForDescription.Left = FilterForTitle.Left;
            FilterForDescription.Width = FilterForTitle.Width;

            FilterForImplementer.Width = FilterForTitle.Width;
            FilterForImplementer.Left = FilterForTitle.Left;

            FilterForDaysLeftFrom.Width = (FilterForTitle.Width - 40) / 2;
            FilterForDaysLeftFrom.Left = FilterForTitle.Left;

            FilterForDaysLeftTo.Left = FilterForDaysLeftFrom.Right + 40;
            FilterForDaysLeftTo.Width = FilterForDaysLeftFrom.Width;

            Title.Left = MatchData.Right + 100;
            Progress.Left = Title.Left;
            Description.Left = Title.Left;
            Implementer.Left = Title.Left;
            DaysLeft.Left = Title.Left;
            Separator.Left = FilterForDaysLeftFrom.Right + 10;

            SAX.Left = Title.Left + (MatchData.Width - 340) / 2;
            DOM.Left = SAX.Right + 10;
            LINQ.Left = DOM.Right + 10;

            Clear.Left = Title.Left + (MatchData.Width - 500) / 2;
            Search.Left = Clear.Right + 20;
            RefreshData.Left = Search.Right + 20;

            ToHTML.Left = MatchData.Right - 150;
        }
        void StrategyChanged(object sender, EventArgs e)
        {
            RadioButton selectedTool = sender as RadioButton;
            if (selectedTool.Name == "LINQ") Parser = new StrategyLINQ(PathToXML);
            else if (selectedTool.Name == "DOM") Parser = new StrategyDOM(PathToXML);
            else if (selectedTool.Name == "SAX") Parser = new StrategySAX(PathToXML);
        }
        void ParserFormClosed(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to leave Parser?", "Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                e.Cancel = true;
        }
        
        void ClearFilters()
        {
            CurrentFilters = new Filter();
            FilterForProgress.Text = "";
            FilterForTitle.Text = "";
            FilterForImplementer.Text = "";
            FilterForDescription.Text = ""; 
            FilterForDaysLeftFrom.Text = "";
            FilterForDaysLeftTo.Text = "";
            FillMatchData();
        }
        void FillMatchData()
        {
            var res = Parser.Parse(CurrentFilters);
            string TodosStr = "";
            foreach (var Todo in res.TodosList)
            {
                TodosStr += Todo.Prettify() + "\n";
            }
            MatchData.Text = TodosStr;

            FilterForProgress.Items.Clear();
            FilterForImplementer.Items.Clear();
            
            FilterForProgress.Items.AddRange(res.ProgressStages);
            FilterForImplementer.Items.AddRange(res.Implementers);
        }
        void TransformToHTML()
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(PathToXSLTRules);
            transform.Transform(PathToXML, PathToHTML);
        }
        void LoadXML()
        {
            Parser.GetXML(PathToXML);
            FillMatchData();
        }
    }
}
