using BabyStroller.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Animal.Lib;

namespace BabyStroller.App {
    class Program {
        static void Main(string[] args) {
            var folder = Path.Combine(Environment.CurrentDirectory, "Animals");
            var files = Directory.GetFiles(folder);
            var animalTypes = new List<Type>();
            foreach(var file in files) {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                var types = assembly.GetTypes();
                foreach(var t in types) {
                    if (t.GetInterfaces().Contains(typeof(IAnimal))) {
                        object[] o=t.GetCustomAttributes(false);
                        var otypes = new List<Type>();
                        foreach (var i in o) otypes.Add(i.GetType());
                        var isUnfinished = otypes.Contains(typeof(UnfinishedAttribute));

                        //var isUnfinished = t.GetCustomAttributes(false).Any(a => a.GetType() == typeof(UnfinishedAttribute));
                        if (isUnfinished) continue;
                        animalTypes.Add(t);
                    }
                }
            }

            while (true) {
                for(int i = 0; i < animalTypes.Count; i++) {
                    Console.WriteLine($"{i+1}  {animalTypes[i].Name}");
                }
                Console.WriteLine("------------------");
                Console.WriteLine("please choose animal");
                int index = int.Parse(Console.ReadLine());
                if(index<1 || index > animalTypes.Count) {
                    Console.WriteLine("No such an animal");
                    continue;
                }

                int times = int.Parse(Console.ReadLine());
                Type t = animalTypes[index - 1];
                //MethodInfo m = t.GetMethod("Voice");
                object o = Activator.CreateInstance(t);
                Console.WriteLine(o.GetType().Name);
                IAnimal a = (IAnimal)o;
                a.Voice(times);
            }
        }
    }
}
