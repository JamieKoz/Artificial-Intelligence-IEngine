using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEngine
{
    public abstract class Agent
    {
        private string _tell;
        private string _ask;
        private List<string> _agenda;
        private List<string> _facts;
        private List<Clause> _clauses;
        private List<int> _count;
        private List<string> _entailed;

        public Agent(string ask, string tell)
        {
            // initialise variables
            _agenda = new List<string>();
            _clauses = new List<Clause>();
            _entailed = new List<string>();
            _facts = new List<string>();
            _count = new List<int>();
            _tell = Program.RemoveWhiteSpacesInFile(tell);
            _ask = ask;
        }
        public string Tell
        {
            get { return _tell; }
            set { _tell = value; }
        }
        public string Ask
        {
            get { return _ask; }
            set { _ask = value; }
        }
        public List<string> Agenda
        {
            get { return _agenda; }
            set { _agenda = value; }
        }
        public List<string> Facts
        {
            get { return _facts; }
            set { _facts = value; }
        }
        public List<Clause> Clauses
        {
            get { return _clauses; }
            set { _clauses = value; }
        }
        public List<int> Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public List<string> Entailed
        {
            get { return _entailed; }
            set { _entailed = value; }
        }

    }

}