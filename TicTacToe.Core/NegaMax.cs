﻿
namespace TicTacToe.Core
{

    public record SearchResult(Square? square, int score);

    public class NegaMax
    {
        public static int MinSearch = -100000000;
        public static int MaxSearch = 100000000;
        public NegaMax() { }

        
        public static SearchResult search(int depth, int alpha, int beta, Board boardState)
        {
            // Evaluate if at terminal states (here terminal means end of search depth).
            if (boardState.HasWon(boardState.Player1))
                return new SearchResult(null, 1000 * Math.Max(1, depth));

            if (boardState.HasWon(boardState.Player2))
                return new SearchResult(null, -1000 * Math.Max(1, depth));

            if (depth == 0) return new SearchResult(null, 0);
            

            SearchResult bestSoFar = new SearchResult(null, MinSearch);
            var moves = boardState.GetMoves();
            
            // terminal state, draw game, nobody won and no more moves, so must be draw.
            if (moves.Count == 0) return new SearchResult(null, 0); 

            foreach(var move in moves)
            {
                var newBoard = boardState.MakeMove(move);
                var searchResult = NegaMax.search(depth-1, -beta, -alpha, newBoard);
                var score = -searchResult.score;

                if (score > bestSoFar.score)
                {
                    bestSoFar = new SearchResult(move,score);
                }

                // Prune the search tree.
                alpha = Math.Max(alpha, score);
                if (alpha > beta)
                    break;
            }

            return bestSoFar;
        }

    }
}
