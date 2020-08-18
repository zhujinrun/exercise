using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace BuilderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Dector dector = new Dector();
            Build build1 = new BuildProduct1();
            Build build2 = new BuildProduct2();
            
            Project project = build1.GetProduct();
            dector.Construct(build1);
            project.Show();
            dector.Construct(build2);

            project = build2.GetProduct();
            project.Show();
            Console.Read();
        }
    }

    public class Dector
    {
        public void  Construct(Build build)
        {
            build.BuidlPartA();
            build.BuidlPartB();
        }

    }

    public abstract class Build
    {
        public abstract void BuidlPartA();
        public abstract void BuidlPartB();
        public abstract Project GetProduct();
    }

    public class BuildProduct1 : Build
    {
        private Project project = new Project();
        public override void BuidlPartA()
        {
            project.Add("PartX");
        }

        public override void BuidlPartB()
        {
            project.Add("PartY");
        }

        public override Project GetProduct()
        {
            return project;
        }
    }

    public class BuildProduct2 : Build
    {
        private Project project = new Project();
        public override void BuidlPartA()
        {
            project.Add("PartM");
        }

        public override void BuidlPartB()
        {
            project.Add("PartN");
        }

        public override Project GetProduct()
        {
            return project;
        }
    }
    public class Project
    {
        public ArrayList parts = new ArrayList();

        public void Add(string part)
        {
            parts.Add(part);
        }
        public void Show()
        {
            foreach (var item in parts)
            {
                Console.WriteLine("产品构件中...");
                Console.WriteLine(item);
            }
        }
    }
}
