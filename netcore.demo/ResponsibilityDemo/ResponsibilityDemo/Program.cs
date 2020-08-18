using System;

namespace ResponsibilityDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("模拟请假流程。。。。。。");

            ApplyContext context = new ApplyContext()
            {
                Id = 1,
                Name = "lzx",
                Hours = 100,
                Description = "请假打游戏",
                AuditRemark = "看着来",
                AuditResult = false
            };

            AbstractAuditor auditor = new PM() { Name = "PM" };
            auditor.SetNext(auditor);
            auditor = new Charge() { Name = "Charge" };
            auditor.SetNext(auditor);
            auditor = new Manager() { Name = "Manager" };
            auditor.SetNext(auditor);
            auditor = new Chif() { Name = "Chif" };
            auditor.SetNext(auditor);
            auditor = new CEO() { Name = "CEO" };
            auditor.SetNext(auditor);
            auditor.Audit(context);
            if (context.AuditResult)
            {
                Console.WriteLine("审批通过了,enjoy my job");
            }
            else
            {
                Console.WriteLine("审批失败");
            }
        }
    }
    public class PM : AbstractAuditor
    {
        public PM() { }
        public PM(AbstractAuditor auditor) : base(auditor) { }
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine("这里是项目经理 {0} 审批", this.Name);
            if (context.Hours <= 8)
            {
                context.AuditResult = true;
                context.AuditRemark = "enjoy your vacation !";
            }
            else
            {
                base.AuditNext(context);
            }
        }
    }
    public class Manager : AbstractAuditor
    {
        public Manager() { }
        public Manager(AbstractAuditor auditor) : base(auditor) { }
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine("这里是经理 {0} 审批", this.Name);
            if (context.Hours <= 32)
            {
                context.AuditResult = true;
                context.AuditRemark = "enjoy your vacation !";
            }
            else
            {
                base.AuditNext(context);
            }
        }
    }
    public class CEO : AbstractAuditor
    {
        public CEO() { }
        public CEO(AbstractAuditor auditor) : base(auditor) { }
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine("这里是CEO {0} 审批", this.Name);
            if(context.Hours <= 128)
            {
                context.AuditResult = true;
                context.AuditRemark = "enjoy your vacation !";
            }
            else
            {
                base.AuditNext(context);
            }
        }
    }
    public class Charge : AbstractAuditor
    {
        public Charge() { }
        public Charge(AbstractAuditor auditor) : base(auditor) { }
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine("这里是主管 {0} 审批", this.Name);
            if (context.Hours <= 16)
            {
                context.AuditResult = true;
                context.AuditRemark = "enjoy your vacation !";
            }
            else
            {
                base.AuditNext(context);
            }
        }
    }
    
    public class Chif : AbstractAuditor
    {
        public Chif() { }
        public Chif(AbstractAuditor auditor) : base(auditor) { }
        public override void Audit(ApplyContext context)
        {
            Console.WriteLine("这里是总监 {0} 审批", this.Name);
            if (context.Hours <= 64)
            {
                context.AuditResult = true;
                context.AuditRemark = "enjoy your vacation !";
            }
            else
            {
                base.AuditNext(context);
            }
        }
    }
    public abstract class AbstractAuditor
    {
        public string Name { get; set; }

        public AbstractAuditor() { }
        public abstract void Audit(ApplyContext context);

        private AbstractAuditor _NextAuditor = null;

        public void SetNext(AbstractAuditor auditor)
        {
            this._NextAuditor = auditor;
        }

        public AbstractAuditor(AbstractAuditor nextAuditor)   //此构造函数会导致第一个待审批的对象无法new出来
        {
            _NextAuditor = nextAuditor;
        }

        protected void AuditNext(ApplyContext context)
        {
            if(_NextAuditor != null)
            {
                _NextAuditor.Audit(context);
            }
        }
    }
    public class ApplyContext
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public DateTime CreateTime { get; set; }
        public bool AuditResult { get; set; }
        public string Description { get; set; }
        public string AuditRemark { get; set; }
    }
}
