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

#include "../../ext/libmicrohttpd/mhd/src/include/microhttpd.h"
#include "SqlNotebookCore.h"
using namespace SqlNotebookCore;
using namespace System::Text;

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
    int ret = MHD_queue_response(connection, MHD_HTTP_OK, response);
    MHD_destroy_response(response);
    return ret;
}

HttpServer::HttpServer(uint16_t port) {
    struct sockaddr_in hostAddr;
    memset(&hostAddr, 0, sizeof(hostAddr));
    hostAddr.sin_family = AF_INET;
    hostAddr.sin_addr.S_un.S_addr = htonl(INADDR_LOOPBACK);
    hostAddr.sin_port = htons(port);

    _mhd = MHD_start_daemon(
        /* flags */ MHD_USE_THREAD_PER_CONNECTION,
        /* port */ port,
        /* apc */ NULL,
        /* apc_cls */ NULL,
        /* dh */ &AccessHandlerCallback,
        /* dh_cls */ _thisRef = new gcroot<HttpServer^>(this),
        /* options... */ MHD_OPTION_SOCK_ADDR, &hostAddr, MHD_OPTION_END
    );
    if (!_mhd) {
        throw gcnew InvalidOperationException("Unable to start the web server.");
    }
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
