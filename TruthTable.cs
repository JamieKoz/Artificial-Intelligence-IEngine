using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IEngine
{
    public class TruthTable : Agent
    {
        private int truth;

        public TruthTable(string ask, string tell) : base(ask, tell)
        {
            BuildFromFile(tell);
            truth = 0;
        }
        public void BuildFromFile(string tell)
        {
            string[] factsInFile = tell
                .Replace(" ", string.Empty)
                .Split(";", int.MaxValue, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (var statementInFile in factsInFile)
            {
                //find out if its a fact
                if (!statementInFile.Contains("=>"))
                {
                    // if the symbol list doesnt have this fact then add it
                    if (!Agenda.Contains(statementInFile))
                    {
                        Agenda.Add(statementInFile);
                    }
                    // if the list of facts doesnt have this fact then add it
                    if (!Facts.Contains(statementInFile))
                    {
                        Facts.Add(statementInFile);
                    }
                }
                else  //else its a clause not a fact
                {
                    Clause newClause = new(statementInFile);
                    Clauses.Add(newClause);

                    string[] dividedClause = statementInFile
                        .Split(new[] { "=>", "&" }, int.MaxValue, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);


                    foreach (var item in dividedClause)
                    {
                        if (!Agenda.Contains(item))
                            Agenda.Add(item);
                    }
                }
            }
        }
        public string PrintSolution()
        {
            string result;
            if (TruthTableEntails())
            {
                // the returned true so it entails
                result = "Yes: ";
                result += truth;
            }
            else
                result = "No";
            return result;
        }

        public bool IsAskTrue(Dictionary<string, bool> model)
        {
            // this is comparing each of the newly created possibilities to 'Ask' 
            return model[Ask];
        }

        public bool DeriveTruthinessOfBodyFromModel(Dictionary<string, bool> model, string body)
        {
            // a = true, b = true     this implication true a implies b if a is true then b is true
            // a = true, b = false    this implication is false, if a is true then b must be true 
            // a = false, b = true    if a is false then b 
            // a = false, b = false   if a is false then b must be false

            // Here we deal with a clause that only has a single symbol in the head. i.e  (a => b). 
            if (!body.Contains("&"))
                return model[body];

            // Here we deal with a clause body that contains multiple symbols. i.e (a&b => c)
            foreach (var symbol in body.Split("&"))
            {
                if (!model[symbol])
                    return false;
            }

            return true;

        }

        // this is checking if the knowledge base is derivable
        // verifying if it is true or false
        public bool IsKnowledgeBaseTrue(Dictionary<string, bool> model)
        {
            foreach (var clause in Clauses)
            {
                bool derivedBodyTruthiness = DeriveTruthinessOfBodyFromModel(model, clause.Body);

                // If there's no implication between the body and head of the clause the knowledge base is considered false. (Not derivable.)
                if (!(!derivedBodyTruthiness || model[clause.Head]))
                {
                    return false;
                }
            }

            // if the model doesn't contain a fact it's not derivable.
            foreach (string fact in Facts)
            {
                if (!model.ContainsKey(fact))
                {
                    return false;
                }
            }

            return true;
        }

        public bool TruthTableEntails()
        {
            return TruthTableCheck(Agenda, new Dictionary<string, bool>());
        }

        public bool TruthTableCheck(List<string> agenda, Dictionary<string, bool> model)
        {
            List<string> agendaFork = new List<string>(agenda);
            if (agenda.Count == 0)
            {
                if (IsKnowledgeBaseTrue(model))
                {
                    if (IsAskTrue(model))
                    {
                        truth++;
                        return true;
                    }
                    // return true;
                }

                return true;
            }
            else
            {
                string symbol = agendaFork[0];
                agendaFork.RemoveAt(0);

                Dictionary<string, bool> forkedModel = new(model);

                model.Add(symbol, false);
                forkedModel.Add(symbol, true);

                return TruthTableCheck(agendaFork, model) && TruthTableCheck(agendaFork, forkedModel);
            }
        }
    }
}

