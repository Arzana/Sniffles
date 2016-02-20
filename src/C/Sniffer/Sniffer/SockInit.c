#include "SockInit.h"

int InitNet(void)
{
	struct in_addr addr;
	int in, i, j;

	char hostname[100];
	struct hostent *local;
	WSADATA wsa;

	printf("Initializing Winsock...");
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		printf("WSAStartup() failed (%d).\n", WSAGetLastError());
		return 1;
	}
	printf("Initialized.");

	printf("\nCreating RAW Socket...");
	if ((sniffer = socket(AF_INET, SOCK_RAW, IPPROTO_IP)) == INVALID_SOCKET)
	{
		printf("Failed to create raw socket (%d).\n", WSAGetLastError());
		return 1;
	}
	printf("Created");

	if (gethostname(hostname, sizeof(hostname)) == SOCKET_ERROR)
	{
		printf("\nError: %d.", WSAGetLastError());
		return 1;
	}
	printf("\nHost name: %s.\n", hostname);

	local = gethostbyname(hostname);
	printf("\nAvailable Network Interfaces:\n");
	if (local == NULL)
	{
		printf("Error: %d.", WSAGetLastError());
		return 1;
	}

	for (i = 0; i < local->h_addr_list[i] != 0; ++i)
	{
		memcpy(&addr, local->h_addr_list[i], sizeof(struct in_addr));
		printf("%d - %s\n", i, inet_ntoa(addr));
	}

	printf("Enter the interface number you whould like to sniff: ");
	scanf("%d", &in);

	memset(&dest, 0, sizeof(dest));
	memcpy(&dest.sin_addr.s_addr, local->h_addr_list[in], sizeof(dest.sin_addr.s_addr));
	dest.sin_family = AF_INET;
	dest.sin_port = 0;

	printf("Binding socket to local system and port 0...");
	if (bind(sniffer, (struct sockaddr*)&dest, sizeof(dest)) == SOCKET_ERROR)
	{
		printf("bind(%s) failed (%d).\n", inet_ntoa(addr), WSAGetLastError());
		return 1;
	}
	printf("Binding successful.");

	j = 1;
	printf("\nSetting socket to promiscious mode...");
	if (WSAIoctl(sniffer, SIO_RCVALL, &j, sizeof(j), 0, 0, (LPDWORD)&in, 0, 0) == SOCKET_ERROR)
	{
		printf("WSAIoctl() failed (%d).\n", WSAGetLastError());
		return 1;
	}
	printf("Socket set\n\n");
	return 0;
}