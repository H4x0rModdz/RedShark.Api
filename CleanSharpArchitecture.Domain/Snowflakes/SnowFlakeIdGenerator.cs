namespace CleanSharpArchitecture.Domain.Snowflakes
{
    /// <summary>
    /// Implementa o algoritmo Snowflake do Twitter para geração de IDs únicos.
    /// </summary>
    public sealed class SnowflakeIdGenerator
    {
        private readonly long _workerId;
        private readonly long _datacenterId;
        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        private readonly object _lock = new object();

        /// <summary>
        /// A época base utilizada para calcular o timestamp. (04/11/2010 01:42:54.657 UTC)
        /// </summary>
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

        /// <summary>
        /// Constrói uma instância do gerador de IDs Snowflake.
        /// </summary>
        /// <param name="workerId">Identificador do worker (entre 0 e 31).</param>
        /// <param name="datacenterId">Identificador do datacenter (entre 0 e 31).</param>
        /// <exception cref="ArgumentException">Se os valores estiverem fora dos limites permitidos.</exception>
        public SnowflakeIdGenerator(long workerId, long datacenterId)
        {
            if (workerId < 0 || workerId > MaxWorkerId)
                throw new ArgumentException($"workerId deve ser entre 0 e {MaxWorkerId}.", nameof(workerId));

            if (datacenterId < 0 || datacenterId > MaxDatacenterId)
                throw new ArgumentException($"datacenterId deve ser entre 0 e {MaxDatacenterId}.", nameof(datacenterId));

            _workerId = workerId;
            _datacenterId = datacenterId;
        }

        /// <summary>
        /// Gera e retorna o próximo ID único.
        /// </summary>
        /// <returns>Um identificador único do tipo long.</returns>
        public long NextId()
        {
            lock (_lock)
            {
                long timestamp = GetCurrentTimestamp();
                if (timestamp < _lastTimestamp)
                {
                    throw new InvalidOperationException(
                        $"O relógio retrocedeu {_lastTimestamp - timestamp} ms. Recusando a gerar ID.");
                }

                if (timestamp == _lastTimestamp)
                {
                    _sequence = (_sequence + 1) & SequenceMask;
                    if (_sequence == 0)
                    {
                        // Sequência esgotada no mesmo milissegundo, aguarda próximo milissegundo
                        timestamp = WaitUntilNextMillis(_lastTimestamp);
                        _sequence = 0; // Reset da sequência para o novo timestamp
                    }
                }
                else
                {
                    // Novo timestamp, reset da sequência
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
