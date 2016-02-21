#pragma once

#include "IpHeader.h"

#include "IcmpHeader.h"
#include "UdpHeader.h"

#define IP_DATA_START(buff)		(buff + ((IPV4_HDR*)buff)->hdrLen * 4)
#define IS_DNS(hdr)	(ntohs((hdr)->destPort) == 53 || ntohs((hdr)->srcPort) == 53)