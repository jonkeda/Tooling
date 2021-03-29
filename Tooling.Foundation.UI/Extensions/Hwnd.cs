using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Tooling.Foundation.Extensions
{
    [DebuggerDisplay("Handle = {_handle}, Text = {Text}, Class = {ClassName}, Id = {Id}")]
    [StructLayout(LayoutKind.Explicit)]
    public struct Hwnd
    {
        [FieldOffset(0)]
        private IntPtr _handle;

       // [FieldOffset(-1)]
        public readonly static Hwnd NULL;

       // [FieldOffset(-1)]
        public readonly static Hwnd HWND_TOP;

       // [FieldOffset(-1)]
        public readonly static Hwnd HWND_BOTTOM;

      //  [FieldOffset(-1)]
        public readonly static Hwnd HWND_TOPMOST;

      //  [FieldOffset(-1)]
        public readonly static Hwnd HWND_NOTOPMOST;

      //  [FieldOffset(-1)]
        public readonly static Hwnd HWND_MESSAGE;

      //  [FieldOffset(-1)]
        public readonly static Hwnd HWND_BROADCAST;

        //public IList<HWND> Children
        //{
        //    get
        //    {
        //        List<HWND> hWNDs = new List<HWND>();
        //        if (!NativeMethodsEx.User32.EnumChildWindows(this, (IntPtr hWnd, int lParam) => {
        //            if (this == NativeMethodsEx.User32.GetParent(hWnd))
        //            {
        //                hWNDs.Add(hWnd);
        //            }
        //            return true;
        //        }, new IntPtr()))
        //        {
        //            throw new Win32Exception();
        //        }
        //        return hWNDs;
        //    }
        //}

        public string ClassName
        {
            get
            {
                return NativeMethodsEx.User32.GetClassName(this, 256) ?? string.Empty;
            }
        }

        internal NativeMethodsEx.WindowClassStyleFlags ClassStyles
        {
            get
            {
                return (NativeMethodsEx.WindowClassStyleFlags)NativeMethodsEx.User32.GetClassLong(this, NativeMethodsEx.ClassLongOffset.GCL_STYLE);
            }
            set
            {
                NativeMethodsEx.User32.SetClassLong(this, NativeMethodsEx.ClassLongOffset.GCL_STYLE, (int)value);
            }
        }

        public IList<Hwnd> Descendants
        {
            get
            {
                IList<Hwnd> hWNDs = NativeMethodsEx.User32.EnumChildWindows(this);
                if (hWNDs == null)
                {
                    throw new Win32Exception();
                }
                return hWNDs;
            }
        }

        internal NativeMethodsEx.ExtendedWindowStyleFlags ExtendedWindowStyles
        {
            get
            {
                return (NativeMethodsEx.ExtendedWindowStyleFlags)NativeMethodsEx.User32.GetWindowLong(this, -20);
            }
            set
            {
                NativeMethodsEx.User32.SetWindowLong(this, -20, (int)value);
            }
        }

        public IntPtr HINSTANCE
        {
            get
            {
                IntPtr intPtr;
                intPtr = (IntPtr.Size != 4 ? NativeMethodsEx.User32.GetWindowLongPtr(this, -6) : (IntPtr)((ulong)NativeMethodsEx.User32.GetWindowLong(this, -6)));
                if (Marshal.GetLastWin32Error() != 0)
                {
                    throw new Win32Exception();
                }
                return intPtr;
            }
        }

        public int Id
        {
            get
            {
                return (int)NativeMethodsEx.User32.GetWindowLong(this, -12);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return NativeMethodsEx.User32.IsWindowEnabled(this);
            }
        }

        public bool IsNull
        {
            get
            {
                return _handle == IntPtr.Zero;
            }
            set
            {
                if (value)
                {
                    _handle = IntPtr.Zero;
                    return;
                }
                if (IsNull)
                {
                    throw new InvalidOperationException("Can't set a null HWND to an arbitrary non-null value.");
                }
            }
        }

        public bool IsWindow
        {
            get
            {
                return NativeMethodsEx.User32.IsWindow(this);
            }
        }

        public Hwnd Owner
        {
            get
            {
                return NativeMethodsEx.User32.GetWindow(this, 4);
            }
        }

        public Hwnd Parent
        {
            get
            {
                return NativeMethodsEx.User32.GetParent(_handle);
            }
        }

        public uint ProcessId
        {
            get
            {
                uint num;
                NativeMethodsEx.User32.GetWindowThreadProcessId(this, out num);
                return num;
            }
        }

        public Hwnd Root
        {
            get
            {
                return NativeMethodsEx.User32.GetAncestor(_handle, NativeMethodsEx.GetAncestorCode.GA_ROOT);
            }
        }

        public Hwnd RootOwner
        {
            get
            {
                return NativeMethodsEx.User32.GetAncestor(_handle, NativeMethodsEx.GetAncestorCode.GA_ROOTOWNER);
            }
        }

        public string Text
        {
            get
            {
                return NativeMethodsEx.User32.GetWindowText(this) ?? string.Empty;
            }
        }

        public uint ThreadId
        {
            get
            {
                uint num;
                return NativeMethodsEx.User32.GetWindowThreadProcessId(this, out num);
            }
        }

        public Rectangle WindowRect
        {
            get
            {
                NativeMethodsEx.RECT rECT = new NativeMethodsEx.RECT();
                if (NativeMethodsEx.User32.GetWindowRect(this, ref rECT) == 0)
                {
                    throw new Win32Exception();
                }
                return new Rectangle(rECT.Left, rECT.Top, rECT.Width, rECT.Height);
            }
            set
            {
                if (!NativeMethodsEx.User32.SetWindowPos(this, NULL, value.Left, value.Top, value.Width, value.Height, 516))
                {
                    throw new Win32Exception();
                }
            }
        }

        internal NativeMethodsEx.WindowStyleFlags WindowStyles
        {
            get
            {
                return (NativeMethodsEx.WindowStyleFlags)NativeMethodsEx.User32.GetWindowLong(this, -16);
            }
            set
            {
                NativeMethodsEx.User32.SetWindowLong(this, -16, (int)value);
            }
        }

        static Hwnd()
        {
            NULL = new Hwnd();
            HWND_TOP = 0;
            HWND_BOTTOM = 1;
            HWND_TOPMOST = -1;
            HWND_NOTOPMOST = -2;
            HWND_MESSAGE = -3;
            HWND_BROADCAST = 65535;
        }

        private Hwnd(IntPtr handle)
        {
            _handle = handle;
        }

        private Hwnd(int iHandle)
        {
            _handle = (IntPtr)iHandle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Hwnd))
            {
                return false;
            }
            return ((Hwnd)obj)._handle == _handle;
        }

        public override int GetHashCode()
        {
            return (int)_handle;
        }

        public static bool operator ==(Hwnd hwnd1, Hwnd hwnd2)
        {
            return hwnd1._handle == hwnd2._handle;
        }

        public static bool operator ==(Hwnd hwnd1, IntPtr hwnd2)
        {
            return hwnd1._handle == hwnd2;
        }

        public static bool operator ==(IntPtr hwnd1, Hwnd hwnd2)
        {
            return hwnd1 == hwnd2._handle;
        }

        public static implicit operator Int32(Hwnd hwnd)
        {
            return hwnd._handle.ToInt32();
        }

        public static implicit operator Hwnd(int hwnd)
        {
            return new Hwnd(hwnd);
        }

        public static implicit operator IntPtr(Hwnd hwnd)
        {
            return hwnd._handle;
        }

        public static implicit operator Hwnd(IntPtr hwnd)
        {
            return new Hwnd(hwnd);
        }

        //public static implicit operator Window(HWND hwnd)
        //{
        //    HwndSource hwndSource = HwndSource.FromHwnd(hwnd);
        //    if (hwndSource == null)
        //    {
        //        return null;
        //    }
        //    return hwndSource.RootVisual as Window;
        //}

        //public static implicit operator HWND(Window window)
        //{
        //    if (window == null)
        //    {
        //        return HWND.NULL;
        //    }
        //    return (new WindowInteropHelper(window)).Handle;
        //}

        public static implicit operator Control(Hwnd hwnd)
        {
            return Control.FromHandle(hwnd);
        }

        public static implicit operator Hwnd(Control control)
        {
            if (control == null)
            {
                return NULL;
            }
            return control.Handle;
        }

        public static implicit operator NativeWindow(Hwnd hwnd)
        {
            return NativeWindow.FromHandle(hwnd);
        }

        public static implicit operator Hwnd(NativeWindow control)
        {
            if (control == null)
            {
                return NULL;
            }
            return control.Handle;
        }

        //public static implicit operator AutomationElement(Hwnd hwnd)
        //{
        //    return AutomationElement.FromHandle(hwnd);
        //}

        //public static implicit operator Hwnd(AutomationElement element)
        //{
        //    if (element == null)
        //    {
        //        return Hwnd.NULL;
        //    }
        //    return new Hwnd(element.Current.NativeWindowHandle);
        //}

        public static bool operator !=(Hwnd hwnd1, Hwnd hwnd2)
        {
            return hwnd1._handle != hwnd2._handle;
        }

        public static bool operator !=(Hwnd hwnd1, IntPtr hwnd2)
        {
            return hwnd1._handle != hwnd2;
        }

        public static bool operator !=(IntPtr hwnd1, Hwnd hwnd2)
        {
            return hwnd1 != hwnd2._handle;
        }

        public int ToInt32()
        {
            return _handle.ToInt32();
        }

        public string ToString(string strFormat)
        {
            return ((int)_handle).ToString(strFormat);
        }

        public override string ToString()
        {
            return ToString("X");
        }
    }
}