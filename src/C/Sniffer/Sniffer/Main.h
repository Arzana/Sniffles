#pragma once

#include "Utils.h"
#include "SockInit.h"
#include "NetHeaders.h"

#pragma comment(lib, "ws2_32.lib")

#define PACK_SIZE 65536

void ProcessPacket(octet*, int);
void ProcessUDP(octet*);
void ProcessICMP(octet*);