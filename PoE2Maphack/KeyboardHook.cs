using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PoE2Maphack
{
    internal partial class KeyboardHook
    {
        #region P/Invoke Methods
        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain.
        /// </summary>
        /// <param name="idHook">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">A pointer to the hook procedure.</param>
        /// <param name="hMod">A handle to the DLL containing the hook procedure.</param>
        /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.</param>
        /// <returns>If successful, returns a handle to the hook procedure; otherwise, returns <see cref="IntPtr.Zero"/>.</returns>
        [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExA", SetLastError = true)]
        private static partial IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the <see cref="SetWindowsHookEx"/> function.
        /// </summary>
        /// <param name="hhk">A handle to the hook to be removed.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool UnhookWindowsHookEx(IntPtr hhk);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain.
        /// </summary>
        /// <param name="hhk">A handle to the current hook.</param>
        /// <param name="nCode">The hook code passed to the current hook procedure.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
        /// <returns>The return value of the next hook procedure in the chain.</returns>
        [LibraryImport("user32.dll", SetLastError = true)]
        private static partial IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Retrieves a module handle for the specified module.
        /// </summary>
        /// <param name="lpModuleName">The name of the module.</param>
        /// <returns>A handle to the specified module; otherwise, <see cref="IntPtr.Zero"/>.</returns>
        [LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleA", SetLastError = true, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
        private static partial IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// Delegate for the low-level keyboard hook procedure.
        /// </summary>
        /// <param name="nCode">The hook code passed to the current hook procedure.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
        /// <returns>The return value of the next hook procedure in the chain.</returns>
        delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Fields
        /// <summary>
        /// The callback method for the low-level keyboard hook.
        /// </summary>
        static readonly LowLevelKeyboardProc proc = HookCallback;
        /// <summary>
        /// Hook type for low-level keyboard events.
        /// </summary>
        const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// Windows message for key down event.
        /// </summary>
        const int WM_KEYDOWN = 0x0100;
        /// <summary>
        /// The handle to the installed hook.
        /// </summary>
        static IntPtr hookID = IntPtr.Zero;
        /// <summary>
        /// Indicates whether the keyboard hook is active.
        /// </summary>
        static bool isStarted;
        /// <summary>
        /// Event triggered when the F9 key is pressed.
        /// </summary>
        public static event Action PressF9Event;
        #endregion

        #region Constructor
        /// <summary>
        /// Static constructor that subscribes to the application exit event.
        /// </summary>
        static KeyboardHook() => Application.ApplicationExit += Application_OnApplicationExit;
        #endregion

        #region Methods
        /// <summary>
        /// Starts the keyboard hook to listen for key presses.
        /// </summary>
        internal static void Start()
        {
            if(!isStarted)
            {
                isStarted = true;
                hookID = SetHook(proc);
            }
        }

        /// <summary>
        /// Stops the keyboard hook and releases resources.
        /// </summary>
        internal static void Stop()
        {
            if (hookID == IntPtr.Zero) return;

            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
            isStarted = false;
        }

        /// <summary>
        /// The callback method invoked when a key is pressed.
        /// </summary>
        /// <param name="nCode">The hook code passed to the current hook procedure.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
        /// <returns>The return value of the next hook procedure in the chain.</returns>
        static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                if (vkCode == (int)Keys.F9)
                {
                    PressF9Event?.Invoke();
                }
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Installs the low-level keyboard hook.
        /// </summary>
        /// <param name="proc">The callback method for the hook.</param>
        /// <returns>A handle to the installed hook.</returns>
        static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule curModule = curProcess.MainModule;
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        /// <summary>
        /// Handles the application exit event to clean up resources.
        /// </summary>
        static void Application_OnApplicationExit(object _1, EventArgs _2) => Stop();
        #endregion
    }
}
