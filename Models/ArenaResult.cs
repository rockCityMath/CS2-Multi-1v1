namespace CS2Multi1v1.Models;

// The result of a single round in a single arena

public struct ArenaResult
{
    public ArenaResultType ResultType;
    public ArenaPlayer? Winner;
    public ArenaPlayer? Loser;

    public ArenaResult(ArenaResultType resultType, ArenaPlayer? winner, ArenaPlayer? loser)
    {
        ResultType = resultType;
        Winner = winner;
        Loser = loser;
    }
}
