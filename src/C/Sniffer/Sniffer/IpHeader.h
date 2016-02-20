#pragma once

#include "Utils.h"

typedef struct IpHdr
{
	octet hdrLen : 4;
	octet version : 4;
	octet tos;
	wyde totalLen;
	wyde id;

	octet fragOffset : 5;
	octet moreFragment : 1;
	octet dontFragment : 1;
	octet reserved_zero : 1;

	octet ttl;
	octet proto;
	wyde checksum;

	uint srcAddr;
	uint destAddr;

} IPV4_HDR;