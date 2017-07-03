/**
 * Created by Eoin Landy on 18/05/2017.
 */
package com.tuarua.fre {
import com.tuarua.*;
import com.tuarua.fre.display.ANDisplayObject;

import flash.desktop.NativeApplication;

import flash.display.Stage;
import flash.events.Event;
import flash.events.FullScreenEvent;
import flash.events.NativeWindowDisplayStateEvent;

import flash.geom.Rectangle;

[RemoteClass(alias="com.tuarua.fre.ANStage")]
public final class ANStage {
    private static var _viewPort:Rectangle;
    private static var _visible:Boolean = true;
    private static const _id:String = "root";
    private static var _stage:Stage;

    public function ANStage() {
    }

    public static function addChild(nativeDisplayObject:ANDisplayObject):void {
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("addNativeChild", _id, nativeDisplayObject);
                nativeDisplayObject.isAdded = true;
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    public static function init(stage:Stage, viewPort:Rectangle, visible:Boolean, transparent:Boolean, backgroundColor:uint = 0):void {
        _stage = stage;
        stage.addEventListener(FullScreenEvent.FULL_SCREEN, onFullScreenEvent);
        stage.addEventListener(Event.RESIZE, onStageResize);
        NativeApplication.nativeApplication.activeWindow.addEventListener(
                NativeWindowDisplayStateEvent.DISPLAY_STATE_CHANGE, onWindowMiniMaxi);
        _viewPort = viewPort;
        _visible = visible;
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("initNativeStage", _viewPort, _visible, transparent, backgroundColor);
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    private static function onWindowMiniMaxi(event:NativeWindowDisplayStateEvent):void {
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("restoreNativeStage");
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    private static function onStageResize(event:Event):void {
        viewPort = _viewPort;
    }

    private static function onFullScreenEvent(event:FullScreenEvent):void {
        try {
            ANContext.getContext().call("fullscreenNativeStage", event.fullScreen);
        } catch (e:Error) {
            trace(e.message);
        }
    }

    public static function add():void {
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("addNativeStage");
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

    public static function get viewPort():Rectangle {
        return _viewPort;
    }

    public static function set viewPort(value:Rectangle):void {
        _viewPort = value;
        update("viewPort", value);
    }

    public static function get visible():Boolean {
        return _visible;
    }

    public static function set visible(value:Boolean):void {
        _visible = value;
        update("visible", value);
    }

    private static function update(type:String, value:*):void {
        if (ANContext.getContext()) {
            try {
                ANContext.getContext().call("updateNativeStage", type, value);
            } catch (e:Error) {
                trace(e.message);
            }
        }
    }

}
}
