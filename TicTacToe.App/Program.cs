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
           


        }
    }
}