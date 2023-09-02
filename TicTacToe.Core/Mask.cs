using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Core
{
    public enum Mask
    {
        RANK_0 = 7,
        RANK_1 = 56,
        RANK_2 = 448,
        A_FILE = 73,
        B_FILE = 146,
        C_FILE = 292,
        LR_DIAGONAL = 273,
        RL_DIAGONAL = 84
    }
}
