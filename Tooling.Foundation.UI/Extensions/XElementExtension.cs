using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tooling.Foundation.Extensions
{
    public static class XElementExtension
    {
        public static string GetAttributeValue(this XElement element, string naam)
        {
            return element.Attributes(naam).FirstOrDefault()?.Value;
        }


        public static IEnumerable<XmlNamespace> GetNamespaces(this XElement element)
        {
            for (XElement i = element; i != null; i = i.Parent)
            {
                XAttribute xAttribute = i.LastAttribute;
                if (xAttribute != null)
                {
                    do
                    {
                        xAttribute = xAttribute.NextAttribute;
                        if (!xAttribute.IsNamespaceDeclaration)
                        {
                            continue;
                        }
                        yield return new XmlNamespace
                        {
                            Prefix = xAttribute.Name.LocalName,
                            Namespace = xAttribute.Value
                        };
                    }
                    while (xAttribute != i.LastAttribute);
                }
            }
        }

        public static void SetElementValue(this XDocument xDocument, int value, params string[] elementNames)
        {
            xDocument.SetElementValue(value.ToString(), elementNames);
        }

        public static void SetElementValue(this XDocument xDocument, string value, params string[] elementNames)
        {
            XElement element = xDocument.GetElement(value, elementNames);
            if (element != null)
            {
                element.Value = value;
            }
        }

        public static XElement GetElement(this XDocument xDocument, string value, params string[] elementNames)
        {
            if (elementNames.Length == 1
                && elementNames[0].Contains('/'))
            {
                elementNames = elementNames[0].Split('/');
            }

            XElement element = xDocument.Root;
            foreach (string name in elementNames.Skip(1))
            {
                if (element == null)
                {
                    break;
                }
                element = element.Element(name);
            }

            return element;
        }
        
    }
}