namespace GamaEdtech.Presentation.ViewModel.File
{
    public sealed class ExamResponseViewModel
    {
        public string? Url { get; set; }
        public ExamViewModel? Exam { get; set; }
        public IEnumerable<TestViewModel>? Tests { get; set; }

        public sealed class ExamViewModel
        {
            public string? Id { get; set; }
            public string? User { get; set; }
            public string? Type { get; set; }
            public string? AzmoonType { get; set; }
            public string? Title { get; set; }
            public string? Tests { get; set; }
            public string? TestsCount { get; set; }
            public string? AzmoonTime { get; set; }
            public string? ScoreType { get; set; }
            public string? Code { get; set; }
            public string? Status { get; set; }
        }

        public sealed class TestViewModel
        {
            public string? Id { get; set; }
            public string? Code { get; set; }
            public string? User { get; set; }
            public string? Question { get; set; }
            public string? Lesson { get; set; }
            public string? Type { get; set; }
            public string? AnswerA { get; set; }
            public string? AnswerB { get; set; }
            public string? AnswerC { get; set; }
            public string? AnswerD { get; set; }
            public string? QFile { get; set; }
            public string? AFile { get; set; }
            public string? BFile { get; set; }
            public string? CFile { get; set; }
            public string? DFile { get; set; }
            public string? AnswerViewType { get; set; }
            public string? Direction { get; set; }
            public bool Owner { get; set; }
            public string? Title { get; set; }
            public string? LessonTitle { get; set; }
            public bool TestImageAnswers { get; set; }
        }
    }
}
