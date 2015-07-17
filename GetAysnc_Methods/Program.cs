using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono;
using Mono.Cecil;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GetAysnc_Methods
{

    class Program
    {
        static List<string> _filelist = new List<string>();

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            string loc = (args.Length >0) ? args[0] : @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\";
            string filepath = (args.Length > 1) ? args[1] : @"c:\temp\list.txt";
            DirSearch(loc);
            _filelist.Sort();
            foreach (string fname in _filelist)
            {
                AssemblyDefinition a = null;
                try
                {
                    a = AssemblyDefinition.ReadAssembly(fname);
                }
                catch (Exception e) { continue; }
                sb.Append(string.Join(Environment.NewLine, a.GetAsyncMethods().ToArray()));
            }
            File.WriteAllText(filepath, sb.ToString());
        }

        static void DirSearch(string dir)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir, "*.dll")) { _filelist.Add(f); }
                foreach (string d in Directory.GetDirectories(dir)) { DirSearch(d); }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static class GetMethod
    {
        public static List<string> GetAsyncMethods(this AssemblyDefinition assembly)
        {
            List<string> _methodnamelist = new List<string>();
            var modules = assembly.Modules;
            try
            {
                foreach (var module in modules)
                {
                    foreach (var type in module.Types)
                    {
                        foreach (var method in type.Methods)
                        {
                            foreach (var att in method.CustomAttributes)
                            {

                                // AsyncStateMachineAttribute - This attribute is not available in all methodes - meaning there are methods with a name ending with async
                                if (att.AttributeType.Name == "AsyncStateMachineAttribute")// || method.Name.ToUpper().IndexOf("ASYNC", 0) > 0)
                                {
                                    Debug.Print(method.Attributes.ToString());
                                    Debug.Print("\t" + att.AttributeType.Name);
                                    _methodnamelist.Add("\"" + module.Assembly.FullName + "\"," + type.Name + ", " + method.Name);
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return _methodnamelist;
        }
    }
}
