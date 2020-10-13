using System;
using System.Collections.Generic;
using System.Text;

namespace TestFactory
{
    /// <summary>
    /// 声明的抽象工厂类型
    /// </summary>
    public interface IFactory
    {
        IProduct Create();
    }

    public interface IFactory<T>
    {
        T Create();
    }
}
