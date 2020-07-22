//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using Blazorships.Shared.Helpers;
using System.ComponentModel;

namespace Blazorships.Shared.Boards
{
    /// <summary>
    /// The basic class for this modelling practice.  Represents a single square on the game board.
    /// </summary>
    public class Panel
    {
        public OccupationType OccupationType { get; set; }
        public Coordinates Coordinates { get; set; }

        public Panel(int row, int column)
        {
            Coordinates = new Coordinates(row, column);
            OccupationType = OccupationType.Empty;
        }

        public Panel() { }

        public string Status =>
            OccupationType.GetAttributeOfType<DescriptionAttribute>().Description;

        public bool IsHit { get; set; }

        public bool IsOccupied =>
            OccupationType == OccupationType.Battleship
            || OccupationType == OccupationType.Destroyer
            || OccupationType == OccupationType.Cruiser
            || OccupationType == OccupationType.Submarine
            || OccupationType == OccupationType.Carrier;

        public bool IsRandomAvailable =>
            (Coordinates.Row % 2 == 0 && Coordinates.Column % 2 == 0)
            || (Coordinates.Row % 2 == 1 && Coordinates.Column % 2 == 1);
    }
}
