using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodingTheory
{
	class Program
	{
		static void Main(string[] args)
		{
			string text = File.ReadAllText(@"C:\Users\IIMul\Downloads\Telegram Desktop\Энтропия - Гилязов Ленар\book.txt");
			
			while (true)
			{
				
				Dictionary<char, int> letters = text.GroupBy(c => c).ToDictionary(s => s.Key, s => s.Count());
				Dictionary<char, double> probabilities = new Dictionary<char, double>();
				// Обычная энтропия
				double entropy = 0;
				foreach (KeyValuePair<char, int> keyValuePair in letters)
				{
					double probability = keyValuePair.Value / ((double) text.Length);
					entropy += probability * Lg(probability);
					probabilities.Add(keyValuePair.Key, probability);
					Console.WriteLine("Symbol: {0}  probability: {1} result: {2}", keyValuePair.Key, probability, entropy);
				}
				double oldEntropy = -entropy;
				Console.WriteLine(oldEntropy);

				Dictionary<KeyValuePair<char, char>, double> slogProbabilities = new Dictionary<KeyValuePair<char, char>, double>();
				int textLength = text.Length;
				for (int i = 0; i < textLength - 1; i++)
				{
					KeyValuePair<char, char> keyValuePair = new KeyValuePair<char, char>(text[i], text[i + 1]);

					if (slogProbabilities.ContainsKey(keyValuePair))
					{
						slogProbabilities[keyValuePair]++;
					}
					else
					{
						slogProbabilities.Add(keyValuePair, 1);
					}
				}
				foreach(KeyValuePair<KeyValuePair<char, char>, double> v in slogProbabilities)
				{
					Console.WriteLine(v.Key + "  " + v.Value);
				}
				entropy = 0;
				Dictionary<KeyValuePair<char, char>, double> secondProbabilities = new Dictionary<KeyValuePair<char, char>, double>();
				Dictionary<string, double> dictionary = new Dictionary<string, double>();
				foreach (KeyValuePair<char, double> keyValuePair in probabilities)
				{
					foreach (KeyValuePair<KeyValuePair<char, char>, double> pair in slogProbabilities.Where(k => k.Key.Key == keyValuePair.Key))
					{
						double prob = (pair.Value / (textLength - 1)) * Lg(pair.Value / (textLength - 1) / keyValuePair.Value);
						entropy += prob;
						secondProbabilities.Add(pair.Key, prob);
						string s = new string(new char[] { pair.Key.Key, pair.Key.Value });
						dictionary.Add(s, prob);
					}
					//Console.WriteLine(-entropy);
				}
				Console.WriteLine("Entropy of text: {0}, Entropy of text with conditional: {1}!!!", oldEntropy, -entropy);
				Dictionary<KeyValuePair<string, char>, double> additionalDictionary = new Dictionary<KeyValuePair<string, char>, double>();
				for (int  i = 0; i < textLength - 2; i++)
				{
					string k = new string(new char[] { text[i], text[i + 1] });
					KeyValuePair<string, char> key = new KeyValuePair<string, char>(k, text[i + 2]);
					if (additionalDictionary.ContainsKey(key))
					{
						additionalDictionary[key]++;
					}
					else
					{
						additionalDictionary.Add(key, 1);
					}
				}
				entropy = 0;
				//foreach(KeyValuePair<KeyValuePair<char, char>, double> keyValuePair in secondProbabilities)
				//{
				//	foreach(KeyValuePair<string, double> pair in additionalDictionary.Where(p => p.Key[0] == keyValuePair.Key.Key && p.Key[1] == keyValuePair.Key.Value))
				//	{
				//		double prob = (pair.Value / (textLength - 2)) * Lg(pair.Value / (textLength - 2) / keyValuePair.Value);
				//		entropy += prob;
				//		Console.WriteLine(entropy);
				//	}
				//}
				foreach(KeyValuePair<string, double> keyValuePair in dictionary)
				{
					foreach (KeyValuePair<KeyValuePair<string, char>, double> pair in additionalDictionary.Where(p => p.Key.Key == keyValuePair.Key))
					{
						double prob = (pair.Value / (textLength - 2)) * Lg(pair.Value / (textLength - 2) / keyValuePair.Value);
						entropy += prob;
						Console.WriteLine(entropy);
					}
				}
				//foreach(KeyValuePair<KeyValuePair<char, char>, double> keyValuePair in secondProbabilities)
				//{
				//	foreach(KeyValuePair<KeyValuePair<string, char>, double> pair in additionalDictionary.Where(p => keyValuePair == p.Key.Key))
				//	{
				//		double prob = (pair.Value / (textLength - 2)) * Lg(pair.Value / (textLength - 2) / keyValuePair.Value);
				//		entropy += prob;
				//		Console.WriteLine(entropy);
				//	}
				//}
				Console.WriteLine("additional entropy: {0}", -entropy);
				Console.Read();
				double Lg(double x) => Math.Log(x) / Math.Log(2);
			}
		}
	}
}
