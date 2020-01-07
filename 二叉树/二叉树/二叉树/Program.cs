using System;

namespace 二叉树
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            int a = 0;
            double qian = 0;
            for (int i = 1; qian <= 10000; i++)
            {
                qian += ((Math.Pow(2, i)) * 10000) * (Math.Pow(0.15, i));
                a = i;
            }
            Console.WriteLine("发展层数：" + a);
        }
    }
}
