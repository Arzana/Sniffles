#pragma once

#include "Utils.h"

typedef struct IcmpHdr
{
	octet type;
	octet code;
	wyde checksum;
	uint data;
} ICMP_HDR;