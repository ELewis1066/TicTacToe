using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public class Board
    {
        public static UInt16 FULL_BOARD = 511;

        BitBoard _naughts;
        BitBoard _crosses;
        Player _player1;
        Player _player2;

        public Board(BitBoard naughts, BitBoard crosses, Player player1, Player player2)
        {
            _naughts = naughts;
            _crosses = crosses;
            _player1 = player1;
            _player2 = player2;
        }

        public Board() : this(new BitBoard(), new BitBoard(), Player.Naughts, Player.Crosses)
        {

        }

        public Board MakeMove(Square s)
        {
            if (_player1 == Player.Naughts)
            {
                var update = _naughts;
                update.SetBit(s);
                return new Board(update, _crosses, Player.Crosses, Player.Naughts);
            }
            else
            {
                var update = _crosses;
                update.SetBit(s);
                return new Board(_naughts, update, Player.Naughts, Player.Crosses);
            }
        }

        public bool IsFull()
        {
            if ((_naughts.GetValue() | _crosses.GetValue()) == FULL_BOARD)
                return true;

            return false;
        }
        public bool IsTerminalState()
        {
            if ( HasWon(_player1) || HasWon(_player2) ) return true;
            if ( IsFull() ) return true;
            
            return false;
        }

        public bool HasWon(Player player)
        {
            uint side = player == Player.Naughts ? _naughts.GetValue() : _crosses.GetValue();
            var masks = Enum.GetValues(typeof(Mask));
  
            foreach (Mask mask in masks)
            {
                UInt16 maskValue = Convert.ToUInt16(mask);
                if ((side & maskValue) == maskValue)
                {
                    return true;
                }
            }
            
            return false;
        }

        public List<Square> GetMoves()
        {
            var moves = new List<Square>();
            var freeSquares = new BitBoard(~(_naughts.GetValue() | _crosses.GetValue()));
            while (!freeSquares.isEmpty())
            {
                var square = freeSquares.IndexLSB();
                moves.Add(square);
                freeSquares.PopBit(square);
            }
            return moves;
        }

        public override string ToString()
        {
            string repr = "";
            for (var rank = 0; rank < 3; rank++)
            {
                for (var file = 0; file < 3; file++)
                {
                    var square = (Square)(rank * 3 + file);

                    if (_naughts.GetBit(square) != 0)
                        repr += " O ";
                    else if (_crosses.GetBit(square) != 0)
                        repr += " X ";
                    else
                        repr += " . ";
                }
                repr += "\n";
            }
            repr += $"player to move = {_player1}\n";
            return repr;
        }


    }
}
