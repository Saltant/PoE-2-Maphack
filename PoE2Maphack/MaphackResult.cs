namespace Saltant.PoE2Maphack
{
    /// <summary>
    /// Represents the result of a maphack operation, indicating success or failure and providing a message.
    /// </summary>
    internal class MaphackResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the maphack operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// Gets or sets a message describing the result of the maphack operation.
        /// </summary>
        public string Message { get; set; }
    }
}
