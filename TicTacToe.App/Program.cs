using System.Runtime.InteropServices;
using TicTacToe.Core;

namespace TicTacToe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var naughts = new BitBoard();
            naughts.SetBit(Square.A1);
            naughts.SetBit(Square.B1);

            var crosses = new BitBoard();
            crosses.SetBit(Square.C2);

            var board = new Board(naughts, crosses, Player.Crosses, Player.Naughts);
            Console.WriteLine(board);

            var moves = board.GetMoves();
            foreach (var move in moves)
            {
                Console.WriteLine(move);
            }

            var board2 = board.MakeMove(Square.A2);
            var board3 = board2.MakeMove(Square.C1);
            Console.WriteLine(board3);
            Console.WriteLine(board3.HasWon(Player.Naughts));
            Console.WriteLine(board3.HasWon(Player.Crosses));
            Console.WriteLine(board3.IsTerminalState());

            naughts = new BitBoard();
            naughts.SetBit(Square.B0);
            naughts.SetBit(Square.C1);
            naughts.SetBit(Square.A2);
            naughts.SetBit(Square.C2);
            crosses = new BitBoard();
            crosses.SetBit(Square.A0);
            crosses.SetBit(Square.A1);
            crosses.SetBit(Square.B1);
            crosses.SetBit(Square.B2);
            crosses.SetBit(Square.C0);
            board = new Board(naughts, crosses, Player.Crosses, Player.Naughts);
            Console.WriteLine(board);
            Console.WriteLine(board.HasWon(Player.Naughts));
            Console.WriteLine(board.HasWon(Player.Crosses));
            Console.WriteLine(board.IsTerminalState());

        }
    }
}