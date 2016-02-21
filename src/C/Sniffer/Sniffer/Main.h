#pragma once

#include <conio.h>
#include "Utils.h"
#include "SockInit.h"
#include "NetHeaders.h"

#define PACK_SIZE 65536

void ProcessPacket(octet*, int);
void ProcessUDP(octet*, int);
void ProcessICMP(octet*, int);