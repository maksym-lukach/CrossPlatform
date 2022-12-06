namespace LabsLibrary
{
	public static class Lab2
	{
		public static string Run(string pathInpFile = "INPUT.TXT")
		{
			var numberOfTrees = Convert.ToInt32(File.ReadLines(pathInpFile).First().Trim());
			if (numberOfTrees < 1 || numberOfTrees > 50)
			{
				return "Number is out of range";
			}
			else
			{
				return (3 * Math.Pow(2, numberOfTrees - 1)).ToString();
			}
		}
	}
}