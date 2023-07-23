using ChessChallenge.API;

public class MyBot : IChessBot
{

    // HyperAgressor
    // move prioritization:
    // checkmate > attacking king / around king > taking pices > moving closer to king > moving closer to enemy piecies
    // optional goal(s): add defense of own king to priority list, check if the move 

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        return moves[0];
    }
}