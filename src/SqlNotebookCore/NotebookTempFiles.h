#pragma once
using namespace SqlNotebookScript;
using namespace System;

namespace SqlNotebookCore {
    public ref class NotebookTempFiles sealed abstract {
    public:
        static void Init();
        static String^ GetTempFilePath(String^ extension);
        static void DeleteFilesFromThisSession();

    private:
        static void DeleteFilesFromPreviousSessions();
        static int _pid;
        static Object^ _lock;
        static int _count;
        static String^ _path;
        static String^ _filenamePrefix;
    };
}
