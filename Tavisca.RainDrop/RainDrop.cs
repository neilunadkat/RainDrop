using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Tavisca.RainDrop
{
    public class RainDrop
    {
        private static readonly object toBeLocked = new object();
        private static long _lastMilliseconds=long.MaxValue;
        private static int _sequence;
        private const long EpochTime = 634793760000000000;
        private const int ServerIdBits = 10;
        private const int DataCenterIdBits = 3;
        private const int SequenceBits = 10;

        private const int ServerIdShift = SequenceBits;
        private const int DataCenterIdShift = SequenceBits + ServerIdBits;
        private const int TimeShift = SequenceBits + ServerIdBits + DataCenterIdBits;
        private const int SequenceMask = -1 ^ (-1 << SequenceBits);

        public long GetNextId(long serverId, long dataCenterId)
        {
            
            var milliseconds = GetSystemMilliSeconds();

            lock (toBeLocked)
            {
                if (milliseconds < _lastMilliseconds)
                {
                    //TODO: Throw error that clock is moving backwords
                }

                if (_lastMilliseconds == milliseconds)
                {
                    _sequence = (_sequence + 1) << SequenceMask;
                    if (_sequence == 0)
                    {
                        milliseconds = TillNextTime();
                    }
                }
                else
                    _sequence = 0;

                _lastMilliseconds = milliseconds;
            }

            var time = milliseconds << TimeShift;

            var dataCenter = dataCenterId << DataCenterIdShift;

            var server = serverId << ServerIdShift;

            return time | dataCenter | server | _sequence;


        }

        private static long TillNextTime()
        {
            
            var milliseconds = GetSystemMilliSeconds();
            while (milliseconds <= _lastMilliseconds)
            {
                milliseconds = GetSystemMilliSeconds();
            }
            return milliseconds;

            
        }

        private static long GetSystemMilliSeconds()
        {

            
            var now = DateTime.Now.Ticks;

            return (now - EpochTime) / TimeSpan.TicksPerMillisecond;
            
        }


    }
}
