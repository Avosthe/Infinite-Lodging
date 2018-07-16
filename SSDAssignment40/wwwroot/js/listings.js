var observe;
if (window.attachEvent) {
    observe = function (element, event, handler) {
        element.attachEvent('on' + event, handler);
    };
}
else {
    observe = function (element, event, handler) {
        element.addEventListener(event, handler, false);
    };
}
function init() {
    var text = document.getElementById('text');
    function resize() {
        text.style.height = 'auto';
        text.style.height = text.scrollHeight + 'px';
    }
    /* 0-timeout to get the already changed text */
    function delayedResize() {
        window.setTimeout(resize, 0);
    }
    observe(text, 'change', resize);
    observe(text, 'cut', delayedResize);
    observe(text, 'paste', delayedResize);
    observe(text, 'drop', delayedResize);
    observe(text, 'keydown', delayedResize);

    text.focus();
    text.select();
    resize();
}

function getURL() {
    var url = window.location.href;
    if (url.includes("location="))
    {
        var location = url.search("location=");
        location += 9;
        location = url.substring(location);
        if (location == "") {
            document.getElementById("myheader").innerHTML = "Showing all results"
        }
        else {
            document.getElementById("myheader").innerHTML = "Showing results for \"" + location + "\"";
        }
    }
    else
    {
        document.getElementById("myheader").innerHTML = "Showing all results"
    }
}
