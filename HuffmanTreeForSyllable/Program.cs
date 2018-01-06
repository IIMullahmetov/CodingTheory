using EntropyCalculate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithmForSyllable
{
	class Program
	{
		static void Main(string[] args)
		{
			DateTimeOffset start = DateTimeOffset.Now;
			string input = File.ReadAllText(@"C:\TOC\book.txt");
			//string input = "aaaaabbbccd";
			if (input.Length % 2 == 1)
			{
				input += " ";
			}
			HuffmanTree huffmanTree = new HuffmanTree();
			// Encode
			BitArray encoded = huffmanTree.Encode(input);

			//Console.Write("Encoded: ");
			//StringBuilder builder = new StringBuilder();
			//foreach (bool bit in encoded)
			//{
			//	builder.Append(Convert.ToSByte(bit));
			//}
			FileStream stream = File.Create(@"C:\TOC\result_syllable.bin");
			BinaryWriter writer = new BinaryWriter(stream);
			byte[] arr = new byte[encoded.Length / 8 + (encoded.Length % 8 == 0 ? 0 : 1)];
			encoded.CopyTo(arr, 0);
			writer.Write(arr);
			stream.Flush();
			writer.Flush();
			stream.Close();
			writer.Close();
			writer.Dispose();
			byte[] file = File.ReadAllBytes(@"C:\TOC\result_syllable.bin");
			Console.WriteLine($"File size delenyi na count of symbols {(double)(file.Length * 8) / (double)input.Length}");
			EntropyCalulate.GetEntropy(input);


			// Decode
			string decoded = huffmanTree.Decode(encoded);

			//Console.WriteLine("Decoded: " + decoded);
			Console.WriteLine(start - DateTimeOffset.Now);
			Console.ReadLine();
		}
	}
}
