namespace GamaEdtech.Presentation.ViewModel
{
    using GamaEdtech.Common.DataAnnotation;

    public sealed class RangeViewModel<T>
    {
        [Display]
        public T? Start { get; set; }

        [Display]
        public T? End { get; set; }
    }
}
