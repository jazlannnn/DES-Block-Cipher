using System;
using System.Text;
using System.Linq;

public class DESEncryption
{
    // Initial Permutation table
    private static readonly int[] IP = {
        58, 50, 42, 34, 26, 18, 10, 2,
        60, 52, 44, 36, 28, 20, 12, 4,
        62, 54, 46, 38, 30, 22, 14, 6,
        64, 56, 48, 40, 32, 24, 16, 8,
        57, 49, 41, 33, 25, 17,  9, 1,
        59, 51, 43, 35, 27, 19, 11, 3,
        61, 53, 45, 37, 29, 21, 13, 5,
        63, 55, 47, 39, 31, 23, 15, 7
    };

    // Final Permutation table
    private static readonly int[] FP = {
        40, 8, 48, 16, 56, 24, 64, 32,
        39, 7, 47, 15, 55, 23, 63, 31,
        38, 6, 46, 14, 54, 22, 62, 30,
        37, 5, 45, 13, 53, 21, 61, 29,
        36, 4, 44, 12, 52, 20, 60, 28,
        35, 3, 43, 11, 51, 19, 59, 27,
        34, 2, 42, 10, 50, 18, 58, 26,
        33, 1, 41,  9, 49, 17, 57, 25
    };

    // Permuted Choice 1 (PC1) Table
    private static readonly int[] PC1 = {
        57, 49, 41, 33, 25, 17, 9,
        1, 58, 50, 42, 34, 26, 18,
        10,  2, 59, 51, 43, 35, 27,
        19, 11,  3, 60, 52, 44, 36,
        63, 55, 47, 39, 31, 23, 15,
        7, 62, 54, 46, 38, 30, 22,
        14,  6, 61, 53, 45, 37, 29,
        21, 13,  5, 28, 20, 12,  4
    };

    // Permuted Choice 2 (PC2) Table
    private static readonly int[] PC2 = {
        14, 17, 11, 24,  1,  5,
        3, 28, 15,  6, 21, 10,
        23, 19, 12,  4, 26,  8,
        16,  7, 27, 20, 13,  2,
        41, 52, 31, 37, 47, 55,
        30, 40, 51, 45, 33, 48,
        44, 49, 39, 56, 34, 53,
        46, 42, 50, 36, 29, 32
    };

    // Expansion Table
    private static readonly int[] EXPANSION_TABLE = {
        32,  1,  2,  3,  4,  5,
        4,  5,  6,  7,  8,  9,
        8,  9, 10, 11, 12, 13,
        12, 13, 14, 15, 16, 17,
        16, 17, 18, 19, 20, 21,
        20, 21, 22, 23, 24, 25,
        24, 25, 26, 27, 28, 29,
        28, 29, 30, 31, 32,  1
    };

    // P Table
    private static readonly int[] P_TABLE = {
        16, 7, 20, 21, 29, 12, 28, 17,
        1, 15, 23, 26, 5, 18, 31, 10,
        2, 8, 24, 14, 32, 27, 3, 9,
        19, 13, 30, 6, 22, 11, 4, 25
    };

    // Shift Schedule
    private static readonly int[] SHIFT_SCHEDULE = {
        1, 2, 4, 6, 8, 10, 12, 14,
        15, 17, 19, 21, 23, 25, 27, 0
    };

    private static readonly int[][][] S_BOXES = {
        new int[][]{ // S1
            new int[]{14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
            new int[]{0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
            new int[]{4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
            new int[]{15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
        },
        new int[][]{ // S2
            new int[]{15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
            new int[]{3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
            new int[]{0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
            new int[]{13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
        },
        new int[][]{ // S3
            new int[]{10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
            new int[]{13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
            new int[]{13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
            new int[]{1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
        },
        new int[][]{ // S4
            new int[]{7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
            new int[]{13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
            new int[]{10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
            new int[]{3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
        },
        new int[][]{ // S5
            new int[]{2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
            new int[]{14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
            new int[]{4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
            new int[]{11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
        },
        new int[][]{ // S6
            new int[]{12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
            new int[]{10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
            new int[]{9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
            new int[]{4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
        },
        new int[][]{ // S7
            new int[]{4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
            new int[]{13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
            new int[]{1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
            new int[]{6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
        },
        new int[][]{ // S8
            new int[]{13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
            new int[]{1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
            new int[]{7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
            new int[]{2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
        }
    };

    static void Main(string[] args)
    {
        Console.Write("Enter plaintext (64-bit binary): ");
        string plaintext = Console.ReadLine();
        if (!IsValidBinary(plaintext, 64))
        {
            Console.WriteLine("Invalid plaintext. Must be a 64-bit binary string.");
            return;
        }

        Console.Write("Enter key (64-bit binary with parity check bits): ");
        string key = Console.ReadLine();
        if (!IsValidBinary(key, 64))
        {
            Console.WriteLine("Invalid key. Must be a 64-bit binary string.");
            return;
        }

        Console.WriteLine($"Plaintext in Hexadecimal: {BinaryToHex(plaintext)}");
        Console.WriteLine($"Key in Hexadecimal: {BinaryToHex(key)}");

        // Step 2: Initial Permutation
        string permutedPlaintext = Permute(plaintext, IP);
        Console.WriteLine($"\nInitial Permutation (L0R0): {ToHex(permutedPlaintext)}");

        // Divide into L0 and R0
        string L = permutedPlaintext.Substring(0, 32);
        string R = permutedPlaintext.Substring(32);

        Console.WriteLine($"\nL0: {ToHex(L)}");
        Console.WriteLine($"R0: {ToHex(R)}");

        // Step 3: Perform 16 rounds of encryption
        for (int round = 1; round <= 16; round++)
        {
            Console.WriteLine($"\nRound {round}:");

            string keyRound = GenerateRoundKey(key, round); // Key scheduling
            Console.WriteLine($"K{round}: {ToHex(keyRound)}");

            string expandedR = Expand(R); // Expansion function
            Console.WriteLine($"E(R{round - 1}): {ToHex(expandedR)}");

            string xorResult = Xor(expandedR, keyRound);
            Console.WriteLine($"E(R{round - 1}) + K{round}: {ToHex(xorResult)}");

            string fFunctionResult = FFunction(xorResult); // F-function
            Console.WriteLine($"f(R{round - 1}, K{round}): {ToHex(fFunctionResult)}");

            string newR = Xor(L, fFunctionResult);
            L = R;
            R = newR;

            Console.WriteLine($"L{round}R{round}: {ToHex(L)} - {ToHex(R)}");
        }

        // Combine R16 and L16
        string combined = R + L;

        // Step 4: Final Permutation
        string finalCipher = Permute(combined, FP);
        Console.WriteLine($"\nReversed L16R16: {ToHex(combined)}");
        Console.WriteLine($"Final Ciphertext: {finalCipher}");
        Console.WriteLine($"Ciphertext (Hex): {ToHex(finalCipher)}");
    }

    // Helper methods

    private static bool IsValidBinary(string input, int length)
    {
        return input.Length == length && input.All(c => c == '0' || c == '1');
    }

    private static string Permute(string input, int[] table)
    {
        StringBuilder output = new StringBuilder();
        foreach (int i in table)
        {
            output.Append(input[i - 1]); // Convert to 0-based index
        }
        return output.ToString();
    }

    private static string GenerateRoundKey(string key, int round)
    {
        string permutedKey = Permute(key, PC1);
        string left = permutedKey.Substring(0, 28);
        string right = permutedKey.Substring(28, 28);

        // Shift the halves according to the shift schedule
        left = LeftShift(left, SHIFT_SCHEDULE[round - 1]);
        right = LeftShift(right, SHIFT_SCHEDULE[round - 1]);

        string roundKey = left + right;
        return Permute(roundKey, PC2);
    }

    private static string LeftShift(string input, int shifts)
    {
        return input.Substring(shifts) + input.Substring(0, shifts);
    }

    private static string Expand(string R)
    {
        return Permute(R, EXPANSION_TABLE);
    }

    private static string FFunction(string input)
    {
        StringBuilder output = new StringBuilder();

        // Split the input into 8 blocks of 6 bits
        for (int i = 0; i < 48; i += 6)
        {
            string block = input.Substring(i, 6);

            // Determine the row and column
            int row = Convert.ToInt32($"{block[0]}{block[5]}", 2); // First and last bit
            int col = Convert.ToInt32(block.Substring(1, 4), 2);   // Middle 4 bits

            // Get the value from the S-box
            int sBoxValue = S_BOXES[i / 6][row][col];

            // Convert the S-box value to 4-bit binary and append to output
            output.Append(Convert.ToString(sBoxValue, 2).PadLeft(4, '0'));
        }

        // Apply P-table permutation to the output
        return Permute(output.ToString(), P_TABLE);
    }

    private static string Xor(string a, string b)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < a.Length; i++)
        {
            result.Append(a[i] == b[i] ? '0' : '1');
        }
        return result.ToString();
    }

    private static string ToHex(string binary)
    {
        StringBuilder hex = new StringBuilder();
        for (int i = 0; i < binary.Length; i += 4)
        {
            string chunk = binary.Substring(i, 4);
            hex.Append(Convert.ToByte(chunk, 2).ToString("X"));
        }
        return hex.ToString();
    }

    private static string BinaryToHex(string binary)
    {
        StringBuilder hex = new StringBuilder();
        // Pad the binary string to a multiple of 4 bits if necessary
        while (binary.Length % 4 != 0)
        {
            binary = "0" + binary;
        }

        for (int i = 0; i < binary.Length; i += 4)
        {
            string chunk = binary.Substring(i, 4);
            hex.Append(Convert.ToByte(chunk, 2).ToString("X"));
        }
        return hex.ToString();
    }

}
