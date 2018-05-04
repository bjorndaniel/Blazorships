//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Blazorships.Shared.Helpers;

namespace Blazorships.Shared.GameObjects
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            Name = "Submarine";
            Width = 3;
            OccupationType = OccupationType.Submarine;
        }
    }
}