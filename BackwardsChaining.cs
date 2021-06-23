using System.Collections.Generic;
using System.Linq;

namespace IEngine
{
    public class BackwardsChaining : Agent
    {
        public BackwardsChaining(string ask, string tell) : base(ask, tell)
        {
            Agenda.Add(Ask);
            Setup(tell);
        }
        public void Setup(string tell)
        {
            Agenda.Add(Ask);
            string[] factsInFile = tell.Split(";")
                .Where(s => !string.IsNullOrEmpty(s))
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToArray();
            foreach (var statementInFile in factsInFile)
            {
                //find out if its a fact
                if (!statementInFile.Contains("=>"))
                {
                    // if the symbol list doesnt have this fact then add it
                    Facts.Add(statementInFile);
                }
                else
                {
                    Clause newClause = new(statementInFile);
                    Clauses.Add(newClause);
                    Count.Add(statementInFile.Split("&").Length);
                }
            }
        }
        public string PrintSolution()
        {
            string result;

            if (Entails())
            {
                result = "Yes: ";
                foreach (var fact in Facts)
                {
                    if (!(fact == ""))
                    {
                        result += fact + ", ";
                    }
                }
                result += Ask + " is true.";
            }
            else
            {
                result = "No";
            }
            return result;
        }
        public bool Entails()
        {
            while (Agenda.Any())
            {
                var p = Agenda[^1]; //Removes object at the beginning of the queue
                Agenda.RemoveAt(Agenda.Count - 1);

                if (Facts.Contains(p))
                {
                    continue;
                }
                if (p != Ask)
                {
                    if (!Entailed.Contains(p))
                        Entailed.Insert(0, p);
                }

                if (!Facts.Contains(p))
                {
                    foreach (Clause clause in Clauses)
                    {
                        // if clause is p1 => p3
                        if (clause.BodyContains(p))
                        {
                            Facts.Add(clause.Head);
                        }

                        if (Facts.Count == 0)
                        {
                            return false;
                        }
                        else
                        {
                            foreach (var fact in Facts)
                            {
                                if (!Agenda.Contains(fact))
                                {
                                    Agenda.Add(fact);
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
