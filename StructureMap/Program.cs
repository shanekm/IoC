namespace StructureMap
{
    using System;
    using StartProject;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    class Program
    {
        static void Main(string[] args)
        {
            // REGISTRATION
            var container = new Container();
            // Register MasterCard conrete type to ICreditCard
            container.Configure(x => x.For<ICreditCard>().Use<MasterCard>());

            // Shopper class has ICreditCard in it's constructor
            // IoC will automatically get concrete type to MasterCard
            var shopper = container.GetInstance<Shopper>();
            shopper.Charge();

            // LIFECYCLE
            // Default lifecycle is "PerRequest"
            // Specify your own this way
            container.Configure(x => x.For<ICreditCard>().LifecycleIs(new SingletonLifecycle()));

            // FEATURES
            // TryGet => see if available => returns null if ICreditCard is NOT registered
            container.TryGetInstance<ICreditCard>();

            // Debugging => will show you what you have in container
            Console.WriteLine(container.WhatDoIHave());

            // Put registration into one registry class
            container = new Container(new MyRegistry());
        }
    }

    public class MyRegistry : Registry
    {
        // Do all the work in constructor
        public MyRegistry()
        {
            For<ICreditCard>().LifecycleIs(new SingletonLifecycle());
        }
    }
}
