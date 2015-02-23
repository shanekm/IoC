namespace Unity
{
    using System;

    using Microsoft.Practices.Unity;
    using StartProject;

    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();

            // Main Usage
            // 1. RegisterType<T, TConcrete>()
            // 2. Resolve<T>()

            // REGISTRATION
            // Register MasterCard => whenever I want ICreditCard resolve it to MasterCard
            container.RegisterType<ICreditCard, MasterCard>();

            // Example - when creating MasterCard also set it's property ChargeCount
            // Give me as MasterCard and set ChargeCount property to 5
            container.RegisterType<ICreditCard, MasterCard>(
                new InjectionProperty("ChargeCount", 5));

            // From now on VisaCard will be instantiatied as DefaultCard => DefaultCard()
            container.RegisterType<ICreditCard, VisaCard>("DefaultCard");

            // Resolve to MasterCard instance
            var card = new MasterCard();
            container.RegisterInstance(card);

            // USING CONTAINER
            // Shopper doesn't need to be registered as it is NOT Interface (Unity feature)
            var shopper = container.Resolve<Shopper>();

            // Overload
            // Even though we may have registered ICreditCard to be resolved to MasterCard
            // it will be overwritten with VisaCard()
            var shopper2 = container.Resolve<Shopper>(new ParameterOverride("creditCard", new VisaCard()));

            // LIFE CYCLE OF OBJECT
            // Create a singleton - only one instance for two shoppers below
            container.RegisterType<ICreditCard, MasterCard>(new ContainerControlledLifetimeManager());
            var shopper3 = container.Resolve<Shopper>();
            
            // When shopper3 increments a charge shopper5 will see that change
            // both are using singleton ContainerControlledLifetimeManager()
            shopper3.Charge();
            Console.WriteLine(shopper3.ChargesForCurrentCard);

            var shopper5 = container.Resolve<Shopper>();
            Console.WriteLine(shopper5.ChargesForCurrentCard);

            // UNITY FEATURES
            // Intercept a creation of types inside a container
            //container.AddExtension();

            // BuildUp method() => it would inject myClass to dependent class
            var myClass = new object();
            container.BuildUp(myClass);

            // ResolveAll()
            // Resolved all in a list
        }
    }
}
