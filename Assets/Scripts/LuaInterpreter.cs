using UnityEngine;
using MoonSharp.Interpreter;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        private string sourceCode;

        public Script Script { get; private set; }

        /// <summary>
        /// Create a new lua script instance.
        /// </summary>
        /// <param name="_sourceCode">LUA source code for the script.</param>
        public LuaInterpreter(string _sourceCode)
        {
            Script = new Script();
            Script.DefaultOptions.DebugPrint = Debug.Log;
            sourceCode = _sourceCode;
        }

        /// <summary>
        /// Builds the LUA script. This should be executed after all globals are set for the script.
        /// </summary>
        public void BuildScript()
        {
            Script.LoadString(sourceCode);
        }

        /// <summary>
        /// Runs the script assigned to this interpreter if possible and returns the result.
        /// </summary>
        /// <returns></returns>
        public DynValue Run()
        {
            var result = Script.DoString(sourceCode);
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
            var result = Script.Call(Script.Globals[_func], _args);
            return result;
        }
    }
}
