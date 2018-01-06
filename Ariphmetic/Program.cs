using IronRuby.StandardLibrary.BigDecimal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Ariphmetic
{
	class Program
	{
		static void Main(string[] args)
		{
			string input = "only some text";

			Dictionary<char, int> dictionary = input.GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
			Dictionary<char, Element> elements = new Dictionary<char, Element>();
			double border = 0d;
			foreach(KeyValuePair<char, int> keyValuePair in dictionary)
			{
				elements.Add(keyValuePair.Key, new Element { left = border, right = border + (keyValuePair.Value / input.Length), symbol = keyValuePair.Key });
				border += keyValuePair.Value / input.Length;
			}
		}

		static StringBuilder result = new StringBuilder();
		static int countZeroOnBegin = 0;

		public static void ArithmeticCoding(Dictionary<char, Element> segments, StringBuilder text)
		{
			char[] letters = text.ToString().ToCharArray();
			double left = 0;
			double right = 1;
			for (int i = 0; i < letters.Length; i++)
			{
				Element segment = segments[letters[i]];
				double newRight = left + (right - left) * segment.right;
				double newLeft = left + (right - left) * segment.left;
				Element checkSegment = CheckOverlap(newLeft, newRight);
				left = checkSegment.left;
				right = checkSegment.right;
			}
			result.Append($"{(left + right) / 2}".Substring(2));
			Console.WriteLine("Binary Write " + BigInteger.Parse(result.ToString()));
			Console.WriteLine("ALL " + result);

			while (result.ToString().StartsWith("0"))
			{
				countZeroOnBegin++;
				result.Remove(0, 1);
			}
			Console.WriteLine("ALL");

		}


		public static Element CheckOverlap(double left, double right)
		{
			double newLeft = left * 10;
			double newRight = right * 10;
			while ((int)newLeft == (int)newRight)
			{
				result.append(string.valueOf((int)newLeft));
				//            left = new BigDecimal(newLeft - (int) newLeft).doubleValue();
				left = new BigDecimal(newLeft - (int)newLeft).setScale(16, BigDecimal.DOUBLE_FIG).doubleValue();
				//            right = new BigDecimal(newRight - (int) newRight).doubleValue();
				right = new BigDecimal(newRight - (int)newRight).setScale(16, BigDecimal.DOUBLE_FIG).doubleValue();
				//            right = newRight - (int) newRight;
				newLeft = left * 10;
				newRight = right * 10;
			}
			return new Segment(" ", left, right);
		}

	}
}
