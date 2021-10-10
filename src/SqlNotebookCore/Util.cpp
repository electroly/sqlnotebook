#include <Windows.h>
#include "SqlNotebookCore.h"
using namespace System::Runtime::InteropServices;
using namespace SqlNotebookCore;

// .NET string -> UTF-16 C++ string
std::wstring Util::WStr(String^ mstr) {
    auto utf16 = Marshal::StringToHGlobalUni(mstr);
    std::wstring wstr((const wchar_t*)utf16.ToPointer());
    Marshal::FreeHGlobal(utf16);
    return wstr;
}

// .NET string -> UTF-8 C++ string
std::string Util::CStr(String^ mstr) {
    auto utf16 = Marshal::StringToHGlobalUni(mstr);
    auto utf16Str = (const wchar_t*)utf16.ToPointer();

    int bufferSizeBytes = WideCharToMultiByte(CP_UTF8, 0, utf16Str, -1, nullptr, 0, nullptr, nullptr);
    if (bufferSizeBytes <= 0) {
        Marshal::FreeHGlobal(utf16);
        throw gcnew InvalidOperationException("Unable to convert string from UTF-16 to UTF-8.");
    }
    char* buffer = new char[bufferSizeBytes];
    if (WideCharToMultiByte(CP_UTF8, 0, utf16Str, -1, buffer, bufferSizeBytes, nullptr, nullptr) <= 0) {
        Marshal::FreeHGlobal(utf16);
        delete buffer;
        throw gcnew InvalidOperationException("Unable to convert string from UTF-16 to UTF-8.");
    }
    Marshal::FreeHGlobal(utf16);
    std::string cstr(buffer);
    delete buffer;
    return cstr;
}

// UTF-16 C string -> .NET string
String^ Util::Str(const wchar_t* utf16Str) {
    return Marshal::PtrToStringUni(IntPtr((void*)utf16Str));
}

// UTF-8 C string -> .NET string
String^ Util::Str(const char* utf8Str) {
    int nDataLen = (int)strlen(utf8Str) + 1;
    int bufferSizeChars = MultiByteToWideChar(CP_UTF8, 0, utf8Str, nDataLen, nullptr, 0);
    if (bufferSizeChars <= 0) {
        throw gcnew InvalidOperationException("Unable to convert string from UTF-8 to UTF-16.");
    }
    wchar_t* buffer = new wchar_t[bufferSizeChars];
    if (MultiByteToWideChar(CP_UTF8, 0, utf8Str, nDataLen, buffer, bufferSizeChars) <= 0) {
        delete buffer;
        throw gcnew InvalidOperationException("Unable to convert string from UTF-8 to UTF-16.");
    }
    auto mstr = Marshal::PtrToStringUni(IntPtr(buffer));
    delete buffer;
    return mstr;
}
