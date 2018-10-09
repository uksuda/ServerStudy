#ifndef __SERVER_UTILS_H__
#define __SERVER_UTILS_H__

#define SOCKET_OPTION_TRUE 1
#define SOCKET_OPTION_FALSE 0

#define SOCKET_TIME_WAIT_FALSE 0

#define BUFFER_SIZE 1024

#define SAFE_DELETE(p) {if (p) { delete (p); (p) = nullptr; }}
#define SAFE_DELETE_ARRAY(p) {if (p) { delete [] (p); (p) = nullptr; }}

#endif