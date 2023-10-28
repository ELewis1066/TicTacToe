using TicTacToe.Core;

namespace TicTacToe
{
    public class Program
    {
        // If the player to move 'Player1' can move, check if Player2 has 
        // already won, or if the game is drawn.
        static bool IsWinOrDraw(Board board)
        {
            if (board.HasWon(board.Player2))
            {
                Console.WriteLine($"Player {board.Player2} has won!");
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
            GameLoop();
        }

        static void GameLoop(int maxDepth = 9)
        {
            Console.WriteLine("Tic Tac Toe");
            Console.WriteLine("Enter 'exit' to quit.");
            Console.WriteLine("Enter moves; rank, file format, e.g. 0,2 for C0.");
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
                   
                    if (IsWinOrDraw(board))
                    {
                        Console.WriteLine(board);
                        board = new Board();
                        continue;
                    }
                    Console.WriteLine("Thinking...");
                    SearchResult result = NegaMax.search(maxDepth, NegaMax.MinSearch, NegaMax.MaxSearch, board);

                    if (result.square != null)
                    {
                        board = board.MakeMove(result.square.Value);
                    }
                    else
                    {
                        // Could throw, this should never happen as we only call Algorithm to
                        // find move, if the board isn't in a terminal state.
                        throw new Exception("Could not find move, board in terminal state.");
                    }
         
                    if (IsWinOrDraw(board))
                    {
                        Console.WriteLine(board);
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