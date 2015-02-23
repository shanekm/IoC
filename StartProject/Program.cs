using System;

namespace StartProject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            // No container here => manually resolving dependencies here
            ICreditCard creditCard = new MasterCard();
            ICreditCard visaCard = new VisaCard();

            // TAKE 1
            // shopper class is only dependent on ICreditCard
            // so we can have different implementations
            //var shopper = new Shopper(creditCard);
            //shopper.Charge();
            //Console.Read();

            // TAKE 2 => creating resolver 
            // Resolver => Return concrete type
            //Resolver resolver = new Resolver();
            //var shopper = new Shopper(resolver.ResolveCreditCard());

            // TAKE 3 => container
            // Given type resolve that for us
            ResolverContainer resolverContainer = new ResolverContainer();

            // Need to register types first => whenever I ask for a type Iinterface, give me a concrete type
            resolverContainer.Register<Shopper, Shopper>();
            resolverContainer.Register<ICreditCard, VisaCard>();

            var shopper = resolverContainer.Resolve<Shopper>();
            shopper.Charge();
            Console.Read();
        }
    }

    public class MasterCard : ICreditCard
    {
        public string Charge()
        {
            return "Swiping MasterCard";
        }


        public int ChargeCount { get; set; }
    }

    public class VisaCard : ICreditCard
    {
        public string Charge()
        {
            return "Swiping Visa";
        }


        public int ChargeCount
        {
            get
            {
                return 0;
            }
        }
    }

    public class Shopper
    {
        private readonly ICreditCard creditCard;

        // Constructor Injection
        public Shopper(ICreditCard creditCard)
        {
            this.creditCard = creditCard;
        }

        public void Charge()
        {
            var chargeMessage = creditCard.Charge();
            Console.WriteLine(chargeMessage);
        }

        public int ChargesForCurrentCard { 
            get
            {
                return this.creditCard.ChargeCount;
            } 
        }
    }

    public class ShopperWithProperty
    {
        public ICreditCard CreditCard { get; set; }

        public void Charge()
        {
            var chargeMessage = CreditCard.Charge();
            Console.WriteLine(chargeMessage);
        }

        public int ChargesForCurrentCard
        {
            get
            {
                return this.CreditCard.ChargeCount;
            }
        }
    }

    public interface ICreditCard
    {
        string Charge();

        int ChargeCount { get; }
    }

    public class Resolver
    {
        // Return concrete type
        public ICreditCard ResolveCreditCard()
        {
            // Random value less than 2
            if (new Random().Next(2) == 1)
            {
                return new VisaCard();
            }

            return new MasterCard();
        }
    }

    // IoC Container
    public class ResolverContainer
    {
        // Used to store dependencies (ICreditCard, MasterCard) => ICreditCard is mapped to MasterCard
        private Dictionary<Type, Type> dependencyMap = new Dictionary<Type, Type>();

        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        private object Resolve(Type typeToResolve)
        {
            Type resolvedType = null;
            try
            {
                resolvedType = dependencyMap[typeToResolve];
            }
            catch
            {
                // Need to register first otherwise this will be thrown
                throw new Exception(string.Format("Could not resolve type {0}", typeToResolve));
            }

            // Reflection
            // Get constructor (first) for that type
            var firstConstructor = resolvedType.GetConstructors().First();

            // Get parameters for that constructor
            var constructorParameters = firstConstructor.GetParameters();

            // If no parameters (ie. MasterCard) then create concrete type MasterCard etc.
            if (constructorParameters.Count() == 0)
            {
                return Activator.CreateInstance(resolvedType);
            }

            IList<object> parameters = new List<object>();
            foreach (var parameterToResolve in constructorParameters)
            {
                parameters.Add(this.Resolve(parameterToResolve.ParameterType));
            }

            // Now create object with parameter list
            // Can't use Activator because we have parameters
            return firstConstructor.Invoke(parameters.ToArray());
        }

        public void Register<TFrom, TTo>()
        {
            // Add registrations to my dependencyMap dictionary (ICreditCard => MasterCard etc)
            dependencyMap.Add(typeof(TFrom), typeof(TTo));
        }
    }
}
