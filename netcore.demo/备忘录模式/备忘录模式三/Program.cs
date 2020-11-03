using System;

namespace 备忘录模式三
{
    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student();
            student.Id = 001;
            student.Name = "test001";
            student.Age = 100;
            student.Save();
            student.Show();

            student.Id = 0010;
            student.Name = "test0010";
            student.Age = 99;
            student.Show();
            student.Load();
            student.Show();
            Console.Read();

        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public void Show()
        {
            Console.WriteLine("***********************");
            Console.WriteLine($"         Id:{Id}");
            Console.WriteLine($"         Name:{Name}");
            Console.WriteLine($"         Age:{Age}");
            Console.WriteLine("***********************");
        }
        public void Save()
        {
            StudentMemento studentMemento = new StudentMemento(this.Name,this.Age);
            Caretaker.SaveStudentMemento(studentMemento);
        }
        public void Load()
        {
            StudentMemento studentMemento = Caretaker.GetStudentMemento();
            this.Age = studentMemento.Age;
            this.Name = studentMemento.Name;
        }
    }
    public class StudentMemento
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public StudentMemento(string name,int age)
        {
            Name = name;
            Age = age;
        }
    }

    public class Caretaker
    {
        private static StudentMemento _studentMemento = null;
        public static void SaveStudentMemento(StudentMemento studentMemento)
        {
            _studentMemento = studentMemento;
        }
        public static StudentMemento GetStudentMemento()
        {
            return _studentMemento;
        }
    }
}
