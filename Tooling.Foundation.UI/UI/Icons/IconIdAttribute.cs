using System;

namespace Tooling.Foundation.UI.Icons
{
    /// <summary>
    /// Represents the id (css class name) of the icon.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class IconIdAttribute
        : Attribute
    {
        /// <summary>
        /// Gets or sets the id (css class name) of the icon.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the FontAwesome.WPF.IconIdAttribute class.
        /// </summary>
        /// <param name="id">The icon id (css class name).</param>
        public IconIdAttribute(string id)
        {
            Id = id;
        }
    }
}