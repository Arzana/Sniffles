#pragma once

#include <WinSock2.h>
#include "Utils.h"

#define HOSTNAME_LEN 1024
#define SIO_RCVALL _WSAIOW(IOC_VENDOR, 1)

WSADATA* InitWinSock(void);
SOCKET GetSocket(void);
SOCKADDR_IN* GetSocketAddr(void);
int SetPromiscuous(SOCKET s);
int Bind(SOCKET, SOCKADDR_IN *);
int InitNet(SOCKET*);