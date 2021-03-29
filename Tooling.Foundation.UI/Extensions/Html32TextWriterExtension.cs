using System.Web.UI;

namespace Tooling.Foundation.Extensions
{
    public static class Html32TextWriterExtension
    {
        public static void WriteValue(this Html32TextWriter w, string value, string classname)
        {
            w.WriteBeginTag("td");
            w.WriteAttribute("class", classname);
            w.Write(">");
            if (!string.IsNullOrEmpty(value))
            {
                w.WriteEncodedText(value);
            }
            w.WriteEndTag("td");
        }

        public static void WriteValue(this Html32TextWriter w, string value)
        {
            w.WriteFullBeginTag("td");
            if (!string.IsNullOrEmpty(value))
            {
                w.WriteEncodedText(value);
            }
            w.WriteEndTag("td");
        }

        public static void WriteValue(this Html32TextWriter w, int value)
        {
            w.WriteFullBeginTag("td");
            if (value != 0)
            {
                w.Write(value);
            }
            w.WriteEndTag("td");
        }

    }
}
