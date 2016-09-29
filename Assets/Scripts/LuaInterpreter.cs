using MoonSharp.Interpreter;
using UnityEngine.Assertions.Must;

namespace Assets.Scripts
{
    public class LuaInterpreter
    {

        public static LuaInterpreter Current { get; private set; }

        private string sourceCode;

        private DynValue coroutine;

        public Script Script { get; private set; }

        /// <summary>
        /// Creates a new LUA interpreter instance.
        /// </summary>
        public static void Create()
        {
            Current = new LuaInterpreter();
            Current.Script = new Script();
        }

        /// <summary>
        /// Create a new lua script instance.
        /// </summary>
        /// <param name="_sourceCode">LUA source code for the script.</param>
        public void SetSourceCode(string _sourceCode)
        {
            Current.sourceCode = _sourceCode;
        }

        /// <summary>
        /// Runs the script assigned to this interpreter if possible and returns the result.
        /// </summary>
        /// <returns></returns>
        public DynValue Run()
        {
            var wrappedCode = @"return function() " + Current.sourceCode +
                              @"
                                end";
            var result = Current.Script.DoString(wrappedCode);
            Current.coroutine = Current.Script.CreateCoroutine(result);
            Current.coroutine.Coroutine.AutoYieldCounter = 100;
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

        /// <summary>
        /// Terminates the current LUA interpreter execution.
        /// </summary>
        public void Terminate()
        {
            if (Current.coroutine != null && Current.coroutine.Coroutine.State != CoroutineState.Dead)
            {
                // Recreate the interpreter instance to kill LUA execution
                Create();
            }
        }

        /// <summary>
        /// Resumes the LUA coroutine if it is not yet dead. This will resume the LUA script execution for things like while true loops etc. and not
        /// lock the unity thread.
        /// </summary>
        public void Resume()
        {
            if (Current.coroutine != null && Current.coroutine.Coroutine.State != CoroutineState.Dead)
            {
                Current.coroutine.Coroutine.Resume();
            }
        }
    }
}
