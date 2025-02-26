namespace GamaEdtech.Common.Mapping
{
    public static class TypeAdapter
    {
        /// <summary>
        /// Adapt the source object to the destination type.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDestination">Destination type.</typeparam>
        /// <param name="source">Source object to adapt.</param>
        /// <returns>Adapted destination type.</returns>
        public static TDestination AdaptData<TSource, TDestination>(this TSource source) => Mapster.TypeAdapter.Adapt<TSource, TDestination>(source);

        /// <summary>
        /// Adapt the source object to the existing destination object.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TDestination">Destination type.</typeparam>
        /// <param name="source">Source object to adapt.</param>
        /// <param name="destination">The destination object to populate.</param>
        /// <returns>Adapted destination type.</returns>
        public static TDestination AdaptData<TSource, TDestination>(this TSource source, TDestination destination) => Mapster.TypeAdapter.Adapt(source, destination);
    }
}
