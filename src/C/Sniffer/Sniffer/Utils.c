#include "Utils.h"

void WriteData(const char *msg)
{
	FILE *pFile = fopen(".File1.txt", "a");
	printf(msg);
	fprintf(pFile, msg);
	fclose(pFile);
}