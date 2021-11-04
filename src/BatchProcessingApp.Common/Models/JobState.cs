namespace BatchProcessingApp.Common.Models
{
    public class JobState
    {
        public int TotalItems { get; set; }
        public int ProcessedItems { get; set; }
        public int Errors { get; set; }
    }
}
