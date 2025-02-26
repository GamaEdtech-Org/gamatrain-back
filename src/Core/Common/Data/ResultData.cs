namespace GamaEdtech.Common.Data
{
    using System.Collections.Generic;

    using static GamaEdtech.Common.Core.Constants;

    public struct ResultData<T>(OperationResult operationResult)
    {
        public T? Data { get; set; }

        public OperationResult OperationResult { get; set; } = operationResult;

        public IEnumerable<Error>? Errors { get; set; }
    }
}
