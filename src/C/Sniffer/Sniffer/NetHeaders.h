#pragma once

#include "IpHeader.h"

#include "IcmpHeader.h"
#include "UdpHeader.h"

#define IP_DATA_START(buff)		(buff + ((IPV4_HDR*)buff)->hdrLen * 4)