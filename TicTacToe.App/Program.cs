using TicTacToe.Core;

namespace TicTacToe
{
    public class Program
    {
        // Player 1 is the player making a move,
        // When we do MakeMove, Player1 becomes Player2.
        // Before we search for a next move, we check to
        // see if the previous move (made by now Player2)
        // actually won the game.
        static bool WasLastMoveWinning(Board board)
        {
            if (board.HasWon(board.Player2))
            {
                Console.WriteLine($"{board.Player2} has won!");
                Console.WriteLine("Winning game state:");
                return true;
            }
            else if (board.IsFull())
            {
                Console.WriteLine("Game drawn.");
                return true;
            }

            return false;
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Tic Tac Toe");
            Console.WriteLine("Press 1. for NegaMax.");
            Console.WriteLine("Press any other key, for Monte Carlo Search Tree.");
            char input = Console.ReadKey().KeyChar;
            if (input == '1')
            {
                Console.WriteLine("\nEnter the max depth of NegaMax search (9 for strong play):");
                uint maxDepth = Convert.ToUInt32(Console.ReadLine());
                GameLoop("negamax", maxDepth);
            }
            else
            {
                Console.WriteLine("\nEnter max iterations for Monte Carlo tree search (e.g. 1500 for strong play):");
                uint numIterations = Convert.ToUInt32(Console.ReadLine());
                // we could tidy this up, maxDepth is unused for mcts.
                GameLoop("mcts",9,numIterations);
            }
        }

        static void GameLoop(string algorithm= "negamax", uint maxDepth = 9, uint numIterations = 1000)
        {
            // maxDepth used by negamax, numIterations used by MCTS.
            Console.WriteLine($"\nAlgorithm: {algorithm}");
            Console.WriteLine("Enter 'exit' to quit.");
            Console.WriteLine("Enter moves; rank, file format, e.g. 0,2 for C0, i.e:");
            Console.WriteLine("0,0      0,1     0,2");
            Console.WriteLine("1,0      1,1     1,2");
            Console.WriteLine("2,0      2,1     2,2");
            Board board = new Board();
            while (true)
            {
                Console.WriteLine(board);
                Console.Write(">");
                string userInput = Console.ReadLine();
                
                if (string.IsNullOrEmpty(userInput))
                    continue;

                if (userInput == "exit")
                    break;
               
                try {
                    var input = userInput.Replace(" ",string.Empty).Split(',').Select(x=>Convert.ToInt32(x)).ToArray();
                    var rank = input[0];
                    var file = input[1];
                    var square = (Square)(rank * 3 + file);

                    if (! board.IsSquareEmpty(square))
                    {
                        Console.WriteLine("Illegal move, square occupied.");
                        continue;
                    }

                    board = board.MakeMove(square);
                    Console.WriteLine(board);
                   
                    if (WasLastMoveWinning(board))
                    {
                        Console.WriteLine(board);
                        board = new Board();
                        continue;
                    }

                    if (algorithm == "negamax")
                    {
                        SearchResult result = NegaMax.search((int) maxDepth, NegaMax.MinSearch, NegaMax.MaxSearch, board);

                        if (result.square != null)
                        {
                            board = board.MakeMove(result.square.Value);
                        }
                        else
                        {
                            // Could throw, this should never happen as we only call Algorithm to
                            // find move, if the board isn't in a terminal state.
                            throw new Exception("NegaMax, could not find move, board in terminal state.");
                        }
                    }
                    else
                    {
                        // mcts
                        board = Mcts.Search(board, numIterations);
                    }
         
                    if (WasLastMoveWinning(board))
                    {
                        Console.WriteLine(board);
                        Console.WriteLine("\nNew Game, type exit to quit.");
                        board = new Board();
                        continue;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            

        }
    }
}