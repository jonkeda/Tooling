namespace Tooling.Foundation.UI.Icons.Converters
{
    /// <summary>
    /// Defines the CssClassNameConverter mode. 
    /// </summary>
    public enum CssClassConverterMode
    {
        /// <summary>
        /// Default mode. Expects a string and converts to a FontAwesomeIcon.
        /// </summary>
        FromStringToIcon = 0,
        /// <summary>
        /// Expects a FontAwesomeIcon and converts it to a string.
        /// </summary>
        FromIconToString
    }
}