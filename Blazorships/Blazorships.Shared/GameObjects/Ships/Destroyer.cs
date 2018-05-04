//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Blazorships.Shared.Helpers;

namespace Blazorships.Shared.GameObjects
{
 public class Destroyer : Ship
    {
        public Destroyer()
        {
            Name = "Destroyer";
            Width = 2;
            OccupationType = OccupationType.Destroyer;
        }
    }
}