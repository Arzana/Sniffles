#pragma once
#include "Utils.h"

#ifdef _WIN32
#include <WinSock2.h>
#pragma comment(lib, "ws2_32.lib")

#define SIO_RCVALL _WSAIOW(IOC_VENDOR, 1)

SOCKET sniffer;

int InitWinsock(WSADATA*);
int MakePromiscious(int);
#elif __linux__
#include <sys\socket.h>
int sniffer;
#endif

struct sockaddr_in source, dest;

int InitNet(void);
int CreateSock(void);
int GetLocalHost(HOSTENT *);
int SetWebInterface(HOSTENT*, IN_ADDR*, int*);
int BindSocket(IN_ADDR*);