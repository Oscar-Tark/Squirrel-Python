using IronPython;
using IronPython.Compiler;
using IronPython.Modules;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace SquirrelPython
{
    public static class SquirrelPython
    {
        public static object? runPythonInLine(string inline_code, string python_result_variable)
        {
            ScriptEngine engine =  Python.CreateEngine();
            var scope = engine.CreateScope();
            engine.Execute(inline_code, scope);

            dynamic returns = scope.GetVariable(python_result_variable);
            return returns;
        }

        public static object runPythonFileNoReturn(string path)
        {
            try
            {
                System.Console.Out.WriteLine($"Running python3 script using IronPython: {path}");

                Microsoft.Scripting.Hosting.ScriptEngine pythonEngine = IronPython.Hosting.Python.CreateEngine();

                ICollection<string> searchPaths = pythonEngine.GetSearchPaths();
                //searchPaths = pythonEngine.SetSearchPaths()

                // Now modify the search paths to include the directory from
                // which we execute the script
                //searchPaths.Add("./Lib/");
                //searchPaths.Add("/usr/lib/python3/dist-packages/");
                searchPaths.Add("/usr/lib/python3/dist-packages/pyrsistent");
                //searchPaths.Add("/home/ferret/PYSTD/Lib/");

                pythonEngine.SetSearchPaths(searchPaths);

                foreach(string str in searchPaths)
                    System.Console.Out.WriteLine("Search paths: {0}", str);

                Microsoft.Scripting.Hosting.ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromFile(path);
                PythonCompilerOptions compiler_options = new PythonCompilerOptions();
                compiler_options.SkipFirstLine = true;
                
                CompiledCode c_python_bytecode = pythonScript.Compile();
                return c_python_bytecode.Execute();
            }
            catch(Exception e){ Console.WriteLine("{0} :: {1}", e.Message, e.StackTrace); }
            return null;
        }
    }
}
