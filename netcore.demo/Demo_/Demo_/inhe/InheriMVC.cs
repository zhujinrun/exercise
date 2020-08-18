using Demo_.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_
{
    
    public class InheriMVC : ConcAbstractMVC
    {
        public override void Action()
        {
            Console.WriteLine("权限认证");
            base.Action();
        }
    }

    public class Child: InheriMVC
    {
        public override void Action()
        {
            base.Action();
        }
    }
}
