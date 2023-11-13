////using System;
using System;

namespace BattleshipGame
{
    class Program
    {
        private static int boardSize;

        static void Main(string[] args)
        {
            const int boardSize = 10;
            char[,] playerBoard = new char[boardSize, boardSize];
            char[,] computerBoard = new char[boardSize, boardSize];

            // Инициализация игровых досок
            InitializeBoard(playerBoard);
            InitializeBoard(computerBoard);

            // Расстановка кораблей игрока
            Console.WriteLine("Расстановка ваших кораблей:");
            DeployShips(playerBoard);

            // Расстановка кораблей компьютера
            Console.WriteLine("Расстановка кораблей компьютера...");
            DeployShipsRandomly(computerBoard);

            Console.WriteLine("Начинаем игру!");

            bool gameOver = false;
            while (!gameOver)
            {
                // Ход игрока
                Console.WriteLine("Ваша доска:");
                PrintBoard(playerBoard);
                Console.WriteLine("Куда направляете атаку? (например, A1)");
                string playerMove = Console.ReadLine().ToUpper();
                bool isPlayerHit = ProcessMove(computerBoard, playerMove);
                PrintMoveResult(isPlayerHit);
                if (CheckGameOver(computerBoard))
                {
                    Console.WriteLine("Вы победили! Игра окончена.");
                    break;
                }

                // Ход компьютера
                Console.WriteLine("Ход компьютера:");
                Random random = new Random();
                int computerRow = random.Next(0, boardSize);
                int computerCol = random.Next(0, boardSize);
                string computerMove = ConvertToBoardCoordinates(computerRow, computerCol);
                bool isComputerHit = ProcessMove(playerBoard, computerMove);
                PrintMoveResult(isComputerHit);
                if (CheckGameOver(playerBoard))
                {
                    Console.WriteLine("Компьютер победил! Игра окончена.");
                    break;
                }
            }

            Console.WriteLine("Нажмите любую клавишу, чтобы закрыть программу...");
            Console.ReadKey();
        }

        // Инициализация игровой доски
        static void InitializeBoard(char[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    board[row, col] = '-';
                }
            }
        }

        // Вывод игровой доски на экран
        static void PrintBoard(char[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            Console.Write("  ");
            for (int col = 0; col < cols; col++)
            {
                Console.Write((char)('A' + col) + " ");
            }
            Console.WriteLine();

            for (int row = 0; row < rows; row++)
            {
                Console.Write(row + 1 + " ");
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(board[row, col] + " ");
                }
                Console.WriteLine();
            }
        }

        // Расстановка кораблей игрока
        static void DeployShips(char[,] board)
        {
            const int shipCount = 5;
            int placedShips = 0;

            while (placedShips < shipCount)
            {
                Console.WriteLine($"Расстановка корабля {placedShips + 1} из {shipCount}:");
                Console.WriteLine("Введите координаты начала корабля (например, A1):");
                string startCoordinate = Console.ReadLine().ToUpper();
                Console.WriteLine("Введите направление корабля (H - горизонтальное, V - вертикальное):");
                string direction = Console.ReadLine().ToUpper();

                bool isValidPlacement = PlaceShip(board, startCoordinate, direction[0]);
                if (isValidPlacement)
                {
                    placedShips++;
                }
                else
                {
                    Console.WriteLine("Ошибка! Корабль нельзя расположить здесь. Попробуйте снова.");
                }
            }
        }

        // Расстановка кораблей компьютера случайным образом
        static void DeployShipsRandomly(char[,] board)
        {
            const int shipCount = 5;
            int placedShips = 0;
            Random random = new Random();

            while (placedShips < shipCount)
            {
                int row = random.Next(0, boardSize);
                int col = random.Next(0, boardSize);
                char direction = random.Next(0, 2) == 0 ? 'H' : 'V';

                bool isValidPlacement = PlaceShip(board, ConvertToBoardCoordinates(row, col), direction);
                if (isValidPlacement)
                {
                    placedShips++;
                }
            }
        }

        // Размещение корабля на игровой доске
        static bool PlaceShip(char[,] board, string startCoordinate, char direction)
        {
            int row = int.Parse(startCoordinate.Substring(1)) - 1;
            int col = startCoordinate[0] - 'A';
            int shipLength = 5;

            if (direction == 'H')
            {
                if (col + shipLength > boardSize)
                {
                    return false;
                }
                for (int i = 0; i < shipLength; i++)
                {
                    if (board[row, col + i] != '-')
                    {
                        return false;
                    }
                }
                for (int i = 0; i < shipLength; i++)
                {
                    board[row, col + i] = 'S';
                }
            }
            else if (direction == 'V')
            {
                if (row + shipLength > boardSize)
                {
                    return false;
                }
                for (int i = 0; i < shipLength; i++)
                {
                    if (board[row + i, col] != '-')
                    {
                        return false;
                    }
                }
                for (int i = 0; i < shipLength; i++)
                {
                    board[row + i, col] = 'S';
                }
            }

            return true;
        }

        // Преобразование координат игровой доски
        static string ConvertToBoardCoordinates(int row, int col)
        {
            char colChar = (char)('A' + col);
            return colChar.ToString() + (row + 1).ToString();
        }

        // Обработка хода игрока или компьютера
        static bool ProcessMove(char[,] board, string move)
        {
            int col = move[0] - 'A';
            int row = int.Parse(move.Substring(1)) - 1;

            if (board[row, col] == 'S')
            {
                board[row, col] = 'X';
                return true;
            }
            else
            {
                board[row, col] = 'O';
                return false;
            }
        }

        // Проверка окончания игры
        static bool CheckGameOver(char[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (board[row, col] == 'S')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // Вывод результата хода
        static void PrintMoveResult(bool isHit)
        {
            if (isHit)
            {
                Console.WriteLine("Попадание!");
            }
            else
            {
                Console.WriteLine("Мимо!");
            }
        }
    }
}


//using System;

//namespace ConsoleBattleship
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            const int boardSize = 10;
//            char[,] playerBoard = new char[boardSize, boardSize];
//            char[,] computerBoard = new char[boardSize, boardSize];

//            InitializeBoards(playerBoard, computerBoard);

//            Console.WriteLine("Добро пожаловать в игру 'Морской бой'!");

//            while (true)
//            {
//                Console.WriteLine();
//                Console.WriteLine("Ваше поле:");
//                DisplayBoard(playerBoard, true);

//                Console.WriteLine();
//                Console.WriteLine("Поле компьютера:");
//                DisplayBoard(computerBoard, false);

//                Console.WriteLine();
//                Console.WriteLine("Ход игрока:");
//                PerformPlayerTurn(computerBoard);

//                Console.WriteLine();
//                Console.WriteLine("Ход компьютера:");
//                PerformComputerTurn(playerBoard);

//                if (CheckGameOver(playerBoard, computerBoard))
//                {
//                    Console.WriteLine();
//                    Console.WriteLine("Игра окончена!");
//                    break;
//                }
//            }
//        }

//        static void InitializeBoards(char[,] playerBoard, char[,] computerBoard)
//        {
//            const char empty = '~';
//            for (int i = 0; i < playerBoard.GetLength(0); i++)
//            {
//                for (int j = 0; j < playerBoard.GetLength(1); j++)
//                {
//                    playerBoard[i, j] = empty;
//                    computerBoard[i, j] = empty;
//                }
//            }
//        }

//        static void DisplayBoard(char[,] board, bool hideShips)
//        {
//            const char empty = '~';
//            const char ship = 'S';
//            const char miss = 'O';
//            const char hit = 'X';

//            Console.Write("  ");
//            for (int i = 0; i < board.GetLength(0); i++)
//            {
//                Console.Write(i + " ");
//            }
//            Console.WriteLine();

//            for (int i = 0; i < board.GetLength(0); i++)
//            {
//                Console.Write(i + " ");
//                for (int j = 0; j < board.GetLength(1); j++)
//                {
//                    char cell = board[i, j];
//                    if (hideShips && cell == ship)
//                    {
//                        cell = empty;
//                    }
//                    Console.Write(cell + " ");
//                }
//                Console.WriteLine();
//            }
//        }

//        static void PerformPlayerTurn(char[,] computerBoard)
//        {
//            while (true)
//            {
//                Console.Write("Введите координаты выстрела (например, 3 4): ");
//                string input = Console.ReadLine();
//                string[] coordinates = input.Split(' ');

//                if (coordinates.Length == 2 &&
//                    int.TryParse(coordinates[0], out int row) &&
//                    int.TryParse(coordinates[1], out int col) &&
//                    row >= 0 && row < computerBoard.GetLength(0) &&
//                    col >= 0 && col < computerBoard.GetLength(1))
//                {
//                    char cell = computerBoard[row, col];
//                    if (cell == 'S')
//                    {
//                        computerBoard[row, col] = 'X';
//                        Console.WriteLine("Вы попали!");

//                        if (CheckGameOver(null, computerBoard))
//                        {
//                            return;
//                        }
//                    }
//                    else if (cell == '~')
//                    {
//                        computerBoard[row, col] = 'O';
//                        Console.WriteLine("Промах!");

//                        return;
//                    }
//                    else
//                    {
//                        Console.WriteLine("Вы уже стреляли в эту клетку!");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine("Некорректные координаты! Попробуйте снова.");
//                }
//            }
//        }

//        static void PerformComputerTurn(char[,] playerBoard)
//        {
//            Random random = new Random();
//            while (true)
//            {
//                int row = random.Next(0, playerBoard.GetLength(0));
//                int col = random.Next(0, playerBoard.GetLength(1));

//                char cell = playerBoard[row, col];
//                if (cell == 'S')
//                {
//                    playerBoard[row, col] = 'X';
//                    Console.WriteLine($"Компьютер попал в клетку {row} {col}!");

//                    if (CheckGameOver(playerBoard, null))
//                    {
//                        return;
//                    }
//                }
//                else if (cell == '~')
//                {
//                    playerBoard[row, col] = 'O';
//                    Console.WriteLine($"Компьютер промахнулся в клетку {row} {col}!");

//                    return;
//                }
//            }
//        }

//        static bool CheckGameOver(char[,] playerBoard, char[,] computerBoard)
//        {
//            return (playerBoard != null && HasLost(playerBoard)) || (computerBoard != null && HasLost(computerBoard));
//        }

//        static bool HasLost(char[,] board)
//        {
//            const char ship = 'S';
//            for (int i = 0; i < board.GetLength(0); i++)
//            {
//                for (int j = 0; j < board.GetLength(1); j++)
//                {
//                    if (board[i, j] == ship)
//                    {
//                        return false;
//                    }
//                }
//            }
//            return true;
//        }
//    }
//}

////class Program
////{
////    static char[,] playerBoard = new char[10, 10];
////    static char[,] computerBoard = new char[10, 10];

////    static void Main(string[] args)
////    {
////        InitializeBoards();
////        PlaceShips();

////        while (true)
////        {
////            Console.Clear();
////            PrintBoards();

////            Console.WriteLine("Ваш ход:");
////            Console.Write("Введите координату x: ");
////            int x = int.Parse(Console.ReadLine());

////            Console.Write("Введите координату y: ");
////            int y = int.Parse(Console.ReadLine());

////            PlayerMove(x, y);

////            if (CheckGameOver())
////            {
////                Console.WriteLine("Вы победили!");
////                break;
////            }

////            ComputerMove();

////            if (CheckGameOver())
////            {
////                Console.WriteLine("Компьютер победил!");
////                break;
////            }
////        }

////        Console.ReadLine();
////    }

////    static void InitializeBoards()
////    {
////        for (int i = 0; i < 10; i++)
////        {
////            for (int j = 0; j < 10; j++)
////            {
////                playerBoard[i, j] = ' ';
////                computerBoard[i, j] = ' ';
////            }
////        }
////    }

////    static void PrintBoards()
////    {
////        Console.WriteLine("Ваше поле:");
////        PrintBoard(playerBoard);

////        Console.WriteLine("\nПоле компьютера:");
////        PrintBoard(computerBoard);
////    }

////    static void PrintBoard(char[,] board)
////    {
////        Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");
////        for (int i = 0; i < 10; i++)
////        {
////            Console.Write(i + " ");
////            for (int j = 0; j < 10; j++)
////            {
////                Console.Write(board[i, j] + " ");
////            }
////            Console.WriteLine();
////        }
////    }

////    static void PlaceShips()
////    {
////        Console.WriteLine("Расположите ваши корабли:");
////        Console.WriteLine("Вводите координаты так, чтобы между кораблями была минимум одна пустая клетка!!!");

////        for (int i = 1; i <= 4; i++)
////        {
////            Console.WriteLine($"Расположение корабля длиной {i}:");
////            for (int j = 0; j < i; j++)
////            {
////                Console.Write($"Введите координату x для корабля {j + 1}: ");
////                int x = int.Parse(Console.ReadLine());

////                Console.Write($"Введите координату y для корабля {j + 1}: ");
////                int y = int.Parse(Console.ReadLine());

////                playerBoard[x, y] = 'O';
////            }
////        }

////        Console.Clear();
////    }

////    static void PlayerMove(int x, int y)
////    {
////        if (computerBoard[x, y] == 'O')
////        {
////            Console.WriteLine("Вы попали!");
////            playerBoard[x, y] = 'X';
////        }
////        else
////        {
////            Console.WriteLine("Вы промахнулись!");
////            playerBoard[x, y] = '*';
////        }
////    }

////    static void ComputerMove()
////    {
////        Random random = new Random();
////        int x, y;

////        do
////        {
////            x = random.Next(10);
////            y = random.Next(10);
////        } while (playerBoard[x, y] == 'X' || playerBoard[x, y] == '*');

////        if (playerBoard[x, y] == 'O')
////        {
////            Console.WriteLine("Компьютер попал!");
////            playerBoard[x, y] = 'X';
////        }
////        else
////        {
////            Console.WriteLine("Компьютер промахнулся!");
////            playerBoard[x, y] = '*';
////        }
////    }

////    static bool CheckGameOver()
////    {
////        for (int i = 0; i < 10; i++)
////        {
////            for (int j = 0; j < 10; j++)
////            {
////                if (playerBoard[i, j] == 'O')
////                    return false;
////                if (computerBoard[i, j] == 'O')
////                    return false;
////            }
////        }

////        return true;
////    }
////}