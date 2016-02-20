#include "Main.h"

SOCKET sock;


int main(int argc, const char *argv[])
{
	octet *buffer = malloc(PACK_SIZE);
	int bufflen;
	if (!InitNet(&sock))
	{
		printf("Failed to initialize WinSock!\n");
		goto Failed;
	}

	while (1)
	{
		bufflen = recv(sock, buffer, PACK_SIZE, 0);
		if (bufflen < 0)
		{
			printf("Recv error at get packets: %d.\n", WSAGetLastError());
			goto Failed;
		}

		ProcessPacket(buffer, bufflen);
	}

	closesocket(sock);
	printf("Finished.\n");

	getchar();
	free(buffer);
	return 0;

Failed:
	getchar();
	free(buffer);
	return -1;
}

void ProcessPacket(octet *buff, int len)
{
	IPV4_HDR *iphdr = buff;

	switch (iphdr->proto)
	{
	case 1:		// ICMP
	case 58:	// ICMP (IPv6)
		ProcessICMP(buff);
		break;
	case 4:		// IP in IP
		ProcessPacket(IP_DATA_START(buff), len);
		break;
	case 17:	// UDP
		ProcessUDP(buff);
		break;
	default:
		printf("%d\n", iphdr->proto);
		break;
	}
}

void ProcessUDP(octet *buff)
{
	UDP_HDR *udphdr = IP_DATA_START(buff);
	printf("UDP\n");
}

void ProcessICMP(octet *buff)
{
	ICMP_HDR *icmphdr = IP_DATA_START(buff);

	char *type;
	switch (icmphdr->type)
	{
	case 0:
		type = "EchoReply";
		break;
	case 1:
	case 2:
	case 7:
		type = "Unassigned";
		break;
	case 3:
		type = "DestinationUnreachable";
		break;
	case 4:
		type = "SourceQuench";
		break;
	case 5:
		type = "Redirect";
		break;
	case 6:
		type = "AlternateHostAddress";
		break;
	case 8:
		type = "Echo";
		break;
	case 9:
		type = "RouterAdvertisement";
		break;
	case 10:
		type = "RouterSelection";
		break;
	case 11:
		type = "TimeExceeded";
		break;
	case 12:
		type = "ParameterProblem";
		break;
	case 13:
		type = "Timestamp";
		break;
	case 14:
		type = "TimestampReply";
		break;
	case 15:
		type = "InformationRequest";
		break;
	case 16:
		type = "InformationReply";
		break;
	case 17:
		type = "AddressMaskRequest";
		break;
	case 18:
		type = "AddressMaskReply";
		break;
	case 19:
		type = "ReservedForSecurity";
		break;
	case 20:
	case 21:
	case 22:
	case 23:
	case 24:
	case 25:
	case 26:
	case 27:
	case 28:
	case 29:
		type = "ReservedForRobustnessExperiment";
		break;
	case 30:
		type = "Traceroute";
		break;
	case 31:
		type = "DatagramConversionError";
		break;
	case 32:
		type = "MobileHostRedirect";
		break;
	case 33:
		type = "IPv6WhereAreYou";
		break;
	case 34:
		type = "IPv6IAmHere";
		break;
	case 35:
		type = "MobileRegistrationRequest";
		break;
	case 36:
		type = "MobileRegistrationReply";
		break;
	case 37:
		type = "DomainNameRequest";
		break;
	case 38:
		type = "DomainNameReply";
		break;
	case 39:
		type = "SKIP";
		break;
	case 40:
		type = "Photuris";
		break;
	default:
		type = "Reserved";
		break;
	}

	printf("ICMP - %s\n", type);
}