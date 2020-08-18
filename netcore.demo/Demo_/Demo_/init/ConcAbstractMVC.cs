using Demo_.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_
{
    public class ConcAbstractMVC : AbstractMVC
    {
        public override void Action()
        {
            Console.WriteLine("action 执行中");
        }
    }
}
