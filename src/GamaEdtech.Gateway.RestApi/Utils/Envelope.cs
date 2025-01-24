namespace GamaEdtech.Gateway.RestApi.Utils;

public class Envelope
{
	public static Envelope Ok()
	{
		return new Envelope();
	}

	public static Envelope Ok(object result)
	{
		return new ResultEnvelope(result);
	}

	public static Envelope Error(string errorCode)
	{
		return new ErrorEnvelope(errorCode);
	}
}


public class ResultEnvelope : Envelope
{
	public object Result { get; }

	internal ResultEnvelope(object result)
	{
		Result = result;
	}
}

public class ErrorEnvelope : Envelope
{
	public string ErrorCode { get; }

	internal ErrorEnvelope(string errorCode)
	{
		ErrorCode = errorCode;
	}
}
