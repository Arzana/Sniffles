#include "Utils.h"
#include <time.h>

void WriteData(const char *msg)
{
	printf(msg);
	fprintf(pFile, msg);
}

void StartWriteData(void)
{
	time_t rawTime;
	struct tm *timeInfo;

	time(&rawTime);
	timeInfo = localtime(&rawTime);

	pFile = fopen("Log.txt", "a");
	fprintf(pFile, "%s", asctime(timeInfo));
}

void EndWriteData(void)
{
	fclose(pFile);
}