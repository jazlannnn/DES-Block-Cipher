
using System;
using System.Linq;

public class SimplifiedDES
{
    // Example S-box (simplified for demonstration)
    private static readonly int[][] S1 = new int[][]
    {
        new int[] { 0, 1, 2, 3, 4, 5, 6, 7 },
        new int[] { 7, 6, 5, 4, 3, 2, 1, 0 }
    };

    // Permutation and key schedule setup
    private static readonly int[] IP = { 1, 5, 2, 0, 3, 7, 4, 6 };  // Example Initial Permutation
    private static readonly int[] FP = { 2, 0, 6, 4, 1, 7, 3, 5 };  // Example Final Permutation
    private static readonly int[] E = { 3, 0, 1, 2, 3, 2, 1, 0 };  // Expansion permutation (simplified)
    private static readonly int[] P4 = { 0, 3, 2, 1 };  // Simplified P4 permutation
    private static readonly int[] PC1 = { 0, 1, 2, 3, 4, 5, 6, 7 };  // Simplified key permutation

    // Initial 8-bit key
    private static string Key = "10101010";

    private static string Encrypt(string plainText)
    {
        // Validate that input is 8 bits
        if (plainText.Length != 8 || !plainText.All(c => c == '0' || c == '1'))
        {
            throw new ArgumentException("Input must be a valid 8-bit binary string.");
        }

        // Convert the plain text to binary (assuming plainText is a string of '0' and '1')
        string permutedData = ApplyPermutation(plainText, IP);

        // Split the permuted data into two halves (4 bits each)
        string left = permutedData.Substring(0, 4);  // First 4 bits
        string right = permutedData.Substring(4, 4); // Next 4 bits

        // Generate the round key
        string roundKey = GenerateRoundKey(Key);

        // One round of simplification (You can expand to multiple rounds as needed)
        string newLeft = XOR(FeistelFunction(left, roundKey), right);
        string newRight = left;

        // Combine the halves and apply final permutation
        string combined = newLeft + newRight;
        string finalData = ApplyPermutation(combined, FP);

        return finalData;
    }

    private static string Decrypt(string cipherText)
    {
        // Validate that input is 8 bits
        if (cipherText.Length != 8 || !cipherText.All(c => c == '0' || c == '1'))
        {
            throw new ArgumentException("Input must be a valid 8-bit binary string.");
        }

        string permutedData = ApplyPermutation(cipherText, IP); // cipherText is 8 bits long

        // Split the permuted data into two halves (4 bits each)
        string left = permutedData.Substring(0, 4);  // First 4 bits
        string right = permutedData.Substring(4, 4); // Next 4 bits

        // Generate the round key (this can be simplified for now)
        string roundKey = GenerateRoundKey(Key);

        // Perform XOR and substitutions for decryption
        string newLeft = XOR(FeistelFunction(left, roundKey), right);
        string newRight = left;

        // Combine the halves and apply final permutation (FP)
        string combined = newLeft + newRight;

        // Ensure combined data is 8 bits before applying final permutation
        if (combined.Length != 8)
        {
            throw new ArgumentException($"Combined data length is not 8 bits: {combined.Length}");
        }

        string finalData = ApplyPermutation(combined, FP); // Apply FP to 8-bit data
        return finalData;
    }

    private static string FeistelFunction(string input, string key)
    {
        // Step 1: Expand the 4-bit input to 8 bits (if needed)
        string expanded = Expand(input);

        // Step 2: XOR with round key (produces 8 bits)
        string xored = XOR(expanded, key);

        // Step 3: Split the 8-bit result into two 4-bit halves
        string leftHalf = xored.Substring(0, 4); // Left half (4 bits)
        string rightHalf = xored.Substring(4, 4); // Right half (4 bits)

        // Step 4: Substitute the left and right halves (resulting 4-bit output each)
        string substitutedLeft = Substitute(leftHalf);
        string substitutedRight = Substitute(rightHalf);

        // Step 5: Combine the substituted halves into an 8-bit string
        string combined = substitutedLeft + substitutedRight;

        // Step 6: Apply the P4 permutation to the substituted 8-bit string
        return ApplyPermutation(substitutedRight, P4); // Apply P4 to the right half only
    }

    private static string Expand(string input)
    {
        // Ensure the input is 4 bits long before expansion
        if (input.Length != 4)
        {
            throw new ArgumentException($"Expected 4-bit input for expansion, but got {input.Length} bits.");
        }

        // Expanding the 4-bit input to 8 bits (simplified for demonstration)
        return string.Join("", E.Select(index => input[index].ToString()));
    }

    private static string Substitute(string input)
    {
        // Ensure the input string is exactly 4 bits long
        if (input.Length != 4)
        {
            throw new ArgumentException($"Substitution input must be a 4-bit binary string. Got: {input}");
        }

        // Validate that the string contains only '0' or '1'
        if (!input.All(c => c == '0' || c == '1'))
        {
            throw new ArgumentException($"Substitution input must be a binary string containing only '0' or '1'. Got: {input}");
        }

        // Convert the 4-bit binary string to a decimal index
        int index = Convert.ToInt32(input, 2);

        // Instead of throwing an error, use modulo 8 to map to the valid range (0-7)
        int sBoxIndex = index % 8;

        // Perform the substitution using the S-box (S1) and return the result as a 4-bit string
        return Convert.ToString(S1[0][sBoxIndex], 2).PadLeft(4, '0');
    }

    private static string XOR(string a, string b)
    {
        // Ensure both strings are of the same length (8 bits in this case)
        if (a.Length != b.Length)
        {
            throw new ArgumentException($"XOR input lengths do not match: {a.Length} vs {b.Length}");
        }

        // Perform the XOR operation on each bit
        return string.Join("", a.Zip(b, (x, y) => (x == y ? '0' : '1')));
    }

    // Ensure that the permutation logic works for a variety of input lengths and types
    private static string ApplyPermutation(string input, int[] permutation)
    {
        // Ensure the length of input matches the permutation length
        if (input.Length != permutation.Length)
        {
            throw new ArgumentException($"Input length ({input.Length}) does not match the permutation length ({permutation.Length}).");
        }

        // Apply the permutation based on the indices
        return string.Join("", permutation.Select(index => input[index].ToString()));
    }

    private static string GenerateRoundKey(string key)
    {
        return ApplyPermutation(key, PC1);
    }

    public static void Main(string[] args)
    {

        Console.WriteLine("========================================");
        Console.WriteLine("Welcome to Simplified DES Application !!");
        Console.WriteLine("========================================");

        // Add a blank line 
        Console.WriteLine();

        while (true)
        {
            // Display menu
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Quit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                // Encrypt
                Console.WriteLine("Enter an 8-bit binary string for encryption (e.g., 11111111):");
                string input = Console.ReadLine().Trim();  // Trim any leading/trailing spaces

                // Validate the input
                if (input.Length != 8 || !input.All(c => c == '0' || c == '1'))
                {
                    Console.WriteLine("Invalid input. Please enter an 8-bit binary string.");
                    continue;
                }

                string encryptedData = Encrypt(input);
                Console.WriteLine("Encrypted Data: " + encryptedData);

                // Add a blank line after encryption
                Console.WriteLine();
            }
            else if (choice == "2")
            {
                // Decrypt
                Console.WriteLine("Enter an 8-bit binary string for decryption (e.g., 11111111):");
                string input = Console.ReadLine().Trim();  // Trim any leading/trailing spaces

                // Validate the input
                if (input.Length != 8 || !input.All(c => c == '0' || c == '1'))
                {
                    Console.WriteLine("Invalid input. Please enter an 8-bit binary string.");
                    continue;
                }

                string decryptedData = Decrypt(input);
                Console.WriteLine("Decrypted Data: " + decryptedData);

                // Add a blank line after decryption
                Console.WriteLine();
            }
            else if (choice == "3")
            {
                // Exit the program
                Console.WriteLine("Exiting the application...");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            // Add a blank line after each iteration
            Console.WriteLine();
        }
    }
}
