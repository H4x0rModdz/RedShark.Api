namespace CleanSharpArchitecture.Domain.Snowflakes
{
    // Twitter Snowflake algorithm for unique ID generation
    public sealed class SnowflakeIdGenerator
    {
        private readonly long _workerId;
        private readonly long _datacenterId;
        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        private readonly object _lock = new object();

        // Twitter epoch timestamp (04/11/2010 01:42:54.657 UTC)
        private const long Twepoch = 1288834974657L;

        private const int WorkerIdBits = 5;
        private const int DatacenterIdBits = 5;
        private const int SequenceBits = 12;

        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits);       // 31
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits);   // 31

        private const int WorkerIdShift = SequenceBits;
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits;
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;
        private const long SequenceMask = -1L ^ (-1L << SequenceBits);

        public SnowflakeIdGenerator(long workerId, long datacenterId)
        {
            if (workerId < 0 || workerId > MaxWorkerId)
                throw new ArgumentException($"workerId must be between 0 and {MaxWorkerId}.", nameof(workerId));

            if (datacenterId < 0 || datacenterId > MaxDatacenterId)
                throw new ArgumentException($"datacenterId must be between 0 and {MaxDatacenterId}.", nameof(datacenterId));

            _workerId = workerId;
            _datacenterId = datacenterId;
        }

        public long NextId()
        {
            lock (_lock)
            {
                long timestamp = GetCurrentTimestamp();
                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException(
                        $"Clock moved backwards {_lastTimestamp - timestamp} ms. Refusing to generate ID.");
                }

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                    {
                        // Sequence exhausted within millisecond, wait for next
                        timestamp = WaitUntilNextMillis(_lastTimestamp);
                        _sequence = 0; // Reset sequence for new timestamp
                    }
                }
                else
                {
                    // New timestamp, reset sequence
                    _sequence = 0;
                }

                _lastTimestamp = timestamp;
                return ((timestamp - Twepoch) << TimestampLeftShift) |
                       (_datacenterId << DatacenterIdShift) |
                       (_workerId << WorkerIdShift) |
                       _sequence;
            }
        }

        private static long GetCurrentTimestamp() =>
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        private long WaitUntilNextMillis(long lastTimestamp)
        {
            long timestamp = GetCurrentTimestamp();
            while (timestamp <= lastTimestamp)
                timestamp = GetCurrentTimestamp();
            
            return timestamp;
        }
    }
}
