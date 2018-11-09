using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriorityQueue; // Using provided Priority Queue

namespace Assignment2
{
    class Program
    {
        // Skeleton code given
        class Node : IComparable
        {
            public char Character { get; set; }
            public int Frequency { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(char character, int frequency, Node left, Node right)
            {
                Character = character;
                Frequency = frequency;
                Left = left;
                Right = right;
            }

            public int CompareTo(Object obj)
            {
                Node n = obj as Node;
                if (Frequency < n.Frequency)
                    return -1;
                else if (Frequency == n.Frequency) // If the frequency is the same, compare the ACII code
                {
                    if(Left!=null && n.Left!=null)
                    {

                    if ((byte)Left.Character <= (byte)n.Left.Character)
                        return -1;
                    else
                        return 1;
                    }
                    else
                    {
                        
                    if ((byte)Character <= (byte)n.Character)
                        return -1;
                    else
                        return 1;
                    }
                }
                else
                    return 1;
            }
        }

        class Huffman
        {
            private Node HT; // Huffman tree to create codes and decode text
            private Dictionary<Char, string> D = new Dictionary<char, string>(); // Dictionary to encode text
            private Dictionary<Char, int> F = new Dictionary<char, int>(); // Dictionary to store frequency

            // Constructor
            public Huffman(string S)
            {
                AnalyzeText(S);
                Build(F);
                CreateCodes(HT, "");
                string encoded = Encode(S);
                Console.WriteLine(encoded);
                string decoded = Decode(encoded);
                Console.WriteLine(decoded);
            }

            // Return the frequency of each character in the given text (invoked by Huffman)
            private void AnalyzeText(string S)
            {
                char[] C = S.ToCharArray();
                foreach(char c in C)
                {
                    try
                    {
                        F.Add(c, 1);
                    }
                    catch // If the character is already in the dictionary, increase the frequency
                    {
                        F[c] = F[c] + 1;
                    }
                }
            }

            // Build a Huffman tree based on the character frequencies greater than 0 (invoked by Huffman)
            private void Build(Dictionary<char, int> F)
            {
                PriorityQueue<Node> PQ = new PriorityQueue<Node>(F.Count) ;
                PriorityQueue<Node> N = new PriorityQueue<Node>(F.Count) ;
                
                // Middle step to stage characters as nodes
                foreach (KeyValuePair<Char,int> kvp in F)
                {
                    Node test;
                    test = new Node(kvp.Key, kvp.Value, null, null);
                    N.Add(test);
                }

                // Adding those characters into the working queue
                while(N.Size() != 0)
                {
                    if (N.Size() > 1)
                    {
                        Node temp1 = N.Front();
                        N.Remove();
                        Node temp2 = N.Front();
                        N.Remove();
                        PQ.Add(new Node(' ', temp1.Frequency + temp2.Frequency, temp1, temp2));
                    }
                    else
                    {
                        PQ.Add(N.Front());
                        N.Remove();
                    }
                }
                
                // pairs all the lowest frequency chars together first and builds whole tree
                while (PQ.Size() > 1)
                {
                    Node temp1 = PQ.Front();
                    PQ.Remove();
                    Node temp2= PQ.Front();
                    PQ.Remove();
                    PQ.Add(new Node(' ', temp1.Frequency+temp2.Frequency, temp1, temp2));

                }
                // Set HT to the node with the largest frequency/the root node
                HT = PQ.Front();
            }

            // Create the code of 0s and 1s for each character by traversing the Huffman tree (invoked by Huffman)
            private void CreateCodes(Node root, string code)
            {
                if (F.Count == 1) // If there is only one character in the message
                {
                    D.Add(root.Character, "0");
                }
                else if(root.Left== null && root.Right== null) // This is a leaf node
                {
                    D.Add(root.Character, code);
                }
                else // Not a leaf node
                {
                    //Recurse on left subtree
                    if(root.Left!= null)
                    {
                        CreateCodes(root.Left, code+"0");
                    }
                    //Recurse on right subtree
                    if(root.Right!= null)
                    {
                        CreateCodes(root.Right, code+"1");
                    }
                }
            }

            // Encode the given text and return a string of 0s and 1s
            public string Encode(string S)
            {
                char[] C = S.ToCharArray();
                string encoded = "";

                foreach (char c in C)
                    encoded += D[c];
                return encoded;
            }

            // Decode the given string of 0s and 1s and return the original text
            public string Decode(string S)
            {
                string temp = "";
                char[] C = S.ToCharArray();
                string decoded = "";

                foreach (char c in C)
                {
                    temp += c;
                    if (D.ContainsValue(temp))
                    {
                        decoded += D.FirstOrDefault(x => x.Value == temp).Key;
                        temp = "";
                    }
                }
                return decoded;
            }

        }

        static void Main()
        {
            bool exit = false;
            while (!exit)
            {
                // request string
                Console.WriteLine("Please insert a phrase to be encoded");
                string input = Console.ReadLine();
                if (input == "")
                {
                }
                else
                {
                    new Huffman(input);
                }
                Console.WriteLine("Press ESC to exit and any key to continue.");
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    exit = true;
                }
                Console.WriteLine("\n");
            }
        }
    }
}
