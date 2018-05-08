//From https://github.com/exceptionnotfound/BattleshipModellingPractice
using Blazorships.Shared.GameObjects;
using System;

public class Game
{
    public string Id { get; set; }
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public string Winner { get; set; }
    public bool GameOver { get; set; }
    public string StatusText
    {
        get
        {
            if (string.IsNullOrEmpty(Player1?.Name) || string.IsNullOrEmpty(Player2?.Name))
            {
                return "Waiting for player";
            }
            if (GameOver)
            {
                return $"The winner is {Winner}";
            }
            if (Player1.IsMyTurn)
            {
                return $"{Player1.Name} firing";
            }
            if (Player2.IsMyTurn)
            {
                return $"{Player2.Name} firing";
            }
            return string.Empty;
        }

    }
    public Game(bool empty = true)
    {
        Id = Guid.NewGuid().ToString();
        if (empty)
        {
            Player1 = new Player("");
            Player2 = new Player("");
        }
    }

    public Game()
    {
        Player1 = new Player("");
        Player2 = new Player("");
    }

    public void AddPlayer(string name, bool isPlayer1 = true)
    {
        if (isPlayer1)
        {
            Player1 = new Player(name);
        }
        else
        {
            Player2 = new Player(name);
        }
    }

    public void Start()
    {
        Player1.IsMyTurn = true;
    }

    public bool SpotAvailable => string.IsNullOrEmpty(Player2?.Name);

    public void PlayRound()
    {
        //Each exchange of shots is called a Round.
        //One round = Player 1 fires a shot, then Player 2 fires a shot.
        var coordinates = Player1.FireShot();
        var result = Player2.ProcessShot(coordinates);
        Player1.ProcessShotResult(coordinates, result);

        if (!Player2.HasLost)//If player 2 already lost, we can't let them take another turn.
        {
            coordinates = Player2.FireShot();
            result = Player1.ProcessShot(coordinates);
            Player2.ProcessShotResult(coordinates, result);
        }
    }

    public Player GetPlayer(string id) => Player1.Id == id ? Player1 : Player2;

    public Player GetPlayerByName(string name)
    {
        if (Player1.Name == name)
        {
            return Player1;
        }
        if (Player2.Name == name)
        {
            return Player2;
        }
        return null;
    }

    public void Fire(string playerId, int row = -1, int column = -1)
    {
        if (Player1.Id == playerId)
        {
            if (!Player1.IsMyTurn)
            {
                Console.WriteLine("Not Player1's turn");
                return;
            }
            var coordinates = new Coordinates { Row = row, Column = column };
            if(coordinates.Column < 0 || coordinates.Row < 0)
            {
                coordinates = Player1.FireShot();
            }
            var result = Player2.ProcessShot(coordinates);
            Player1.ProcessShotResult(coordinates, result);
        }
        else
        {
            if (!Player2.IsMyTurn)
            {
                Console.WriteLine("Not Player2's turn");
                return;
            }
            var coordinates = new Coordinates { Row = row, Column = column };
            if (coordinates.Column < 0 || coordinates.Row < 0)
            {
                coordinates = Player2.FireShot();
            }
            var result = Player1.ProcessShot(coordinates);
            Player2.ProcessShotResult(coordinates, result);
        }
        Player1.IsMyTurn = !Player1.IsMyTurn;
        Player2.IsMyTurn = !Player2.IsMyTurn;
        if (Player1.HasLost)
        {
            GameOver = true;
            Winner = Player2.Name;
        }
        else if (Player2.HasLost)
        {
            GameOver = true;
            Winner = Player1.Name;
        }
    }

}