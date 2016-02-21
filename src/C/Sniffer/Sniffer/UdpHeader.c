#include "UdpHeader.h"
#include <WinSock2.h>

#define LOG(x, y)	fprintf(pFile, x, y)

void WriteUdpHdr(UDP_HDR* hdr)
{
#ifdef FILE_LOG
	fprintf(pFile, "UDP Header\n");
	LOG("|- Source Port: %u\n", ntohs(hdr->srcPort));
	LOG("|- Destination Port: %u\n", ntohs(hdr->destPort));
	LOG("|- Checksum: %u\n", ntohs(hdr->checksum));
	LOG("|- Packet Size: %u Octets\n", ntohs(hdr->octetLen));
#endif
}