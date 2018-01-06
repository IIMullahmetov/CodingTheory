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

		private static int Divide(List<KeyValuePair<string, double>> list, int left, int right)
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

		private static void Fano(List<KeyValuePair<string, double>> pairs, Dictionary<string, string> codes, int left, int right)
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
			string input = "айм блю дабуди дабудай дабуди дабудай дабуди дабудай";
			if (input.Length % 2 == 1)
			{
				input += " ";
			}
			//List<KeyValuePair<char, double>> charsAndProbs = input.GroupBy(c => c)
			//	.Select(c => new KeyValuePair<char, double>(c.Key, c.Count()))
			//	.OrderByDescending(k => k.Value).ToList();
			List<KeyValuePair<string, double>> list = new List<KeyValuePair<string, double>>();

			for (int i = 0; i < input.Length - 2; i += 2) 
			{
				string key = input.Substring(i, 2);
				list.Add(new KeyValuePair<string, double>(key, 1));
			}
			list = list.GroupBy(k => k.Key)
				.Select(c => new KeyValuePair<string, double>(c.Key, c.Count()))
				.OrderByDescending(k => k.Value).ToList();
			Dictionary<string, string> codes = new Dictionary<string, string>();
			Fano(list, codes, 0, list.Count - 1);

			foreach (KeyValuePair<string, string> a in codes)
				Console.WriteLine($"{a.Key}: {a.Value}");

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < input.Length - 2; i += 2)
			{
				string key = input.Substring(i, 2);
				builder.Append(codes[key]);
			}
			string encoded = builder.ToString();
			Console.WriteLine("Encoded:");
			Console.WriteLine(encoded);

			StringBuilder decoder = new StringBuilder();
			for (int i = 0; i < encoded.Length; i++)
			{
				try
				{
					foreach (KeyValuePair<string, string> keyValuePair in codes)
					{
						if (encoded.Substring(i, keyValuePair.Value.Length) == keyValuePair.Value)
						{
							decoder.Append(keyValuePair.Key);
							i += keyValuePair.Value.Length - 1;
							break;
						}
					}
				}
				catch { }
			}
			string decoded = decoder.ToString();
			Console.WriteLine("Decoded:");
			Console.WriteLine(decoded);
			Console.WriteLine($"Alphabit: {list.Count}");
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

			FileStream stream = File.Create(@"C:\TOC\result_fano_syllable.bin");
			BinaryWriter writer = new BinaryWriter(stream);
			byte[] arr = new byte[encoded.Count / 8 + (encoded.Count % 8 == 0 ? 0 : 1)];
			encoded.CopyTo(arr, 0);
			writer.Write(arr);
			stream.Flush();
			writer.Flush();
			stream.Close();
			writer.Close();
			writer.Dispose();
			byte[] file = File.ReadAllBytes(@"C:\TOC\result_fano_syllable.bin");
			Console.WriteLine($"File size devided na count of symbols {(double)(file.Length * 8) / (double)text.Length}");
		}
	}
}
