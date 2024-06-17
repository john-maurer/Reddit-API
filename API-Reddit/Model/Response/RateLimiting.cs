namespace API_Reddit.Model.Response
{
    public class RateLimiting
    {
        public RateLimiting()
        {

        }

        public RateLimiting(RateLimiting rateLimiting) 
        {
            Used = rateLimiting.Used;
            Remaining = rateLimiting.Remaining;
            Reset = rateLimiting.Reset;
        }

        public float Used { get; set; }
        public float Remaining { get; set; }
        public float Reset { get; set; }
    }
}