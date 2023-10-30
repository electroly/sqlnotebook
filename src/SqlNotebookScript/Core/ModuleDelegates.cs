using System;
using System.Runtime.InteropServices;

namespace SqlNotebookScript.Core.ModuleDelegates;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleCreateDelegate(IntPtr db, IntPtr pAux, int argc, IntPtr argv, IntPtr ppVTab, IntPtr pzErr);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleBestIndexDelegate(IntPtr pVTab, IntPtr infoPtr);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleDestroyDelegate(IntPtr pVTab);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleOpenDelegate(IntPtr pVTab, IntPtr ppCursor);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleCloseDelegate(IntPtr pCur);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleFilterDelegate(IntPtr pCur, int idxNum, IntPtr idxStr, int argc, IntPtr argv);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleNextDelegate(IntPtr pCur);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleEofDelegate(IntPtr pCur);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleColumnDelegate(IntPtr pCur, IntPtr ctx, int n);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleRowidDelegate(IntPtr pCur, IntPtr pRowid);

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int ModuleRenameDelegate(IntPtr pVtab, IntPtr zNew);
