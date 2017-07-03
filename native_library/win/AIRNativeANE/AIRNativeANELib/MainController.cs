using System;
using System.Collections.Generic;
using System.Linq;
using TuaRua.AIRNative;
using TuaRua.AIRNative.Internal;
using FREObject = System.IntPtr;

namespace AIRNativeLib {
    public class MainController : FreSharpController {
        public string[] GetFunctions() {
            FunctionsDict =
                new Dictionary<string, Func<FREObject, uint, FREObject[], FREObject>> {
                    {"initNativeStage", FreStageSharp.Init},
                    {"addNativeStage", FreStageSharp.AddRoot},
                    {"updateNativeStage", FreStageSharp.Update},
                    {"addNativeChild", FreDisplayList.AddChild},
                    {"updateNativeChild", FreDisplayList.UpdateChild},
                    {"fullscreenNativeStage", FreStageSharp.OnFullScreen},
                    {"restoreNativeStage", FreStageSharp.Restore},
                };


            return FunctionsDict.Select(kvp => kvp.Key).ToArray();
        }
    }
}