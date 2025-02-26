namespace GamaEdtech.Common.DataAccess.Exceptions
{
    using System;

#pragma warning disable CA1032 // Implement standard exception constructors
    public class RepositoryNotFoundException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public RepositoryNotFoundException(string repositoryName, string? message)
            : base(message)
        {
            if (string.IsNullOrEmpty(repositoryName))
            {
                throw new ArgumentException($"{nameof(repositoryName)} cannot be null or empty.", nameof(repositoryName));
            }

            RepositoryName = repositoryName;
        }

        public string? RepositoryName { get; }
    }
}
