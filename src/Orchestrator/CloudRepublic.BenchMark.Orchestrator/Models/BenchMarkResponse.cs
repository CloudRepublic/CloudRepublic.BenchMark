namespace CloudRepublic.BenchMark.Orchestrator.Models
{
    public class BenchMarkResponse
    {
        public bool Success { get; }
        public long Duration { get; }

        public BenchMarkResponse(bool success, long duration)
        {
            Success = success;
            Duration = duration;
        }
    }
}