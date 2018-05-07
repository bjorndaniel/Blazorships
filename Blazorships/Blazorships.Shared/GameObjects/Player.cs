//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using System;
using System.Collections.Generic;
using System.Linq;
using Blazorships.Shared.GameObjects;
using Blazorships.Shared.Helpers;

public class Player
{
    public string Id { get; set; }
    public string Name { get; set; }
    public GameBoard GameBoard { get; set; }
    public FiringBoard FiringBoard { get; set; }
    public List<Ship> Ships { get; set; }
    public bool HasLost => Ships.All (x => x.IsSunk);
    public bool IsMyTurn { get; set; }
    public string ConnectionId { get; set; }
    public Player (string name)
    {
        Id = Guid.NewGuid ().ToString ();
        Name = name;
        Ships = new List<Ship> ()
        {
            new Destroyer (),
            new Submarine (),
            new Cruiser (),
            new Battleship (),
            new Carrier ()
        };

        GameBoard = new GameBoard ();
        FiringBoard = new FiringBoard ();
        if (!string.IsNullOrEmpty (name))
        {
            PlaceShips ();
        }
    }
    public Player() { }

    private void PlaceShips ()
    {
        //Random class creation stolen from http://stackoverflow.com/a/18267477/106356
        Random rand = new Random (Guid.NewGuid ().GetHashCode ());
        foreach (var ship in Ships)
        {
            //Select a random row/column combination, then select a random orientation.
            //If none of the proposed panels are occupied, place the ship
            //Do this for all ships

            bool isOpen = true;
            while (isOpen)
            {
                var startcolumn = rand.Next (1, 11);
                var startrow = rand.Next (1, 11);
                int endrow = startrow, endcolumn = startcolumn;
                var orientation = rand.Next (1, 101)% 2; //0 for Horizontal

                List<int> panelNumbers = new List<int> ();
                if (orientation == 0)
                {
                    for (int i = 1; i < ship.Width; i++)
                    {
                        endrow++;
                    }
                }
                else
                {
                    for (int i = 1; i < ship.Width; i++)
                    {
                        endcolumn++;
                    }
                }

                //We cannot place ships beyond the boundaries of the board
                if (endrow > 10 || endcolumn > 10)
                {
                    isOpen = true;
                    continue;
                }

                //Check if specified panels are occupied
                var affectedPanels = GameBoard.Panels.Range (startrow, startcolumn, endrow, endcolumn);
                if (affectedPanels.Any (x => x.IsOccupied))
                {
                    isOpen = true;
                    continue;
                }

                foreach (var panel in affectedPanels)
                {
                    panel.OccupationType = ship.OccupationType;
                }
                isOpen = false;
            }
        }
    }

    public Coordinates FireShot ()
    {
        //If there are hits on the board with neighbors which don't have shots, we should fire at those first.
        var hitNeighbors = FiringBoard.GetHitNeighbors ();
        Coordinates coords;
        if (hitNeighbors.Any ())
        {
            coords = SearchingShot ();
        }
        else
        {
            coords = RandomShot ();
        }
        Console.WriteLine (Name + " says: \"Firing shot at " + coords.Row.ToString ()+ ", " + coords.Column.ToString ()+ "\"");
        return coords;
    }

    private Coordinates RandomShot ()
    {
        var availablePanels = FiringBoard.GetOpenRandomPanels ();
        Random rand = new Random (Guid.NewGuid ().GetHashCode ());
        var panelID = rand.Next (availablePanels.Count);
        return availablePanels[panelID];
    }

    private Coordinates SearchingShot ()
    {
        Random rand = new Random (Guid.NewGuid ().GetHashCode ());
        var hitNeighbors = FiringBoard.GetHitNeighbors ();
        var neighborID = rand.Next (hitNeighbors.Count);
        return hitNeighbors[neighborID];
    }

    public ShotResult ProcessShot (Coordinates coords)
    {
        var panel = GameBoard.Panels.At (coords.Row, coords.Column);
        if (!panel.IsOccupied)
        {
            Console.WriteLine (Name + " says: \"Miss!\"");
            return ShotResult.Miss;
        }
        var ship = Ships.First (x => x.OccupationType == panel.OccupationType);
        ship.Hits++;
        panel.IsHit = true;
        Console.WriteLine (Name + " says: \"Hit!\"");
        if (ship.IsSunk)
        {
            Console.WriteLine (Name + " says: \"You sunk my " + ship.Name + "!\"");
        }
        return ShotResult.Hit;
    }

    public void ProcessShotResult (Coordinates coords, ShotResult result)
    {
        var panel = FiringBoard.Panels.At (coords.Row, coords.Column);
        switch (result)
        {
            case ShotResult.Hit:
                panel.OccupationType = OccupationType.Hit;
                break;

            default:
                panel.OccupationType = OccupationType.Miss;
                break;
        }
    }
}