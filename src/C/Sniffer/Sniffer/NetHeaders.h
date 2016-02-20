#pragma once

#include "IpHeader.h"

#include "IcmpHeader.h"
#include "UdpHeader.h"

#define IP_DATA_START(buff)		(buff + sizeof(IPV4_HDR))