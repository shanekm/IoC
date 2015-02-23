using Castle.Windsor;
using Castle.MicroKernel.Registration;
using StartProject;

namespace CastleWindsor
{
    using Castle.MicroKernel.SubSystems.Configuration;

    class Program
    {
        static void Main(string[] args)
        {
            // REGISTRATION
            var container = new WindsorContainer();

            // For windows you need to register Shopper. Unity does this for you automatically
            //container.Register(Component.For<Shopper>());

            // Map ICreditCard to MasterCard concrete type with default 
            // name by "DefaultType"
            container.Register(Component.For<ICreditCard>().ImplementedBy<MasterCard>().Named("DefaultType"));

            // Registration with instance => instead of specifying ICreditCard resolve to concrete type
            var cc = new VisaCard();
            container.Register(Component.For<ICreditCard>().Instance(cc));

            // RESOLVE
            container.Register(Component.For<Shopper>());
            container.Register(Component.For<ICreditCard>().ImplementedBy<MasterCard>());
            var shopper = container.Resolve<Shopper>();
            shopper.Charge();

            // AUTOMATIC SETTER INJECTION
            // Windsor will automatically resolve ICreditCard property
            // and resolve it without specifying it like in Unity
            // This is enought for it to resolve
            //container.Register(Component.For<Shopper>());

            // LIFECYCLE
            // Transient => everything time shopper is created a new one is created
            container.Register(Component.For<Shopper>().LifeStyle.Transient);
            // Everytime I ask for MasterCard a new one is created
            container.Register(Component.For<ICreditCard>().ImplementedBy<MasterCard>().LifeStyle.Transient);

            // WINDSOR FEATURES
            // AddFacility => inject into Windsor engine
            //container.AddFacility()

            // Install => provides a way to put all registration in one place
            container.Install(new ShoppingInstaller());
        }


        // Install feature => lets you register everything in one place
        public class ShoppingInstaller : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<Shopper>());
                container.Register(Component.For<ICreditCard>().ImplementedBy<MasterCard>());
            }
        }


    }
}
