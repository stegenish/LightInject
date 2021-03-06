﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightInject.SampleLibrary
{



    public interface IFoo { }

    public class Foo : IFoo
    {
        public static int Instances { get; set; }
        
        public Foo()
        {
            Instances++;
        }
    }

    public class FooWithStaticDependency : IFoo
    {
        public static IBar Bar { get; set; }
    }


    public class AnotherFoo : IFoo { }

    public class FooWithDependency : IFoo
    {
        public FooWithDependency(IBar bar)
        {
            Bar = bar;
        }

        public IBar Bar { get; private set; }
    }

    public class FooWithEnumerableIFooDependency : IFoo
    {
        public IEnumerable<IFoo> FooList { get; private set; }

        public FooWithEnumerableIFooDependency(IEnumerable<IFoo> fooList)
        {
            FooList = fooList;
        }
    }
    
    public class FooWithEnumerableAndRegularDependency : IFoo
    {
        public IEnumerable<IBar> Bars { get; private set; }

        public IBar Bar { get; private set; }

        public FooWithEnumerableAndRegularDependency(IEnumerable<IBar> bars, IBar bar)
        {
            Bars = bars;
            Bar = bar;
        }
    }


    public class FooWithSameDependencyTwice : IFoo
    {
        private readonly IBar m_bar1;

        private readonly IBar m_bar2;

        public FooWithSameDependencyTwice(IBar bar1, IBar bar2)
        {
            m_bar1 = bar1;
            m_bar2 = bar2;
        }

        public IBar Bar2
        {
            get
            {
                return this.m_bar2;
            }
        }

        public IBar Bar1
        {
            get
            {
                return this.m_bar1;
            }
        }
    }
    
    public class FooWithSampleServiceDependency : IFoo
    {
        public FooWithSampleServiceDependency(IBar bar, ISampleService sampleService)
        {
            SampleService = sampleService;
            Bar = bar;
        }
        public ISampleService SampleService { get; private set; }
        public IBar Bar { get; private set; }
    }


    public class BarWithSampleServiceDependency : IBar
    {
        public BarWithSampleServiceDependency(ISampleService sampleService)
        {
            SampleService = sampleService;
        }

        public ISampleService SampleService { get; private set; }
    }

    


    public class FooWithSamePropertyDependencyTwice : IFoo
    {
        public IBar Bar1 { get; set; }

        public IBar Bar2 { get; set; }
    }


    public class FooDecorator : IFoo
    {
        private readonly IFoo foo;

        public FooDecorator(IFoo foo)
        {
            this.foo = foo;
        }

        public IFoo DecoratedInstance
        {
            get
            {
                return this.foo;
            }
        }
    }


    public class FooWithReferenceTypeDependency : IFoo
    {
        public FooWithReferenceTypeDependency(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    public class FooWithReferenceTypePropertyDependency : IFoo
    {
        public string Value { get; set; }
    }

    public class FooWithValueTypeDependency : IFoo
    {
        public FooWithValueTypeDependency(int value)
        {
            Value = value;
        }

        public int Value { get; private set; }
    }

    public class FooWithValueTypePropertyDependency : IFoo
    {
        public int Value { get; set; }
    }


    public class FooWithEnumDependency : IFoo
    {
        public FooWithEnumDependency(Encoding value)
        {
            Value = value;
        }

        public Encoding Value { get; private set; }
    }

    public class FooWithEnumPropertyDependency : IFoo
    {


        public Encoding Value { get; set; }
    }


    public class FooWithEnumerableDependency : IFoo
    {
        public FooWithEnumerableDependency(IEnumerable<IBar> bars)
        {
            Bars = bars;
        }

        public IEnumerable<IBar> Bars { get; private set; }
    }

    public class FooWithEnumerablePropertyDependency : IFoo
    {
        public FooWithEnumerablePropertyDependency()
        {            
        }

        public IEnumerable<IBar> Bars { get; set; }
    }

    public class FooWithRecursiveDependency : IFoo
    {
        public FooWithRecursiveDependency(IFoo foo)
        {
        }
    }

    public interface IFoo<T> { }

    public class Foo<T> : IFoo<T> { }

    public class AnotherFoo<T> : IFoo<T> { }

    public class FooWithGenericDependency<T> : IFoo<T>
    {        
        public FooWithGenericDependency(T dependency)
        {
            Dependency = dependency;
        }

        public T Dependency { get; private set; }        
    }

    public class FooWithOpenGenericDependency<T> : IFoo<T>
    {
        private readonly IBar<T> dependency;

        public FooWithOpenGenericDependency(IBar<T> dependency)
        {
            this.dependency = dependency;
        }

        public IBar<T> Dependency
        {
            get { return dependency; }
        }
    }

    public class FooWithSameOpenGenericDependencyTwice<T> : IFoo<T>
    {
        private readonly IBar<T> bar1;

        private readonly IBar<T> bar2;

        public FooWithSameOpenGenericDependencyTwice(IBar<T> bar1, IBar<T> bar2)
        {
            this.bar1 = bar1;
            this.bar2 = bar2;
        }

        public IBar<T> Bar1
        {
            get { return bar1; }
        }

        public IBar<T> Bar2
        {
            get { return bar2; }
        }
    }


    public class FooWithGenericPropertyDependency<T> : IFoo<T>
    {
        public T Dependency { get; set; }
    }


    public class FooWithStringTypeParameter : IFoo<string> {}
    

    public class FooWithLazyDependency : IFoo
    {
        public FooWithLazyDependency(Lazy<IBar> lazyService)
        {
            LazyService = lazyService;
        }

        public Lazy<IBar> LazyService { get; private set; }
    }




    internal class FooFactory : IFactory
    {

        public static int Instances { get; set; }

        public FooFactory()
        {
            Instances++;
        }

        public object GetInstance(ServiceRequest serviceRequest)
        {
            ServiceRequest = serviceRequest;
            CallCount++;
            ServiceName = serviceRequest.ServiceName;
            if (serviceRequest.CanProceed)
                return new FooDecorator((IFoo)serviceRequest.Proceed());

            return new Foo();


        }

        public bool CanGetInstance(Type serviceType, string serviceName)
        {
            return typeof(IFoo).IsAssignableFrom(serviceType);
        }

        public ServiceRequest ServiceRequest { get; private set; }

        public string ServiceName { get; private set; }

        public int CallCount { get; private set; }
    }

    internal class GenericFooFactory : IFactory
    {


        public object GetInstance(ServiceRequest serviceRequest)
        {
            ServiceRequest = serviceRequest;
            return serviceRequest.CanProceed ? serviceRequest.Proceed() : new AnotherFoo<int>();
        }

        public bool CanGetInstance(Type serviceType, string serviceName)
        {
            return serviceType.IsGenericType;
        }

        public ServiceRequest ServiceRequest { get; private set; }
    }


    public class FooWithMultipleConstructors : IFoo
    {
        public FooWithMultipleConstructors()
        {
        }

        public FooWithMultipleConstructors(IBar bar)
        {
            Bar = bar;
        }

        public IBar Bar { get; private set; }
    }

    public class FooWithProperyDependency : IFoo
    {
        public IBar Bar { get; set; }
    }


    public class FooWithFuncDependency : IFoo
    {
        public FooWithFuncDependency(Func<IBar> getBar)
        {
            GetBar = getBar;
        }
        public Func<IBar> GetBar { get; private set; } 
    }

    public class FooWithNamedFuncDependency : IFoo
    {
        public FooWithNamedFuncDependency(Func<string, IBar> getBar)
        {
            GetBar = getBar;
        }
        public Func<string,IBar> GetBar { get; private set; }
    }
}
