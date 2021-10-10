#include "../../ext/libmicrohttpd/mhd/src/include/microhttpd.h"
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::Text;
using namespace System::Net;
using namespace System::Net::Sockets;

static int AccessHandlerCallback(void *cls, struct MHD_Connection *connection, const char *url, const char *method,
const char *version, const char *upload_data, size_t *upload_data_size, void **con_cls) {
    HttpServer^ server = *(gcroot<HttpServer^>*)cls;
    auto e = gcnew HttpRequestEventArgs();
    e->Url = Util::Str(url);
    e->Result = gcnew array<byte>(0);

    auto searchStr = MHD_lookup_connection_value(connection, MHD_GET_ARGUMENT_KIND, "q");
    if (searchStr) {
        e->Url += "?q=" + Util::Str(searchStr);
    }

    server->SendRequestEvent(e);
    
    auto len = e->Result->Length;
    auto buf = (byte*)malloc(len);
    for (int i = 0; i < len; i++) {
        buf[i] = e->Result[i];
    }
    auto response = MHD_create_response_from_buffer(len, buf, MHD_RESPMEM_MUST_FREE);
    int resultCode = e->ResultCode != 0 ? e->ResultCode : MHD_HTTP_OK;
    MHD_add_response_header(response, "Content-Type",
        e->ContentType == HttpContentType::Css ? "text/css" :
        e->ContentType == HttpContentType::Html ? "text/html" :
        e->ContentType == HttpContentType::JavaScript ? "text/javascript" :
        e->ContentType == HttpContentType::Png ? "image/png" : 
        "text/plain");
    int ret = MHD_queue_response(connection, resultCode, response);
    MHD_destroy_response(response);
    return ret;
}

static int FindUnusedPort() {
    auto listener = gcnew TcpListener(IPAddress::Loopback, 0);
    listener->Start();
    int port = ((IPEndPoint^)listener->LocalEndpoint)->Port;
    listener->Stop();
    return port;
}

HttpServer::HttpServer(uint16_t port) {
    // port=0 means automatically select a port
    struct sockaddr_in hostAddr;
    memset(&hostAddr, 0, sizeof(hostAddr));
    hostAddr.sin_family = AF_INET;
    hostAddr.sin_addr.S_un.S_addr = htonl(INADDR_LOOPBACK);
    hostAddr.sin_port = htons(port);
    uint16_t chosenPort = port;
    _mhd = nullptr;

    do {
        if (port == 0) {
            chosenPort = (uint16_t)FindUnusedPort();
            hostAddr.sin_port = htons(chosenPort);
        }

        // a race condition exists here. another app could bind to this port before we have a chance to do so.
        // if so, MHD_start_daemon will fail and we'll try again with a different port number.

        _mhd = MHD_start_daemon(
            /* flags */ MHD_USE_THREAD_PER_CONNECTION,
            /* port */ chosenPort,
            /* apc */ NULL,
            /* apc_cls */ NULL,
            /* dh */ &AccessHandlerCallback,
            /* dh_cls */ _thisRef = new gcroot<HttpServer^>(this),
            /* options... */ MHD_OPTION_SOCK_ADDR, &hostAddr, MHD_OPTION_END
        );
    } while (!_mhd && port == 0);

    if (!_mhd) {
        throw gcnew InvalidOperationException("Unable to start the web server.");
    }

    _port = chosenPort;
}

HttpServer::~HttpServer() {
    if (_isDisposed) {
        return;
    }
    this->!HttpServer();
    _isDisposed = true;
}

HttpServer::!HttpServer() {
    if (_mhd) {
        MHD_stop_daemon((MHD_Daemon*)_mhd);
        _mhd = nullptr;
    }
    if (_thisRef) {
        delete (gcroot<HttpServer^>*)_thisRef;
        _thisRef = nullptr;
    }
}

void HttpServer::SendRequestEvent(HttpRequestEventArgs^ e) {
    Request(this, e);
}
