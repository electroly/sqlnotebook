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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlNotebookCore;

namespace SqlNotebookScript {

    public static class SqlValidator {
        public struct Result {
            public readonly bool IsValid;
            public readonly int NumValidTokens;

            // if invalid
            public readonly string InvalidMessage;

            public Result(int numTokens) {
                IsValid = true;
                NumValidTokens = numTokens;
                InvalidMessage = null;
            }

            public Result(string message, int numValidTokens) {
                IsValid = false;
                NumValidTokens = numValidTokens;
                InvalidMessage = message;
            }
        }

        public static Result ReadExpr(TokenQueue q) {
            var startingLocation = q.GetLocation();
            var matchResult = Matcher.Match("expr", q);
            var numTokens = q.GetLocation() - startingLocation;
            if (matchResult.IsMatch) {
                return new Result(numTokens);
            } else {
                return new Result(matchResult.ErrorMessage ?? "Not an expression.", numTokens);
            }
        }

        public static Result ReadStmt(TokenQueue q) {
            var startingLocation = q.GetLocation();
            var matchResult = Matcher.Match("sql-stmt", q);
            var numTokens = q.GetLocation() - startingLocation;
            if (matchResult.IsMatch) {
                return new Result(numTokens);
            } else {
                return new Result(matchResult.ErrorMessage ?? "Not a statement.", numTokens);
            }
        }
    }
}
