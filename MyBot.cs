using ChessChallenge.API;
using System;
using System.Collections.Generic;
using System.Linq;

public class MyBot : IChessBot
{

    // HyperAgressor
    // move prioritization:
    // checkmate > attacking king / around king > taking pices > moving closer to king > moving closer to enemy piecies
    // optional goal(s): add defense of own king to priority list

    // Piece values: null, pawn, knight, bishop, rook, queen, king
    float[] captureValues = { 0, 1, 3, 3, 5, 9, 100 };

    bool myColor;

    public Move Think(Board board, Timer timer)
    {
        myColor = board.IsWhiteToMove;

        Move[] allMoves = board.GetLegalMoves();

        Dictionary<int, List<Move>> scoredMoves = new Dictionary<int, List<Move>>();

        foreach (Move move in allMoves)
        {
            // Always play checkmate in one
            if (IsMoveCheckmate(board, move))
                return move;

            // bucket sort moves
            float moveScore = ScoreMove(board, move);
            if (scoredMoves.ContainsKey((int)moveScore))
                scoredMoves[(int)moveScore].Add(move);
            else
                scoredMoves.Add((int)moveScore, new List<Move>() { move });
        }

        Random rng = new Random();
        int maxKey = scoredMoves.Max(x => x.Key);

        foreach (var key in scoredMoves.Keys)
        {
            Console.Write(key + " ");
        }
        Console.WriteLine();
        return scoredMoves[maxKey][rng.Next(scoredMoves[maxKey].Count)];
    }

    float ScoreMove(Board board, Move move)
    {
        float score = 0;

        switch (move.MovePieceType)
        {
            case PieceType.Pawn:
                if (myColor)
                    score = move.TargetSquare.Rank / 7.0f;
                else
                    score = 1.0f - (move.TargetSquare.Rank / 7.0f);
                score += 1;
                break;
            case PieceType.Knight:
                score = DistanceToKing(board, move, myColor) / 10;
                break;
            case PieceType.Bishop:
                score = DistanceToKing(board, move, myColor) / 10;
                break;
            case PieceType.Rook:
                score = DistanceToKing(board, move, myColor) / 10;
                break;
            case PieceType.Queen:
                score = DistanceToKing(board, move, myColor) / 10;
                break;
            case PieceType.King:
                score = DistanceToKing(board, move, myColor) / 10;
                break;
        }

        if (move.IsCapture)
            score = ScoreCapture(board, move);
        if (IsMovingIntoAttack(board, move))
        {
            score -= 1;
            if (move.CapturePieceType == PieceType.King)
                score -= 10;
        }
        if (IsGettingAttacked(board, move))
            score += 1;
        if (move.IsEnPassant)
            score *= 1;
        if (move.IsPromotion)
            score += 3;
        if (DistanceToKing(board, move, myColor) < 1.5)
            score -= 5;
        return score;
    }

    float ScoreCapture(Board board, Move move)
    {
        float targetValue = captureValues[(int)move.CapturePieceType];
        float selfValue = captureValues[(int)move.MovePieceType];
        if (IsMovingIntoAttack(board, move))
            return targetValue * (targetValue / selfValue);
        return targetValue;
    }

    float DistanceToKing(Board board, Move move, bool color)
    {
        return Distance(move.TargetSquare, board.GetKingSquare(!color));
    }

    float Distance(Square from, Square to)
    {
        return MathF.Sqrt(MathF.Pow(to.File - from.File, 2) + MathF.Pow(to.Rank - from.Rank, 2));
    }

    bool IsMovingIntoAttack(Board board, Move move)
    {
        return board.SquareIsAttackedByOpponent(move.TargetSquare);
    }

    bool IsGettingAttacked(Board board, Move move)
    {
        return board.SquareIsAttackedByOpponent(move.StartSquare);
    }

    bool IsMoveCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    bool IsMoveCheck(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheck();
        board.UndoMove(move);
        return isMate;
    }
}