using CrossplatformLab3;
using System.Text;

class Program
{
    const string OutputPath = @"..\..\..\output.txt";
    static string[] tasklines;

    public static void Main(string[] args)
    {
        Console.WriteLine("Write input data path:");
        string path = Console.ReadLine();
        try
        {
            tasklines = File.ReadAllLines(path);
            CheckInput();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        char[,] chessboard = CreateChessBoard();
        Cell[,] field = CreateField(chessboard, out Cell start, out Cell finish);

        Solve(chessboard, field, start, finish);      
    }

    static char[,] CreateChessBoard()
    {
        char[,] chessboard = new char[int.Parse(tasklines[0]), int.Parse(tasklines[0])];
        for (int i = 1; i < tasklines.Length; i++)
        {
            string s = tasklines[i];
            for (int j = 0; j < tasklines[i].Length; j++)
            {
                chessboard[i - 1, j] = s[j];
            }
        }
        return chessboard;
    }

    static void CheckInput()
    {
        if(tasklines.Count() <= 1)
            throw new Exception("Incorrect data.");
        if (int.TryParse(tasklines[0], out int res) && res != tasklines[1].Length)
        {
            throw new Exception("Incorrect N.");
            return;
        }
        for (int i = 1; i < tasklines.Length; i++)
        {
            string curstr = tasklines[i];
            foreach (var symbol in curstr)
            {
                if (symbol != '#' && symbol != '@' && symbol != '.')
                    throw new Exception($"Wrong symbol: {symbol} in line {i}.");
            }
        }
    }

    static Cell[,] CreateField(char[,] chessfield, out Cell start, out Cell finish)
    {
        Cell[,] field = new Cell[chessfield.GetLength(0), chessfield.GetLength(1)];
        start = null;
        finish = null;
        int counter = 0;
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                if (chessfield[i, j] == '#')
                    field[i, j] = new Cell(i, j, null, true);
                else if (chessfield[i, j] == '@')
                {
                    if (counter == 0)
                    {
                        field[i, j] = new Cell(i, j, null, false);
                        start = field[i, j];
                        start.Previous = start;
                        counter++;
                    }
                    else if (counter > 0)
                    {
                        field[i, j] = new Cell(i, j, null, false);
                        finish = field[i, j];
                    }
                }
                else
                {
                    field[i, j] = new Cell(i, j, null, false);
                }
            }
        }
        if (start == null || finish == null)
            throw new Exception("There is no start or finish point on the board.");
        return field;
    }

    static void Solve(char[,] chessboard, Cell[,] field, Cell start, Cell finish)
    {
        if (chessboard.GetLength(0) == 2) 
        {
            WriteResult("Immposible");
            return;
        }
        List<Cell> explored = new List<Cell>();
        List<Cell> reachable = new List<Cell>();
        int numberofblockedcells = 0;
        foreach (var cell in field)
        {
            if (cell.IsBlocked)
                numberofblockedcells++;
        }

        reachable.Add(start);
        while (!reachable.Contains(finish))
        {
            
            reachable.Add(GetCell(field, reachable[0].X + 2, reachable[0].Y - 1, explored));
            reachable.Add(GetCell(field, reachable[0].X + 2, reachable[0].Y + 1, explored));
            reachable.Add(GetCell(field, reachable[0].X - 2, reachable[0].Y - 1, explored));
            reachable.Add(GetCell(field, reachable[0].X - 2, reachable[0].Y + 1, explored));
            reachable.Add(GetCell(field, reachable[0].X + 1, reachable[0].Y + 2, explored));
            reachable.Add(GetCell(field, reachable[0].X - 1, reachable[0].Y + 2, explored));
            reachable.Add(GetCell(field, reachable[0].X + 1, reachable[0].Y - 2, explored));
            reachable.Add(GetCell(field, reachable[0].X - 1, reachable[0].Y - 2, explored));
            for (int i = 0; i < reachable.Count; i++)
            {
                if (reachable[i] == null || reachable[i].IsBlocked == true || explored.Contains(reachable[i]))
                {
                    reachable.Remove(reachable[i]);
                    i--;
                }
                else 
                {
                    if (i != 0 && reachable[i].Previous==null)
                        reachable[i].Previous = reachable[0];
                }
            }
            if (reachable.Count == 0)
                break;
            explored.Add(reachable[0]);
            reachable.Remove(reachable[0]);
            numberofblockedcells++;
            if (numberofblockedcells + 1 == explored.Count)
                break;
        }
        Cell currentcell = finish;

        if (currentcell.Previous == null)
        {
            WriteResult("Immposible");
            return;
        }
        while (currentcell != start) 
        {          
            currentcell = currentcell.Previous;
            chessboard[currentcell.X, currentcell.Y] = '@';
        }
        WriteResult(chessboard);
    }

    static void WriteResult(string text) 
    {
        File.WriteAllText(OutputPath, text);
    }

    static void WriteResult(char[,] chessboard) 
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < chessboard.GetLength(1); j++)
            {
                sb.Append(chessboard[i,j]);
            }
            sb.Append('\n');
        }
        WriteResult(sb.ToString());
    }

    static Cell GetCell(Cell[,] field, int x, int y, List<Cell> explored)
    {
        try
        {
            return field[x, y];
        }
        catch
        {
            return null;
        }
    }
}
