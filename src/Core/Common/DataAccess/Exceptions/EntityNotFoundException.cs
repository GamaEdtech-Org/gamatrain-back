namespace GamaEdtech.Common.DataAccess.Exceptions
{
    using System;

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityNotFoundException(string entityName, int entityKey)
            : base($"Entity of type '{entityName}' and key {entityKey} not found in the current context.")
        {
        }
    }
}
