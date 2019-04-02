using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Floatingpoint
{
    class Program
    {
        static void Main(string[] args)
        {
            float a, b;
            Console.WriteLine("Enter first float signed value:");
            a = float.Parse(Console.ReadLine());
            Console.WriteLine("Enter second float signed value:");
            b = float.Parse(Console.ReadLine());
            Floating f = new Floating(a, b);
            f.Do();
        }
    }
    class Floating
    {

        public float First { get; set; }
        public float Second { get; set; }
        public Floating(float x, float y)
        {
            First = x;
            Second = y;
        }
        public void Do()
        {
            int bias = (int)(Math.Pow(2, 7) - 1);

            bool is_adding = First * Second >= 0;
            if (Math.Abs(Second) > Math.Abs(First))
            {
                float temp;
                temp = First;
                First = Second;
                Second = temp;
            }

            Console.WriteLine("Adding {0} (a), to {1} (b)", First, Second);
            int a_sign_bit = First < 0 ? 1 : 0,
                b_sign_bit = Second < 0 ? 1 : 0;
            First = Math.Abs(First);
            Second = Math.Abs(Second);

            int a_int_bits = (int)First,
                b_int_bits = (int)Second;
            First -= a_int_bits;
            Second -= b_int_bits;

            FloatBits(First, out int a_float_bits);
            FloatBits(Second, out int b_float_bits);


            Console.WriteLine("  Convert \"a\" to binary (without exponent and normalization):\n\t{0}",
                ResultToBinaryString(a_sign_bit, 0, a_float_bits));
            Console.WriteLine("  Convert \"b\" to binary (without exponent and normalization):\n\t{0}",
                ResultToBinaryString(b_sign_bit, 0, b_float_bits));

            Int16 exp_a = Normalize(a_int_bits, ref a_float_bits),
                exp_b = Normalize(b_int_bits, ref b_float_bits);

            byte exponent_a = (byte)(exp_a + bias),
                exponent_b = (byte)(exp_b + bias);

            string a_float_bits_string = ResultToBinaryString(a_sign_bit, exponent_a, a_float_bits);
            Console.WriteLine("  Normalize \"a\":\n\t{0}", a_float_bits_string);
            Console.WriteLine("  Normalize \"b\" :\n\t{0}", ResultToBinaryString(b_sign_bit, exponent_b, b_float_bits));

            b_float_bits >>= exp_a - exp_b;

            string b_float_bits_string = ResultToBinaryString(b_sign_bit, exponent_b, b_float_bits);
            Console.WriteLine("  Shift left \"b\" on {0}:\n\t{1}", exp_a - exp_b, b_float_bits_string);
            Console.WriteLine("  Adding \"a\" to \"b\":\n\t{0}\n      +\t{1}", a_float_bits_string, b_float_bits_string);

            Int32 result = is_adding ? a_float_bits + b_float_bits : a_float_bits - b_float_bits;
            NormilizeResult(ref result, ref exp_a, is_adding);
            exponent_a = (byte)(exp_a + bias);

            Console.WriteLine("  Answer is:\n\tIn decemal: {0}\n\tIn binary: {1}",
                ConvertToDecimal(exp_a, result, a_sign_bit), ResultToBinaryString(a_sign_bit, exponent_a, result));
        }

        private void FloatBits(float value, out Int32 float_bits)
        {
            const int amount_of_mantisa_bits = 23;
            int i = 0;

            float_bits = 0;
            while (value != 0 && i < 22) // check on overflow
            {
                value *= 2;
                if (value >= 1)
                {
                    float_bits |= 1;
                    value -= 1;
                }
                float_bits <<= 1;
                ++i;
            }
            float_bits <<= amount_of_mantisa_bits - i - 1;
            Console.WriteLine();
        }

        private Int16 Normalize(int value, ref Int32 float_bits)
        {
            Int16 exp = 0;
            Int32 hidden_one = 1 << 23;

            if (value > 0)
            {
                while (value > 1)
                {
                    ++exp;
                    float_bits >>= 1;
                    float_bits |= (value & 1) << 22;
                    value >>= 1;
                }
                float_bits |= hidden_one;
            }

            return exp;
        }

        private void NormilizeResult(ref Int32 result, ref Int16 exp, bool is_adding)
        {
            Int32 hidden_one = 1 << 23;

            if ((result & hidden_one) == hidden_one)
            {
                ++exp;
                result >>= 1;
                return;
            }

            if (is_adding)
                do
                {
                    ++exp;
                    result >>= 1;
                } while ((result & hidden_one) != hidden_one);
            else
                do
                {
                    --exp;
                    result <<= 1;
                } while ((result & hidden_one) != hidden_one);
        }

        static string ResultToBinaryString(int sign_bit, byte exponent, Int32 result)
        {
            string result_string = sign_bit + " | ";
            for (int i = 7; i >= 0; --i)
                result_string += exponent >> i & 1;
            result_string += " | ";
            for (int i = 22; i >= 0; --i)
                result_string += result >> i & 1;

            return result_string;
        }

        private float ConvertToDecimal(Int16 exp_a, Int32 mantissa, int sign_bit)
        {
            float result = 0,
                multiplier = (float)Math.Pow(2, exp_a);


            for (int i = 23; i >= 0; --i, multiplier /= 2)
                result += multiplier * (mantissa >> i & 1);
            if (sign_bit == 1)
                result = -result;
            return result;
        }
    }
}
