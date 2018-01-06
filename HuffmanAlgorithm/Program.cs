using EntropyCalculate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithm
{
	class Program
	{
		static void Main(string[] args)
		{
			DateTimeOffset start = DateTimeOffset.Now;
			string input = "айм блю дабуди дабудай дабуди дабудай дабуди дабудай";
			var d = input.GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count()).OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
			foreach(var e in d)
			{
				Console.WriteLine(e);
			}
			Console.ReadLine();
			//string input = "aaaaabbbccd";
			HuffmanTree huffmanTree = new HuffmanTree();
			// Encode
			BitArray encoded = huffmanTree.Encode(input);

			//Console.Write("Encoded: ");
			StringBuilder builder = new StringBuilder();
			foreach (bool bit in encoded)
			{
				builder.Append(Convert.ToSByte(bit));
			}
			Console.WriteLine(builder.ToString());
			Console.ReadLine();
			FileStream stream = File.Create(@"C:\TOC\result_symbol.bin");
			
			BinaryWriter writer = new BinaryWriter(stream);
			byte[] arr = new byte[encoded.Length / 8 + (encoded.Length % 8 == 0 ? 0 : 1)];
			encoded.CopyTo(arr, 0);
			writer.Write(arr);
			stream.Flush();
			writer.Flush();
			stream.Close();
			writer.Close();
			writer.Dispose();
			byte[] file =File.ReadAllBytes(@"C:\TOC\result_symbol.bin");
			Console.WriteLine($"File size delenyi na count of symbols {(double)(file.Length * 8) / (double)input.Length}");;
			Console.WriteLine(EntropyCalulate.GetEntropy(input));
			// Decode
			string decoded = huffmanTree.Decode(encoded);

			Console.WriteLine(start - DateTimeOffset.Now);
			Console.ReadLine();
		}
	}
}
