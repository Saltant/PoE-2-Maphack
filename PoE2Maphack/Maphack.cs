using Saltant.PoE2Maphack;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Timer = System.Windows.Forms.Timer;

namespace PoE2Maphack
{
    internal partial class Maphack
    {
        #region P/Invoke Methods
        /// <summary>
        /// Opens a process with specific access rights.
        /// </summary>
        /// <param name="dwDesiredAccess">The desired access to the process.</param>
        /// <param name="bInheritHandle">Whether the handle is inheritable.</param>
        /// <param name="dwProcessId">The process identifier.</param>
        /// <returns>A handle to the opened process.</returns>
        [LibraryImport("kernel32.dll", SetLastError = true)]
        private static partial IntPtr OpenProcess(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Reads data from an area of memory in a specified process.
        /// </summary>
        /// <param name="hProcess">A handle to the process with memory to be read.</param>
        /// <param name="lpBaseAddress">A pointer to the base address in the specified process.</param>
        /// <param name="lpBuffer">A buffer that receives the contents from the process memory.</param>
        /// <param name="dwSize">The number of bytes to be read.</param>
        /// <param name="lpNumberOfBytesRead">The number of bytes transferred into the buffer.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        /// <summary>
        /// Writes data to an area of memory in a specified process.
        /// </summary>
        /// <param name="hProcess">A handle to the process memory to be modified.</param>
        /// <param name="lpBaseAddress">A pointer to the base address in the specified process.</param>
        /// <param name="lpBuffer">A buffer that contains data to be written.</param>
        /// <param name="dwSize">The number of bytes to be written.</param>
        /// <param name="lpNumberOfBytesWritten">The number of bytes transferred into the process.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);

        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns>True if the function succeeds; otherwise, false.</returns>
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool CloseHandle(IntPtr hObject);
        #endregion

        #region Fields
        /// <summary>
        /// Access rights for reading the process memory.
        /// </summary>
        const int PROCESS_VM_READ = 0x0010;
        /// <summary>
        /// Access rights for writing to the process memory.
        /// </summary>
        const int PROCESS_VM_WRITE = 0x0020;
        /// <summary>
        /// Access rights for performing operations on the process memory.
        /// </summary>
        const int PROCESS_VM_OPERATION = 0x0008;
        /// <summary>
        /// The name of the game process to be manipulated.
        /// </summary>
        internal const string PROCESS_NAME = "PathOfExile";
        /// <summary>
        /// A byte pattern used to locate specific memory addresses in the game process.
        /// </summary>
        static readonly byte[] searchPattern = [0x41, 0x00, 0x00, 0x00, 0x00, 0x74, 0x00, 0x0F, 0x00, 0x00, 0xEB, 0x00, 0x41, 0x00, 0x00, 0x00, 0xBA, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x00, 0x49, 0x00, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x48, 0x00, 0x00, 0x74, 0x00, 0x4C, 0x00, 0x00, 0xEB, 0x00, 0x4C, 0x00, 0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x74, 0x00];
        /// <summary>
        /// The offset from the base address where the maphack state is stored.
        /// </summary>
        static readonly int offset = 0x3D;
        /// <summary>
        /// The original value at the memory address before modification.
        /// </summary>
        static byte originalValue = 0x00;
        /// <summary>
        /// The base address of the memory region where the pattern is found.
        /// </summary>
        static IntPtr baseAddress = IntPtr.Zero;
        /// <summary>
        /// A handle to the game process.
        /// </summary>
        static IntPtr processHandle = IntPtr.Zero;
        /// <summary>
        /// A timer used to periodically check the game process status.
        /// </summary>
        static Timer timer;
        /// <summary>
        /// An event that is triggered when the maphack state changes.
        /// </summary>
        public static event Action<MaphackState> ChangeStateEvent;
        #endregion

        #region Constructor
        /// <summary>
        /// Static constructor that subscribes to the application exit event.
        /// </summary>
        static Maphack() => Application.ApplicationExit += Application_OnApplicationExit;
        #endregion

        #region Methods
        /// <summary>
        /// Changes the maphack state and returns the result of the operation.
        /// </summary>
        /// <param name="state">The desired state of the maphack (Activate or Deactivate).</param>
        /// <returns>A <see cref="MaphackResult"/> object indicating success or failure.</returns>
        internal static MaphackResult ChangeMaphackState(MaphackState state) => new()
        {
            IsSuccess = TryMakeChanges(state, out string message),
            Message = message,
        };

        /// <summary>
        /// Starts the maphack functionality by initializing the timer and hooking keyboard events.
        /// </summary>
        internal static void Start()
        {
            if (timer == null)
            {
                KeyboardHook.PressF9Event += OnPressF9;
                timer = new Timer
                {
                    Interval = 2000
                };
                timer.Tick += Timer_Tick;
                timer.Start();
            }
        }

        /// <summary>
        /// Handles the F9 key press event to toggle the maphack state.
        /// </summary>
        static void OnPressF9()
        {
            var currentState = GetCurrentState();
            if(currentState != null)
            {
                ChangeStateEvent?.Invoke(currentState == MaphackState.Deactivate ? MaphackState.Activate : MaphackState.Deactivate);
            }
        }

        /// <summary>
        /// Handles the timer tick event to check if the game process is running.
        /// </summary>
        static void Timer_Tick(object _1, EventArgs _2)
        {
            Process[] processes = Process.GetProcessesByName(PROCESS_NAME);
            if (processes.Length == 0)
            {
                KeyboardHook.Stop();
            }
            else
            {
                timer.Stop();
                processes[0].EnableRaisingEvents = true;
                processes[0].Exited += Game_OnExited;
                KeyboardHook.Start();
            }
        }

        /// <summary>
        /// Stops the maphack functionality by stopping the timer and closing the process handle.
        /// </summary>
        static void Stop()
        {
            timer?.Stop();
            timer?.Dispose();
            if (processHandle != IntPtr.Zero)
            {
                try
                {
                    CloseHandle(processHandle);
                }
                catch (Exception) { }
                finally
                {
                    processHandle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Handles the game process exit event to restart the timer.
        /// </summary>
        static void Game_OnExited(object sender, EventArgs _)
        {
            ((Process)sender).Exited -= Game_OnExited;
            MainForm.Instance.Invoke(new MethodInvoker(() =>
            {
                timer.Start();
            }));
        }

        /// <summary>
        /// Attempts to modify the game process memory to change the maphack state.
        /// </summary>
        /// <param name="state">The desired state of the maphack (Activate or Deactivate).</param>
        /// <param name="result">A message describing the result of the operation.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        static bool TryMakeChanges(MaphackState state, out string result)
        {
            result = string.Empty;
            Process[] processes = Process.GetProcessesByName(PROCESS_NAME);
            if (processes.Length == 0)
            {
                result = "Game process not found!";
                return false;
            }

            Process gameProcess = processes[0];
            processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, gameProcess.Id);

            if (processHandle == IntPtr.Zero)
            {
                result = "Failed to open the process!";
                return false;
            }

            if (baseAddress == IntPtr.Zero)
            {
                baseAddress = FindPattern(gameProcess, processHandle, searchPattern);
                if (baseAddress == IntPtr.Zero)
                {
                    result = "AOB not found!";
                    CloseHandle(processHandle);
                    return false;
                }
            }

            IntPtr addressToModify = baseAddress + offset;

            byte[] buffer = new byte[1];
            if (ReadProcessMemory(processHandle, addressToModify, buffer, buffer.Length, out _))
            {
                if (originalValue == 0x00)
                {
                    originalValue = buffer[0];
                }
            }

            byte[] newValueBytes = [(byte)state];
            if (WriteProcessMemory(processHandle, addressToModify, newValueBytes, newValueBytes.Length, out _))
            {
                result = $"{(state == MaphackState.Activate ? "Activated" : "Deactivated")}";
            }

            CloseHandle(processHandle);

            return true;
        }

        /// <summary>
        /// Retrieves the current state of the maphack from the game process memory.
        /// </summary>
        /// <returns>The current state of the maphack, or null if the state could not be determined.</returns>
        static MaphackState? GetCurrentState()
        {
            var gameProcess = Process.GetProcessesByName(PROCESS_NAME)[0];
            processHandle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, gameProcess.Id);

            if (processHandle == IntPtr.Zero)
            {
                return null;
            }

            if (baseAddress == IntPtr.Zero)
            {
                baseAddress = FindPattern(gameProcess, processHandle, searchPattern);
                if (baseAddress == IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                    return null;
                }
            }
            byte[] buffer = new byte[1];
            IntPtr addressToModify = baseAddress + offset;
            try
            {
                if (ReadProcessMemory(processHandle, addressToModify, buffer, buffer.Length, out _))
                {
                    originalValue = buffer[0];
                }
            }
            finally
            {
                CloseHandle(processHandle);
            }

            return (MaphackState)originalValue;
        }

        /// <summary>
        /// Searches for a specific byte pattern in the game process memory.
        /// </summary>
        /// <param name="gameProcess">The game process to search in.</param>
        /// <param name="processHandle">A handle to the game process.</param>
        /// <param name="pattern">The byte pattern to search for.</param>
        /// <returns>The address where the pattern was found, or IntPtr.Zero if not found.</returns>
        static IntPtr FindPattern(Process gameProcess, IntPtr processHandle, byte[] pattern)
        {
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            foreach (ProcessModule module in gameProcess.Modules)
            {
                IntPtr baseAddress = module.BaseAddress;
                int moduleSize = module.ModuleMemorySize;

                for (int offset = 0; offset < moduleSize; offset += bufferSize)
                {
                    int bytesToRead = Math.Min(bufferSize, moduleSize - offset);
                    if (ReadProcessMemory(processHandle, baseAddress + offset, buffer, bytesToRead, out _))
                    {
                        for (int i = 0; i < bytesToRead - pattern.Length; i++)
                        {
                            bool found = true;
                            for (int j = 0; j < pattern.Length; j++)
                            {
                                if (pattern[j] != 0x00 && buffer[i + j] != pattern[j])
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                return baseAddress + offset + i;
                            }
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Handles the application exit event to clean up resources.
        /// </summary>
        static void Application_OnApplicationExit(object _1, EventArgs _2) => Stop();
        #endregion

        #region Enum
        /// <summary>
        /// Represents the possible states of the maphack.
        /// </summary>
        internal enum MaphackState : byte
        {
            Activate = 0x75,
            Deactivate = 0x74,
        }
        #endregion
    }
}