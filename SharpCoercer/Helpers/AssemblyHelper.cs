using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharpCoercer.Helpers
{
    internal class AssemblyHelper
    {


        public static IEnumerable<Type> GetImplementationsOfInterface<T>()
        {
            try
            {
                return Assembly
              .GetExecutingAssembly()
              .GetTypes()
              .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
            }
            catch (Exception ex)
            {
                //if (Program.Debug)
                //    Console.WriteLine("[!] Error on GetInsancesOf -> {0}", ex.Message);
            }
            return Enumerable.Empty<Type>();
        }


        public static IEnumerable<T> GetInsancesOf<T>(params object[] args)
        {
            List<T> insances = new List<T>();
            foreach (var type in GetImplementationsOfInterface<T>())
            {
                try
                {
                    insances.Add((T)Activator.CreateInstance(type, args));
                }
                catch (Exception ex)
                {
                    //if (Program.Debug)
                    //    Console.WriteLine("[!] Error on GetInsancesOf -> {0}", ex.Message);
                }
            }
            return insances;
        }
    }
}
