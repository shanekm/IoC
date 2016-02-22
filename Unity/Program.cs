using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Moq;

namespace StructureMap
{
    using System;
    using StartProject;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    public class Program
    {
        public static Container sContainer { get; set; }
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



            sContainer = new Container();
            sContainer.Configure(x => x.For<ICreditCard>().Use<MasterCard>());


            // Put registration into one registry class
            container = new Container(new MyRegistry());

            // Testing named instance
            var classA = container.GetInstance<ClassA>();
            var classB = container.GetInstance<ClassB>();

            //var c = new StructureMap.Container(ce =>
            //{
            //    ce.ForRequestedType<ICreditCard>().TheDefaultIsConcreteType<VisaCard>();
            //});
            //var test = c.GetInstance<ICreditCard>();

            //var mock = new Mock<Container>();
            //var t1 = mock.Setup(x => x.GetInstance<ICreditCard>()).Returns(new VisaCard());

            //IoC.Initialize();
            //var ccc = IoC.container.GetInstance<ICreditCard>();

            var mock = new Mock<IContainer>();
            var tt = mock.Object;

            mock.Setup(x => x.GetInstance<ICreditCard>()).Returns(new MasterCard());
            //var tt2 = mock.Object.GetInstance<ICreditCard>();

            var worker = new Worker();
            worker.Work(mock.Object);

            var m = new Mock<HttpContextBase>();
            //var t2 = m.Setup(x => x.Request).Returns();
        }
    }
    //public static class IoC
    //{
    //    public static IContainer container;
    //    public static IContainer Initialize()
    //    {
    //        ObjectFactory.Initialize(x =>
    //        {
    //            x.For<ICreditCard>().Use<VisaCard>();
    //        });

    //        container = ObjectFactory.Container;
    //        return container;
    //    }
    //}
    public class Worker
    {
        public void Work(IContainer container)
        {
            //var c = new StructureMap.Container(ce =>
            //{
            //    ce.ForRequestedType<ICreditCard>().TheDefaultIsConcreteType<VisaCard>();
            //});


            var cc = container.GetInstance<ICreditCard>();
            var test = cc;
        }
    }

    public class MyRegistry : Registry
    {
        // Do all the work in constructor
        public MyRegistry()
        {
            For<ICreditCard>().Use<MasterCard>();
            For<ICreditCard>().LifecycleIs(new SingletonLifecycle());

            // Named instances
            For<ClassA>()
                .Use<ClassA>()
                .Ctor<IService>("service").Is<ServiceA>();

            For<ClassB>()
                .Use<ClassB>()
                .Ctor<IService>("service").Is<ServiceB>();

            //c.For<IDbConnection>().Use<SqlConnection>().Ctor<string>().Is("YOUR_CONNECTION_STRING");
            // For<IUsingInterface>().Add<UsingInterfaceImpl>().Ctor<IMyInterface>().Is(i => i.GetInstance<IMyInterface>("MyInterfaceImpl1"));
            //For<IConnector>().Add<ConnectorA>().Ctor<ConnectorA>().Is(i => i.GetInstance<ConnectorA>());
        }
    }

    public interface IService
    { }

    public class ServiceA : IService
    { }

    public class ServiceB : IService
    { }

    public class ClassA
    {
        public IService service { get; set; }
        public ICreditCard CreditCard { get; set; }

        public ClassA(IService service, ICreditCard credit)
        {
            this.service = service;
            this.CreditCard = credit;
        }
    }

    public class ClassB
    {
        public IService service { get; set; }
        public ICreditCard CreditCard { get; set; }

        public ClassB(IService service, ICreditCard credit)
        {
            this.service = service;
            this.CreditCard = credit;
        }
    }
}
