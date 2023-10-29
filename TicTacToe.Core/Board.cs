
namespace TicTacToe.Core
{
    public class Board
    {
        public static UInt16 FULL_BOARD = 511;

        BitBoard _naughts;
        BitBoard _crosses;
        public Player Player1 { get; }
        public Player Player2 { get; }

        public Board(BitBoard naughts, BitBoard crosses, Player player1, Player player2)
        {
            _naughts = naughts;
            _crosses = crosses;
            Player1 = player1;
            Player2 = player2;
        }

        public Board() : this(new BitBoard(), new BitBoard(), Player.Naughts, Player.Crosses)
        {

        }

        public bool IsSquareEmpty(Square square)
        {
            if ( _naughts.GetBit(square) == 0 && _crosses.GetBit(square) ==0)
                return true;
            return false;
        }

        public Board MakeMove(Square s)
        {
            if (Player1 == Player.Naughts)
            {
                var update = new BitBoard(_naughts.GetValue());
                update.SetBit(s);
                return new Board(update, _crosses, Player.Crosses, Player.Naughts);
            }
            else
            {
                var update = new BitBoard(_crosses.GetValue());
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
            if ( HasWon(Player1) || HasWon(Player2) ) return true;
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

            while (!freeSquares.IsEmpty())
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
            repr += $"player to move = {Player1}\n";
#if DEBUG
            repr += $"(debug) naughts = {_naughts.GetValue()}\n";
            repr += $"(debug) crosses = {_crosses.GetValue()}\n";
#endif
            return repr;
        }


    }
}
