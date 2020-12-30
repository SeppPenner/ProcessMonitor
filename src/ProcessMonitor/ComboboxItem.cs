// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboboxItem.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The combo box item class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ProcessMonitor
{
    /// <summary>
    /// The combo box item class.
    /// </summary>
    public class ComboboxItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Converts the object to a <see cref="string"/>.
        /// </summary>
        /// <returns>A object as <see cref="string"/>.</returns>
        public override string ToString()
        {
            return this.Text;
        }
    }
}