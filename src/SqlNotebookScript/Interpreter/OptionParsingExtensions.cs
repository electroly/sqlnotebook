using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlNotebookScript.Interpreter;

public static class OptionParsingExtensions {
    public static IEnumerable<string> GetOptionKeys(this Ast.OptionsList optionsList) {
        if (optionsList != null) {
            return optionsList.Options.Keys.Select(x => x.ToUpper());
        } else {
            return new string[0];
        }
    }

    public static T GetOption<T>(this Ast.OptionsList optionsList, string name, ScriptRunner runner, ScriptEnv env,
    T defaultValue) {
        Ast.Expr valueExpr;
        if (optionsList != null && optionsList.Options.TryGetValue(name.ToLower(), out valueExpr)) {
            return runner.EvaluateExpr<T>(valueExpr, env);
        } else {
            return defaultValue;
        }
    }

    public static bool GetOptionBool(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
    ScriptEnv env, bool defaultValue) {
        long num = optionsList.GetOption<long>(name, runner, env, defaultValue ? 1 : 0);
        if (num != 0 && num != 1) {
            throw new Exception($"{name} must be 0 or 1.");
        } else {
            return num == 1;
        }
    }

    public static Encoding GetOptionEncoding(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
    ScriptEnv env) {
        var encodingNum = optionsList.GetOptionInt(name, runner, env, 0);
        if (encodingNum == 0) {
            return null;
        } else if (encodingNum < 0 || encodingNum > 65535) {
            throw new Exception($"{name} must be between 0 and 65535.");
        } else {
            Encoding encoding = null;
            try {
                encoding = Encoding.GetEncoding(encodingNum);
            } catch (ArgumentOutOfRangeException) {
                // invalid codepage
            } catch (ArgumentException) {
                // invalid codepage
            } catch (NotSupportedException) {
                // invalid codepage
            }

            if (encoding == null) {
                throw new Exception($"The code page {encodingNum} is invalid or is not supported on this system.");
            } else {
                return encoding;
            }
        }
    }

    public static long GetOptionLong(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
    ScriptEnv env, long defaultValue, long? minValue = null, long? maxValue = null) {
        var num = optionsList.GetOption<long>(name, runner, env, defaultValue);
        bool outOfRange =
            (minValue.HasValue && num < minValue.Value) ||
            (maxValue.HasValue && num > maxValue.Value);
        if (outOfRange) {
            if (minValue.HasValue && !maxValue.HasValue) {
                throw new Exception($"{name} must be an integer ≥ {minValue.Value}.");
            } else if (minValue.HasValue && maxValue.HasValue) {
                throw new Exception($"{name} must be an integer between {minValue.Value} and {maxValue.Value}.");
            } else if (!minValue.HasValue && maxValue.HasValue) {
                throw new Exception($"{name} must be an integer ≤ {maxValue.Value}.");
            }
        }
        return num;
    }

    public static int GetOptionInt(this Ast.OptionsList optionsList, string name, ScriptRunner runner,
    ScriptEnv env, int defaultValue, int? minValue = null, int? maxValue = null) {
        var longValue = GetOptionLong(optionsList, name, runner, env, defaultValue, minValue, maxValue);
        if (longValue < int.MinValue || longValue > int.MaxValue) {
            throw new Exception($"{name} must be a 32-bit integer.");
        } else {
            return (int)longValue;
        }
    }
}
