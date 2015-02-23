namespace StartProject
{
    class InjectionTypes
    {
        public interface ICreditCard
        {
            string CCNumber { get; set; }
        }

        public class MasterCard : ICreditCard
        {
            public string CCNumber { get; set; }
        }

        // Constructor Injection
        public class ConstructorInjection
        {
            // Creation of creditCard object is moved outside Shopper class() => IoC
            ICreditCard creditCard = new MasterCard();
            //Shopper shopper = new Shopper(creditCard);

            private class Shopper
            {
                private readonly ICreditCard creditCard;

                public Shopper(ICreditCard creditCard)
                {
                    this.creditCard = creditCard;
                }
            }
        }

        // Setter Injection
        // 1. Create a setter on dependent class
        // 2. Use the setter to set the dependency
        // Use whenever dependency (shopper) may or may not use depedent class (creditCard)
        public class SetterInjection
        {
            // Creation of creditCard object is moved outside Shopper class() => IoC
            ICreditCard creditCard = new MasterCard();
            Shopper shopper = new Shopper();
            //shopper.CreditCard = creditCard;

            // Setter injection happends here

            private class Shopper
            {
                public ICreditCard CreditCard { get; set; }
            }
        }

        // Interface Injection
        // dependent class implements an interface
        // injector uses the interface to set the dependency
        public class IntefaceInjection
        {
            ICreditCard creditCard = new MasterCard();
            Shopper shopper = new Shopper();
            
            // Inject creditCard object to shopper
            //((IDependentOnCreditCard)shopper).Inject(creditCard);

            public interface IDependentOnCreditCard
            {
                void Inject(ICreditCard creditCard);
            }

            private class Shopper : IDependentOnCreditCard
            {
                private ICreditCard creditCard;

                public void Inject(ICreditCard creditCard)
                {
                    this.creditCard = creditCard;
                }
            }
        }
    }
}
