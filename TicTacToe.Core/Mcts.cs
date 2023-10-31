

namespace TicTacToe.Core
{

    public class Node
    {
        public Board Board { get; set; }
        public Node? Parent { get; set; }
        public double Visits { get; set; }
        public double Score { get; set; }
        public List<Node> Children { get; set; }
        public bool IsTerminal { get; set; }
        public bool IsFullyExpanded { get; set; }

        public Node(Board board, Node? parent)
        {
            Board = board;
            Parent = parent;
            Visits = 0.0;
            Score = 0.0;
            Children = new List<Node>();
            IsTerminal = board.IsTerminalState();
            IsFullyExpanded = board.IsTerminalState();
        }

    }

    public class Mcts
    {
        public static Random random = new Random();
        public static double Epsilon { get; } = Math.Pow(10, -5);

        public static bool IsEqual(double a, double b)
        {
            if (Math.Abs(a) <= double.Epsilon || Math.Abs(b) <= double.Epsilon)
                return Math.Abs(a - b) <= double.Epsilon;

            return Math.Abs(1.0 - a / b) <= Epsilon;
        }

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

    }
}

