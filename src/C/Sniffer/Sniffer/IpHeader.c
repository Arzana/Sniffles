#include "IpHeader.h"
#include <WinSock2.h>

#define LOG(x, y)	fprintf(pFile, x, y)
#define STRBOOL(x)	((x) ? "True" : "False")

void WriteIPv4Hdr(IPV4_HDR* hdr)
{
	struct sockaddr_in src, dest;
	src.sin_addr.s_addr = hdr->srcAddr;
	dest.sin_addr.s_addr = hdr->destAddr;

#ifdef FILE_LOG
	fprintf(pFile, "IP Header\n");
	LOG("|- IP Version: %u\n", hdr->version);
	LOG("|- IP Header Length: %u Octets\n", hdr->hdrLen * 4);
	LOG("|- Type of Service: %u\n", hdr->tos);
	LOG("|- IP Total Length: %u Octets(Size of packet)\n", ntohs(hdr->totalLen));
	LOG("|- Identifier: %u\n", ntohs(hdr->id));
	LOG("|- Don't Fragment: %s\n", STRBOOL(hdr->dontFragment));
	LOG("|- More Fragment: %s\n", STRBOOL(hdr->moreFragment));
	LOG("|- Time to Live: %u\n", hdr->ttl);
	LOG("|- Protocol: %u\n", hdr->proto);
	LOG("|- Checksum: %u\n", ntohs(hdr->checksum));
	LOG("|- Source IP: %s\n", inet_ntoa(src.sin_addr));
	LOG("|- Destination IP: %s\n", inet_ntoa(dest.sin_addr));
#endif
}