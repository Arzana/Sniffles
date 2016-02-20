#pragma once

#include <WinSock2.h>
#include "Utils.h"

#pragma comment(lib, "ws2_32.lib")

#define SIO_RCVALL _WSAIOW(IOC_VENDOR, 1)

SOCKET sniffer;
struct sockaddr_in source, dest;

int InitNet(void);