using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithmForSyllable
{
	class HuffmanTree
	{
		private List<Node> Nodes { get; set; } = new List<Node>();
		public Node Root { get; set; }
		private void Build(string source)
		{
			double textLength = source.Length;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < source.Length - 1; i++)
			{
				string syllable = builder.Append(source[i]).Append(source[i + 1]).ToString();
				if (!Nodes.Any(n => n.Syllable == syllable)) {
					Node node = new Node
					{
						Count = 1,
						Syllable = syllable,
						Frequency = 1 / (textLength-1) 
					};
					Nodes.Add(node);
				}
				else
				{
					Node node = Nodes.First(n => n.Syllable == syllable);
					node.Count++;
					node.Frequency = node.Count / (textLength - 1);
				}
				builder.Clear();
			}
			while (Nodes.Count > 1)
			{
				IEnumerable<Node> orderedNodes = Nodes.OrderBy(n => n.Frequency);

				if (orderedNodes.Count() > 1)
				{
					// Take first two items
					List<Node> taken = orderedNodes.Take(2).ToList();

					// Create a parent node by combining the frequencies
					Node parent = new Node()
					{
						Syllable = "**",
						Frequency = taken[0].Frequency + taken[1].Frequency,
						Left = taken[0],
						Right = taken[1]
					};
					Nodes.RemoveAll(n => taken.Contains(n));
					Nodes.Add(parent);
				}
				Root = Nodes.First();
			}
			Console.WriteLine(Root.Frequency);
		}

		public BitArray Encode(string source)
		{
			Build(source);
			List<bool> encodedSource = new List<bool>();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			StringBuilder builder = new StringBuilder();
			List<bool> encodedSymbol;
			for (int i = 0; i < source.Length - 1; i += 2)
			{
				string syllable = new StringBuilder().Append(source[i]).Append(source[i + 1]).ToString();
				encodedSymbol = Root.Pass(syllable, new List<bool>());
				encodedSource.AddRange(encodedSymbol);
				foreach (bool b in encodedSymbol)
					builder.Append(Convert.ToSByte(b));

				if (!dictionary.Any(k => k.Key == syllable))
					dictionary.Add(syllable, builder.ToString());
				builder.Clear();
			}

			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				Console.WriteLine(keyValuePair);
			}
			BitArray bits = new BitArray(encodedSource.ToArray());

			return bits;
		}

		public string Decode(BitArray bits)
		{
			Node current = Root;
			StringBuilder decoded = new StringBuilder();

			foreach (bool bit in bits)
			{
				if (bit)
				{
					if (current.Right != null)
					{
						current = current.Right;
					}
				}
				else
				{
					if (current.Left != null)
					{
						current = current.Left;
					}
				}

				if (IsLeaf(current))
				{
					decoded.Append(current.Syllable);
					current = Root;
				}
			}

			return decoded.ToString();
		}

		public bool IsLeaf(Node node) => (node.Left == null && node.Right == null);
	}
}
