namespace GamaEdtech.Presentation.ViewModel.Game
{
    using System.Collections.ObjectModel;

    public class GameCoinResponseViewModel
    {
        public ReadOnlyCollection<string> Coins { get; init; } = new List<string>().AsReadOnly();
    }
}
