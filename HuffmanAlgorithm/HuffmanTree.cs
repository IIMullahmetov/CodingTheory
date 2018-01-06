using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithm
{
	class HuffmanTree
	{
		private List<Node> Nodes { get; set; }
		public Node Root { get; set; }
		private void Build(string source)
		{
			double textLength = source.Length;
			Nodes = source.GroupBy(c => c)
				.ToDictionary(c => c.Key, c => c.Count() / textLength)
				.Select(k => new Node() { Symbol = k.Key, Frequency = k.Value })
				.ToList();
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
						Symbol = '*',
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
			Dictionary<char, string> dictionary = new Dictionary<char, string>();
			StringBuilder builder = new StringBuilder();
			List<bool> encodedSymbol;
			for (int i = 0; i < source.Length; i++)
			{
				encodedSymbol = Root.Pass(source[i], new List<bool>());
				encodedSource.AddRange(encodedSymbol);
				foreach(bool b in encodedSymbol)
					builder.Append(Convert.ToSByte(b));

				if (!dictionary.Any(k => k.Key == source[i]))
					dictionary.Add(source[i], builder.ToString());
				builder.Clear();
			}
			foreach(KeyValuePair<char, string> keyValuePair in dictionary)
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
					decoded.Append(current.Symbol);
					current = Root;
				}
			}

			return decoded.ToString();
		}

		public bool IsLeaf(Node node) => (node.Left == null && node.Right == null);
	}
}
