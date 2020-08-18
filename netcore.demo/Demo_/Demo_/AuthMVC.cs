using Demo_.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_
{
    public class AuthMVC : DeAbstractMVC
    {
        private AbstractMVC _mvc;
        public AuthMVC(AbstractMVC mvc) :base(mvc)
        {
            _mvc = mvc;
        }
        public override void Action()
        {
            Console.WriteLine("action执行前: 权限认证");
            _mvc.Action();
        }
    }



}
