#include "Utils.h"
#include <time.h>
#include <string.h>

void WriteData(const char *msg)
{
	printf(msg);
#ifdef FILE_LOG
	fprintf(pFile, msg);
#endif
}

void StartWriteData(void)
{
	time_t rawTime;
	struct tm *timeInfo;

	time(&rawTime);
	timeInfo = localtime(&rawTime);

	pFile = fopen("Log.txt", "w");
	fprintf(pFile, "%s", asctime(timeInfo));
}

void EndWriteData(void)
{
	fclose(pFile);
}

void WriteLongData(const char *data, int size)
{
	char a, line[17], c;

	for (int i = 0; i < size; ++i)
	{
		c = data[i];
		// Print hex value of current char
		fprintf(pFile, " %.2x", (octet)c);
		// Add char to data line
		a = (c >= 32 && c <= 128) ? (octet)c : '.';
		line[i % 16] = a;

		// If last char of a line, print line (16 chars in 1 line)
		if ((i != 0 && (i + 1) % 16 == 0) || i == size - 1)
		{
			line[i % 16 + 1] = '\0';
			fprintf(pFile, "          ");

			for (int j = strlen(line); j < 16; ++j)
			{
				fprintf(pFile, "   ");
			}

			fprintf(pFile, "\t%s\n", line);
		}
	}

	fprintf(pFile, "\n");
}