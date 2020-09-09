using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static MVCDesign.cls2;

namespace MVCDesign
{
    class Program
    {
        
        static void Main(string[] args)
        {


            Test test = new Test();
            test.TestMethod();
        //    Controller controller = new Controller();
        //    controller.Model = new Randomizer();
        //    controller += new TraceView();
        //    controller += new EventLogView();
         //   controller.Process();

            Demo demo = new Demo();
            demo.PrintData();
            Console.Read();
        }
    }

    /// <summary>
    /// 主动模式
    /// </summary>
public class cls2
{
        public class Test
        {
            public void TestMethod()
            {
                Controller controller = new Controller();
                IModel model = new Model();
                controller.Model = model;
                controller += new TraceView();
                controller += new EventLogView();
                model[1] = 2000;
                model[3] = -100;
            }
        }
        public class ModelEventArgs:EventArgs
        {
            private string content;
            public string Context { get { return this.content; } }

            public ModelEventArgs(int[] data)
            {
                content = string.Join(",", Array.ConvertAll<int, string>(data, i => Convert.ToString(i)));
            }
        }
        public interface IModel
        {
            event EventHandler<ModelEventArgs> DataChanged;
            int this[int index] { get;set; }
        }

        public interface IView
        { 
            EventHandler<ModelEventArgs> Handler { get; }
            void Print(string data);
        }

        public class Controller
        {
            private IModel model;
            public virtual IModel Model { get { return model; } set { model = value; } }

            public static Controller operator +(Controller controller,IView view)
            {
                if (view == null) throw new ArgumentNullException("view");
                controller.Model.DataChanged += view.Handler;
                return controller;
            }

            public static Controller operator -(Controller controller, IView view)
            {
                if (view == null) throw new ArgumentNullException("view");
                controller.Model.DataChanged -= view.Handler;
                return controller;
            }
        }

        public class Model : IModel
        {
            private int[] data;
            public int this[int index]
            {
                get { return data[index]; }
                set
                {
                    this.data[index] = value;
                    if (DataChanged != null)
                        DataChanged(this, new ModelEventArgs(data));
                }

            }
            public Model()
            {
                Random random = new Random();
                data = new int[10];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = random.Next() % 1023;
                }
            }

            public event EventHandler<ModelEventArgs> DataChanged;
        }
        public abstract class ViewBase : IView
        {
            public virtual EventHandler<ModelEventArgs> Handler => this.OnDataChanged;

            private void OnDataChanged(object sender, ModelEventArgs e)
            {
                Print(e.Context);
            }

            public abstract void Print(string data);
        }

        public class EventLogView : ViewBase
        {
            public override void Print(string data)
            {
                EventLog.WriteEntry("Demo",data);
            }
        }
        public class TraceView : ViewBase
        {
            public override void Print(string data)
            {
                Trace.WriteLine(data);
            }
        }
    }
   public class cls1
    {
        #region 被动模式
        public class Randomizer : IModel
        {
            public int[] Data
            {
                get
                {
                    Random random = new Random();
                    int[] data = new int[10];
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = random.Next() % 1023;
                    }
                    return data;
                }
            }
        }
        public class TraceView : IView
        {
            public void Print(string data)
            {
                Trace.WriteLine(data);
            }
        }
        public class EventLogView : IView
        {
            public void Print(string data)
            {
                EventLog.WriteEntry("Demo", data);
            }
        }

        public interface IView
        {
            void Print(string data);
        }

        public interface IModel
        {
            int[] Data { get; }
        }

        public class Controller
        {
            private IList<IView> views = new List<IView>();

            private IModel model;
            public virtual IModel Model
            {
                get { return model; }
                set { model = value; }
            }

            public void Process()
            {
                if (views.Count == 0) return;
                string result = string.Join(",", Array.ConvertAll<int, string>(model.Data, (n) => { return Convert.ToString(n); }));

                foreach (IView view in views)
                {
                    view.Print(result);
                }
            }

            public static Controller operator +(Controller controller, IView view)
            {
                if (view == null) throw new ArgumentNullException("view");
                controller.views.Add(view);
                return controller;
            }

            public static Controller operator -(Controller controller, IView view)
            {
                if (view == null) throw new ArgumentNullException("view");
                controller.views.Remove(view);
                return controller;
            }
        }
        #endregion
    }

    public class Demo
    {
        private const int Max = 10;

        private int[] Generate()
        {
            Random random = new Random();
            int[] data = new int[Max];
            for (int i = 0; i < Max; i++)
            {
                data[i] = random.Next()%1023;
            }
            return data;
        }
        public void PrintData()
        {
            string result = string.Join(",", Array.ConvertAll<int, string>(Generate(), (n) => { return Convert.ToString(n); }));
            Trace.WriteLine(result);
            EventLog.WriteEntry("Demo", result);
        }
    }
}
