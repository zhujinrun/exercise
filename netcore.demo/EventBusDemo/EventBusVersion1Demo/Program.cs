using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace EventBusVersion1Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Collections.Generic.List`1[T]
            Console.WriteLine(typeof(List<>));
            var jeff = new FishingMan("姜太公");
            var fishingRod = new FishingRod(); //鱼竿
            jeff.FishingRod = fishingRod; //鱼竿分配
            //注册观察者
            //fishingRod.FishingEvent += jeff.Update;
            //fishingRod.FishingEvent += new FishingEventHandler().HandleEvent;
            //循环钓鱼
            while (jeff.FishCount < 5)
            {
                jeff.Fishing();
                Console.WriteLine("----------------------");
                Thread.Sleep(5000);
            }
            Console.ReadLine();
        }
    }

    /// <summary>
    /// 定义事件源接口,所有事件源都要实现该接口
    /// </summary>
    public interface IEventData
    {
        DateTime EventTime { get; set; }
        object EventSource { get; set; } //出发事件的对象
    }

    /// <summary>
    /// 事件源实现
    /// </summary>
    public class EventData : IEventData
    {
        public DateTime EventTime { get ; set ; }
        public object EventSource { get; set ; }

        public EventData()
        {
            EventTime = DateTime.Now;
        }
    }

    /// <summary>
    /// 扩展时间源
    /// </summary>
    public class FishingEventData : EventData
    {
        public FishType FishType { get; set; }
        public FishingMan FishingMan { get; set; }
    }

    /// <summary>
    /// 定义事件处理器公共接口，所有事件处理都要实现该接口
    /// </summary>
    public interface IEventHandler
    {

    }
    
    public interface IEventHandler<TEventData>:IEventHandler where TEventData : IEventData
    {
        void HandleEvent(TEventData eventData);
    }
    public enum FishType
    {
        鲫鱼,
        鲤鱼,
        黑鱼,
        青鱼,
        草鱼,
        鲈鱼
    }

    /// <summary>
    /// 姜太公
    /// </summary>
    public class FishingMan: IEventHandler<IEventData>
    {
        public string Name { get; set; }
        public int FishCount { get; set; }
        public FishingMan(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 鱼竿
        /// </summary>
        public FishingRod FishingRod { get; set; }

        /// <summary>
        /// 垂钓
        /// </summary>
        public void Fishing()
        {
            this.FishingRod.ThrowHook(this);
        }

        ///// <summary>
        ///// 更新状态
        ///// </summary>
        ///// <param name="fishType"></param>
        //public void Update(FishType fishType)
        //{
        //    FishCount++;
        //    Console.WriteLine("{0} : 钓到一条[{2}],已经都钓到{1}条鱼了!", Name, FishCount, fishType);
        //}

        public void HandleEvent(IEventData eventData)
        {
            if (eventData is FishingEventData)
            {

            }
            else
            {

            }
        }
    }
    public class FishingEventHandler : IEventHandler<FishingEventData>
    {
        public void HandleEvent(FishingEventData eventData)
        {
            eventData.FishingMan.FishCount++;
            Console.WriteLine("{0}：钓到一条[{2}]，已经钓到{1}条鱼了！", eventData.FishingMan.Name, eventData.FishingMan.FishCount, eventData.FishType);
        }
    }
    public class FishingRod
    {
        public FishingRod()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IEventHandler).IsAssignableFrom(type) && type.Name == "FishingMan")
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1"); //本身的接口会导致报错，需要继承类才行
                    Type eventDataType = handlerInterface.GetGenericArguments()[0];
                    if (eventDataType.Equals(typeof(FishingEventData)))
                    {
                        var handler = Activator.CreateInstance(type) as IEventHandler<FishingEventData>;
                        FishingEvent += handler.HandleEvent;
                             
                    }
                }
            }
        }
        public delegate void FishingHandler(FishingEventData eventData);
        public event FishingHandler FishingEvent;

        /// <summary>
        /// 钓竿工作->钓鱼
        /// </summary>
        /// <param name="man"></param>
        public void ThrowHook(FishingMan man)
        {
            Console.WriteLine("开始下钩! ");

            if (new Random().Next() % 2 == 0)
            {
                var type = (FishType)new Random().Next(0, 5);
                Console.WriteLine("铃铛：叮叮叮，鱼儿咬钩了");
                if (FishingEvent != null)
               
                {
                    var eventData = new FishingEventData { FishType = type, FishingMan = man, EventTime = DateTime.Now, EventSource = "垂钓" };
                    EventBus.Default.Trigger<FishingEventData>(eventData);
                }
            }
        }
    }

    public class EventBus
    {
        public static EventBus Default => new EventBus();
        private readonly ConcurrentDictionary<Type, List<Type>> _eventAndHandlerMapping;
        public EventBus()
        {
            _eventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();
            MapEventToHandler();
        }

        private void MapEventToHandler()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IEventHandler).IsAssignableFrom(type))
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1");
                    if (handlerInterface != null)
                    {
                        Type eventDataType = handlerInterface.GetGenericArguments()[0];
                        if (_eventAndHandlerMapping.ContainsKey(eventDataType))
                            {
                            List<Type> handlerTypes = _eventAndHandlerMapping[eventDataType];
                            handlerTypes.Add(type);
                            _eventAndHandlerMapping[eventDataType] = handlerTypes;
                        }
                        else
                        {
                            var handlerTypes = new List<Type> { type };
                            _eventAndHandlerMapping[eventDataType] = handlerTypes;
                        }
                    }
                }
            }
        }

        public void Register<EventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _eventAndHandlerMapping[typeof(EventData)];
            if (!handlerTypes.Contains(eventHandler))
            {
                handlerTypes.Add(eventHandler);
                _eventAndHandlerMapping[typeof(EventData)] = handlerTypes; 
            }
        }
         
        public void UnRegister<TEventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _eventAndHandlerMapping[typeof(TEventData)];
            if (handlerTypes.Contains(eventHandler))
            {
                handlerTypes.Remove(eventHandler);
                _eventAndHandlerMapping[typeof(TEventData)] = handlerTypes;
            }
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            List<Type> handlers = _eventAndHandlerMapping[eventData.GetType()];
            if(handlers!=null && handlers.Count > 0)
            {
                foreach (var handler in handlers)
                {
                    MethodInfo methodInfo = handler.GetMethod("HandleEvent");
                    if (methodInfo != null)
                    {
                        object obj = Activator.CreateInstance(handler);
                        methodInfo.Invoke(obj, new object[] { eventData });
                    }
                }
            }
        }
    }
}
