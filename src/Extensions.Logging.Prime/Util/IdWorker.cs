using Yitter.IdGenerator;

namespace Extensions.Logging.Prime.Util
{
    internal class IdWorker
    {
        public static long SnowflakeId()
        {
            if (YitIdHelper.IdGenInstance == null)
            {
                var options = new IdGeneratorOptions
                {
                    Method = 1,
                    WorkerId = 1,
                };
                YitIdHelper.SetIdGenerator(options);
            }
            return YitIdHelper.NextId();
        }
    }
}
