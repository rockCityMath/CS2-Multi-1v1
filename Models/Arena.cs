using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using Microsoft.Extensions.Logging;

namespace CS2Multi1v1.Models;

// A single 1v1 arena, holds 2 players, tracks their round stats, and handles much of the functionality relating to playing the round

internal class Arena
{
    private Tuple<SpawnPoint, SpawnPoint> _spawns;
    private int _rank;
    private RoundType _roundType;
    private ILogger<CS2Multi1v1> _logger;

    public ArenaPlayer? _player1;
    private int _player1Kills;
    private bool _player1HasLastKill;

    public ArenaPlayer? _player2; // these should be private, use util methods for interation
    private int _player2Kills;

    public Arena(ILogger<CS2Multi1v1> logger, Tuple<SpawnPoint, SpawnPoint> spawns)
    {
        _spawns = spawns;
        _rank = 0;
        _logger = logger;
        _player1Kills = 0;
        _player1HasLastKill = true;
        _player2Kills = 0;
        _roundType = RoundType.Rifle;
    }

    public void AddPlayers(ArenaPlayer? player1, ArenaPlayer? player2, int rank = -1)
    {
        _rank = rank;
        _player1 = player1;
        _player1Kills = 0;
        _player1HasLastKill = true;
        _player2 = player2;
        _player2Kills = 0;

        // TODO: Check player prefs for acceptable roundtypes?
        List<RoundType> roundTypes = new List<RoundType>(){
            RoundType.Rifle,
            RoundType.Smg,
            RoundType.Scout,
            RoundType.Awp,
            RoundType.Pistol,
            RoundType.Deagle
        };

        Random rng = new Random();
        int roundTypeIndex = rng.Next(0, 5);

        _roundType = roundTypes[roundTypeIndex];

        // Set player teams, notify of arena and opponent
        if (isP1Valid())
        {
            _logger.LogInformation("Switch p1 team..");
            _player1?.PlayerController.SwitchTeam(CsTeam.Terrorist);

            string opponentName = isP2Valid() ? _player2!.PlayerController.PlayerName : "No Opponent";

            _player1?.PrintToChat($"Arena:      {ChatColors.Gold}{_rank}");
            _player1?.PrintToChat($"Round Type: {ChatColors.Gold}{_roundType.Name}");
            _player1?.PrintToChat($"Opponent:   {ChatColors.Gold}{opponentName}");

            _player1!.PlayerController.Clan = $"ARENA {_rank}";
        }

        if (isP2Valid())
        {
            _logger.LogInformation("Switch p2 team..");
            _player2?.PlayerController.SwitchTeam(CsTeam.CounterTerrorist);

            string opponentName = isP1Valid() ? _player1!.PlayerController.PlayerName : "No Opponent";

            _player2?.PrintToChat($"Arena:      {ChatColors.Gold}{_rank}");
            _player2?.PrintToChat($"Round Type: {ChatColors.Gold}{_roundType.Name}");
            _player2?.PrintToChat($"Opponent:   {ChatColors.Gold}{opponentName}");

            _player2!.PlayerController.Clan = $"ARENA {_rank}";
        }

        LogCurrentInfo();
    }

    public void OnPlayerSpawn(CCSPlayerController playerController)
    {
        bool wasPlayer1 = isP1Valid() && _player1!.PlayerController == playerController;
        bool wasPlayer2 = isP2Valid() && _player2!.PlayerController == playerController;

        // If a player in this arena respawned
        if (wasPlayer1 || wasPlayer2)
        {
            // Randomly assign which player spawns at which of the 2 arena spawns
            SpawnPoint p1Spawn;
            SpawnPoint p2Spawn;
            Random rng = new Random();
            int p1SpawnNum = rng.Next(0, 2);

            if (p1SpawnNum == 1)
            {
                p1Spawn = _spawns.Item1;
                p2Spawn = _spawns.Item2;
            }
            else
            {
                p1Spawn = _spawns.Item2;
                p2Spawn = _spawns.Item1;
            }

            if (isP1Valid())
            {
                // Get spawnpoint from the arena
                Vector? pos = p1Spawn.AbsOrigin;
                QAngle? angle = p1Spawn.AbsRotation;
                Vector? velocity = new Vector(0, 0, 0);

                // Teleport player there
                if (pos != null && angle != null)
                {
                    _player1?.PlayerController?.Pawn.Value?.Teleport(pos, angle, velocity);
                }

                // Reset weapons and health
                _player1!.ResetPlayerWeapons(_roundType);
                _player1!.PlayerController!.Pawn!.Value!.Health = 100;
            }

            if (isP2Valid())
            {
                // Get spawnpoint from the arena
                Vector? pos = p2Spawn.AbsOrigin;
                QAngle? angle = p2Spawn.AbsRotation;
                Vector? velocity = new Vector(0, 0, 0);

                // Teleport player there
                if (pos != null && angle != null)
                {
                    _player2!.PlayerController!.Pawn.Value?.Teleport(pos, angle, velocity);
                }
                _player2!.ResetPlayerWeapons(_roundType);
                _player2!.PlayerController!.Pawn!.Value!.Health = 100;
            }
        }
    }

    // Show both players opponent's and their own kills
    private void showPlayersCurrentScore()
    {
        if (isP1Valid() && isP2Valid())
        {
            _player1?.PrintToChat($"You: {ChatColors.Green}{_player1Kills}{ChatColors.Default} | {_player2?.PlayerController.PlayerName}: {ChatColors.LightRed}{_player2Kills}");
            _player2?.PrintToChat($"You: {ChatColors.Green}{_player2Kills}{ChatColors.Default} | {_player1?.PlayerController.PlayerName}: {ChatColors.LightRed}{_player1Kills}");
        }
    }

    public void OnPlayerDeath(CCSPlayerController playerController)
    {
        bool wasPlayer1 = isP1Valid() && _player1!.PlayerController == playerController;
        bool wasPlayer2 = isP2Valid() && _player2!.PlayerController == playerController;

        if (wasPlayer2)
        {
            _player1Kills += 1;
            _player1HasLastKill = true;
            showPlayersCurrentScore();
        }

        if (wasPlayer1)
        {
            _player2Kills += 1;
            _player1HasLastKill = false;
            showPlayersCurrentScore();
        }
    }

    public void LogCurrentInfo()
    {
        if (isP1Valid() || isP2Valid())
        {
            _logger.LogInformation($"------ ARENA {_rank} -----");
            if (isP1Valid()) _logger.LogInformation($"Player1: {_player1.PlayerController.PlayerName}");
            if (isP2Valid()) _logger.LogInformation($"Player2: {_player2.PlayerController.PlayerName}");
            _logger.LogInformation($"Round Type: {_roundType.Name}");
        }
    }

    public void OnRoundEnd()
    {
        // Notify player of win/loss
        if (isP1Valid() && isP2Valid())
        {
            if (_player1Kills > _player2Kills)
            {
                _player1!.PrintToChat($"{ChatColors.Green}You won!");
                _player2!.PrintToChat($"{ChatColors.Red}You lost!");
            }
            else if (_player2Kills > _player1Kills)
            {
                _player2!.PrintToChat($"{ChatColors.Green}You won!");
                _player1!.PrintToChat($"{ChatColors.Red}You lost!");
            }
            else if (_player1HasLastKill)
            {
                _player1!.PrintToChat($"{ChatColors.Green}You won!");
                _player2!.PrintToChat($"{ChatColors.Red}You lost!");
            }
            else
            {
                _player2!.PrintToChat($"{ChatColors.Green}You won!");
                _player1!.PrintToChat($"{ChatColors.Red}You lost!");
            }
        }
    }

    public ArenaResult GetArenaResult()
    {
        // If both players valid, use normal logic to determine winner
        if (isP1Valid() && isP2Valid())
        {
            if (_player1Kills > _player2Kills)
            {
                return new ArenaResult(ArenaResultType.Win, _player1, _player2);
            }
            else if (_player2Kills > _player1Kills)
            {
                return new ArenaResult(ArenaResultType.Win, _player2, _player1);
            }
            else if (_player1HasLastKill)
            {
                return new ArenaResult(ArenaResultType.Win, _player1, _player2);
            }
            else
            {
                return new ArenaResult(ArenaResultType.Win, _player2, _player1);
            }
        }

        // If player1 was valid, give them the win
        if (isP1Valid())
        {
            return new ArenaResult(ArenaResultType.NoOpponent, _player1, null);
        }

        // If player2 was valid, give them the win
        if (isP2Valid())
        {
            return new ArenaResult(ArenaResultType.NoOpponent, _player2, null);
        }

        // If this point is reached, the arena either started with or now has no players
        return new ArenaResult(ArenaResultType.Empty, null, null);
    }

    private bool isP1Valid()
    {
        return _player1 != null && _player1.PlayerController.IsValid && _player1.PlayerController.Connected == PlayerConnectedState.PlayerConnected && _player1.PlayerController.Pawn.Value.IsValid;
    }

    private bool isP2Valid()
    {
        return _player2 != null && _player2.PlayerController.IsValid && _player2.PlayerController.Connected == PlayerConnectedState.PlayerConnected && _player2.PlayerController.Pawn.Value.IsValid;
    }
}
