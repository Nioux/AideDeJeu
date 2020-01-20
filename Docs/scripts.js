function init() {
	var body = document.documentElement;
	var range = document.getElementById("range");
	var map = document.getElementById("map");
	map.style.width = "" + range.value + "%";
	range.oninput = function() {
		map.style.width = "" + range.value + "%";
		console.log(range.value);
	}
	//range.addEventListener("wheel", onwheel);
	addWheelListener(body, onwheel);
	
	var curYPos = 0,
  curXPos = 0,
  curDown = false;

window.addEventListener('mousemove', function(e) {
  if (curDown === true) {
    window.scrollTo(document.body.scrollLeft + (curXPos - e.pageX), document.body.scrollTop + (curYPos - e.pageY));
  }
	e.preventDefault();
});

window.addEventListener('mousedown', function(e) {
  curDown = true;
  curYPos = e.pageY;
  curXPos = e.pageX;
	e.preventDefault();
});
window.addEventListener('mouseup', function(e) {
  curDown = false;
	e.preventDefault();
});


}

function onwheel(e) {
	var range = document.getElementById("range");
	var map = document.getElementById("map");
	//alert(range.value);
	//var percent = range.value.toString().substring(0, range.length - 1);
	//alert(percent);
	console.log(range.value);
	range.value = parseInt(range.value) - Math.sign(e.deltaY) * 10;
	map.style.width = "" + range.value + "%";
	//map.style.width = "" + range.value + "%";
	//map.style.width = "" + (parseInt(range.value) + 10) + "%";
	console.log(range.value);
	e.preventDefault();
}


// creates a global "addWheelListener" method
// example: addWheelListener( elem, function( e ) { console.log( e.deltaY ); e.preventDefault(); } );
(function(window,document) {

    var prefix = "", _addEventListener, support;

    // detect event model
    if ( window.addEventListener ) {
        _addEventListener = "addEventListener";
    } else {
        _addEventListener = "attachEvent";
        prefix = "on";
    }

    // detect available wheel event
    support = "onwheel" in document.createElement("div") ? "wheel" : // Modern browsers support "wheel"
              document.onmousewheel !== undefined ? "mousewheel" : // Webkit and IE support at least "mousewheel"
              "DOMMouseScroll"; // let's assume that remaining browsers are older Firefox

    window.addWheelListener = function( elem, callback, useCapture ) {
        _addWheelListener( elem, support, callback, useCapture );

        // handle MozMousePixelScroll in older Firefox
        if( support == "DOMMouseScroll" ) {
            _addWheelListener( elem, "MozMousePixelScroll", callback, useCapture );
        }
    };

    function _addWheelListener( elem, eventName, callback, useCapture ) {
        elem[ _addEventListener ]( prefix + eventName, support == "wheel" ? callback : function( originalEvent ) {
            !originalEvent && ( originalEvent = window.event );

            // create a normalized event object
            var event = {
                // keep a ref to the original event object
                originalEvent: originalEvent,
                target: originalEvent.target || originalEvent.srcElement,
                type: "wheel",
                deltaMode: originalEvent.type == "MozMousePixelScroll" ? 0 : 1,
                deltaX: 0,
                deltaY: 0,
                deltaZ: 0,
                preventDefault: function() {
                    originalEvent.preventDefault ?
                        originalEvent.preventDefault() :
                        originalEvent.returnValue = false;
                }
            };
            
            // calculate deltaY (and deltaX) according to the event
            if ( support == "mousewheel" ) {
                event.deltaY = - 1/40 * originalEvent.wheelDelta;
                // Webkit also support wheelDeltaX
                originalEvent.wheelDeltaX && ( event.deltaX = - 1/40 * originalEvent.wheelDeltaX );
            } else {
                event.deltaY = originalEvent.deltaY || originalEvent.detail;
            }

            // it's time to fire the callback
            return callback( event );

        }, useCapture || false );
    }

})(window,document);

