// SQL Notebook
// Copyright (C) 2016 Brian Luft
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
// OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
