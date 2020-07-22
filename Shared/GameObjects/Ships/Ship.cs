//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using Blazorships.Shared.Helpers;

namespace Blazorships.Shared.GameObjects.Ships
{
    /// <summary>
    /// Represents a player's ship as placed on their Game Board.
    /// </summary>
    public class Ship
    {
        public Ship() { }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Hits { get; set; }
        public OccupationType OccupationType { get; set; }
        public bool IsSunk
        {
            get
            {
                return Hits >= Width;
            }
        }
    }
}
