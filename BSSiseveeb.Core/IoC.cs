using System;

namespace BSSiseveeb.Core
{
    public class IoC
    {
        private static Func<Type, object> _resolver;
        private static object _container;

        public static T Resolve<T>()
        {
            return (T) _resolver(typeof (T));
        }

        public static void Setup(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public static void SetContainer(object container)
        {
            _container = container;
        }

        public static T GetContainer<T>()
        {
            return (T) _container;
        }
    }
}