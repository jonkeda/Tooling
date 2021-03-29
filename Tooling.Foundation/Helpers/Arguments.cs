using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Foundations.Helpers
{
    public class Argument
    {
        public Argument(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }

    }

    /// <summary>
    /// Arguments class
    /// </summary>
    public class Arguments : IEnumerable<Argument>
    {
        // Variables
        private readonly HybridDictionary _parameters;

        public Arguments(string args) : this(SplitArguments(args))
        {
        }
        static readonly Regex Spliter = new Regex(@"^-{1,2}|^/|=",
             RegexOptions.IgnoreCase | RegexOptions.Compiled);

        static readonly Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // Constructor
        public Arguments(string[] args)
        {
            _parameters = new HybridDictionary(true);

            string parameter = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                string[] parts = Spliter.Split(txt, 3);

                switch (parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (parameter != null)
                        {
                            if (!_parameters.Contains(parameter))
                            {
                                parts[0] =
                                    Remover.Replace(parts[0], "$1");

                                _parameters.Add(parameter, new Argument(parameter, parts[0]));
                            }
                            parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!_parameters.Contains(parameter))
                            {
                                _parameters.Add(parameter, new Argument(parameter, "true"));
                            }
                        }
                        parameter = parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!_parameters.Contains(parameter))
                            {
                                _parameters.Add(parameter, new Argument(parameter, "true"));
                            }
                        }

                        parameter = parts[1];

                        // Remove possible enclosing characters (",')
                        if (!_parameters.Contains(parameter))
                        {
                            parts[2] = Remover.Replace(parts[2], "$1");

                            _parameters.Add(parameter, new Argument(parameter, parts[2]));
                        }

                        parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!_parameters.Contains(parameter))
                {
                    _parameters.Add(parameter, new Argument(parameter, "true"));
                }
            }
        }

        public bool Contains(string param)
        {
            return _parameters.Contains(param);
        }

        // Retrieve a parameter value if it exists 
        // (overriding C# indexer property)
        public string this[string param]
        {
            get
            {
                Argument argument = (Argument)_parameters[param];

                return argument?.Value;
            }
        }

        IEnumerator<Argument> IEnumerable<Argument>.GetEnumerator()
        {
            foreach (object value in _parameters.Values)
            {
                yield return (Argument)value;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return _parameters.Values.GetEnumerator();
        }

        // https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
        public static string[] SplitArguments(string commandLine)
        {
            var parmChars = commandLine.ToCharArray();
            var inSingleQuote = false;
            var inDoubleQuote = false;
            for (var index = 0; index < parmChars.Length; index++)
            {
                if (parmChars[index] == '"' && !inSingleQuote)
                {
                    inDoubleQuote = !inDoubleQuote;
                    parmChars[index] = '\n';
                }
                if (parmChars[index] == '\'' && !inDoubleQuote)
                {
                    inSingleQuote = !inSingleQuote;
                    parmChars[index] = '\n';
                }
                if (!inSingleQuote && !inDoubleQuote && parmChars[index] == ' ')
                    parmChars[index] = '\n';
            }
            return (new string(parmChars)).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
