using System;
using System.Linq;
using System.Reflection;

namespace Write.Domain
{
    public static class EventRedirector
    {
        public static void ToWhen<T>(T instance, IAccountEvent e)
        {
            var method =
                instance.GetType()
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance) //Public
                    .Where(x => x.Name == "When")
                    .SingleOrDefault(x => MethodSupportsEvent(e, x));

            if (method == null)
            {
                throw new InvalidOperationException($"Unable to locate event handler for {e.GetType().Name} in {instance.GetType().Name}");
            }

            method.Invoke(instance, new object[] { e });
        }

        private static bool MethodSupportsEvent(IAccountEvent e, MethodInfo method)
        {
            var parameters = method.GetParameters();
            var eventType = e.GetType();

            if (parameters.Length != 1)
            {
                return false;
            }

            var parameter = parameters.Single();

            return parameter.ParameterType == eventType;
        }
    }
}