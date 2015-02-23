namespace Ninject
{
    using System;

    using Ninject.Modules;

    using StartProject;

    class Program
    {
        static void Main(string[] args)
        {
            // REGISTRATION
            var kernel = new StandardKernel();
            kernel.Bind<ICreditCard>().To<MasterCard>();

            var shopper = kernel.Get<Shopper>();
            shopper.Charge();
            Console.Read();

            // Rebind => to Visa
            kernel.Rebind<ICreditCard>().To<VisaCard>();

            // LIFECYCLE
            // Default => Transient => each copy is made
            // To make it singleton
            kernel.Bind<ICreditCard>().To<MasterCard>().InSingletonScope();

            // FEATURES
            // Configure or do something prior to returning a concrete type. Lots of power
            kernel.Bind<ICreditCard>().ToMethod(
                context =>
                    {
                        Console.WriteLine("Creating new card!");
                        return new MasterCard();
                    });

            // Put everything in one place
            var kernel2 = new StandardKernel(new MyModule());
        }
    }

    public class MyModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ICreditCard>().To<MasterCard>();
        }
    }

}
