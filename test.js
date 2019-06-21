function rgb2hex(orig) {
    var rgb = orig.replace(/\s/g, '').match(/^rgba?\((\d+),(\d+),(\d+)/i);
    return (rgb && rgb.length === 4) ? "#" +
        ("0" + parseInt(rgb[1], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[2], 10).toString(16)).slice(-2) +
        ("0" + parseInt(rgb[3], 10).toString(16)).slice(-2) : orig;
}
JSON.stringify($("td").toArray().filter(
    function (td) {

        var rgbz = (rgb2hex($(td).css("background-color")));
        return rgbz != "#000000";
    }
).map(function(td){return (rgb2hex($(td).css("background-color")));}));