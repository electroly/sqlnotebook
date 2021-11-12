using System;

namespace SqlNotebookScript.Core.ModuleDelegates;

public delegate int ModuleCreateDelegate(IntPtr db, IntPtr pAux, int argc, IntPtr argv, IntPtr ppVTab,
    IntPtr pzErr);
public delegate int ModuleBestIndexDelegate(IntPtr pVTab, IntPtr infoPtr);
public delegate int ModuleDestroyDelegate(IntPtr pVTab);
public delegate int ModuleOpenDelegate(IntPtr pVTab, IntPtr ppCursor);
public delegate int ModuleCloseDelegate(IntPtr pCur);
public delegate int ModuleFilterDelegate(IntPtr pCur, int idxNum, IntPtr idxStr, int argc, IntPtr argv);
public delegate int ModuleNextDelegate(IntPtr pCur);
public delegate int ModuleEofDelegate(IntPtr pCur);
public delegate int ModuleColumnDelegate(IntPtr pCur, IntPtr ctx, int n);
public delegate int ModuleRowidDelegate(IntPtr pCur, IntPtr pRowid);
public delegate int ModuleRenameDelegate(IntPtr pVtab, IntPtr zNew);
