//From https://github.com/exceptionnotfound/BattleshipModellingPractice

using System.ComponentModel;
using Blazorships.Shared.Helpers;

namespace Blazorships.Shared.GameObjects
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

        public string Status
        {
            get
            {
                return OccupationType.GetAttributeOfType<DescriptionAttribute>().Description;
            }
        }

        public bool IsOccupied
        {
            get
            {
                return OccupationType == OccupationType.Battleship
                    || OccupationType == OccupationType.Destroyer
                    || OccupationType == OccupationType.Cruiser
                    || OccupationType == OccupationType.Submarine
                    || OccupationType == OccupationType.Carrier;
            }
        }

        public bool IsRandomAvailable
        {
            get
            {
                return (Coordinates.Row % 2 == 0 && Coordinates.Column % 2 == 0)
                    || (Coordinates.Row % 2 == 1 && Coordinates.Column % 2 == 1);
            }
        }
    }
}