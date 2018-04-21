namespace TextReplacer.Components
{
    /// <summary>
    /// Result of the replacement.
    /// </summary>
    public class ReplaceResult
    {
        /// <summary>
        /// Processed string, or input string if no replacement is done.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Flag indicating whether the replacement has been happened. 
        /// </summary>
        public bool IsProcessed { get; set; }

        /// <summary>
        /// Error message if any error has been happened during processing.
        /// </summary>
        public string Error { get; set; }
    }
}