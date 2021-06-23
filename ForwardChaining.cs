using System.Collections.Generic;
using System.Linq;

namespace IEngine
{

    public class ForwardChaining : Agent
    {

        public Dictionary<string, bool> inferred = new Dictionary<string, bool>();

        public Dictionary<string, int> count = new Dictionary<string, int>();

        // public Stack<string> agenda = new Stack<string>();
        // public KnowledgeBase knowledgeBase = new KnowledgeBase();


        public ForwardChaining(string ask, string tell) : base(ask, tell)
        {

            Setup(tell);
        }
        public void Setup(string tell)
        {
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
                    Agenda.Add(statementInFile);
                }
                else
                {
                    Clause newClause = new(statementInFile);
                    Clauses.Add(newClause);
                    count[newClause.FullClause] = newClause.BodyCount;
                }
            }
        }
        public string PrintSolution()
        {
            string result;

            if (Entails())
            {
                result = "Yes: ";
                foreach (var entail in Entailed)
                {
                    if (!(entail == ""))
                    {
                        result += entail + ", ";
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
            // count - indexed by clause initially the number of premises 
            // inferred -a table indexed by symbol, each entry initially false
            // agenda  - a list of symbols, initally the symbols are known to be true
            while (Agenda.Any())
            {
                string p = Agenda[0];
                Agenda.RemoveAt(0); // pop off the first item

                if (Entailed.Contains(p)) // entailed = inferred
                {
                    continue;
                }
                Entailed.Add(p);
                foreach (Clause clause in Clauses) //whose premise p appears
                {
                    if (p == Ask)
                    {
                        return true;
                    }
                    if (clause.BodyContains(p))
                    {

                        // reduce the count of elements we dont know
                        count[clause.FullClause]--;
                        // if all the elements in the body are now known, we check the head
                        if (count[clause.FullClause] == 0)
                        {
                            Agenda.Add(clause.Head);
                        }
                    }
                }
            }
            return false;
        }
    }
}