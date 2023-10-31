using System.Numerics;


namespace TicTacToe.Core
{
    public class BitBoard 
    {
        private uint _value;

        public BitBoard(uint value)
        {
            SetValue(value);
        }
        public BitBoard() : this(0)
        {
        }

        private void SetValue(uint value)
        {
            // 11111111111111111111111 000 000 000 = 4294966784
            // this is the same as zero for our purposes, where
            // we are using the first few bits.
            _value = value == 4294966784 ? (uint) 0 : value;
        }
        public uint GetValue()
        {
            // see: { get; } or  {get; set;} for shorthand version of this.
            return _value;
        }

        public void SetBit(Square square)
        {
            _value |= uint.RotateLeft(1, Convert.ToUInt16(square));
        }

        public uint GetBit(Square square)
        {
            return _value & uint.RotateLeft(1, Convert.ToUInt16(square));
        }

        public void PopBit(Square square)
        {
            var popped = _value  & ~uint.RotateLeft(1, Convert.ToUInt16(square));
            SetValue(popped);
        }

       public Square IndexLSB()
        {
            var tmp = (_value & (-_value)) - 1;
            return (Square) BitOperations.PopCount( (uint) tmp);
        }


        public bool IsEmpty()
        {
            return _value == 0;
        }

        public override string ToString()
        {
            string repr = "";
            for (var rank=0; rank<3; rank++)
            {
                for (var file=0; file<3; file++)
                {
                    var square = (Square) (rank * 3 + file);
                    if (this.GetBit(square) != 0)
                        repr += " 1 ";
                    else
                        repr += " 0 ";
                }
                repr += "\n";
            }
            repr += $"value = {_value}\n";

            return repr;
        }


    }
}
