using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplic.Text
{
    /// <summary>
    /// Parameter query helper
    /// </summary>
    public static class QueryParameterParser
    {
        /// <summary>
        /// Replace named parameter withing a query with another char
        /// </summary>
        /// <param name="query">Query to parse</param>
        /// <param name="namedParameter">List of named parameter</param>
        /// <param name="namedParameterStart">Named parameter start char, e.g. : / @</param>
        /// <param name="replaceString">String to replace the named parameter with</param>
        /// <returns>Prepared result</returns>
        public static ParameterResult ReplaceNamedParameter(this string query, IList<string> namedParameter, string namedParameterStart = ":", string replaceString = "?")
        {
            var returnValue = new ParameterResult();
            returnValue.Query = query;

            if (string.IsNullOrWhiteSpace(query))
                return returnValue;

            if (namedParameter == null || namedParameter.Count == 0)
                return returnValue;

            var orderedParameter = new List<string>();

            int lastIndex = 0;
            while (lastIndex != -1)
            {
                var nextIndex = int.MaxValue;
                string nextParameter = "";
                foreach (var parameter in namedParameter)
                {
                    var index = query.IndexOf($"{namedParameterStart}{parameter}", lastIndex);
                    if (index != -1 && index < nextIndex)
                    {
                        nextIndex = index;
                        nextParameter = parameter;
                    }
                }

                // Break if nothing was found
                if (nextIndex == int.MaxValue)
                {
                    lastIndex = -1;
                    break;
                }
                else
                {
                    // Add parameter list
                    orderedParameter.Add(nextParameter);
                    query = query.ReplaceFirst($"{namedParameterStart}{nextParameter}", replaceString);
                    lastIndex = nextIndex;
                }
            }
            returnValue.Query = query;
            returnValue.OrderedParameter = orderedParameter;

            return returnValue;
        }
    }

    /// <summary>
    /// Query and parameter result for named parameter replace method
    /// </summary>
    public class ParameterResult
    {
        /// <summary>
        /// Gets or sets the prepared query
        /// </summary>
        public string Query
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ordered parameter
        /// </summary>
        public IList<string> OrderedParameter
        {
            get;
            set;
        } = new List<string>();
    }
}
