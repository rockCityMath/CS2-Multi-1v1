namespace CS2Multi1v1.Models;

// The various types of results an arena can return based on the round

public enum ArenaResultType
{
    Win,         // Both players were present in the arena and a winner was determined
    NoOpponent,  // Only one player was present in the arena and they are the "winner"
    Empty        // No players were in the arena
}
