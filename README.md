## TicTacToe

### C# Tic-Tac-Toe Negamax &amp; Monte Carlo Tree Search

This is a C# implementation of the Tic Tac Toe Game.
Two options are provided for computer play:

- NegaMax
- Monte Carlo Tree Search

### Design

Two BitBoards (unsigned integers) are used to represent the TicTacToe board.

We have a representation of the board:




|A0 |B0 |C0
|A1 |B1 |C1
|A2 |B2 |C2



Squares are defined by an enum:

    public enum Square
    {
        A0 = 0,
        B0 = 1,
        C0 = 2,
        A1 = 3,
        B1 = 4,
        C1 = 5,
        A2 = 6,
        B2 = 7,
        C2 = 8
    }

So we can represent all naughts on a BitBoard by setting the appropriate bit:

C2, B2, A2, C1, B1, A1, C0, B0, A0  

So, for example the unsigned int:  000 000 111 = 7 is equivalent to C0 B0 A0 all set.

So 7 is the mask for the first rank.





To determine if we have an empty square (zero bit in the location for both bit boards) and other operations

are done in the `BitBoard` class. 

Example, to set a bit:

```cs
    public void SetBit(Square square)
    {
        _value |= uint.RotateLeft(1, Convert.ToUInt16(square));
    }
```

To get a bit:

```cs
    public uint GetBit(Square square)
    {
        return _value & uint.RotateLeft(1, Convert.ToUInt16(square));
    }
```



The `Board` class uses two bit boards to represent the full state.

To test if a player has won, we check against all the masks;

```cs
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
```



The computer play is a choice between negamax with alpha beta pruning (implemented by `NegaMax` class)

or Monte Carlo Tree Search (implemented by `Mcts` class).



Negamax:

```cs
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
```

Monte Carlo Tree Search:

        public static Board Search(Board initial_state, uint numIterations)
        {
            var root = new Node(initial_state, null);
    
            for (uint i = 0; i < numIterations; i++)
            {
                var node = Select(root);
                var score = Rollout(node.Board);
                Backpropagate(node, score);
            }
    
            return GetBestMove(root, 0.0).Board;
        }
    
        public static Node Select(Node node)
        {
            while (!node.IsTerminal)
            {
                if (node.IsFullyExpanded)
                {
                    node = GetBestMove(node, 2.0);
                }
                else
                    return Expand(node);
            }
    
            return node;
        }
    
        public static Node Expand(Node node)
        {
            var states = node.Board.GenerateStates();
            var xs = node.Children.Select(child => child.Board).ToList();
    
            foreach (var state in states)
            {
                if (!xs.Contains(state))
                {
                    Node newNode = new Node(state, node);
                    node.Children.Add(newNode);
                    if (states.Count == node.Children.Count)
                        node.IsFullyExpanded = true;
                    return newNode;
                }
            }
    
            throw new Exception("Runtime error, mcts 'Expand' failed.");
        }
    
    
        public static double Rollout(Board board)
        {
            while (!(board.HasWon(Player.Naughts) || board.HasWon(Player.Crosses)))
            {
                var possibleStates = board.GenerateStates();
                if (possibleStates.Count == 0)
                    break;
                board = possibleStates[random.Next(possibleStates.Count)];
            }
    
    
            if (board.HasWon(Player.Crosses))
                return -1.0;
    
            return board.HasWon(Player.Naughts) ? 1.0 : 0.0;
        }
    
        public static void Backpropagate(Node? node, double score)
        {
            while (node != null)
            {
                node.Visits += 1.0;
                node.Score += score;
                node = node.Parent;
            }
        }
    
    
        public static Node GetBestMove(Node node, double exploration)
        {
            var bestSoFar = -1000000.0;
            var bestMoves = new List<Node>();
    
            foreach (var child in node.Children)
            {
                double sign = child.Board.Player2 == Player.Naughts ? 1 : -1;
                var score = sign * child.Score / child.Visits +
                            exploration * Math.Sqrt(Math.Log(node.Visits / child.Visits));
    
                if (score > bestSoFar)
                {
                    bestSoFar = score;
                    bestMoves = new List<Node> { child };
                }
                else if (IsEqual(score, bestSoFar))
                {
                    bestMoves.Add(child);
                }
            }
    
            return bestMoves[random.Next(bestMoves.Count)];
    
        }
