using System;
using System.IO;

namespace Tests;

public sealed class TestUtil
{
    public static string GetTestsDir()
    {
        var testsDir = typeof(ScriptTest).Assembly.Location;
        while (!Path.GetFileName(testsDir).Equals("Tests", StringComparison.OrdinalIgnoreCase))
        {
            testsDir = Path.GetDirectoryName(testsDir);
        }
        return testsDir;
    }
}
