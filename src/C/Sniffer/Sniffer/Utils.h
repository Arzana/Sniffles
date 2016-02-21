#pragma once

#include <stdio.h>

#define FILE_LOG
#define PACK_START(proto)	fprintf(pFile, "--------------------"); \
							fprintf(pFile, (proto)); \
							fprintf(pFile, "--------------------")
#define PACK_END			fprintf(pFile, "----------------------------------------")

typedef unsigned char octet;
typedef unsigned short wyde;
typedef unsigned int uint;

FILE *pFile;

void StartWriteData(void);
void WriteData(const char*);
void WriteLongData(const char *, int);
void EndWriteData(void);