using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                else if (Frequency == n.Frequency)
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
            private int count = 0;
            
            private Node HT; // Huffman tree to create codes and decode text
            private Node temp; // Huffman tree to create codes and decode text
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
                Console.ReadKey();
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
                    catch
                    {
                        F[c] = F[c] + 1;
                    }
                }
            }

            //// Build a Huffman tree based on the character frequencies greater than 0 (invoked by Huffman)
            private void Build(Dictionary<char, int> F)
            {
                Node test;
                Node NL;
                Node OL;
                
                List<Node> PQ = new List<Node>();
                List<Node> N = new List<Node>();
                foreach (KeyValuePair<Char,int> kvp in F)
                {
                    test = new Node(kvp.Key, kvp.Value, null, null);
                    N.Add(test);
                }

                N.Sort();
                
                for (int i = 0; i < N.Count - 1; i+=2)
                {
                    if (N[i + 1] != null)
                    {
                        NL = new Node('|', N[i].Frequency+N[i+1].Frequency, N[i], N[i + 1]);
                        PQ.Add(NL);
                    }
                    else
                    {
                        PQ.Add(N[i]);
                    }
                }
                
                
                PQ.Sort();
                //PQ.Reverse();
                for (int j = 0; j < N.Count-2; j+=2)
                {
                    if (j + 1 <= PQ.Count-1)
                    {
                        OL = new Node('|', PQ[j].Frequency+PQ[j+1].Frequency, PQ[j], PQ[j + 1]);
                        PQ.Add(OL);
                        PQ.Sort();
                        //PQ.Reverse();
                    }
                    else
                    {
                            OL = new Node('|', PQ[j].Frequency + PQ[PQ.Count-1].Frequency, PQ[j], PQ[PQ.Count-1]);
                            PQ.Add(OL);
                            PQ.Sort();
                            //PQ.Reverse();
                    }
                }
                
                HT = PQ[PQ.Count -1];
            }

            // Create the code of 0s and 1s for each character by traversing the Huffman tree (invoked by Huffman)
            private void CreateCodes(Node root, string code)
            {
                count++;
                if(root.Left== null && root.Right== null)
                {
                    //This is a leaf node; print its value
                    try
                    {
                    D.Add(root.Character, code);
                    }
                    catch
                    {

                    }
                    //code = "";
                }
                else
                {
                    //Recurse on left subtree
                    if(root.Left!= null) {
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
                {
                    encoded += D[c];
                }
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
            // request string
            Console.WriteLine("Please insert a phrase to be encoded");
            string input = Console.ReadLine();
            
            new Huffman(input);
        }
    }
}
