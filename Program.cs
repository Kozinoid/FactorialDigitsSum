using System;
using System.Collections.Generic;
using System.Text;

namespace FactorialDigitsSum
{
    // For multiply a big numbers let's create special class - BigUnsignedNumber. It contain a big number by separate digits in array
    class Program
    {
        static void Main(string[] args)
        {
            BigUnsignedNumber factorial = new BigUnsignedNumber(1);
            for (ulong i = 1; i <= 100; i++)
            {
                factorial.Multiply(new BigUnsignedNumber(i));
            }

            Console.WriteLine("100! = {0}", factorial.ToString());
            Console.WriteLine("Digit Sum of 100! = {0}", factorial.DigitSum());
        }

        // SPECIAL CLASS FOR MULTIPLY BIG NUMBERS
        class BigUnsignedNumber
        {
            // Main array to store a Big Unsigned Number by digits
            private readonly List<byte> digits = new List<byte>();
            // Indexator
            public byte this[int index]
            {
                get { return digits[index]; }
            }
            // Count property
            public int Count
            {
                get { return digits.Count; }
            }

            // ----------------  Constructor  --------------------
            public BigUnsignedNumber(ulong value)
            {
                do
                {
                    digits.Add((byte)(value % 10));
                    value /= 10;
                }
                while (value > 0);
            }

            // --------------  Multiply operation ----------------
            public void Multiply(BigUnsignedNumber multiplier)
            {
                // 'Dirty' multiply (with overflow in each byte)
                int accumulator;
                List<byte> result = new List<byte>();
                for (int multIndex = 0; multIndex < multiplier.Count; multIndex++)
                {
                    for (int digitIndex = 0; digitIndex < digits.Count; digitIndex++)
                    {
                        accumulator = digits[digitIndex] * multiplier[multIndex];
                        if ((digitIndex + multIndex) < result.Count) result[digitIndex + multIndex] += (byte)accumulator; else result.Add((byte)accumulator);
                    }
                }

                // DECIMAL Normalize result (calculate overflow on each byte)
                int overflow = 0;
                for (int i = 0; i < result.Count; i++)
                {
                    result[i] += (byte)overflow;
                    overflow = result[i] / 10;
                    result[i] %= 10;
                }
                if (overflow > 0)
                {
                    result.Add((byte)overflow);
                }

                CopyFromList(result);
            }

            // ------------------  Digit sum  --------------------
            public ulong DigitSum()
            {
                ulong result = 0;   // Calculate sum of digits
                for (int i = 0; i < digits.Count; i++)
                {
                    result += digits[i];
                }
                return result;
            }

            // ----------------  Copy from byteList  -------------
            public void CopyFromList(List<byte> source)
            {
                // Fill BigUnsignedNumber from another list
                digits.Clear();
                for (int i = 0; i < source.Count; i++)
                {
                    digits.Add(source[i]);
                }
            }

            // ------------------  Get string value  -------------
            public override string ToString()
            {
                StringBuilder builder = new StringBuilder("");
                for (int i = digits.Count - 1; i >= 0; i--)
                {
                    builder.Append(digits[i]);
                }
                return builder.ToString();
            }
        }
    }
}
