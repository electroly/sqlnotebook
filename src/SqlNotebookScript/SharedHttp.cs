using System.Net.Http;

namespace SqlNotebookScript;

public static class SharedHttp {
    public static HttpClient Client { get; } = new();
}
