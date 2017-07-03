#pragma once
#include "FlashRuntimeExtensions.h"
extern "C" {
	__declspec(dllexport) void TRANExtInizer(void** extData, FREContextInitializer* ctxInitializer, FREContextFinalizer* ctxFinalizer);
	__declspec(dllexport) void TRANExtFinizer(void* extData);
}

