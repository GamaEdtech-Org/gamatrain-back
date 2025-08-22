namespace GamaEdtech.Presentation.ViewModel.Game
{
    using System.ComponentModel.DataAnnotations;

    public class GameRequestViewModel
    {
        [Range(1, 1000, ErrorMessage = "Points value must be between 1 and 1000.")]
        public int Points { get; set; }
    }
}
