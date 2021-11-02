#include <msclr/auto_handle.h>
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::IO;
using namespace msclr;

// You can call Init() multiple times as long as you ensure that any calls to other methods finish before you call
// Init() and that you don't call any other methods while Init() is running. This is true for unit tests, so we can
// freely call Init() at the beginning of test methods.
void NotebookTempFiles::Init() {
    _lock = gcnew Object();
    _count = 0;

    auto p = System::Diagnostics::Process::GetCurrentProcess();
    _pid = p->Id;
    _filenamePrefix = p->Id.ToString() + ".";

    _path = Path::Combine(Path::GetTempPath(), "SqlNotebookTemp");
    Directory::CreateDirectory(_path);
    DeleteFilesFromThisSession();
    DeleteFilesFromPreviousSessions();
}

String^ NotebookTempFiles::GetTempFilePath(String^ extension) {
    Monitor::Enter(_lock); // If you're crashing here, it's because you didn't call Init()!
    try {
        auto filePath = Path::Combine(_path, _filenamePrefix + (++_count).ToString() + extension);
        File::WriteAllText(filePath, "");
        return filePath;
    } finally {
        Monitor::Exit(_lock);
    }
}

void NotebookTempFiles::DeleteFilesFromThisSession() {
    try {
        for each (auto filePath in Directory::GetFiles(_path)) {
            auto filename = Path::GetFileName(filePath);
            if (filename->StartsWith(_filenamePrefix)) {
                try {
                    File::Delete(filePath);
                }
                catch (...) {
                }
            }
        }
    }
    catch (...) {
    }
}

void NotebookTempFiles::DeleteFilesFromPreviousSessions() {
    // Track process IDs we've seen before so we don't call GetProcessById more than once per process.
    auto pids = gcnew HashSet<int>();

    try {
        for each (auto filePath in Directory::GetFiles(_path)) {
            auto filename = Path::GetFileName(filePath);

            // If the first part of the filename matches the PID of a running process, then skip the file for now.
            auto firstPart = filename->Split('.')[0];
            int pid = 0;
            if (int::TryParse(firstPart, pid)) {
                if (pids->Contains(pid)) {
                    // We know this PID exists. Don't delete the file.
                    continue;
                }

                try {
                    System::Diagnostics::Process::GetProcessById(pid);
                    // If we made it this far, then a process with this PID actually exists. Don't delete the file.
                    pids->Add(pid);
                    continue;
                }
                catch (...) {
                }
            }

            try {
                File::Delete(filePath);
            }
            catch (...) {
            }
        }
    }
    catch (...) {
    }
}