using System.Diagnostics;
using MoonSharp.Interpreter;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        private string sourceCode;

        private DynValue coroutine;

        public Script Script { get; private set; }

        /// <summary>
        /// Create a new lua script instance.
        /// </summary>
        /// <param name="_sourceCode">LUA source code for the script.</param>
        public LuaInterpreter(string _sourceCode)
        {
            Script = new Script();
            sourceCode = _sourceCode;
        }

        /// <summary>
        /// Runs the script assigned to this interpreter if possible and returns the result.
        /// </summary>
        /// <returns></returns>
        public DynValue Run()
        {
            var wrappedCode = @"return function() " + sourceCode +
                              @"
                                end";
            var result = Script.DoString(wrappedCode);
            coroutine = Script.CreateCoroutine(result);
            coroutine.Coroutine.AutoYieldCounter = 10;
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

        public void Update()
        {
            coroutine.Coroutine.Resume();
        }
    }
}
