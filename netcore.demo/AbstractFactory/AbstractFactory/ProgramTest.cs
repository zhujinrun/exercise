using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractFactory
{
    public class ProgramTest
    {
        static void _Back(string[] args)
        {
            ContinentFactory factoryA = new AmericaFactory();
            ContinentFactory factoryB = new AfricaFactory();

            AnimalWorld animaWorld1 = new AnimalWorld(factoryA);
            animaWorld1.RunFoodChain();
            AnimalWorld animaWorld2 = new AnimalWorld(factoryB);
            animaWorld2.RunFoodChain();

            Console.Read();


        }
    }

    public abstract class ContinentFactory
    {
        public abstract Herbivore CreateHerbivore();
        public abstract Carnivore CreateCarnivore();
    }

    public class AfricaFactory : ContinentFactory
    {
        public override Carnivore CreateCarnivore()
        {
            return new Lion();
        }

        public override Herbivore CreateHerbivore()
        {
            return new Wildebeest();
        }
    }

    public class AmericaFactory : ContinentFactory
    {
        public override Carnivore CreateCarnivore()
        {
            return new Wolf();
        }

        public override Herbivore CreateHerbivore()
        {
            return new Bison();
        }
    }
    public class AnimalWorld
    {
        public Herbivore _herbivore;
        public Carnivore _carnivore;
        public AnimalWorld(ContinentFactory factory)
        {
            _herbivore = factory.CreateHerbivore();
            _carnivore = factory.CreateCarnivore();
        }

        public void RunFoodChain()
        {
            _carnivore.Eat(_herbivore);
        }
    }
    public abstract class Herbivore { }
    public abstract class Carnivore
    {

        public abstract void Eat(Herbivore h);
    }

    public class Wildebeest: Herbivore
    {

    }

    public class Lion : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            Console.WriteLine(this.GetType().Name + " eats " + h.GetType().Name);
        }
    }

    public class Bison: Herbivore { }

    public class Wolf : Carnivore
    {
        public override void Eat(Herbivore h)
        {
            Console.WriteLine(this.GetType().Name + " eats " + h.GetType().Name);
        }
    }
}
