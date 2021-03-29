using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Tooling.Foundation.Extensions
{
    internal static class NativeMethodsEx
    {
        public const int SET_FEATURE_ON_THREAD = 1;

        public const int SET_FEATURE_ON_PROCESS = 2;

        public const int SET_FEATURE_IN_REGISTRY = 4;

        public const int SET_FEATURE_ON_THREAD_LOCALMACHINE = 8;

        public const int SET_FEATURE_ON_THREAD_INTRANET = 16;

        public const int SET_FEATURE_ON_THREAD_TRUSTED = 32;

        public const int SET_FEATURE_ON_THREAD_INTERNET = 64;

        public const int SET_FEATURE_ON_THREAD_RESTRICTED = 128;

        public const uint TME_HOVER = 1;

        public const uint TME_LEAVE = 2;

        public const uint HOVER_DEFAULT = 4294967295;

        public const uint WS_POPUP = 2147483648;

        public const uint WS_CHILD = 1073741824;

        public const uint WS_CLIPSIBLINGS = 67108864;

        public const uint WS_CLIPCHILDREN = 33554432;

        public const uint WS_THICKFRAME = 262144;

        public const uint WS_VISIBLE = 268435456;

        public const uint WS_EX_APPWINDOW = 262144;

        public const uint WS_EX_CONTROLPARENT = 65536;

        public const uint WS_EX_LAYERED = 524288;

        public const uint WS_EX_MDICHILD = 64;

        public const uint WS_EX_TOOLWINDOW = 128;

        public const uint WS_EX_LAYOUTRTL = 4194304;

        public const uint WS_EX_TOPMOST = 8;

        public const uint WS_EX_TRANSPARENT = 32;

        public const uint MAPVK_VK_TO_VSC = 0;

        public const uint MAPVK_VSC_TO_VK = 1;

        public const uint MAPVK_VK_TO_CHAR = 2;

        public const uint MAPVK_VSC_TO_VK_EX = 3;

        public const uint MAPVK_VK_TO_VSC_EX = 4;

        public const int WM_NULL = 0;

        public const int WM_CLOSE = 16;

        public const int WM_DESTROY = 2;

        public const int WM_COMMAND = 273;

        public const int WM_SYSCOMMAND = 274;

        public const int BM_CLICK = 245;

        public const int WM_ACTIVATE = 6;

        public const int MA_ACTIVATE = 1;

        public const int WM_MOUSEACTIVATE = 33;

        public const int WM_SETTEXT = 12;

        public const int WA_INACTIVE = 0;

        public const int WA_ACTIVE = 1;

        public const int WA_CLICKACTIVE = 2;

        public const int CB_FINDSTRING = 332;

        public const int CB_SETCURSEL = 334;

        public const int CB_GETCURSEL = 327;

        public const int CBEM_GETEDITCONTROL = 1031;

        public const int SC_RESTORE = 61728;

        public const int SC_CLOSE = 61536;

        public const int SC_MAXIMIZE = 61488;

        public const int SC_MINIMIZE = 61472;

        public const int SW_HIDE = 0;

        public const int RDW_INVALIDATE = 1;

        public const int RDW_INTERNALPAINT = 2;

        public const int RDW_ERASE = 4;

        public const int RDW_VALIDATE = 8;

        public const int RDW_NOINTERNALPAINT = 16;

        public const int RDW_NOERASE = 32;

        public const int RDW_NOCHILDREN = 64;

        public const int RDW_ALLCHILDREN = 128;

        public const int RDW_UPDATENOW = 256;

        public const int RDW_ERASENOW = 512;

        public const int RDW_FRAME = 1024;

        public const int RDW_NOFRAME = 2048;

        public const int GWL_STYLE = -16;

        public const int GWL_EXSTYLE = -20;

        public const int GWL_USERDATA = -21;

        public const int GWL_ID = -12;

        public const int GWL_HWNDPARENT = -8;

        public const int GWL_HINSTANCE = -6;

        public const int GWLP_HINSTANCE = -6;

        public const uint SWP_NOSIZE = 1;

        public const uint SWP_NOMOVE = 2;

        public const uint SWP_NOZORDER = 4;

        public const uint SWP_NOREDRAW = 8;

        public const uint SWP_NOACTIVATE = 16;

        public const uint SWP_FRAMECHANGED = 32;

        public const uint SWP_SHOWWINDOW = 64;

        public const uint SWP_HIDEWINDOW = 128;

        public const uint SWP_NOCOPYBITS = 256;

        public const uint SWP_NOOWNERZORDER = 512;

        public const uint SWP_NOSENDCHANGING = 1024;

        public const int FLASHW_STOP = 0;

        public const int FLASHW_CAPTION = 1;

        public const int FLASHW_TRAY = 2;

        public const int FLASHW_ALL = 3;

        public const int FLASHW_TIMER = 4;

        public const int FLASHW_TIMERNOFG = 12;

        public const int SRCCOPY = 13369376;

        public const int CAPTUREBLT = 67108864;

        public const uint MAX_PATH = 260;

        public readonly static IntPtr HWND_TOPMOST;

        public readonly static IntPtr HWND_NOTOPMOST;

        public readonly static IntPtr HWND_TOP;

        public readonly static IntPtr HWND_BOTTOM;

        static NativeMethodsEx()
        {
            HWND_TOPMOST = new IntPtr(-1);
            HWND_NOTOPMOST = new IntPtr(-2);
            HWND_TOP = new IntPtr(0);
            HWND_BOTTOM = new IntPtr(1);
        }

      //  [DllImport("urlmon.dll", CharSet = CharSet.None, ExactSpelling = false)]
      //  internal static extern int CoInternetSetFeatureEnabled(INTERNETFEATURELIST FeatureEntry, int dwFlags, bool fEnable);

        [DllImport("fusion.dll", CharSet = CharSet.None, ExactSpelling = false)]
        internal static extern IntPtr CreateAssemblyCache(out IAssemblyCache ppAsmCache, int reserved);

        [DllImport("kernel32", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string ReadPrivateProfileString(string path, string section, string key)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", stringBuilder, 255, path);
            return stringBuilder.ToString();
        }

        [DllImport("kernel32", CharSet = CharSet.None, ExactSpelling = false)]
        internal static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        internal enum AccessibleEvents
        {
            MIN = 1,
            SystemSound = 1,
            SystemAlert = 2,
            SystemForeground = 3,
            SystemMenuStart = 4,
            SystemMenuEnd = 5,
            SystemMenuPopupStart = 6,
            SystemMenuPopupEnd = 7,
            SystemCaptureStart = 8,
            SystemCaptureEnd = 9,
            SystemMoveSizeStart = 10,
            SystemMoveSizeEnd = 11,
            SystemContextHelpStart = 12,
            SystemContextHelpEnd = 13,
            SystemDragDropStart = 14,
            SystemDragDropEnd = 15,
            SystemDialogStart = 16,
            SystemDialogEnd = 17,
            SystemScrollingStart = 18,
            SystemScrollingEnd = 19,
            SystemSwitchStart = 20,
            SystemSwitchEnd = 21,
            SystemMinimizeStart = 22,
            SystemMinimizeEnd = 23,
            Create = 32768,
            Destroy = 32769,
            Show = 32770,
            Hide = 32771,
            Reorder = 32772,
            Focus = 32773,
            Selection = 32774,
            SelectionAdd = 32775,
            SelectionRemove = 32776,
            SelectionWithin = 32777,
            StateChange = 32778,
            LocationChange = 32779,
            NameChange = 32780,
            DescriptionChange = 32781,
            ValueChange = 32782,
            ParentChange = 32783,
            HelpChange = 32784,
            DefaultActionChange = 32785,
            AcceleratorChange = 32786,
            MAX = 2147483647
        }

        internal struct ASSEMBLY_INFO
        {
            public int cbAssemblyInfo;

            public int assemblyFlags;

            public long assemblySizeInKB;

            public string currentAssemblyPath;

            public int cchBuf;
        }

        public enum ClassLongOffset
        {
            GCL_HICONSM = -34,
            GCW_ATOM = -32,
            GCL_STYLE = -26,
            GCL_WNDPROC = -24,
            GCL_CBCLSEXTRA = -20,
            GCL_CBWNDEXTRA = -18,
            GCL_HMODULE = -16,
            GCL_HICON = -14,
            GCL_HCURSOR = -12,
            GCL_HBRBACKGROUND = -10,
            GCL_MENUNAME = -8
        }

        internal struct COPYDATASTRUCT
        {
            public IntPtr dwData;

            public int cbData;

            public string lpData;
        }

        internal struct CWPRETSTRUCT
        {
            public IntPtr lResult;

            public IntPtr lParam;

            public IntPtr wParam;

            public WindowsMessages message;

            public Hwnd hwnd;
        }

        internal struct CWPSTRUCT
        {
            public IntPtr lParam;

            public IntPtr wParam;

            public WindowsMessages message;

            public Hwnd hwnd;

            public static implicit operator MSG(CWPSTRUCT source)
            {
                MSG mSG = new MSG()
                {
                    hwnd = source.hwnd,
                    message = (int)source.message,
                    wParam = source.wParam,
                    lParam = source.lParam,
                    pt_x = -1,
                    pt_y = -1,
                    time = 0
                };
                return mSG;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate bool EnumThreadProc(IntPtr hwnd, IntPtr lParam);

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        internal delegate bool EnumWindowsProc(IntPtr hwnd, int lParam);

        [Flags]
        public enum ExtendedWindowStyleFlags
        {
            WS_EX_LEFT = 0,
            WS_EX_LTRREADING = 0,
            WS_EX_RIGHTSCROLLBAR = 0,
            WS_EX_DLGMODALFRAME = 1,
            WS_EX_NOPARENTNOTIFY = 4,
            WS_EX_TOPMOST = 8,
            WS_EX_ACCEPTFILES = 16,
            WS_EX_TRANSPARENT = 32,
            WS_EX_MDICHILD = 64,
            WS_EX_TOOLWINDOW = 128,
            WS_EX_WINDOWEDGE = 256,
            WS_EX_PALETTEWINDOW = 392,
            WS_EX_CLIENTEDGE = 512,
            WS_EX_OVERLAPPEDWINDOW = 768,
            WS_EX_CONTEXTHELP = 1024,
            WS_EX_RIGHT = 4096,
            WS_EX_RTLREADING = 8192,
            WS_EX_LEFTSCROLLBAR = 16384,
            WS_EX_CONTROLPARENT = 65536,
            WS_EX_STATICEDGE = 131072,
            WS_EX_APPWINDOW = 262144,
            WS_EX_LAYERED = 524288,
            WS_EX_NOINHERITLAYOUT = 1048576,
            WS_EX_LAYOUTRTL = 4194304,
            WS_EX_COMPOSITED = 33554432,
            WS_EX_NOACTIVATE = 134217728
        }

        internal static class Gdi32
        {
            [DllImport("gdi32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int CombineRgn(IntPtr dest, IntPtr src1, IntPtr src2, int flags);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr CreateBrushIndirect(ref LOGBRUSH brush);

            [DllImport("gdi32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr CreateRectRgnIndirect(ref RECT rect);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int GetClipBox(IntPtr hDC, ref RECT rectBox);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool PatBlt(IntPtr hDC, int x, int y, int width, int height, uint flags);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

            [DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        public enum GetAncestorCode : uint
        {
            GA_PARENT = 1,
            GA_ROOT = 2,
            GA_ROOTOWNER = 3
        }

        public enum GetWindow_Cmd
        {
            GW_HWNDFIRST,
            GW_HWNDLAST,
            GW_HWNDNEXT,
            GW_HWNDPREV,
            GW_OWNER,
            GW_CHILD,
            GW_ENABLEDPOPUP
        }

        internal delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        public enum HookType
        {
            WH_JOURNALRECORD,
            WH_JOURNALPLAYBACK,
            WH_KEYBOARD,
            WH_GETMESSAGE,
            WH_CALLWNDPROC,
            WH_CBT,
            WH_SYSMSGFILTER,
            WH_MOUSE,
            WH_HARDWARE,
            WH_DEBUG,
            WH_SHELL,
            WH_FOREGROUNDIDLE,
            WH_CALLWNDPROCRET,
            WH_KEYBOARD_LL,
            WH_MOUSE_LL
        }

        [Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAssemblyCache
        {
            int Dummy1();

            int Dummy2();

            int Dummy3();

            int Dummy4();

            IntPtr QueryAssemblyInfo(int flags, string assemblyName, ref ASSEMBLY_INFO assemblyInfo);
        }

        internal static class Kernel32
        {
            public const string KERNEL32_MODULE_NAME = "kernel32.dll";

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern bool CloseHandle([In] IntPtr hObject);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint GetCurrentProcessId();

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint GetCurrentThreadId();

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern bool GetExitCodeProcess([In] IntPtr hProcess, out uint lpExitCode);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern bool GetExitCodeThread([In] IntPtr hThread, out uint lpExitCode);

            //public static Win32ErrorCode GetLastError()
            //{
            //    return (Win32ErrorCode)Marshal.GetLastWin32Error();
            //}

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr GetModuleHandle([In] string lpModuleName);

            [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool IsWow64Process([In] IntPtr processHandle, out bool wow64Process);

            [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr OpenProcess(ProcessAccessRights dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
        }

        [Flags]
        public enum KeyFlags
        {
            KF_EXTENDED = 256,
            KF_DLGMODE = 2048,
            KF_MENUMODE = 4096,
            KF_ALTDOWN = 8192,
            KF_REPEAT = 16384,
            KF_UP = 32768
        }

        [Flags]
        internal enum MouseFlags
        {
            None = 0,
            MK_LBUTTON = 1,
            MK_RBUTTON = 2,
            MK_SHIFT = 4,
            MK_CONTROL = 8,
            MK_MBUTTON = 16,
            MK_XBUTTON1 = 32,
            MK_XBUTTON2 = 64
        }

        [Flags]
        public enum ProcessAccessRights : uint
        {
            PROCESS_TERMINATE = 1,
            PROCESS_CREATE_THREAD = 2,
            PROCESS_SET_SESSIONID = 4,
            PROCESS_VM_OPERATION = 8,
            PROCESS_VM_READ = 16,
            PROCESS_VM_WRITE = 32,
            PROCESS_DUP_HANDLE = 64,
            PROCESS_CREATE_PROCESS = 128,
            PROCESS_SET_QUOTA = 256,
            PROCESS_SET_INFORMATION = 512,
            PROCESS_QUERY_INFORMATION = 1024,
            PROCESS_SUSPEND_RESUME = 2048,
            PROCESS_QUERY_LIMITED_INFORMATION = 4096,
            DELETE = 65536,
            READ_CONTROL = 131072,
            STANDARD_RIGHTS_EXECUTE = 131072,
            STANDARD_RIGHTS_READ = 131072,
            STANDARD_RIGHTS_WRITE = 131072,
            WRITE_DAC = 262144,
            WRITE_OWNER = 524288,
            STANDARD_RIGHTS_REQUIRED = 983040,
            SYNCHRONIZE = 1048576,
            STANDARD_RIGHTS_ALL = 2031616,
            PROCESS_ALL_ACCESS = 2097151,
            GENERIC_ALL = 268435456,
            GENERIC_EXECUTE = 536870912,
            GENERIC_WRITE = 1073741824,
            GENERIC_READ = 2147483648
        }

        [Flags]
        public enum QueryFullProcessImageNameFlags : uint
        {
            PROCESS_NAME_NATIVE = 1
        }

        internal struct RECT
        {
            public int Left;

            public int Top;

            public int Right;

            public int Bottom;

            internal int Height
            {
                get
                {
                    return Math.Abs(Bottom - Top);
                }
            }

            internal int Width
            {
                get
                {
                    return Math.Abs(Right - Left);
                }
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public delegate void SENDASYNCPROC(Hwnd hwnd, WindowsMessages uMsg, UIntPtr dwData, IntPtr lResult);

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0,
            SMTO_BLOCK = 1,
            SMTO_ABORTIFHUNG = 2,
            SMTO_NOTIMEOUTIFNOTHUNG = 8
        }

        [Flags]
        internal enum SetWinEventHookParameter
        {
            WINEVENT_OUTOFCONTEXT = 0,
            WINEVENT_SKIPOWNTHREAD = 1,
            WINEVENT_SKIPOWNPROCESS = 2,
            WINEVENT_INCONTEXT = 4
        }

        [Flags]
        public enum StandardAccessRights : uint
        {
            DELETE = 65536,
            READ_CONTROL = 131072,
            STANDARD_RIGHTS_EXECUTE = 131072,
            STANDARD_RIGHTS_READ = 131072,
            STANDARD_RIGHTS_WRITE = 131072,
            WRITE_DAC = 262144,
            WRITE_OWNER = 524288,
            STANDARD_RIGHTS_REQUIRED = 983040,
            SYNCHRONIZE = 1048576,
            STANDARD_RIGHTS_ALL = 2031616,
            GENERIC_ALL = 268435456,
            GENERIC_EXECUTE = 536870912,
            GENERIC_WRITE = 1073741824,
            GENERIC_READ = 2147483648
        }

        internal static class SystemWindowClasses
        {
            public const string IEFrame = "IEFrame";

            public const string DirectUIHWND = "DirectUIHWND";

            public const string CtrlNotifySink = "CtrlNotifySink";

            public const string ToolbarWindow32 = "ToolbarWindow32";

            public const string Button = "Button";

            public const string ComboBox = "ComboBox";

            public const string Edit = "Edit";

            public const string ListBox = "ListBox";

            public const string MDIClient = "MDIClient";

            public const string ScrollBar = "ScrollBar";

            public const string Static = "Static";

            public const string ComboLBox = "ComboLBox";

            public const string DDEMLEvent = "DDEMLEvent";

            public const string Message = "Message";

            public const string Menu = "#32768";

            public const string Desktop = "#32769";

            public const string Dialog = "#32770";

            public const string TaskSwitch = "#32771";

            public const string IconTitles = "#32772";
        }

        internal static class User32
        {
            public const string USER32_MODULE_NAME = "user32.dll";

            public static int MajorVersion
            {
                get
                {
                    return Environment.OSVersion.Version.Major;
                }
            }

            public static int MinorVersion
            {
                get
                {
                    return Environment.OSVersion.Version.Minor;
                }
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool AnimateWindow(IntPtr hWnd, uint dwTime, uint dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr AttachThreadInput(uint idAttach, uint idAttachTo, int fAttach);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool BlockInput(bool fBlockIt);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int BringWindowToTop(IntPtr hWnd);

            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool ClientToScreen(IntPtr hWnd, ref Point pt);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool DispatchMessage(ref MSG msg);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool DrawFocusRect(IntPtr hWnd, ref RECT rect);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

            public static IList<Hwnd> EnumChildWindows(Hwnd hWndParent)
            {
                List<Hwnd> hWNDs = new List<Hwnd>();
                EnumChildWindows(hWndParent, (IntPtr hWnd, int lParam) =>
                {
                    hWNDs.Add(hWnd);
                    return true;
                }, new IntPtr());
                return hWNDs;
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool EnumThreadWindows(int threadId, EnumThreadProc pfnEnum, IntPtr lParam);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr FindWindowEx(IntPtr parentHwnd, IntPtr childAfterHwnd, IntPtr className, IntPtr windowText);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern Hwnd GetActiveWindow();

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern Hwnd GetAncestor([In] Hwnd hwnd, [In] GetAncestorCode gaFlags);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint GetClassLong(Hwnd hWnd, ClassLongOffset nIndex);

            [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

            public static string GetClassName(IntPtr hWnd, int iTryBufferLength = 256)
            {
                StringBuilder stringBuilder = new StringBuilder(iTryBufferLength);
                
                  stringBuilder.Length = GetClassName(hWnd, stringBuilder, stringBuilder.Capacity);
                if (stringBuilder.Length <= 0)
                {
                    return null;
                }
                return stringBuilder.ToString();
            }

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern bool GetClientRect(IntPtr hWnd, [In][Out] ref RECT rect);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr GetDesktopWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr GetDlgItem(IntPtr handleToWindow, int ControlId);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern uint GetDoubleClickTime();

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr GetFocus();

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr GetKeyboardLayout(uint idThread);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int GetKeyboardState(byte[] pbKeyState);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int GetKeyNameText(int lParam, [Out] StringBuilder lpString, int nSize);

            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern short GetKeyState(int vKey);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool GetMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr GetParent(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int GetSystemMetrics(int abc);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr GetWindow(IntPtr hWnd, int cmd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr GetWindowDC(IntPtr ptr);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr GetWindowLongPtr(Hwnd hwnd, int nIndex);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int GetWindowRect(IntPtr hWnd, ref RECT lpRect);

            [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);

            public static string GetWindowText(Hwnd hWnd)
            {
                StringBuilder stringBuilder = new StringBuilder(GetWindowTextLength(hWnd) + 1);
                
                   stringBuilder.Length = GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
                if (stringBuilder.Length <= 0)
                {
                    return null;
                }
                return stringBuilder.ToString();
            }

            [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool HideCaret(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern bool IntersectRect(out RECT lprcDst, [In] ref RECT lprcSrc1, [In] ref RECT lprcSrc2);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int InvalidateRect(IntPtr hWnd, IntPtr lpRect, int bErase);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool IsGUIThread(bool bConvert);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int IsIconic(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            internal static extern bool IsRectEmpty([In] ref RECT lprc);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool IsWindow([In] IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool IsWindowEnabled(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int IsWindowVisible(IntPtr hWnd);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int IsZoomed(IntPtr hwnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr LoadCursor(IntPtr hInstance, uint cursor);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadStringW", ExactSpelling = false, SetLastError = true)]
            public static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "LoadStringW", ExactSpelling = false, SetLastError = true)]
            private static extern int LoadString(IntPtr hInstance, uint uID, out IntPtr lpBuffer, int nBufferMax = 0);

            public static string LoadString(IntPtr hInstance, uint uID)
            {
                IntPtr intPtr;
                int num = LoadString(hInstance, uID, out intPtr, 0);
                if (num == 0)
                {
                    return null;
                }
                return Marshal.PtrToStringUni(intPtr, num);
            }

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern uint MapVirtualKey(uint uCode, uint uMapType);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In][Out] ref RECT rect, int cPoints);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool PeekMessage(ref MSG msg, int hWnd, uint wFilterMin, uint wFilterMax, uint wFlag);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern uint RegisterWindowMessage(string lpString);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool ReleaseCapture();

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool ScreenToClient(IntPtr hWnd, ref POINT pt);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, WindowsMessages wMsg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, string lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, EntryPoint = "SendMessageCallbackW", ExactSpelling = false, SetLastError = true)]
            public static extern bool SendMessageCallback(Hwnd hwnd, WindowsMessages Msg, IntPtr wParam, IntPtr lParam, SENDASYNCPROC lpCallback, UIntPtr dwData);

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SetActiveWindow(IntPtr hWnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool SetCapture(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern uint SetClassLong(Hwnd hWnd, ClassLongOffset nIndex, int dwNewLong);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr SetCursor(IntPtr hCursor);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern IntPtr SetFocus(IntPtr hWnd);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern long SetWindowLong(IntPtr hWnd, int nIndex, int newLong);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int X, int Y, int Width, int Height, uint flags);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hInstance, int threadId);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern IntPtr SetWinEventHook(AccessibleEvents eventMin, AccessibleEvents eventMax, IntPtr eventHookAssemblyHandle, WinEventProc eventHookHandle, uint processId, uint threadId, SetWinEventHookParameter parameterFlags);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool ShowCaret(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int bRetValue, uint fWinINI);

            [DllImport("user32", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, ref uint lpwTransKey, int fuState);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

            //[DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            //public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

            //[DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            //public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENTS tme);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool TranslateMessage(ref MSG msg);

            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool UnhookWindowsHookEx(IntPtr idHook);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern bool UnhookWinEvent(IntPtr eventHook);

            //[DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            //public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize, IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

            [DllImport("user32.dll", CharSet = CharSet.None, ExactSpelling = false)]
            public static extern int UpdateWindow(IntPtr hWnd);

            [DllImport("User32.dll", CharSet = CharSet.Auto, ExactSpelling = false)]
            public static extern bool WaitMessage();

            [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = false, SetLastError = true)]
            public static extern IntPtr WindowFromPoint(POINT Point);
        }

        [Flags]
        public enum WindowClassStyleFlags : uint
        {
            CS_VREDRAW = 1,
            CS_HREDRAW = 2,
            CS_KEYCVTWINDOW = 4,
            CS_DBLCLKS = 8,
            CS_OWNDC = 32,
            CS_CLASSDC = 64,
            CS_PARENTDC = 128,
            CS_NOKEYCVT = 256,
            CS_NOCLOSE = 512,
            CS_SAVEBITS = 2048,
            CS_BYTEALIGNCLIENT = 4096,
            CS_BYTEALIGNWINDOW = 8192,
            CS_GLOBALCLASS = 16384,
            CS_IME = 65536
        }

        internal enum WindowsActivationsConstants
        {
            WM_INACTIVE,
            WM_ACTIVE,
            WM_CLICKACTIVE
        }

        internal enum WindowsMessages : uint
        {
            WM_NULL = 0,
            WM_CREATE = 1,
            WM_DESTROY = 2,
            WM_MOVE = 3,
            WM_SIZE = 5,
            WM_ACTIVATE = 6,
            WM_SETFOCUS = 7,
            WM_KILLFOCUS = 8,
            WM_ENABLE = 10,
            WM_SETREDRAW = 11,
            WM_SETTEXT = 12,
            WM_GETTEXT = 13,
            WM_GETTEXTLENGTH = 14,
            WM_PAINT = 15,
            WM_CLOSE = 16,
            WM_QUERYENDSESSION = 17,
            WM_QUIT = 18,
            WM_QUERYOPEN = 19,
            WM_ERASEBKGND = 20,
            WM_SYSCOLORCHANGE = 21,
            WM_ENDSESSION = 22,
            WM_SHOWWINDOW = 24,
            WM_SETTINGCHANGE = 26,
            WM_WININICHANGE = 26,
            WM_DEVMODECHANGE = 27,
            WM_ACTIVATEAPP = 28,
            WM_FONTCHANGE = 29,
            WM_TIMECHANGE = 30,
            WM_CANCELMODE = 31,
            WM_SETCURSOR = 32,
            WM_MOUSEACTIVATE = 33,
            WM_CHILDACTIVATE = 34,
            WM_QUEUESYNC = 35,
            WM_GETMINMAXINFO = 36,
            WM_PAINTICON = 38,
            WM_ICONERASEBKGND = 39,
            WM_NEXTDLGCTL = 40,
            WM_SPOOLERSTATUS = 42,
            WM_DRAWITEM = 43,
            WM_MEASUREITEM = 44,
            WM_DELETEITEM = 45,
            WM_VKEYTOITEM = 46,
            WM_CHARTOITEM = 47,
            WM_SETFONT = 48,
            WM_GETFONT = 49,
            WM_SETHOTKEY = 50,
            WM_GETHOTKEY = 51,
            WM_QUERYDRAGICON = 55,
            WM_COMPAREITEM = 57,
            WM_GETOBJECT = 61,
            WM_COMPACTING = 65,
            WM_WINDOWPOSCHANGING = 70,
            WM_WINDOWPOSCHANGED = 71,
            WM_POWER = 72,
            WM_COPYDATA = 74,
            WM_CANCELJOURNAL = 75,
            WM_NOTIFY = 78,
            WM_INPUTLANGCHANGEREQUEST = 80,
            WM_INPUTLANGCHANGE = 81,
            WM_TCARD = 82,
            WM_HELP = 83,
            WM_USERCHANGED = 84,
            WM_NOTIFYFORMAT = 85,
            WM_CONTEXTMENU = 123,
            WM_STYLECHANGING = 124,
            WM_STYLECHANGED = 125,
            WM_DISPLAYCHANGE = 126,
            WM_GETICON = 127,
            WM_SETICON = 128,
            WM_NCCREATE = 129,
            WM_NCDESTROY = 130,
            WM_NCCALCSIZE = 131,
            WM_NCHITTEST = 132,
            WM_NCPAINT = 133,
            WM_NCACTIVATE = 134,
            WM_GETDLGCODE = 135,
            WM_SYNCPAINT = 136,
            WM_NCMOUSEMOVE = 160,
            WM_NCLBUTTONDOWN = 161,
            WM_NCLBUTTONUP = 162,
            WM_NCLBUTTONDBLCLK = 163,
            WM_NCRBUTTONDOWN = 164,
            WM_NCRBUTTONUP = 165,
            WM_NCRBUTTONDBLCLK = 166,
            WM_NCMBUTTONDOWN = 167,
            WM_NCMBUTTONUP = 168,
            WM_NCMBUTTONDBLCLK = 169,
            WM_KEYDOWN = 256,
            WM_KEYFIRST = 256,
            WM_KEYUP = 257,
            WM_CHAR = 258,
            WM_DEADCHAR = 259,
            WM_SYSKEYDOWN = 260,
            WM_SYSKEYUP = 261,
            WM_SYSCHAR = 262,
            WM_SYSDEADCHAR = 263,
            WM_KEYLAST = 264,
            WM_IME_STARTCOMPOSITION = 269,
            WM_IME_ENDCOMPOSITION = 270,
            WM_IME_COMPOSITION = 271,
            WM_IME_KEYLAST = 271,
            WM_INITDIALOG = 272,
            WM_COMMAND = 273,
            WM_SYSCOMMAND = 274,
            WM_TIMER = 275,
            WM_HSCROLL = 276,
            WM_VSCROLL = 277,
            WM_INITMENU = 278,
            WM_INITMENUPOPUP = 279,
            WM_SYSTIMER = 280,
            WM_MENUSELECT = 287,
            WM_MENUCHAR = 288,
            WM_ENTERIDLE = 289,
            WM_MENURBUTTONUP = 290,
            WM_MENUDRAG = 291,
            WM_MENUGETOBJECT = 292,
            WM_UNINITMENUPOPUP = 293,
            WM_MENUCOMMAND = 294,
            WM_OPERATIONEND = 296,
            WM_QUERYUISTATE = 297,
            WM_CTLCOLORMSGBOX = 306,
            WM_CTLCOLOREDIT = 307,
            WM_CTLCOLORLISTBOX = 308,
            WM_CTLCOLORBTN = 309,
            WM_CTLCOLORDLG = 310,
            WM_CTLCOLORSCROLLBAR = 311,
            WM_CTLCOLORSTATIC = 312,
            WM_MOUSEFIRST = 512,
            WM_MOUSEMOVE = 512,
            WM_LBUTTONDOWN = 513,
            WM_LBUTTONUP = 514,
            WM_LBUTTONDBLCLK = 515,
            WM_RBUTTONDOWN = 516,
            WM_RBUTTONUP = 517,
            WM_RBUTTONDBLCLK = 518,
            WM_MBUTTONDOWN = 519,
            WM_MBUTTONUP = 520,
            WM_MBUTTONDBLCLK = 521,
            WM_MOUSELAST = 522,
            WM_MOUSEWHEEL = 522,
            WM_XBUTTONDOWN = 523,
            WM_XBUTTONUP = 524,
            WM_XBUTTONDBLCLK = 525,
            WM_PARENTNOTIFY = 528,
            WM_ENTERMENULOOP = 529,
            WM_EXITMENULOOP = 530,
            WM_NEXTMENU = 531,
            WM_SIZING = 532,
            WM_CAPTURECHANGED = 533,
            WM_MOVING = 534,
            WM_DEVICECHANGE = 537,
            WM_MDICREATE = 544,
            WM_MDIDESTROY = 545,
            WM_MDIACTIVATE = 546,
            WM_MDIRESTORE = 547,
            WM_MDINEXT = 548,
            WM_MDIMAXIMIZE = 549,
            WM_MDITILE = 550,
            WM_MDICASCADE = 551,
            WM_MDIICONARRANGE = 552,
            WM_MDIGETACTIVE = 553,
            WM_MDISETMENU = 560,
            WM_ENTERSIZEMOVE = 561,
            WM_EXITSIZEMOVE = 562,
            WM_DROPFILES = 563,
            WM_MDIREFRESHMENU = 564,
            UNKNOWN_0x247 = 583,
            WM_IME_SETCONTEXT = 641,
            WM_IME_NOTIFY = 642,
            WM_IME_CONTROL = 643,
            WM_IME_COMPOSITIONFULL = 644,
            WM_IME_SELECT = 645,
            WM_IME_CHAR = 646,
            WM_IME_REQUEST = 648,
            WM_IME_KEYDOWN = 656,
            WM_IME_KEYUP = 657,
            WM_NCMOUSEHOVER = 672,
            WM_MOUSEHOVER = 673,
            WM_NCMOUSELEAVE = 674,
            WM_MOUSELEAVE = 675,
            WM_CUT = 768,
            WM_COPY = 769,
            WM_PASTE = 770,
            WM_CLEAR = 771,
            WM_UNDO = 772,
            WM_RENDERFORMAT = 773,
            WM_RENDERALLFORMATS = 774,
            WM_DESTROYCLIPBOARD = 775,
            WM_DRAWCLIPBOARD = 776,
            WM_PAINTCLIPBOARD = 777,
            WM_VSCROLLCLIPBOARD = 778,
            WM_SIZECLIPBOARD = 779,
            WM_ASKCBFORMATNAME = 780,
            WM_CHANGECBCHAIN = 781,
            WM_HSCROLLCLIPBOARD = 782,
            WM_QUERYNEWPALETTE = 783,
            WM_PALETTEISCHANGING = 784,
            WM_PALETTECHANGED = 785,
            WM_HOTKEY = 786,
            WM_GETSYSMENU = 787,
            WM_PRINT = 791,
            WM_PRINTCLIENT = 792,
            WM_HANDHELDFIRST = 856,
            WM_HANDHELDLAST = 863,
            WM_AFXFIRST = 864,
            WM_AFXLAST = 895,
            WM_PENWINFIRST = 896,
            WM_PENWINLAST = 911,
            WM_USER = 1024,
            TTS_ROUTEDMSG = 1280,
            TTS_MOUSEMOVE = 1281,
            TTS_TOGGLEFREEZEMODE = 1282,
            TTS_TOGGLEHIGHLIGHTING = 1283,
            TTS_DISABLERECORDING = 1284,
            TTS_ENABLERECORDING = 1285,
            TTS_TOGGLERECORDING = 1286,
            WM_APP = 32768
        }

        [Flags]
        public enum WindowStyleFlags : uint
        {
            WS_OVERLAPPED = 0,
            Class_0x0001 = 1,
            Class_0x0002 = 2,
            Class_0x0004 = 4,
            Class_0x0008 = 8,
            Class_0x0010 = 16,
            Class_0x0020 = 32,
            Class_0x0040 = 64,
            Class_0x0080 = 128,
            Class_0x0100 = 256,
            Class_0x0200 = 512,
            Class_0x0400 = 1024,
            Class_0x0800 = 2048,
            Class_0x1000 = 4096,
            Class_0x2000 = 8192,
            Class_0x4000 = 16384,
            Class_0x8000 = 32768,
            WS_MAXIMIZEBOX = 65536,
            WS_TABSTOP = 65536,
            WS_GROUP = 131072,
            WS_MINIMIZEBOX = 131072,
            WC_THICKFRAME = 262144,
            WS_SIZEFRAME = 262144,
            WS_SYSMENU = 524288,
            WS_HSCROLL = 1048576,
            WS_VSCROLL = 2097152,
            WS_DLGFRAME = 4194304,
            WS_BORDER = 8388608,
            WS_CAPTION = 12582912,
            WS_OVERLAPPEDWINDOW = 13565952,
            WS_MAXIMIZE = 16777216,
            WS_CLIPCHILDREN = 33554432,
            WS_CLIPSIBLINGS = 67108864,
            WS_DISABLED = 134217728,
            WS_VISIBLE = 268435456,
            WS_MINIMIZE = 536870912,
            WS_CHILD = 1073741824,
            WS_POPUP = 2147483648,
            WS_POPUPWINDOW = 2156396544
        }

        internal delegate void WinEventProc(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);
    }
}