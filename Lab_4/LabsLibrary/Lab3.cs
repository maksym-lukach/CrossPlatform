namespace LabsLibrary
{
	enum Side
	{
		Up,
		Down,
		Left,
		Right
	}
	public static class Lab3
	{

		public static string Run(string pathInpFile = "INPUT.TXT")
		{
			var inputData = File.ReadLines(pathInpFile);
			var buf = inputData.First().Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
			int k = Convert.ToInt32(buf[0]), n = Convert.ToInt32(buf[1]), m = Convert.ToInt32(buf[2]);
			var game = inputData.Skip(1).Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToArray()).ToArray();

			if (k < 0 || k > 5 || n < 1 || n > 20 || m < 1 || m > 20)
			{
				return "Out of range exception!";
			}
			else
			{
				return GetMinTime(game, k).ToString();
			}
		}

		public static int GetMinTime(int[][] game, int rightCount)
		{
			(int row, int col) startPos = (-1, -1);
			for (int i = 0; i < game.Length; i++)
			{
				for (int j = 0; j < game[0].Length; j++)
				{
					if (game[i][j] == 2)
					{
						startPos = (i, j);
						break;
					}
				}
				if (startPos.row != -1)
				{
					break;
				}
			}
			if (startPos.row == -1)
			{
				return -1;
			}
			else
			{
				List<int> timeList = new List<int>();
				timeList.Add(getMinTime(startPos, Side.Up, game, rightCount));
				timeList.Add(getMinTime(startPos, Side.Down, game, rightCount));
				timeList.Add(getMinTime(startPos, Side.Left, game, rightCount));
				timeList.Add(getMinTime(startPos, Side.Right, game, rightCount));
				timeList = timeList.Where(time => time != -1).ToList();
				return timeList.Any() ? timeList.Min() : -1;
			}
		}

		private static List<Tuple<int, int, Side, int>> gameBuf = new List<Tuple<int, int, Side, int>>();
		private static int getMinTime((int row, int col) currPos, Side currWatchSide, int[][] game, int rightCount, int time = -1)
		{
			if (gameBuf.Contains(new Tuple<int, int, Side, int>(currPos.row, currPos.col, currWatchSide, rightCount)))
			{
				return -1;
			}
			gameBuf.Add(new Tuple<int, int, Side, int>(currPos.row, currPos.col, currWatchSide, rightCount));
			time++;
			if (game[currPos.row][currPos.col] == 3)
			{
				return time;
			}
			List<((int row, int col) nextPos, Side nextWatchSide, bool usedRight)> whereToGoList = getPosToGoList(currPos, currWatchSide, game, rightCount > 0);
			List<int> timeList = new List<int>();
			foreach (var whereToGo in whereToGoList)
			{
				timeList.Add(getMinTime(whereToGo.nextPos, whereToGo.nextWatchSide, game, whereToGo.usedRight ? rightCount - 1 : rightCount, time));
			}
			timeList = timeList.Where(time => time != -1).ToList();
			return timeList.Any() ? timeList.Min() : -1;
		}

		private static List<((int row, int col) nextPos, Side nextWatchSide, bool usedRight)> getPosToGoList((int row, int col) currPos, Side currWatchSide, int[][] game, bool hasRightSide)
		{
			List<((int row, int col), Side, bool)> result = new List<((int row, int col), Side, bool)>();
			switch (currWatchSide)
			{
				case Side.Up:
					if (currPos.row != 0 && game[currPos.row - 1][currPos.col] != 1)
					{
						result.Add(((currPos.row - 1, currPos.col), Side.Up, false));
					}
					if (currPos.col != 0 && game[currPos.row][currPos.col - 1] != 1)
					{
						result.Add(((currPos.row, currPos.col - 1), Side.Left, false));
					}
					if (hasRightSide && currPos.col != game[0].Length - 1 && game[currPos.row][currPos.col + 1] != 1)
					{
						result.Add(((currPos.row, currPos.col + 1), Side.Right, true));
					}
					break;
				case Side.Down:
					if (currPos.row != game.Length - 1 && game[currPos.row + 1][currPos.col] != 1)
					{
						result.Add(((currPos.row + 1, currPos.col), Side.Down, false));
					}
					if (hasRightSide && currPos.col != 0 && game[currPos.row][currPos.col - 1] != 1)
					{
						result.Add(((currPos.row, currPos.col - 1), Side.Left, true));
					}
					if (currPos.col != game[0].Length - 1 && game[currPos.row][currPos.col + 1] != 1)
					{
						result.Add(((currPos.row, currPos.col + 1), Side.Right, false));
					}
					break;
				case Side.Left:
					if (currPos.col != 0 && game[currPos.row][currPos.col - 1] != 1)
					{
						result.Add(((currPos.row, currPos.col - 1), Side.Left, false));
					}
					if (currPos.row != game.Length - 1 && game[currPos.row + 1][currPos.col] != 1)
					{
						result.Add(((currPos.row + 1, currPos.col), Side.Down, false));
					}
					if (hasRightSide && currPos.row != 0 && game[currPos.row - 1][currPos.col] != 1)
					{
						result.Add(((currPos.row - 1, currPos.col), Side.Down, true));
					}
					break;
				case Side.Right:
					if (currPos.row != 0 && game[currPos.row - 1][currPos.col] != 1)
					{
						result.Add(((currPos.row - 1, currPos.col), Side.Up, false));
					}
					if (hasRightSide && currPos.row != game.Length - 1 && game[currPos.row + 1][currPos.col] != 1)
					{
						result.Add(((currPos.row + 1, currPos.col), Side.Down, true));
					}
					if (currPos.col != game[0].Length - 1 && game[currPos.row][currPos.col + 1] != 1)
					{
						result.Add(((currPos.row, currPos.col + 1), Side.Right, false));
					}
					break;
			}
			return result;
		}
	}
}