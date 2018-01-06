using EntropyCalculate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fano
{
	class Program
	{
		private static void WriteArray(List<KeyValuePair<double, char>> List)
		{
			foreach (KeyValuePair<double, char> a in List)
				Console.WriteLine($"{a.Value}: {a.Key}");
			Console.WriteLine("\n");
		}

		private static int Divide(List<KeyValuePair<char, double>> list, int left, int right)
		{
			double halfProb = 0, pieceProb = 0;
			for (int j = left; j <= right; j++)
			{
				halfProb += list[j].Value;
			}

			halfProb /= 2;

			int i = 0;
			for (i = left; pieceProb < halfProb && i <= right; i++)
			{
				pieceProb += list[i].Value;
			}

			return i - 1;
		}

		private static void Fano(List<KeyValuePair<char, double>> pairs, Dictionary<char, string> codes, int left, int right)
		{
			if (right - left > 0)
			{
				int index = Divide(pairs, left, right);

				for (int i = left; i <= index; i++)
				{
					if (codes.ContainsKey(pairs[i].Key))
						codes[pairs[i].Key] += "1";
					else
						codes.Add(pairs[i].Key, "1");
				}

				for (int i = index + 1; i <= right; i++)
				{
					if (codes.ContainsKey(pairs[i].Key))
						codes[pairs[i].Key] += "0";
					else
						codes.Add(pairs[i].Key, "0");
				}

				Fano(pairs, codes, left, index);
				Fano(pairs, codes, index + 1, right);
			}

		}

		static void Main(string[] args)
		{
			string input = File.ReadAllText(@"C:\TOC\book.txt");
			List<KeyValuePair<char, double>> charsAndProbs = input.GroupBy(c => c)
				.Select(c => new KeyValuePair<char, double>(c.Key, c.Count()))
				.OrderByDescending(k => k.Value).ToList();
			Dictionary<char, string> codes = new Dictionary<char, string>();
			Fano(charsAndProbs, codes, 0, charsAndProbs.Count - 1);

			foreach (KeyValuePair<char, string> a in codes)
				Console.WriteLine($"{a.Key}: {a.Value}");

			StringBuilder builder = new StringBuilder();
			foreach (char symbol in input)
			{
				builder.Append(codes[symbol]);
			}
			string encoded = builder.ToString();
			Console.WriteLine("Encoded:");
			Console.WriteLine(encoded);

			StringBuilder decoder = new StringBuilder();
			for (int i = 0; i < encoded.Length; i++)
			{
				foreach (KeyValuePair<char, string> keyValuePair in codes)
				{
					if (encoded.Substring(i, keyValuePair.Value.Length) == keyValuePair.Value)
					{
						decoder.Append(keyValuePair.Key);
						i += keyValuePair.Value.Length - 1;
						break;
					}
				}
			}
			string decoded = decoder.ToString();
			Console.WriteLine("Decoded:");
			Console.WriteLine(decoded);
			Console.WriteLine($"Alphabit: {charsAndProbs.Count}");
			Console.WriteLine($"Symbols count: {input.Length}");
			Console.WriteLine(
				$"Entropy of decoded text: {EntropyCalulate.GetEntropy(decoded)}" +
				$"{Environment.NewLine}" +
				$"Entropy of encoded text: {EntropyCalulate.GetEntropy(encoded)}");
			Write(encoded, input);
			Console.ReadKey();
		}

		private static void Write(string inputBytes, string text)
		{
			BitArray encoded = new BitArray(inputBytes.Length);
			for (int i = 0; i < inputBytes.Length; i++)
			{
				encoded[i] = inputBytes[i] == '0' ? false : true;
			}

			FileStream stream = File.Create(@"C:\TOC\result_fano_symbol.bin");
			BinaryWriter writer = new BinaryWriter(stream);
			byte[] arr = new byte[encoded.Count / 8 + (encoded.Count % 8 == 0 ? 0 : 1)];
			encoded.CopyTo(arr, 0);
			writer.Write(arr);
			stream.Flush();
			writer.Flush();
			stream.Close();
			writer.Close();
			writer.Dispose();
			byte[] file = File.ReadAllBytes(@"C:\TOC\result_fano_symbol.bin");
			Console.WriteLine($"File size devided na count of symbols {(double)(file.Length * 8) / (double)text.Length}");
		}
	}
}
