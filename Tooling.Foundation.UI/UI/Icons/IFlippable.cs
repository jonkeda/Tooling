namespace Tooling.Foundation.UI.Icons
{
    /// <summary>
    /// Represents a flippable control
    /// </summary>
    public interface IFlippable
    {
        /// <summary>
        /// Gets or sets the current orientation (horizontal, vertical).
        /// </summary>
        FlipOrientation FlipOrientation { get; set; }
    }
}