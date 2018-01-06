using System.Collections.Generic;

namespace HuffmanAlgorithm
{
	public class Node
	{
		public char Symbol { get; set; }
		public double Frequency { get; set; }
		public Node Right { get; set; }
		public Node Left { get; set; }

		public List<bool> Pass(char symbol, List<bool> data)
		{
			// If elemen is leaf
			if (Right == null && Left == null)
			{
				if (symbol == Symbol)
				{
					return data;
				}
				else
				{
					return null;
				}
			}
			else
			{
				List<bool> left = null;
				List<bool> right = null;

				// If node have left element
				if (Left != null)
				{
					List<bool> leftPath = new List<bool>();
					leftPath.AddRange(data);
					leftPath.Add(false);

					left = Left.Pass(symbol, leftPath);
				}
				
				// If node have right element
				if (Right != null)
				{
					List<bool> rightPath = new List<bool>();
					rightPath.AddRange(data);
					rightPath.Add(true);
					right = Right.Pass(symbol, rightPath);
				}

				if (left != null)
				{
					return left;
				}
				else
				{
					return right;
				}
			}
		}
	}
}
