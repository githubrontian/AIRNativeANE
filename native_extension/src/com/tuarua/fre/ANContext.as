/**
 * Created by Eoin Landy on 07/05/2017.
 */
package com.tuarua.fre {
import flash.events.StatusEvent;
import flash.external.ExtensionContext;

public class ANContext {
    public static const NAME:String = "AIRNativeANE";
    private static var _ctx:ExtensionContext;

    public static function getContext():ExtensionContext {
        if (_ctx == null) {
            try {
                _ctx = ExtensionContext.createExtensionContext("com.tuarua." + NAME, null);
                _ctx.addEventListener(StatusEvent.STATUS, gotEvent);
            } catch (e:Error) {
                trace("[" + NAME + "] ANE Not loaded properly.  Future calls will fail.");
            }
        }
        return _ctx;
    }

    private static function gotEvent(event:StatusEvent):void {
        var pObj:Object;
        switch (event.level) {
            case "TRACE":
                trace(event.code);
                break;
        }
    }

    public static function dispose():void {
        if (!_ctx) {
            return;
        }
        trace("[" + NAME + "] Unloading ANE...");
        _ctx.removeEventListener(StatusEvent.STATUS, gotEvent);
        _ctx.dispose();
        _ctx = null;
    }

}
}
