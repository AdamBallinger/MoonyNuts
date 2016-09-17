using System;
using MoonSharp.Interpreter;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        private string sourceCode;
        private Script script;

        /// <summary>
        /// Create a new lua script instance.
        /// </summary>
        /// <param name="_sourceCode">LUA source code for the script.</param>
        public LuaInterpreter(string _sourceCode)
        {
            script = new Script();
            sourceCode = _sourceCode;
        }

        /// <summary>
        /// Registers a C# callback function when executing a LUA function. E.g. RegisterGlobalsCallback(Example, "ExampleLua") will call the C# Example() method
        /// when the lua function "ExampleLua" is called in a lua script.
        /// </summary>
        /// <typeparam name="T">The optional param types for the callback function</typeparam>
        /// <param name="callback">The callback method to invoke</param>
        /// <param name="luaFuncName">The LUA function name that handles executing the given callback function.</param>
        public void RegisterGlobalsCallback<T>(Action<T> callback, string luaFuncName)
        {
            script.Globals[luaFuncName] = callback;
        }

        /// <summary>
        /// Registers a custom object type for use with the LUA interpreter.
        /// </summary>
        /// <param name="_type"></param>
        public void RegisterObjectType(Type _type)
        {
            UserData.RegisterType(_type);
        }

        /// <summary>
        /// Builds the LUA script. This should be executed after all globals are set for the script.
        /// </summary>
        public void BuildScript()
        {
            script.LoadString(sourceCode);
        }

        /// <summary>
        /// Runs the script assigned to this interpreter if possible and returns the result.
        /// </summary>
        /// <returns></returns>
        public DynValue Run()
        {
            var result = script.DoString(sourceCode);
            return result;
        }

        /// <summary>
        /// Call a specific function within the interpreters attatched script with optional parameters
        /// </summary>
        /// <param name="_func"></param>
        /// <param name="_args"></param>
        /// <returns></returns>
        public DynValue Call(string _func, params object[] _args)
        {
            var result = script.Call(script.Globals[_func], _args);
            return result;
        }
    }
}
