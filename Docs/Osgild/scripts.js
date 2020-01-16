function init() {
	var range = document.getElementById("range");
	var map = document.getElementById("map");
	map.style.width = "" + range.value + "%";
	range.oninput = function() {
		map.style.width = "" + range.value + "%";
	}
}
