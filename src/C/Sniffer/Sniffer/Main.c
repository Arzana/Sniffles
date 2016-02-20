#include "Main.h"

SOCKET sock;
FILE *logFile;

int main(int argc, const char *argv[])
{
	logFile = fopen("log.txt", "w");
	if (!logFile) printf("Unable to create file!\n");

	octet *buffer = malloc(PACK_SIZE);
	int bufflen;
	if (!InitNet(&sock))
	{
		printf("Failed to initialize WinSock!\n");
		return -1;
	}

	while (1)
	{
		bufflen = recv(sock, buffer, PACK_SIZE, 0);
		if (bufflen < 0)
		{
			printf("Recv error at get packets: %d.\n", WSAGetLastError());
			return -1;
		}

		ProcessPacket(buffer, bufflen);
	}

	closesocket(sock);
	printf("Finished.\n");
	return 0;
}

void ProcessPacket(octet *buff, int len)
{
	IPV4_HDR *iphdr = buff;
	printf("Protocol: %d\nData: %d\n", iphdr->proto, len - sizeof(IPV4_HDR));
}