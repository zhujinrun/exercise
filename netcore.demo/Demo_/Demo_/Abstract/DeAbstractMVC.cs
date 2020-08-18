using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_.Abstract
{
    public abstract class DeAbstractMVC:AbstractMVC
    {
        private AbstractMVC _mvc;
        public DeAbstractMVC(AbstractMVC mvc)
        {
            _mvc = mvc;
        }
    }
}
