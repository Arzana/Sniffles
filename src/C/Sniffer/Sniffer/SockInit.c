#include "SockInit.h"

int InitNet(void)
{
	IN_ADDR addr;
	HOSTENT *local;
	int in;

#ifdef _WIN32
	WSADATA wsa;
	if (InitWinsock(&wsa)) return 1;
#endif

	if (CreateSock()) return 1;
	if (GetLocalHost(&local)) return 1;
	if (SetWebInterface(local, &addr, &in)) return 1;
	if (BindSocket(&addr)) return 1;

#ifdef _WIN32
	if (MakePromiscious(in)) return 1;
#endif

	return 0;
}

#ifdef _WIN32
int InitWinsock(WSADATA *wsa)
{
	printf("Initializing Winsock...");
	if (WSAStartup(MAKEWORD(2, 2), wsa) != 0)
	{
		printf("WSAStartup() failed (%d).\n", WSAGetLastError());
		return 1;
	}

	printf("Initialized.");
	return 0;
}
#endif

int CreateSock(void)
{
	printf("\nCreating RAW Socket...");
	if ((sniffer = socket(AF_INET, SOCK_RAW, IPPROTO_IP)) == INVALID_SOCKET)
	{
#ifdef _WIN32
		printf("Failed to create raw socket (%d).\n", WSAGetLastError());
#else
		printf("Failed to create raw socket.\n");
#endif
		return 1;
	}
	printf("Created");
	return 0;
}

int GetLocalHost(HOSTENT **local)
{
	char hostname[100];

	if (gethostname(hostname, sizeof(hostname)) == SOCKET_ERROR)
	{
#ifdef _WIN32
		printf("\nError: %d.", WSAGetLastError());
#elif
		printf("\nError getting host name.");
#endif
		return 1;
	}
	printf("\nHost name: %s.\n", hostname);

	*local = gethostbyname(hostname);
	printf("\nAvailable Network Interfaces:\n");
	if (*local == NULL)
	{
#ifdef _WIN32
		printf("Error: %d.", WSAGetLastError());
#elif
		printf("Error getting local host.");
#endif
		return 1;
	}

	return 0;
}

int SetWebInterface(HOSTENT *local, IN_ADDR *addr, int *in)
{
	for (int i = 0; i < local->h_addr_list[i] != 0; ++i)
	{
		memcpy(addr, local->h_addr_list[i], sizeof(struct in_addr));
		printf("%d - %s\n", i, inet_ntoa(*addr));
	}

	printf("Enter the interface number you whould like to sniff: ");
	scanf("%d", in);

	if (*in < 0 || *in > local->h_length)
	{
		printf("\nInvalid interface.");
		return 1;
	}

	memset(&dest, 0, sizeof(dest));
	memcpy(&dest.sin_addr.s_addr, local->h_addr_list[*in], sizeof(dest.sin_addr.s_addr));
	dest.sin_family = AF_INET;
	dest.sin_port = 0;
	return 0;
}

int BindSocket(IN_ADDR *addr)
{
	printf("Binding socket to local system and port 0...");
	if (bind(sniffer, (struct sockaddr*)&dest, sizeof(dest)) == SOCKET_ERROR)
	{
#ifdef _WIN32
		printf("bind(%s) failed (%d).\n", inet_ntoa(*addr), WSAGetLastError());
#elif
		printf("bind(%s) failed.\n", inet_ntoa(*addr));
#endif
		return 1;
	}

	printf("Binding successful.");
	return 0;
}

#ifdef _WIN32
int MakePromiscious(int in)
{
	int opt = 1;
	printf("\nSetting socket to promiscious mode...");
	if (WSAIoctl(sniffer, SIO_RCVALL, &opt, sizeof(opt), 0, 0, (LPDWORD)&in, 0, 0) == SOCKET_ERROR)
	{
		printf("WSAIoctl() failed (%d).\n", WSAGetLastError());
		return 1;
	}

	printf("Socket set\n\n");
	return 0;
}
#endif