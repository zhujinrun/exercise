using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld
{

    class Program
    {
        static List<object[]> ThreeProcessPartial(List<int> lists, int len)
        {
            int count = lists.Count; //已知数组个数
            List<object[]> ints = new List<object[]>();
            //取余分组
            var arrCount = count % len != 0 ? count / len + 1 : count / len;


            object[] obj0 = new object[arrCount];
            object[] obj1 = new object[arrCount];
            object[] obj2 = new object[arrCount];
            int k = 0;
            while (count > 0)        //10 /3 =1     9/3 =0      8/3= 2   7/3=  1     6/3 =  0  
            {

                if (count % 3 == 0) //0
                {

                    obj0[k] = "test" + count;

                }
                else if (count % 3 == 1)    //1
                {
                    obj1[k] = "test" + count;
                }
                else if (count % 3 == 2) //2
                {
                    obj2[k] = "test" + count;
                    k++;
                }

                count--;
            }
            ints.Add(obj0);
            ints.Add(obj1);
            ints.Add(obj2);

            return ints;
        }


        static void Main(string[] args)
        {
            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var result = ThreeProcessPartial(list, 3);
            int i = 1;
            i = ShowResult(result, i);
            Console.Read();
        }

        private static int ShowResult(List<object[]> result, int i)
        {
            foreach (object[] item in result)
            {
                Console.WriteLine($"..........第{i}组.........");
                foreach (var sub in item)
                {
                    Console.WriteLine(sub);
                }
                i++;
            }

            return i;
        }
    }
}
