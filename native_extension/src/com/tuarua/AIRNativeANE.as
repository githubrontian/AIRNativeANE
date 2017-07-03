/**
 * Created by Local Eoin Landy on 02/07/2017.
 */
package com.tuarua {
import com.tuarua.fre.ANContext;

import flash.events.EventDispatcher;

public class AIRNativeANE extends EventDispatcher {

    public function AIRNativeANE() {
    }

    public function dispose():void {
        ANContext.dispose();
    }
}
}
