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

using System;
using System.Collections.Generic;

namespace SqlNotebookScript.Interpreter {
    public sealed class MacroProcessorException : Exception {
        public MacroProcessorException(string message) : base(message) { }
    }

    public sealed class MacroProcessor {
        private readonly INotebook _notebook;
        private readonly List<CustomMacro> _macros = new List<CustomMacro>();

        public MacroProcessor(INotebook notebook) {
            _notebook = notebook;
            LoadMacros();
        }

        public void PreprocessStmt(Ast.SqlStmt input) {
            foreach (var macro in _macros) {
                while (macro.Apply(input)) { }
            }
        }

        private void LoadMacros() {
            var types = typeof(CustomMacro).Assembly.GetExportedTypes();
            foreach (var type in types) {
                if (type.IsAbstract) {
                    continue;
                }
                var baseType = type.BaseType;
                while (baseType != null) {
                    if (baseType == typeof(CustomMacro)) {
                        var macro = (CustomMacro)Activator.CreateInstance(type);
                        macro.Notebook = _notebook;
                        _macros.Add(macro);
                        break;
                    }
                    baseType = baseType.BaseType;
                }
            }
        }
    }
}
