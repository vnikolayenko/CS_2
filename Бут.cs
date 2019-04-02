using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Умножение_алгоритм_Бута__
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter first argument, if positive - 0, negative -1./n " +
                "Then enter digit");
            int sign1 = int.Parse(Console.ReadLine());
            int digit1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter first argument, if positive - 0, negative -1./n " +
                "Then enter digit");
            int sign2 = int.Parse(Console.ReadLine());
            int digit2 = int.Parse(Console.ReadLine());
            Console.WriteLine("You entered {0} {1} for first digit and {2} {3} for second" ,sign1,digit1,sign2,digit2);
            // First part
            int x = BitAmount(digit1);
            int y = BitAmount(digit2);
            Console.WriteLine("Amount of bit for digit1 - {0}. Amount of bit for digit2 - {1}",x,y);
            int bit = 0;
            if (x > y)
            {
                bit = x;
            }
            else
            {
                bit = y;
            }
            //Back digit1 in binary with modele 
            int [] a  = Perevod(digit1,bit);
            Console.WriteLine("Binary digit1 = {0}     {1}. Lenght {2}", sign1, String.Concat<int>(a),a.Length);
            if (sign1 == 1)
            {
                a = AddCode(a);
            }
            //Back digit1 in binary with module
            int[] b = Perevod(digit2,bit);
            //If second digit is negative we convert to addcode 
            if (sign2 == 1)
            {
                b = AddCode(b);
            }
            Console.WriteLine("Binary digit2 = {0}     {1}", sign2, String.Concat<int>(b));
            //Back digit1 with convert binary in addcode
            int[] c = AddCode(a);
            Console.WriteLine("Addcode digit1 = -{0}     {1}", sign1, String.Concat<int>(c));
            int[] A = Amethod(bit,a);
            Console.WriteLine("A  = {0}", String.Concat<int>(A));
            int[] B = Bmethod(bit,c);
            Console.WriteLine("B  = {0}", String.Concat<int>(B));
            int[] P = Pmethod(bit,b);
            Console.WriteLine("P  = {0}", String.Concat<int>(P));
            //Second Part
            for (int i = 0; i < bit; i++)
            {
                
                int[] last = LastTwoBitP(P);
                Console.WriteLine("Step {0}, last two bit {1}", (i + 1), String.Concat<int>(last));
                
                string last1=String.Concat<int>(last);
                switch (last1)
                {
                    case "00" :
                        {
                            P =ShiftRight(P);
                            Console.WriteLine("Made operation shift, P ={0}", String.Concat<int>(P));////???????????????????????????????
                            break;
                        }
                    case "01" :
                        {
                            P = Sum(P, A);
                            Console.WriteLine("Made sum P+A, P ={0}", String.Concat<int>(P));
                            P =ShiftRight(P);
                            Console.WriteLine("Made operation shift, P ={0}", String.Concat<int>(P));
                            break;
                        }
                    case "10" :
                        {
                            P = Sum(P, B);
                            Console.WriteLine("Made sum P+B, P ={0}", String.Concat<int>(P));
                            P =ShiftRight(P);
                            Console.WriteLine("Made operation shift, P ={0}", String.Concat<int>(P));
                            break;
                        }
                    case "11" :
                        {
                            P=ShiftRight(P);
                            Console.WriteLine("Made operation shift, P ={0}", String.Concat<int>(P));
                            break;
                        }
                }
                Console.WriteLine("P after step = {0}    ", String.Concat<int>(P));
            }
            //Finished part
            int [] P1 = new int[P.Length-2];
            P1 = Answer(P);
            Console.WriteLine("Cutted P {0}",String.Concat<int>(Answer(P)));
            string sign = "";
            if (P1[0] == 1)
            {
                sign = "-";
            }
            else
            {
                sign = "+";
            }
            P1 = AddCode(P1);
            Console.WriteLine("Binary answer {0}", String.Concat<int>(P1));
            Console.WriteLine("Answer {1}{0}",ConvertToDecimal(P1),sign);

        }
        public static double ConvertToDecimal(int[] P1)
        {
            double answer = 0;
            for (int i=0;i <P1.Length;i++)
            {
                
                if (P1[i] == 1)
                {
                    answer += Math.Pow(2, (Convert.ToDouble(P1.Length - i-1)));
                }
            }
            return answer;
        }
        public static int[] LastTwoBitP (int[] P)
        {
            
            int[] arr = new int[2];
            for (int i=0;i<arr.Length;i++)
            {
                arr[i] = P[i + P.Length - 2];
            }
            Console.WriteLine("Last two bit in array  {0}  ", String.Concat<int>(arr));
            
            return arr;

        }
        public static int[] Answer(int[]P)
        {
            int[] arr = new int[P.Length-2];
            for (int i=1;i<arr.Length;i++)
            {
                arr[i - 1] = P[i];
            }
            

            return arr ;
        }
        public static int[] Sum(int[] P, int[]C)
        {
            int[] arr = new int[C.Length];
            int temp = 0;
            int temp1 = 0;
            for(int i = C.Length - 1; i > 0;i--)
            {
                temp1 = P[i] + C[i] + temp;
                if (temp1 == 0)
                {
                    arr[i] = 0;
                    temp = 0;
                }
                else if (temp1==1)
                {
                    arr[i] = 1;
                    temp = 0;
                }
                else if (temp1 == 2)
                {
                    arr[i] = 0;
                    temp = 1;
                }
                else if (temp1 == 3)
                {
                    arr[i] = 1;
                    temp = 1;
                }

            }
            if (P[0] + C[0] == 0 || P[0] + C[0] > 1)
            {
                arr[0] = 0;
            }
            else
                arr[0] = 1;
            return arr;
        }
        public static int[] ShiftRight(int[]P)
        {
            int[] arr = new int[P.Length];
            for (int i = arr.Length-1; i > 0; i--)
            {
                arr[i] = P[i - 1];
            }
            arr[0] = P[0];
            return arr;
        }
        public static int[] AddCode(int[] a)
        {
            int[] arr = new int[a.Length];
            for (int i=0; i < arr.Length; i++)
            {
                if (a[i] == 1)
                {
                    arr[i] = 0;
                }
                else
                {
                    arr[i] = 1;
                }
            }
            int temp = 1;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i]+temp>1)
                {
                    arr[i] = 0;
                    temp = 1;
                }
                else
                if (arr[i] + temp == 1)
                {
                    arr[i] = 1;
                    temp = 0;
                }
                else
                if (arr[i] + temp == 0)
                {
                    arr[i] = 0;
                    temp = 0;
                }
            }
            return arr;
        }
        public static int[] Amethod(int bit, int[] a)
        {
            int l = 2 * bit + 2;
            int[] arr = new int[l];
            
            for (int i=0; i < l/2; i++)
            {
                arr[i] = a[i];
            }
            for (int i = l / 2; i < l; i++)
            {
                arr[i] = 0;
            }
            return arr;
        }
        public static int[] Bmethod(int bit, int[] c)
        {
            int l = 2 * bit + 2;
            int[] arr = new int[l];
            for (int i = 0; i < l / 2; i++)
            {
                arr[i] = c[i];
            }
            for (int i = l / 2; i < l; i++)
            {
                arr[i] = 0;
            }
            return arr;
        }
        public static int[] Pmethod(int bit, int[] b)
        {
            int l = 2 * bit + 2;
            int[] arr = new int[l];
            for (int i = 0; i < (l / 2)-1; i++)
            {
                arr[i] = 0;
            }
            for (int i = (l / 2)-1; i < l-1; i++)
            {
                arr[i] = b[i - l/2+1];
            }
            arr[l-1] = 0;
            return arr;

        }
        //получает обратную запись двоичного числа из дсятичного
        public static int[] Perevod(int temp,int bit)
        {
            int temp1 = 0;
            List<int> s = new List<int>();
            while (temp > 0)
            {
                temp1 = temp % 2;
                temp = temp / 2;
                s.Add(temp1);
            }
            return Obrat(s,bit);
        }
        //переворачивает число и возвращает прямую запись двоичного числа.
        public static int[] Obrat(List<int> norm,int bit)
        {
            int[] s = new int[bit+1];
            //заполнение ячеек массива числа битами полученого от листа норм
             for (int i = 0; i <=norm.Count-1; i++)
             {
                 s[s.Length - 1 - i] = norm[i];
             }
            //дозаполнение массива нулями
            for(int i=0;i<(bit+1 - norm.Count); i++)
            {
                s[i] = 0;
            }
            return s;
            //return Convert.ToInt32(string.Join<int>("", s));
        }
        // считаем количество битов для каждого числа
        public static int BitAmount(int a)
        {
            int b = 4;
            if (b >= ((int)(Math.Sqrt(a)) + 1))
            {
                return b;
            }
            else
                return ((int)(Math.Sqrt(a)) + 1);
        }
        
    }
}
