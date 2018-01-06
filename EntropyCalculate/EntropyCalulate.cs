using System;
using System.Collections.Generic;
using System.Linq;

namespace EntropyCalculate
{
	public static class EntropyCalulate
    {
		public static double GetEntropy(string text)
		{
			Dictionary<char, int> letters = text.GroupBy(c => c).ToDictionary(s => s.Key, s => s.Count());
			// Обычная энтропия
			double entropy = 0;
			foreach (KeyValuePair<char, int> keyValuePair in letters)
			{
				double probability = keyValuePair.Value / ((double)text.Length);
				entropy += probability * Lg(probability);
				//Console.WriteLine($"Symbol: {keyValuePair.Key}  probability: {probability} result: {entropy}");
			}
			entropy = -entropy;
			return entropy;

			double Lg(double x) => Math.Log(x) / Math.Log(2);
		}
	}
}
