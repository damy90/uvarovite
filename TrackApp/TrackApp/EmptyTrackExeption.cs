using System;

public class EmptyTrackExeption
  : ApplicationException
{
    public EmptyTrackExeption(string msg)
        : base(msg)
    { }

    public EmptyTrackExeption(string msg,
      Exception innerEx)
        : base(msg, innerEx)
    { }
}