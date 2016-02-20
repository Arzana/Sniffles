#include "SockInit.h"

int InitNet(SOCKET* s)
{
	WSADATA *wsa;
	if (!(wsa = InitWinSock())) return 0;

	if ((*s = GetSocket()) == INVALID_SOCKET) return 0;

	SOCKADDR_IN *sa;
	if (!(sa = GetSocketAddr())) return 0;

	if (!Bind(*s, sa)) return 0;

	return SetPromiscuous(*s);
}

WSADATA* InitWinSock(void)
{
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		printf("Failed.\nError Code: %d\n", WSAGetLastError());
		return NULL;
	}
	else printf("Initialised WinSock.\n");

	return &wsa;
}

SOCKET GetSocket(void)
{
	SOCKET s;
	if ((s = socket(AF_INET, SOCK_RAW, IPPROTO_IP)) == INVALID_SOCKET)
	{
		printf("Could not create socket %d\n", WSAGetLastError());
	}
	else printf("Socket created.\n");

	return s;
}

SOCKADDR_IN* GetSocketAddr(void)
{
	char hostName[HOSTNAME_LEN];
	if (gethostname(hostName, HOSTNAME_LEN) == SOCKET_ERROR)
	{
		printf("Unable to get host name: %d\n", WSAGetLastError());
		return NULL;
	}

	struct hostent *h;
	if ((h = gethostbyname(hostName)) == NULL)
	{
		printf("Unable to get host: %d", WSAGetLastError());
		return NULL;
	}

	printf("Host address found.\n");

	SOCKADDR_IN sa;
	sa.sin_family = AF_INET;
	sa.sin_port = htons(6000);
	memcpy(&sa.sin_addr.S_un.S_addr, h->h_addr_list[0], h->h_length);

	return &sa;
}

int Bind(SOCKET s, SOCKADDR_IN *sa)
{
	if (bind(s, (SOCKADDR*)sa, sizeof(*sa)) == SOCKET_ERROR)
	{
		printf("Unable to bind socket: %d\n", WSAGetLastError());
		return 0;
	}

	printf("Bound socket.\n");
	return 1;
}

int SetPromiscuous(SOCKET s)
{
	uint opt = 1;
	DWORD bytes;
	if (WSAIoctl(s, SIO_RCVALL, &opt, sizeof(opt), NULL, 0, &bytes, NULL, NULL) == SOCKET_ERROR)
	{
		printf("Failed to set promiscuous mode: %d\n", WSAGetLastError());
		return 0;
	}

	return 1;
}