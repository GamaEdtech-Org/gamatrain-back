namespace GamaEdtech.Presentation.ViewModel.Blog
{
    using System;

    using GamaEdtech.Presentation.ViewModel.Tag;

    public sealed class PostResponseViewModel
    {
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public Uri? ImageUri { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public IEnumerable<TagResponseViewModel>? Tags { get; set; }
        public string? CreationUser { get; set; }
    }
}
