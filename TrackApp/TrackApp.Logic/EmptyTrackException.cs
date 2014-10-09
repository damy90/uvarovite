using System;

namespace TrackApp.Logic
{
    public class EmptyTrackException
    : ApplicationException
    {
        public EmptyTrackException(string msg)
            : base(msg)
        {
        }

        public EmptyTrackException(string msg, Exception innerEx)
            : base(msg, innerEx)
        {
        }
    }
}