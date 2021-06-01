using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptAttribute.Delegating
{
    public delegate object UglySmartHandler(params object[] data);
    public class UglySmartDelegateInvoker
    {
        private UglySmartHandler handler;
        public UglySmartDelegateInvoker()
        {
            handler = new UglySmartHandler(Add);
        }
        public object Invoke(params object[] data) => handler(data);

        private object Add(object[] data)
        {
            if(data.Length == 2)
            {
                return Add((int)data[0], (int)data[1]);
            }else if(data.Length == 3)
            {
                return Add((int)data[0], (int)data[1], (int)data[2]);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private int Add(int x, int y) => x + y;
        private int Add(int x, int y, int z) => x + y + z;
    }
}
