#pragma once

#include <stdio.h>

typedef unsigned char octet;
typedef unsigned short wyde;
typedef unsigned int uint;

FILE *pFile;

void StartWriteData(void);
void WriteData(const char*);
void EndWriteData(void);