function init() {
	var range = document.getElementById("range");
	var map = document.getElementById("map");
	map.style.width = "" + range.value + "%";
	range.oninput = function() {
		map.style.width = "" + range.value + "%";
	}
	//range.addEventListener("wheel", onwheel);
}

function onwheel() {
	var range = document.getElementById("range");
	var map = document.getElementById("map");
	//alert(range.value);
	//var percent = range.value.toString().substring(0, range.length - 1);
	//alert(percent);
	range.value = range.value + 10;
	map.style.width = "" + range.value + "%";
	//map.style.width = "" + (parseInt(range.value) + 10) + "%";
	console.log(range.value);
}