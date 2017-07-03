/**
 * Created by Local Eoin Landy on 25/05/2017.
 */
package com.tuarua.fre.display {
import com.tuarua.fre.ANContext;

[RemoteClass(alias="com.tuarua.fre.display.ANSprite")]
public class ANSprite extends ANDisplayObject {
    public function ANSprite() {
        super();
        this.type = SPRITE_TYPE;
    }

    public function addChild(nativeDisplayObject:ANDisplayObject):void {
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("addNativeChild", id, nativeDisplayObject);
                nativeDisplayObject.isAdded = true;
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }
}
}
