using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CS_LAB2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("*Ділення двійкович чисел (Зсув залишку вправо)*");
            Console.WriteLine(new string('-', 70));
            short x, y;
            Console.WriteLine("Введіть 16-бітове число (ділене):");
            x = Int16.Parse(Console.ReadLine());
            Console.WriteLine("Введіть 16-бітове число (дільник):");
            y = Int16.Parse(Console.ReadLine());
            Console.WriteLine(new string('-', 70));
            int Dividend = x;
            int Divisor = y;
            ToDivide(x, y);

            Console.ReadKey();
        }

        private static string
            RegisterPartToBinaryString(Int64 register, byte bits_amount, bool is_divisor = false) //запис бітів в строку
        {
            string result = string.Empty;

            int last_index = is_divisor ? 15 : -1;
            for (int i = bits_amount - 1 + (is_divisor ? 16 : 0); i > last_index; --i)
                result += (register >> i & 1) + (i % 4 == 0 && i != 0 ? " " : "");
            return result;
        }

        public static void ToDivide(int Dividend, int Divisor)
        {
            long register = Dividend, //ділене записали в регістр
                Quotient = 0,

                shiftedDivisor = Divisor << 16,
                shiftedMinusDivisor = -Divisor << 16;

            const int remainder_bits_amount = 17,
                quotientBitsAmount = 16,
                registerBitsAmount = 33;

            Console.WriteLine("Регістр:\t\t   {0}", RegisterPartToBinaryString(register, registerBitsAmount));

            for (int i = 0; i < 16; ++i)
            {
                Console.WriteLine($"Крок {i}");
                register <<= 1; //зсув регістра вліво(зсув діленого вліва)
                Console.WriteLine("Зсув ліворуч:\t\t   {0}", RegisterPartToBinaryString(register, registerBitsAmount));
                Console.WriteLine("Віднімання дільника: \t{0}", RegisterPartToBinaryString(shiftedMinusDivisor, remainder_bits_amount, true));
                register += shiftedMinusDivisor; //від діленого віднімаємо дільник
                Console.WriteLine("Регістр:\t\t   {0}", RegisterPartToBinaryString(register, registerBitsAmount));


                if ((register >> 32 & 1) == 0) //якщо регістр більше дільника (залишок)
                {
                    Quotient <<= 1; //частку вліво бо 0 треба
                    Console.WriteLine("Зсув ліворуч частки {0}", RegisterPartToBinaryString(Quotient, quotientBitsAmount));
                    Quotient |= 1; //встановлюємо молодшу одиницю 
                    Console.WriteLine("Встановлюємо останній біт частки в 1:\t\t   {0}", RegisterPartToBinaryString(Quotient, quotientBitsAmount));
                    Console.WriteLine(new string('-', 70));
                }
                else
                {
                    register += shiftedDivisor;
                    Console.WriteLine("Додаємо дільник до регістру  {0}", RegisterPartToBinaryString(register, registerBitsAmount));
                    Quotient <<= 1;
                    Console.WriteLine("Встановлюємо останній біт частки в 0:\t\t   {0}", RegisterPartToBinaryString(Quotient, quotientBitsAmount));
                    Console.WriteLine(new string('-', 70));
                }

            }
            Console.WriteLine("Регістр:    {0}", RegisterPartToBinaryString(register, registerBitsAmount, true));
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Результат: ");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("Залишок:    {0} (В десятковій системі: {1})", RegisterPartToBinaryString(register, remainder_bits_amount, true), (register >> 16));
            Console.WriteLine("Частка:     {0} (В десятковій системі: {1})", RegisterPartToBinaryString(Quotient, quotientBitsAmount), Quotient);
        }
    }
}

