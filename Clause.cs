using System;
using System.Collections.Generic;
using System.Linq;

namespace IEngine
{
    public class Clause
    {
        public string FullClause { get; set; }
        public int BodyCount { get; set; }
        public string Head { get; set; }
        public string Body { get; set; } //can also be called premise

        public Clause(string fullClause)
        {
            FullClause = fullClause;
            Head = FullClause.Split(new string[] { "=>" }, StringSplitOptions.None)[1].Trim(); //head is the right
            Body = FullClause.Split(new string[] { "=>" }, StringSplitOptions.None)[0].Trim(); //body is the left can be called premise
            BodyCount = Body.Split('&').Length;
        }

        public bool BodyContains(string query)
        {
            //this method returns true if body contains an & symbol
            string[] conjuncts = Body.Split('&');
            if (conjuncts.Length == 1)
            {
                //returns true if body has an & symbol, count = 1 and contains a p
                return Body.Contains(query);
            }
            // otherwise returns true/false if the conjuncts contain the query
            return conjuncts.ToList().Contains(query);
        }
    }
}

