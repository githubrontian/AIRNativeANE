#include "FreSharpMacros.h"
#include "AIRNativeANE.h"
#include "FlashRuntimeExtensions.h"
#include "stdafx.h"
#include "FreSharpBridge.h"

extern "C" {

	[System::STAThreadAttribute]
	BOOL APIENTRY AIRNativeANEMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
		switch (ul_reason_for_call) {
		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
		}
		return true;
	}

	CONTEXT_INIT(TRAN) {

		FREBRIDGE_INIT

		static FRENamedFunction extensionFunctions[] = {
			 MAP_FUNCTION(initNativeStage)
			,MAP_FUNCTION(addNativeStage)
			,MAP_FUNCTION(updateNativeStage)
			,MAP_FUNCTION(addNativeChild)
			,MAP_FUNCTION(updateNativeChild)
			,MAP_FUNCTION(fullscreenNativeStage)
			,MAP_FUNCTION(restoreNativeStage)
		};

		SET_FUNCTIONS

	}

	CONTEXT_FIN(TRAN) {
	}

	EXTENSION_INIT(TRAN)

	EXTENSION_FIN(TRAN)
}
