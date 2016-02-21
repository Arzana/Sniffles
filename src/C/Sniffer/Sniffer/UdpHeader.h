#pragma once

#include "Utils.h"

typedef struct UdpHdr
{
	wyde srcPort;
	wyde destPort;
	wyde octetLen;
	wyde checksum;
} UDP_HDR;

void WriteUdpHdr(UDP_HDR*);